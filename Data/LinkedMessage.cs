using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	public class LinkedMessage
	{
		[JsonPropertyName("type")]
		public MessageLinkType Type { get; set; }

		[JsonPropertyName("sender")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public User? Sender { get; set; }

		[JsonPropertyName("chat_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? ChatId { get; set; }

		[JsonPropertyName("message")]
		public MessageBody Message { get; set; } = new();
	}
}
