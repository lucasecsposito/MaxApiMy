using MaxApiMy.Business;
using MaxApiMy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Common;

namespace MaxApiMy.Common
{
	public static class ApiHelper
	{
		/*
		/// <summary>
		/// Удалить. Тест UserId
		/// </summary>
		public static async Task<Message> SendTextByUserIdAsync(this Api api, long userId, string text, InlineKeyboardAttachment buttons, NewMessageLink reply = null)
		{
			NewMessageBody newMessage = new NewMessageBody
			{
				Text = text,
				Format = Api.DefaultFormat,
			};
			if (buttons != null)
				newMessage.Attachments.Add(buttons);
			if (reply != null)
				newMessage.Link = reply;
			return await api.Messages.AddAsync(userId, 0, JsonSerializer.Serialize(newMessage, Api.JsonOptions));
		}
		*/
		public static async Task<Message> SendTextAsync(this Api api, long chatOrUserId, string text, string buttons = null, string replyId = null)
		{
			var attachment = buttons == null ? null : buttons.ToDoubleList().GetKeyboardAttachment();
			var reply = replyId == null ? null : new NewMessageLink { Type = MessageLinkType.reply, Mid = replyId };
			return await api.SendTextAsync(chatOrUserId, text, attachment, reply);
		}

		public static async Task<Message> SendTextAsync(this Api api, long chatOrUserId, string text, InlineKeyboardAttachment buttons, NewMessageLink reply = null)
		{
			NewMessageBody newMessage = new NewMessageBody
			{
				Text = text,
				Format = Api.DefaultFormat,
			};
			if (buttons != null)
				newMessage.Attachments.Add(buttons);
			if (reply != null)
				newMessage.Link = reply;
			long userId = chatOrUserId > 0 ? chatOrUserId : 0;
			long chatId = chatOrUserId < 0 ? chatOrUserId : 0;
			return await api.Messages.AddAsync(userId, chatId, JsonSerializer.Serialize(newMessage, Api.JsonOptions));
		}

		public static async Task<Message> SendPhotoAsync(this Api api, long chatOrUserId, string text, string photoUrl, string buttons = null, string replyId = null)
		{
			return await api.SendPhotosAsync(chatOrUserId, text, photoUrl.ToList(), buttons, replyId);
		}

		public static async Task<Message> SendPhotoAsync(this Api api, long chatOrUserId, string text, string photoUrl, InlineKeyboardAttachment buttons, NewMessageLink reply = null)
		{
			return await api.SendPhotosAsync(chatOrUserId, text, photoUrl.ToList(), buttons, reply);
		}

		public static async Task<Message> SendPhotosAsync(this Api api, long chatOrUserId, string text, List<string> photoUrls, string buttons = null, string replyId = null)
		{
			var attachment = buttons == null ? null : buttons.ToDoubleList().GetKeyboardAttachment();
			var reply = replyId == null ? null : new NewMessageLink { Type = MessageLinkType.reply, Mid = replyId };
			return await api.SendPhotosAsync(chatOrUserId, text, photoUrls, attachment, reply);
		}

