using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	public class NewMessageLink
	{
		[JsonPropertyName("type")]
		public MessageLinkType Type { get; set; }

		[JsonPropertyName("mid")]
		public string Mid { get; set; } = string.Empty;
	}
}
