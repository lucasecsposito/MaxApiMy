using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum MarkupType
	{
		user_mention,
		bot_mention,
		strong,
		emphasized,
		monospaced,
		link,
		strikethrough,
		underline
	}
}
