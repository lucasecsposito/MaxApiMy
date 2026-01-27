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
	public enum ChatAdminPermission
	{
		read_all_messages,
		add_remove_members,
		add_admins,
		change_chat_info,
		pin_message,
		write,
		can_call,
		edit_link,
		post_edit_delete_message,
		edit_message,
		delete_message
	}
}
