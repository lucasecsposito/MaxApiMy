using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	public class MessageBody
	{
		[JsonPropertyName("mid")]
		public string Mid { get; set; } = string.Empty;

		[JsonPropertyName("seq")]
		public long Seq { get; set; }

		[JsonPropertyName("text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Text { get; set; }

		// В Go: []json.RawMessage + []interface{} Attachments
		// В C#: сохраняем "сырые" JSON элементы. При желании можно распарсить по field "type".
		[JsonPropertyName("attachments")]
		public List<JsonElement> RawAttachments { get; set; } = new();

		[JsonIgnore]
		public List<Attachment> Attachments { get; set; } = new();

		[JsonPropertyName("reply_to")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? ReplyTo { get; set; }

		[JsonPropertyName("markup")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<MarkUp>? Markups { get; set; }
	}
}
