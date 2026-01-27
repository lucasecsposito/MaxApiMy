using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using MaxApiMy.Data;

namespace MaxApiMy.Common
{
	public static class MessageHelper
	{
		/// <summary>
		/// Вложения, у которых есть token
		/// </summary>
		private static AttachmentType[] _tokenAttachments = new AttachmentType[] { AttachmentType.image, AttachmentType.video, AttachmentType.audio, AttachmentType.file, AttachmentType.share };
		public static PhotoAttachment GetPhotoAttachment(this Message message)
		{
			if (message == null || message.Body == null || message.Body.Attachments == null || message.Body.Attachments.FirstOrDefault(at => at.Type == AttachmentType.image) == null)
				return null;
			return message.Body.Attachments.FirstOrDefault(at => at.Type == AttachmentType.image) as PhotoAttachment;
		}

		public static AttachmentType GetAttachmentType(this Message message)
		{
			if (message == null || message.Body == null || message.Body.Attachments == null || message.Body.Attachments.FirstOrDefault(at => at.Type != AttachmentType.inline_keyboard) == null)
				return AttachmentType.text;
			return message.Body.Attachments.FirstOrDefault(at => at.Type != AttachmentType.inline_keyboard).Type;
		}
		/// <summary>
		/// У сообщения есть кнопки?
		/// </summary>
		public static bool HasKeyboard(this Message message)
		{
			if (message == null || message.Body == null || message.Body.Attachments == null)
				return false;
			return message.Body.Attachments.Count(at => at.Type == AttachmentType.inline_keyboard) > 0;
		}
		/// <summary>
		/// В сообщении есть Фото?
		/// </summary>
		public static bool HasPhoto(this Message message)
		{
			if (message == null || message.Body == null || message.Body.Attachments == null)
				return false;
			return message.Body.Attachments.Count(at => at.Type == AttachmentType.image) > 0;
		}
		/// <summary>
		/// В сообщении есть фото?
		/// </summary>
		public static bool HasVideo(this Message message)
		{
			if (message == null || message.Body == null || message.Body.Attachments == null)
				return false;
			return message.Body.Attachments.Count(at => at.Type == AttachmentType.video) > 0;
		}
		/// <summary>
		/// В сообщении есть аудио-файл?
		/// </summary>
		public static bool HasAudio(this Message message)
		{
			if (message == null || message.Body == null || message.Body.Attachments == null)
				return false;
			return message.Body.Attachments.Count(at => at.Type == AttachmentType.audio) > 0;
		}
		/// <summary>
		/// В сообщении есть документ?
		/// </summary>
		public static bool HasFile(this Message message)
		{
			if (message == null || message.Body == null || message.Body.Attachments == null)
				return false;
			return message.Body.Attachments.Count(at => at.Type == AttachmentType.file) > 0;
		}
		/// <summary>
		/// В сообщении есть контакт?
		/// </summary>
		public static bool HasContact(this Message message)
		{
			if (message == null || message.Body == null || message.Body.Attachments == null)
				return false;
			return message.Body.Attachments.Count(at => at.Type == AttachmentType.contact) > 0;
		}
		/// <summary>
		/// В сообщении есть стикер?
		/// </summary>
		public static bool HasSticker(this Message message)
		{
			if (message == null || message.Body == null || message.Body.Attachments == null)
				return false;
			return message.Body.Attachments.Count(at => at.Type == AttachmentType.sticker) > 0;
		}
		/// <summary>
		/// В сообщении есть share?
		/// </summary>
		public static bool HasShare(this Message message)
		{
			if (message == null || message.Body == null || message.Body.Attachments == null)
				return false;
			return message.Body.Attachments.Count(at => at.Type == AttachmentType.share) > 0;
		}
		/// <summary>
		/// В сообщении есть локация?
		/// </summary>
		public static bool HasLocation(this Message message)
		{
			if (message == null || message.Body == null || message.Body.Attachments == null)
				return false;
			return message.Body.Attachments.Count(at => at.Type == AttachmentType.location) > 0;
		}
		/// <summary>
		/// Возвращает token фото, видео, аудио, документа или share
		/// </summary>
		public static string GetAttachmentToken(this Message message)
		{
			if (message == null || message.Body == null || message.Body.Attachments == null || message.Body.Attachments.Count(at => _tokenAttachments.Contains(at.Type)) == 0)
				return null;
			var attachment = message.Body.Attachments.FirstOrDefault(at => _tokenAttachments.Contains(at.Type));
			switch (attachment.Type)
			{
				case AttachmentType.image:
					return (attachment as PhotoAttachment).Payload.Token;
				case AttachmentType.video:
					return (attachment as VideoAttachment).Payload.Token;
				case AttachmentType.audio:
					return (attachment as AudioAttachment).Payload.Token;
				case AttachmentType.file:
					return (attachment as FileAttachment).Payload.Token;
				//case AttachmentType.share:
				//	return (attachment as shareAttachment).Payload.Token;
			}
			return null;
		}
		/// <summary>
		/// Сообщение старше, чем Х секунд?
		/// </summary>
		public static bool IsOlder(this Message message, int seconds)
		{
			return DateTime.UtcNow.Subtract(message.Timestamp.DateTimeFromUnixTime()).TotalSeconds > seconds;
		}
	}
}
