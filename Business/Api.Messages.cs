using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using static System.Net.WebRequestMethods;
using MaxApiMy.Common;
using MaxApiMy.Data;
using Common;

namespace MaxApiMy.Business
{
	public class ApiMessages
	{
		/// <summary>
		/// Отправка сообщения
		/// </summary>
		public async Task<Message> AddAsync(long userId, long chatId, string json)
		{
			string query = "";
			if (userId != 0)
				query += $"user_id={userId}&";
			if (chatId != 0)
				query += $"chat_id={chatId}&";
			query += $"disable_link_preview={Api.DisableLinkPreview}";
			// NOTE :: (AK) если картинка не прочитается, то будет "400 Bad Request"
			var url = $"https://platform-api.max.ru/messages?{query}";
			var response = await Api.HttpClient.PostAsync(url, Api.GetContent(json));
			response.WriteLog("Api.Messages.AddAsync", url, json);
			string data = await response.Content.ReadAsStringAsync();
			MessageResponse mr = JsonSerializer.Deserialize<MessageResponse>(data, Api.JsonOptions);
			if (mr != null && mr.Message != null && mr.Message.Body != null && mr.Message.Body.RawAttachments != null)
				mr.Message.Body.Attachments = ConvertRawAttachments(mr.Message.Body.RawAttachments);
			return mr.Message;
		}

		/// <summary>
		/// Редактирование сообщения
		/// </summary>
		public async Task<SimpleQueryResult> EditAsync(string messageId, string json)
		{
			string query = "";
			if (!string.IsNullOrEmpty(messageId))
				query += $"message_id={messageId}";
			var url = $"https://platform-api.max.ru/messages?{query}";
			var response = await Api.HttpClient.PutAsync(url, Api.GetContent(json));
			response.WriteLog("Api.Messages.EditAsync", url, json);
			string data = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<SimpleQueryResult>(data, Api.JsonOptions);
		}

		/// <summary>
		/// Удаление сообщения
		/// </summary>
		public async Task<SimpleQueryResult> DeleteAsync(string messageId)
		{
			string query = "";
			if (!string.IsNullOrEmpty(messageId))
				query += $"message_id={messageId}";
			var url = $"https://platform-api.max.ru/messages?{query}";
			var response = await Api.HttpClient.DeleteAsync(url);
			response.WriteLog("Api.Messages.DeleteAsync", url, "");
			string data = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<SimpleQueryResult>(data, Api.JsonOptions);
		}

		/// <summary>
		/// Получение сообщения
		/// </summary>
		public async Task<Message> GetAsync(string messageId)
		{
			var url = $"https://platform-api.max.ru/messages/{messageId}";
			//var data = await HttpClient.GetStringAsync(url);
			HttpResponseMessage response = await Api.HttpClient.GetAsync(url);
			string data = await response.Content.ReadAsStringAsync();
			response.WriteLog("Api.Messages.GetAsync", url, data);
			return JsonSerializer.Deserialize<Message>(data, Api.JsonOptions);
		}

		#region Utils
		private List<Attachment> ConvertRawAttachments(IReadOnlyList<JsonElement> rawAttachments)
		{
			var result = new List<Attachment>();

			foreach (var rawAttachment in rawAttachments)
			{
				//var raw = JsonSerializer.SerializeToUtf8Bytes(el, JsonOptions);
				var attachment = ConvertJsonToAttachment(rawAttachment.ToString());
				result.Add(attachment);
			}

			return result;
		}
		/// <summary>
		/// BytesToProperAttachment converts raw JSON bytes to the appropriate attachment type
		/// </summary>
		public Attachment ConvertJsonToAttachment(string json)
		{
			var baseAttachment = JsonSerializer.Deserialize<Attachment>(json, Api.JsonOptions) ?? new Attachment();
			switch (baseAttachment.Type)
			{
				case AttachmentType.image:
					return JsonSerializer.Deserialize<PhotoAttachment>(json, Api.JsonOptions) ?? new PhotoAttachment();
				case AttachmentType.video:
					return JsonSerializer.Deserialize<VideoAttachment>(json, Api.JsonOptions) ?? new VideoAttachment();
				case AttachmentType.audio:
					return JsonSerializer.Deserialize<AudioAttachment>(json, Api.JsonOptions) ?? new AudioAttachment();
				case AttachmentType.file:
					return JsonSerializer.Deserialize<FileAttachment>(json, Api.JsonOptions) ?? new FileAttachment();
				case AttachmentType.contact:
					return JsonSerializer.Deserialize<ContactAttachment>(json, Api.JsonOptions) ?? new ContactAttachment();
				case AttachmentType.sticker:
					return JsonSerializer.Deserialize<StickerAttachment>(json, Api.JsonOptions) ?? new StickerAttachment();
				//case AttachmentType.share:
				//	return JsonSerializer.Deserialize<____>(json, JsonOptions) ?? new _____();
				case AttachmentType.location:
					return JsonSerializer.Deserialize<LocationAttachment>(json, Api.JsonOptions) ?? new LocationAttachment();
				case AttachmentType.inline_keyboard:
					return JsonSerializer.Deserialize<InlineKeyboardAttachment>(json, Api.JsonOptions) ?? new InlineKeyboardAttachment();
			}
			return baseAttachment;
		}
		#endregion Utils
	}
}
