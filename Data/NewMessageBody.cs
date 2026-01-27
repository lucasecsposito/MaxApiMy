using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	public class NewMessageBody
	{
		[JsonPropertyName("text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Text { get; set; }

		[JsonPropertyName("attachments")]
		public List<Attachment> Attachments { get; set; } = new List<Attachment>();

		[JsonPropertyName("link")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public NewMessageLink? Link { get; set; }

		[JsonPropertyName("notify")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public bool? Notify { get; set; }

		[JsonPropertyName("format")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Format { get; set; }
	}
}