		public static async Task<Message> SendPhotosAsync(this Api api, long chatOrUserId, string text, List<string> photoUrls, InlineKeyboardAttachment buttons, NewMessageLink reply = null)
		{
			NewMessageBody newMessage = new NewMessageBody
			{
				Text = text,
				Format = Api.DefaultFormat,
			};
			foreach (var photoUrl in photoUrls)
				newMessage.Attachments.Add(photoUrl.CreateImageAttachment());
			if (buttons != null)
				newMessage.Attachments.Add(buttons);
			if (reply != null)
				newMessage.Link = reply;
			long userId = chatOrUserId > 0 ? chatOrUserId : 0;
			long chatId = chatOrUserId < 0 ? chatOrUserId : 0;
			return await api.Messages.AddAsync(userId, chatId, JsonSerializer.Serialize(newMessage, Api.JsonOptions));
		}
		/*
		public static Message SendVideo(this Api api, long chatOrUserId, string text, string videoToken, string buttons = null)
		{
			return api.SendVideos(chatOrUserId, text, videoToken.ToList(), buttons);
		}

		public static Message SendVideos(this Api api, long chatOrUserId, string text, List<string> videoTokens, string buttons = null)
		{
			NewMessageBody newMessage = new NewMessageBody
			{
				Text = text,
				Format = Api.DefaultFormat,
			};
			foreach (var videoToken in videoTokens)
				newMessage.Attachments.Add(videoToken.CreateVideoAttachment());
			if (!string.IsNullOrEmpty(buttons))
				newMessage.Attachments.Add(buttons.ToDoubleList().GetKeyboardAttachment());
			return api.Messages_Add(0, chatOrUserId, JsonSerializer.Serialize(newMessage, Api.JsonOptions));
		}
		*/
		/// <summary>
		/// Редактирование сообщения и кнопок.
		/// Редактирует сообщение в чате. Если buttons равно null, то кнопки текущего сообщения не изменяются. Если buttons == пустой список, все кнопки будут удалены
		/// </summary>
		public static async Task<SimpleQueryResult> EditTextAsync(this Api api, string messageId, string text, string buttons = null)
		{
			var attachment = buttons == null ? null : buttons.ToDoubleList().GetKeyboardAttachment(true);
			return await api.EditTextAsync(messageId, text, attachment);
		}

		public static async Task<SimpleQueryResult> EditTextAsync(this Api api, string messageId, string text, InlineKeyboardAttachment buttons)
		{
			NewMessageBody newMessage = new NewMessageBody
			{
				Text = text,
				Format = Api.DefaultFormat,
			};
			if (buttons == null)
				newMessage.Attachments = null;
			else if (buttons?.Payload?.Buttons?.Count == 0)
				newMessage.Attachments = new List<Attachment>();
			else
				newMessage.Attachments.Add(buttons);
			return await api.Messages.EditAsync(messageId, JsonSerializer.Serialize(newMessage, Api.JsonOptions));
		}

		/// <summary>
		/// Редактирование сообщения, фото и кнопок.
		/// Редактирует сообщение в чате. Если buttons равно null, то кнопки текущего сообщения не изменяются. Если buttons == пустой список, все кнопки будут удалены
		/// </summary>
		public static async Task<SimpleQueryResult> EditPhotoAsync(this Api api, string messageId, string text, string photoUrl, string buttons = null)
		{
			return await api.EditPhotosAsync(messageId, text, photoUrl == null ? null : photoUrl.ToList(), buttons);
		}

		public static async Task<SimpleQueryResult> EditPhotoAsync(this Api api, string messageId, string text, string photoUrl, InlineKeyboardAttachment buttons = null)
		{
			return await api.EditPhotosAsync(messageId, text, photoUrl == null ? null : photoUrl.ToList(), buttons);
		}

		/// <summary>
		/// Редактирование сообщения, фото и кнопок.
		/// Редактирует сообщение в чате. Если buttons равно null, то кнопки текущего сообщения не изменяются. Если buttons == пустой список, все кнопки будут удалены
		/// </summary>
		public static async Task<SimpleQueryResult> EditPhotosAsync(this Api api, string messageId, string text, List<string> photoUrls, string buttons = null)
		{
			var attachment = buttons == null ? null : buttons.ToDoubleList().GetKeyboardAttachment(true);
			return await api.EditPhotosAsync(messageId, text, photoUrls, attachment);
		}

		public static async Task<SimpleQueryResult> EditPhotosAsync(this Api api, string messageId, string text, List<string> photoUrls, InlineKeyboardAttachment buttons = null)
		{
			NewMessageBody newMessage = new NewMessageBody
			{
				Text = text,
				Format = Api.DefaultFormat,
			};
			if (buttons == null && photoUrls == null)
				newMessage.Attachments = null;
			else if (buttons?.Payload?.Buttons?.Count == 0)
				newMessage.Attachments = new List<Attachment>();
			else if (buttons != null)
				newMessage.Attachments.Add(buttons);
			if (photoUrls != null)
				foreach (var photoUrl in photoUrls)
					newMessage.Attachments.Add(photoUrl.CreateImageAttachment());
			return await api.Messages.EditAsync(messageId, JsonSerializer.Serialize(newMessage, Api.JsonOptions));
		}

