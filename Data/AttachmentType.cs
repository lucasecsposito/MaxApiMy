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
	public enum AttachmentType
	{
		// text добавлен для поддержки работы Box4
		text,
		image,
		video,
		audio,
		file,
		contact,
		sticker,
		share,
		location,
		inline_keyboard

		//public static string Image { get; set; } = "image";
		//public static string Video = "video";
		//public static string Audio = "audio";
		//public static string File = "file";
		//public static string Contact = "contact";
		//public static string Sticker = "sticker";
		//public static string Share = "share";
		//public static string Location = "location";
		//public static string InlineKeyboard = "inline_keyboard";
	}
}
