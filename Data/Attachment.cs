using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	[JsonDerivedType(typeof(InlineKeyboardAttachment))]
	[JsonDerivedType(typeof(PhotoAttachment))]
	[JsonDerivedType(typeof(AudioAttachment))]
	[JsonDerivedType(typeof(VideoAttachment))]
	[JsonDerivedType(typeof(FileAttachment))]
	[JsonDerivedType(typeof(ContactAttachment))]
	[JsonDerivedType(typeof(StickerAttachment))]
	[JsonDerivedType(typeof(LocationAttachment))]
	public class Attachment
	{
		[JsonPropertyName("type")]
		public AttachmentType Type { get; set; }

		//public AttachmentType GetAttachmentType() => Type;
	}

	[JsonDerivedType(typeof(PhotoAttachmentPayload))]
	[JsonDerivedType(typeof(KeyboardAttachmentPayload))]
	[JsonDerivedType(typeof(UploadedInfo))]
	[JsonDerivedType(typeof(MediaAttachmentPayload))]
	[JsonDerivedType(typeof(AttachmentPayload))]
	[JsonDerivedType(typeof(FileAttachmentPayload))]
	[JsonDerivedType(typeof(ContactAttachmentPayload))]
	[JsonDerivedType(typeof(StickerAttachmentPayload))]
	public interface IAttachmentPayload
	{
	}
	/*
	public class PhotoAttachmentPayload : IAttachmentPayload
	{
		[JsonPropertyName("url")]
		public string? Url { get; set; }
		[JsonPropertyName("token")]
		public string? Token { get; set; }
		// TODO :: (AK) https://dev.max.ru/docs-api/objects/NewMessageBody
	}
	*/
	public class UploadedInfo : IAttachmentPayload
	{
		/// <summary>
		/// Токен — уникальный ID загруженного медиафайла
		/// </summary>
		[JsonPropertyName("token")]
		public string? Token { get; set; }
	}

	public class KeyboardAttachmentPayload : IAttachmentPayload
	{
		[JsonPropertyName("buttons")]
		public List<List<Button>> Buttons { get; set; } = new();
	}

	public class AttachmentPayload : IAttachmentPayload
	{
		[JsonPropertyName("url")]
		public string Url { get; set; } = string.Empty;
	}
	/*
	public class AttachmentRequest
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		[JsonPropertyName("type")]
		public AttachmentType Type { get; set; }
		[JsonPropertyName("payload")]
		public IAttachmentPayload Payload { get; set; }
	}
	*/
	/*
	public class Keyboard
	{
		// В Go: [][]ButtonInterface
		// В C#: удобнее хранить как object и включить полиморфную (de)сериализацию при необходимости.
		[JsonPropertyName("buttons")]
		public List<List<object>> Buttons { get; set; } = new();
	}
	*/
	public class InlineKeyboardAttachment : Attachment
	{
		[JsonPropertyName("payload")]
		public KeyboardAttachmentPayload Payload { get; set; } = new();

		public InlineKeyboardAttachment()
		{
			Type = AttachmentType.inline_keyboard;
		}
	}
	/*
	public class InlineKeyboardAttachmentRequest : AttachmentRequest
	{
		[JsonPropertyName("payload")]
		public KeyboardAttachmentRequestPayload Payload { get; set; } = new();

		public static InlineKeyboardAttachmentRequest New(Keyboard payload) =>
			new InlineKeyboardAttachmentRequest { Type = AttachmentType.inline_keyboard, Payload = payload };
	}
	*/
	public class PhotoAttachmentPayload : IAttachmentPayload
	{
		[JsonPropertyName("photo_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? PhotoId { get; set; }

		[JsonPropertyName("token")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Token { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; } = string.Empty;
	}

	public class PhotoAttachment : Attachment
	{
		[JsonPropertyName("payload")]
		public PhotoAttachmentPayload Payload { get; set; } = new();

		public PhotoAttachment()
		{
			Type = AttachmentType.image;
		}
	}

	public class MediaAttachmentPayload : IAttachmentPayload
	{
		[JsonPropertyName("url")]
		public string Url { get; set; } = string.Empty;

		[JsonPropertyName("token")]
		public string Token { get; set; } = string.Empty;
	}

	public class AudioAttachment : Attachment
	{
		[JsonPropertyName("payload")]
		public MediaAttachmentPayload Payload { get; set; } = new();

		public AudioAttachment()
		{
			Type = AttachmentType.audio;
		}
	}
	/*
	public class AudioAttachment : Attachment
	{
		[JsonPropertyName("payload")]
		public UploadedInfo Payload { get; set; } = new();

		public static AudioAttachment New(UploadedInfo payload) =>
			new AudioAttachment { Type = AttachmentType.audio, Payload = payload };
	}
	*/
	public class VideoAttachment : Attachment
	{
		[JsonPropertyName("payload")]
		public MediaAttachmentPayload Payload { get; set; } = new();

		public VideoAttachment()
		{
			Type = AttachmentType.video;
		}
	}
	/*
	public class VideoAttachment : Attachment
	{
		[JsonPropertyName("payload")]
		public UploadedInfo Payload { get; set; } = new();

		public static VideoAttachment New(UploadedInfo payload) =>
			new VideoAttachment { Type = AttachmentType.video, Payload = payload };
	}
	*/
	public class FileAttachmentPayload : IAttachmentPayload
	{
		[JsonPropertyName("url")]
		public string Url { get; set; } = string.Empty;

		[JsonPropertyName("token")]
		public string Token { get; set; } = string.Empty;
	}

	public class FileAttachment : Attachment
	{
		[JsonPropertyName("payload")]
		public FileAttachmentPayload Payload { get; set; } = new();

		[JsonPropertyName("filename")]
		public string Filename { get; set; } = string.Empty;

		[JsonPropertyName("size")]
		public long Size { get; set; }

		public FileAttachment()
		{
			Type = AttachmentType.file;
		}
	}
	/*
	public class FileAttachmentRequest : Attachment
	{
		[JsonPropertyName("payload")]
		public UploadedInfo Payload { get; set; } = new();

		public static FileAttachmentRequest New(UploadedInfo payload) =>
			new FileAttachmentRequest { Type = AttachmentType.file, Payload = payload };
	}
	*/
	public class ContactAttachment : Attachment
	{
		[JsonPropertyName("payload")]
		public ContactAttachmentPayload Payload { get; set; } = new();

		public ContactAttachment()
		{
			Type = AttachmentType.contact;
		}
	}

	public class ContactAttachmentPayload : IAttachmentPayload
	{
		[JsonPropertyName("name")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Name { get; set; }

		[JsonPropertyName("contact_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? ContactId { get; set; }

		[JsonPropertyName("vcf_info")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? VcfInfo { get; set; }

		[JsonPropertyName("vcf_phone")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? VcfPhone { get; set; }

		[JsonPropertyName("max_info")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public User? MaxInfo { get; set; }
	}

	public class StickerAttachmentPayload : IAttachmentPayload
	{
		[JsonPropertyName("url")]
		public string Url { get; set; } = string.Empty;

		[JsonPropertyName("code")]
		public string Code { get; set; } = string.Empty;
	}

	public class StickerAttachment : Attachment
	{
		[JsonPropertyName("payload")]
		public StickerAttachmentPayload Payload { get; set; } = new();

		[JsonPropertyName("width")]
		public int Width { get; set; }

		[JsonPropertyName("height")]
		public int Height { get; set; }

		public StickerAttachment()
		{
			Type = AttachmentType.sticker;
		}
	}

	public class LocationAttachment : Attachment
	{
		[JsonPropertyName("latitude")]
		public double Latitude { get; set; }

		[JsonPropertyName("longitude")]
		public double Longitude { get; set; }

		public LocationAttachment()
		{
			Type = AttachmentType.location;
		}
	}
}
