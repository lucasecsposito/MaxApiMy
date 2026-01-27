using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Common;

namespace MaxApiMy.Data
{
	public class Message
	{
		[JsonPropertyName("sender")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public User? Sender { get; set; }

		[JsonPropertyName("recipient")]
		public Recipient Recipient { get; set; } = new();

		[JsonPropertyName("timestamp")]
		public long Timestamp { get; set; }

		[JsonPropertyName("link")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public LinkedMessage? Link { get; set; }

		[JsonPropertyName("body")]
		public MessageBody Body { get; set; } = new();

		[JsonPropertyName("stat")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public MessageStat? Stat { get; set; }

		[JsonPropertyName("url")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Url { get; set; }

		public DateTime Date => Timestamp.DateTimeFromUnixTime();
	}
}