		/// <summary>
		/// Удаление сообщения
		/// </summary>
		public static async Task<SimpleQueryResult> MessageDeleteAsync(this Api api, string messageId)
		{
			return await api.Messages.DeleteAsync(messageId);
		}

		/// <summary>
		/// Получение сообщения
		/// </summary>
		public static async Task<Message> MessageGetAsync(this Api api, string messageId)
		{
			return await api.Messages.GetAsync(messageId);
		}

		/// <summary>
		/// Ответ на callback текстом.
		/// Этот метод используется для отправки ответа после того, как пользователь нажал на кнопку. Ответом может быть обновленное сообщение и/или одноразовое уведомление для пользователя
		/// </summary>
		public static async Task<SimpleQueryResult> AnswerTextAsync(this Api api, string callbackId, string text, string buttons = null, string notification = null)
		{
			NewMessageBody newMessage = new NewMessageBody
			{
				Text = text,
				Format = Api.DefaultFormat,
			};
			if (buttons == null)
				newMessage.Attachments = null;
			else if (buttons == "")
				newMessage.Attachments = new List<Attachment>();
			else
				newMessage.Attachments.Add(buttons.ToDoubleList().GetKeyboardAttachment());
			CallbackAnswer answer = new CallbackAnswer { Message = newMessage, Notification = notification };
			return await api.AnswersAsync(callbackId, JsonSerializer.Serialize(answer, Api.JsonOptions));
		}

		/// <summary>
		/// Ответ на callback текстом.
		/// Этот метод используется для отправки ответа после того, как пользователь нажал на кнопку. Ответом может быть одноразовое уведомление для пользователя.
		/// NOTE :: (AK) оповещение в мессенджере не отображается, хотя от сервера приходят ответ Success
		/// </summary>
		public static async Task<SimpleQueryResult> AnswerNotificationAsync(this Api api, string callbackId, string notification)
		{
			CallbackAnswer answer = new CallbackAnswer { Message = null, Notification = notification };
			return await api.AnswersAsync(callbackId, JsonSerializer.Serialize(answer, Api.JsonOptions));
		}

		/// <summary>
		/// Ответ на callback текстом.
		/// Этот метод используется для отправки ответа после того, как пользователь нажал на кнопку. Ответом может быть обновленное сообщение и/или одноразовое уведомление для пользователя
		/// </summary>
		public static async Task<SimpleQueryResult> AnswerPhotoAsync(this Api api, string callbackId, string text, string photoUrl, string buttons = null, string notification = null)
		{
			return await api.AnswerPhotosAync(callbackId, text, photoUrl == null ? null : photoUrl.ToList(), buttons, notification);
		}

		/// <summary>
		/// Ответ на callback текстом.
		/// Этот метод используется для отправки ответа после того, как пользователь нажал на кнопку. Ответом может быть обновленное сообщение и/или одноразовое уведомление для пользователя
		/// </summary>
		public static async Task<SimpleQueryResult> AnswerPhotosAync(this Api api, string callbackId, string text, List<string> photoUrls, string buttons = null, string notification = null)
		{
			NewMessageBody newMessage = new NewMessageBody
			{
				Text = text,
				Format = Api.DefaultFormat,
			};
			if (buttons == null && photoUrls == null)
				newMessage.Attachments = null;
			else if (buttons == "")
				newMessage.Attachments = new List<Attachment>();
			else if (buttons != null)
				newMessage.Attachments.Add(buttons.ToDoubleList().GetKeyboardAttachment());
			if (photoUrls != null)
				foreach (var photoUrl in photoUrls)
					newMessage.Attachments.Add(photoUrl.CreateImageAttachment());
			CallbackAnswer answer = new CallbackAnswer { Message = newMessage, Notification = notification };
			return await api.AnswersAsync(callbackId, JsonSerializer.Serialize(answer, Api.JsonOptions));
		}
	}
}
