using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Common;

namespace MaxApiMy.Data
{
	public class Update
	{
		/// <summary>
		/// Короткие названия UpdateType для логов
		/// </summary>
		public static Dictionary<UpdateType, string> UpdateCodes;

		[JsonPropertyName("user_locale")]
		public string UserLocale { get; set; }

		[JsonPropertyName("payload")]
		public string Payload { get; set; }

		[JsonPropertyName("update_type")]
		public UpdateType UpdateType { get; set; }

		// Go: int timestamp (ms/?) and GetUpdateTime(): time.Unix(int64(Timestamp/1000),0)
		// => Timestamp в миллисекундах.
		[JsonPropertyName("timestamp")]
		public long Timestamp { get; set; }

		[JsonIgnore]
		public string DebugRaw { get; set; } = string.Empty;

		public UpdateType GetUpdateType() => UpdateType;
		public string GetUpdateCode() => UpdateCodes.ContainsKey(UpdateType) ? UpdateCodes[UpdateType] : "NA";
		public string GetDebugRaw() => DebugRaw;
		public DateTimeOffset UpdateTime => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).DateTime;
		private bool _isBot => GetUser()?.IsBot == true;

		public virtual long GetUserId() => 0;
		public virtual long GetChatId() => 0;
		public virtual ChatType GetChatType() => ChatType.unknown;
		public virtual User GetUser() => null;
		public virtual Message GetMessage() => null;
		public virtual Recipient GetRecipient() => null;
		public virtual Callback GetCallback() => null;
		public virtual string GetLogInfo(string botShortName) => $"<- {UpdateType} Bot:{botShortName} Chat:{(GetChatId() == 0 ? "" : GetChatId().ToString())} User:{GetUserId()}{(_isBot ? "(bot)" : "")} {(GetMessage() == null ? "" : "'" + (GetMessage().Body?.Text ?? "").ToString().Replace('\r', ' ').Replace('\n', ' ') + "'")}".TakeMax(157);

