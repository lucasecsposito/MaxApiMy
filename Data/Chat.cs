using System.Linq;
using System.Text.Json.Serialization;

namespace MaxApiMy.Data
{
	public class Chat
	{
		[JsonPropertyName("chat_id")]
		public long ChatId { get; set; }

		[JsonPropertyName("type")]
		public ChatType Type { get; set; }

		[JsonPropertyName("status")]
		public ChatStatus Status { get; set; }

		[JsonPropertyName("title")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Title { get; set; }

		[JsonPropertyName("icon")]
		public Image? Icon { get; set; }

		[JsonPropertyName("last_event_time")]
		public long LastEventTime { get; set; }

		[JsonPropertyName("participants_count")]
		public int ParticipantsCount { get; set; }

		[JsonPropertyName("owner_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? OwnerId { get; set; }

		[JsonPropertyName("participants")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Dictionary<string, long>? Participants { get; set; }

		[JsonPropertyName("is_public")]
		public bool IsPublic { get; set; }

		[JsonPropertyName("link")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Link { get; set; }

		[JsonPropertyName("description")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Description { get; set; }
	}
}
