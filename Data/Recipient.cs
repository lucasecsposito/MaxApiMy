using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	public class Recipient
	{
		[JsonPropertyName("chat_id")]
		//[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public long ChatId { get; set; }

		[JsonPropertyName("chat_type")]
		public ChatType ChatType { get; set; }

		[JsonPropertyName("user_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? UserId { get; set; }
	}
}
