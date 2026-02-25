using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

var settings = DiscordApprovalSettings.LoadFromEnvironment();

if (!settings.IsValid(out var validationError))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Ошибка конфигурации: {validationError}");
    Console.ResetColor();
    Console.WriteLine("\nНеобходимые переменные окружения:");
    Console.WriteLine("  DISCORD_EMAIL_USERNAME=anko.software2@gmail.com");
    Console.WriteLine("  DISCORD_EMAIL_APP_PASSWORD=<app-password Gmail>");
    Console.WriteLine("\nНеобязательные:");
    Console.WriteLine("  DISCORD_IMAP_HOST=imap.gmail.com");
    Console.WriteLine("  DISCORD_IMAP_PORT=993");
    Console.WriteLine("  DISCORD_EMAIL_FROM=noreply@discord.com");
    Console.WriteLine("  DISCORD_POLL_INTERVAL_SECONDS=20");
    Console.WriteLine("  DISCORD_CONFIRMATION_TIMEOUT_MINUTES=10");
    return;
}

Console.WriteLine("Мониторинг писем Discord запущен.");
Console.WriteLine($"Почта: {settings.Username}");
Console.WriteLine($"Отправитель: {settings.FromAddress}");
Console.WriteLine($"Интервал проверки: {settings.PollIntervalSeconds} сек.");

while (true)
{
    try
    {
        var result = await DiscordLoginApprover.TryApproveNewestLoginAsync(settings);

        if (result.Status == ApprovalStatus.Approved)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Вход подтвержден. URL: {result.VerificationUrl}");
            Console.ResetColor();
        }
        else if (result.Status == ApprovalStatus.NoPendingEmails)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Новых писем с попыткой входа не найдено.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Требуется ручная проверка: {result.Details}");
            Console.ResetColor();
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Ошибка: {ex.Message}");
        Console.ResetColor();
    }

    await Task.Delay(TimeSpan.FromSeconds(settings.PollIntervalSeconds));
}

internal sealed record DiscordApprovalSettings(
    string Username,
    string Password,
    string ImapHost,
    int ImapPort,
    string FromAddress,
    int PollIntervalSeconds,
    int ConfirmationTimeoutMinutes)
{
    public static DiscordApprovalSettings LoadFromEnvironment()
    {
        return new DiscordApprovalSettings(
            Username: Environment.GetEnvironmentVariable("DISCORD_EMAIL_USERNAME") ?? string.Empty,
            Password: Environment.GetEnvironmentVariable("DISCORD_EMAIL_APP_PASSWORD") ?? string.Empty,
            ImapHost: Environment.GetEnvironmentVariable("DISCORD_IMAP_HOST") ?? "imap.gmail.com",
            ImapPort: int.TryParse(Environment.GetEnvironmentVariable("DISCORD_IMAP_PORT"), out var port) ? port : 993,
            FromAddress: Environment.GetEnvironmentVariable("DISCORD_EMAIL_FROM") ?? "noreply@discord.com",
            PollIntervalSeconds: int.TryParse(Environment.GetEnvironmentVariable("DISCORD_POLL_INTERVAL_SECONDS"), out var pollSeconds) ? pollSeconds : 20,
            ConfirmationTimeoutMinutes: int.TryParse(Environment.GetEnvironmentVariable("DISCORD_CONFIRMATION_TIMEOUT_MINUTES"), out var timeoutMinutes) ? timeoutMinutes : 10);
    }

    public bool IsValid(out string error)
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            error = "Не задан DISCORD_EMAIL_USERNAME.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            error = "Не задан DISCORD_EMAIL_APP_PASSWORD (для Gmail нужен App Password).";
            return false;
        }

        if (PollIntervalSeconds < 5)
        {
            error = "DISCORD_POLL_INTERVAL_SECONDS должен быть >= 5.";
            return false;
        }

        if (ConfirmationTimeoutMinutes < 1)
        {
            error = "DISCORD_CONFIRMATION_TIMEOUT_MINUTES должен быть >= 1.";
            return false;
        }

        error = string.Empty;
        return true;
    }
}

internal enum ApprovalStatus
{
    Approved,
    NeedsManualCheck,
    NoPendingEmails
}

internal sealed record ApprovalResult(ApprovalStatus Status, string Details, string VerificationUrl = "");

