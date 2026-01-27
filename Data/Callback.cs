using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	public class Callback
	{
		[JsonPropertyName("timestamp")]
		public long Timestamp { get; set; }

		[JsonPropertyName("callback_id")]
		public string CallbackId { get; set; } = string.Empty;

		[JsonPropertyName("payload")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Payload { get; set; }

		[JsonPropertyName("user")]
		public User User { get; set; } = new();
	}
}
