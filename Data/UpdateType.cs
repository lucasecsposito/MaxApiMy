using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum UpdateType
	{
		bot_started,
		bot_added,
		bot_removed,
		chat_title_changed,
		message_callback,
		message_created,
		message_removed,
		message_edited,
		user_added,
		user_removed,
	}
}
