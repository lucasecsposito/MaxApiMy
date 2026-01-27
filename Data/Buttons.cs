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
	public enum ButtonType
	{
		link,
		callback,
		request_contact,
		request_geo_location,
		open_app,
		message
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum Intent
	{
		positive,
		negative,
		//default
	}
	
	[JsonDerivedType(typeof(LinkButton))]
	[JsonDerivedType(typeof(CallbackButton))]
	[JsonDerivedType(typeof(OpenAppButton))]
	[JsonDerivedType(typeof(RequestContactButton))]
	[JsonDerivedType(typeof(RequestGeoLocationButton))]
	public class Button
	{
		[JsonPropertyName("type")]
		public ButtonType Type { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; } = string.Empty;
	}

	public class LinkButton : Button
	{
		[JsonPropertyName("url")]
		public string Url { get; set; } = string.Empty;

		public LinkButton()
		{
			Type = ButtonType.link;
		}

		public LinkButton(string text, string url)
		{
			Type = ButtonType.link;
			Text = text;
			Url = url;
		}
	}

	public class CallbackButton : Button
	{
		[JsonPropertyName("payload")]
		public string Payload { get; set; } = string.Empty;

		[JsonPropertyName("intent")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Intent? Intent { get; set; }

		public CallbackButton()
		{
			Type = ButtonType.callback;
		}

		public CallbackButton(string text, string payload)
		{
			Type = ButtonType.callback;
			Text = text;
			Payload = payload;
		}
	}

	public class OpenAppButton : Button
	{
		[JsonPropertyName("web_app")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? WebApp { get; set; }

		[JsonPropertyName("payload")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Payload { get; set; }

		[JsonPropertyName("contact_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? ContactId { get; set; }
	}

	public class RequestContactButton : Button { }

	public class RequestGeoLocationButton : Button
	{
		[JsonPropertyName("quick")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public bool? Quick { get; set; }
	}
}
