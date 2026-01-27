using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	public class MarkUp
	{
		[JsonPropertyName("from")]
		public int From { get; set; }

		[JsonPropertyName("length")]
		public int Length { get; set; }

		[JsonPropertyName("user_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? UserId { get; set; }

		[JsonPropertyName("type")]
		public MarkupType Type { get; set; }
	}
}
