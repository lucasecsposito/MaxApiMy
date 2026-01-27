using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	/// <summary>
	/// Пользователь или бот
	/// </summary>
	public class User
	{
		// User
		// Объект, описывающий пользователя. Имеет несколько вариаций (наследований): User, UserWithPhoto, BotInfo, ChatMember
		[JsonPropertyName("user_id")]
		public long UserId { get; set; }

		[JsonPropertyName("first_name")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? FirstName { get; set; }

		[JsonPropertyName("last_name")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? LastName { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		[JsonPropertyName("username")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Username { get; set; }

		[JsonPropertyName("is_bot")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public bool? IsBot { get; set; }

		[JsonPropertyName("last_activity_time")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? LastActivityTime { get; set; }

		// UserWithPhoto
		// Объект пользователя с фотографией
		[JsonPropertyName("description")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Description { get; set; }

		[JsonPropertyName("avatar_url")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? AvatarUrl { get; set; }

		[JsonPropertyName("full_avatar_url")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? FullAvatarUrl { get; set; }

		// BotInfo
		// Объект, описывающий информацию о боте
		[JsonPropertyName("commands")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<BotCommand>? Commands { get; set; }

		// ChatMember
		// Объект, описывающий участника чата
		[JsonPropertyName("last_access_time")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? LastAccessTime { get; set; }

		[JsonPropertyName("is_owner")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public bool? IsOwner { get; set; }

		[JsonPropertyName("is_admin")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public bool? IsAdmin { get; set; }

		[JsonPropertyName("join_time")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? JoinTime { get; set; }

		[JsonPropertyName("permissions")]
		public List<JsonElement> RawPermissions { get; set; } = new();

		[JsonIgnore]
		public List<ChatAdminPermission> Permissions { get; set; } = new();

		[JsonPropertyName("alias")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Alias { get; set; }

		// calculated
		public DateTime LastActivity => DateTimeOffset.FromUnixTimeMilliseconds(LastActivityTime ?? 0).UtcDateTime;
	}
}