		static Update()
		{
			UpdateCodes = new Dictionary<UpdateType, string>
			{
				{ UpdateType.bot_started, "BS" },
				{ UpdateType.bot_added, "BA" },
				{ UpdateType.bot_removed, "BR" },
				{ UpdateType.chat_title_changed, "CTC" },
				{ UpdateType.message_callback, "CA" },
				{ UpdateType.message_created, "MC" },
				{ UpdateType.message_removed, "MR" },
				{ UpdateType.message_edited, "ME" },
				{ UpdateType.user_added, "UA" },
				{ UpdateType.user_removed, "UR" },
			};
		}
	}

	public class BotAddedToChatUpdate : Update
	{
		[JsonPropertyName("chat_id")]
		public long ChatId { get; set; }

		[JsonPropertyName("user")]
		public User User { get; set; } = new();

		public override long GetUserId() => User?.UserId ?? 0;
		public override long GetChatId() => ChatId;
		public override ChatType GetChatType() => ChatType.chat;
		public override User GetUser() => User;
		public override Message GetMessage() => null;
		public override Recipient GetRecipient() => null;
		public override Callback GetCallback() => null;
	}

	public class BotRemovedFromChatUpdate : Update
	{
		[JsonPropertyName("chat_id")]
		public long ChatId { get; set; }

		[JsonPropertyName("user")]
		public User User { get; set; } = new();

		public override long GetUserId() => User?.UserId ?? 0;
		public override long GetChatId() => ChatId;
		public override ChatType GetChatType() => ChatType.chat;
		public override User GetUser() => User;
		public override Message GetMessage() => null;
		public override Recipient GetRecipient() => null;
		public override Callback GetCallback() => null;
	}

	public class BotStartedUpdate : Update
	{
		[JsonPropertyName("chat_id")]
		public long ChatId { get; set; }

		[JsonPropertyName("user_id")]
		public long UserId { get; set; }

		[JsonPropertyName("user")]
		public User User { get; set; } = new();

		public override long GetUserId() => UserId;
		public override long GetChatId() => ChatId;
		public override ChatType GetChatType() => ChatType.dialog;
		public override User GetUser() => User;
		public override Message GetMessage() => null;
		public override Recipient GetRecipient() => null;
		public override Callback GetCallback() => null;
	}

	public class ChatTitleChangedUpdate : Update
	{
		[JsonPropertyName("chat_id")]
		public long ChatId { get; set; }

		[JsonPropertyName("user")]
		public User User { get; set; } = new();

		[JsonPropertyName("title")]
		public string Title { get; set; } = string.Empty;

		public override long GetUserId() => User?.UserId ?? 0;
		public override long GetChatId() => ChatId;
		public override ChatType GetChatType() => ChatType.chat;
		public override User GetUser() => User;
		public override Message GetMessage() => null;
		public override Recipient GetRecipient() => null;
		public override Callback GetCallback() => null;
	}

	public class MessageCallbackUpdate : Update
	{
		[JsonPropertyName("callback")]
		public Callback Callback { get; set; } = new();

		[JsonPropertyName("message")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Message? Message { get; set; }

		public override long GetUserId() => Callback.User?.UserId ?? 0;
		public override long GetChatId() => Message.Recipient?.ChatId ?? 0;
		public override ChatType GetChatType() => Message.Recipient?.ChatType ?? ChatType.unknown;
		public override User GetUser() => Callback.User;
		public override Message GetMessage() => Message;
		public override Recipient GetRecipient() => Message.Recipient;
		public override Callback GetCallback() => Callback;
	}

	public class MessageCreatedUpdate : Update
	{
		[JsonPropertyName("message")]
		public Message Message { get; set; } = new();

		public override long GetUserId() => Message.Sender?.UserId ?? 0;
		public override long GetChatId() => Message.Recipient?.ChatId ?? 0;
		public override ChatType GetChatType() => Message.Recipient?.ChatType ?? ChatType.unknown;
		public override User GetUser() => Message.Sender;
		public override Message GetMessage() => Message;
		public override Recipient GetRecipient() => Message.Recipient;
		public override Callback GetCallback() => null;

		public string GetText() => Message.Body.Text ?? string.Empty;

		public string GetCommand()
		{
			var text = Message.Body.Text ?? string.Empty;
			if (text.StartsWith("/"))
			{
				var idx = text.IndexOf(':');
				return idx >= 0 ? text.Substring(0, idx) : text;
			}
			return "undefened";
		}

		public string GetParam()
		{
			var text = Message.Body.Text ?? string.Empty;
			if (text.StartsWith("/"))
			{
				var idx = text.IndexOf(':');
				if (idx >= 0 && idx + 1 < text.Length)
					return text.Substring(idx + 1);
				return string.Empty;
			}
			return string.Empty;
		}
	}

	public class MessageEditedUpdate : Update
	{
		[JsonPropertyName("message")]
		public Message Message { get; set; } = new();

		public override long GetUserId() => Message.Sender?.UserId ?? 0;
		public override long GetChatId() => Message.Recipient?.ChatId ?? 0;
		public override ChatType GetChatType() => Message.Recipient?.ChatType ?? ChatType.unknown;
		public override User GetUser() => Message.Sender;
		public override Message GetMessage() => Message;
		public override Recipient GetRecipient() => Message.Recipient;
		public override Callback GetCallback() => null;
	}

	public class MessageRemovedUpdate : Update
	{
		[JsonPropertyName("message_id")]
		public string MessageId { get; set; } = string.Empty;

		public override long GetUserId() => 0;
		public override long GetChatId() => 0;
		// TODO :: (AK) что здесь лучше возвращать: chat, dialog или unknown?
		public override ChatType GetChatType() => ChatType.unknown;
		public override User GetUser() => null;
		public override Message GetMessage() => null;
		public override Recipient GetRecipient() => null;
		public override Callback GetCallback() => null;
	}

	public class UserAddedToChatUpdate : Update
	{
		[JsonPropertyName("chat_id")]
		public long ChatId { get; set; }

		[JsonPropertyName("user")]
		public User User { get; set; } = new();

		[JsonPropertyName("inviter_id")]
		public long InviterId { get; set; }

		public override long GetUserId() => User?.UserId ?? 0;
		public override long GetChatId() => ChatId;
		public override ChatType GetChatType() => ChatType.chat;
		public override User GetUser() => User;
		public override Message GetMessage() => null;
		public override Recipient GetRecipient() => null;
		public override Callback GetCallback() => null;
	}

	public class UserRemovedFromChatUpdate : Update
	{
		[JsonPropertyName("chat_id")]
		public long ChatId { get; set; }

		[JsonPropertyName("user")]
		public User User { get; set; } = new();

		[JsonPropertyName("admin_id")]
		public long AdminId { get; set; }

		public override long GetUserId() => User?.UserId ?? 0;
		public override long GetChatId() => ChatId;
		public override ChatType GetChatType() => ChatType.chat;
		public override User GetUser() => User;
		public override Message GetMessage() => null;
		public override Recipient GetRecipient() => null;
		public override Callback GetCallback() => null;
	}
}