internal static class DiscordLoginApprover
{
    private static readonly Regex HrefRegex = new("href\\s*=\\s*[\"'](?<url>[^\"']+)[\"']", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static async Task<ApprovalResult> TryApproveNewestLoginAsync(DiscordApprovalSettings settings)
    {
        using var client = new ImapClient();
        await client.ConnectAsync(settings.ImapHost, settings.ImapPort, true);
        await client.AuthenticateAsync(settings.Username, settings.Password);

        var inbox = client.Inbox;
        await inbox.OpenAsync(FolderAccess.ReadWrite);

        var since = DateTime.UtcNow.AddMinutes(-settings.ConfirmationTimeoutMinutes);

        var query = SearchQuery.NotSeen
            .And(SearchQuery.DeliveredAfter(since))
            .And(SearchQuery.FromContains(settings.FromAddress))
            .And(
                SearchQuery.SubjectContains("new location")
                    .Or(SearchQuery.SubjectContains("нового места"))
                    .Or(SearchQuery.SubjectContains("new login"))
                    .Or(SearchQuery.SubjectContains("новый вход")));

        var uids = await inbox.SearchAsync(query);
        if (uids.Count == 0)
        {
            await client.DisconnectAsync(true);
            return new ApprovalResult(ApprovalStatus.NoPendingEmails, "Новых писем нет.");
        }

        var uid = uids[^1];
        var message = await inbox.GetMessageAsync(uid);

        var verificationUrl = ExtractVerificationLink(message);
        if (string.IsNullOrWhiteSpace(verificationUrl))
        {
            await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
            await client.DisconnectAsync(true);
            return new ApprovalResult(ApprovalStatus.NeedsManualCheck, "Ссылка подтверждения не найдена в письме.");
        }

        var approveResult = await OpenVerificationLinkAsync(verificationUrl);
        await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
        await client.DisconnectAsync(true);

        return approveResult;
    }

    private static string ExtractVerificationLink(MimeMessage message)
    {
        var bodies = new List<string>();
        if (!string.IsNullOrWhiteSpace(message.HtmlBody))
            bodies.Add(message.HtmlBody);

        if (!string.IsNullOrWhiteSpace(message.TextBody))
            bodies.Add(message.TextBody);

        foreach (var body in bodies)
        {
            foreach (Match match in HrefRegex.Matches(body))
            {
                var url = WebUtility.HtmlDecode(match.Groups["url"].Value);
                if (LooksLikeDiscordVerificationLink(url))
                    return url;
            }

            var plainUrl = ExtractPlainTextDiscordUrl(body);
            if (!string.IsNullOrWhiteSpace(plainUrl))
                return plainUrl;
        }

        return string.Empty;
    }

    private static bool LooksLikeDiscordVerificationLink(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return false;

        return uri.Host.Contains("discord.com", StringComparison.OrdinalIgnoreCase)
               && (uri.AbsolutePath.Contains("verify", StringComparison.OrdinalIgnoreCase)
                   || uri.Query.Contains("token", StringComparison.OrdinalIgnoreCase));
    }

    private static string ExtractPlainTextDiscordUrl(string body)
    {
        var matches = Regex.Matches(body, @"https?://[^\s\"'<>]+", RegexOptions.IgnoreCase);
        foreach (Match match in matches)
        {
            var candidate = WebUtility.HtmlDecode(match.Value.Trim());
            if (LooksLikeDiscordVerificationLink(candidate))
                return candidate;
        }

        return string.Empty;
    }

    private static async Task<ApprovalResult> OpenVerificationLinkAsync(string verificationUrl)
    {
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = true,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        using var client = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("DiscordLoginAutoApprover", "1.0"));
        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");

        using var response = await client.GetAsync(verificationUrl);
        var html = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return new ApprovalResult(
                ApprovalStatus.NeedsManualCheck,
                $"Ссылка открылась с HTTP {(int)response.StatusCode}. Требуется ручная проверка.",
                verificationUrl);
        }

        var approved = ContainsAny(html,
            "Login location verified",
            "Location confirmed",
            "Вход подтверждён",
            "Местоположение подтверждено",
            "You can close this window");

        if (approved)
        {
            return new ApprovalResult(ApprovalStatus.Approved, "Вход подтвержден автоматически.", verificationUrl);
        }

        return new ApprovalResult(
            ApprovalStatus.NeedsManualCheck,
            "Ссылка открыта, но не удалось явно подтвердить успешную авторизацию по содержимому страницы.",
            verificationUrl);
    }

    private static bool ContainsAny(string text, params string[] needles)
        => needles.Any(n => text.Contains(n, StringComparison.OrdinalIgnoreCase));
}
