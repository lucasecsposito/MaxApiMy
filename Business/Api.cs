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
	public class Api
	{
		public ApiChats Chats = new ApiChats();
		public ApiMessages Messages = new ApiMessages();

		public static readonly HttpClient HttpClient = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(60)
		};
		public string Token { get; set; }
		public User Bot { get; set; }

		public static JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
		{
			PropertyNameCaseInsensitive = true,
			WriteIndented = true,
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
		};
		public static bool DisableLinkPreview { get; set; } = true;
		public static string DefaultFormat { get; set; } = "html";

		public Api(string token)
		{
			Token = token;
			HttpClient.DefaultRequestHeaders.Add("Authorization", Token);
		}

		/// <summary>
		/// Возвращает информацию о текущем боте, который идентифицируется с помощью токена доступа. Метод возвращает ID бота, его имя и аватар (если есть)
		/// </summary>
		public async Task<User> Me()
		{
			if (Bot == null)
			{
				var url = $"https://platform-api.max.ru/me";
				//var data = await HttpClient.GetStringAsync(url);
				HttpResponseMessage response = await HttpClient.GetAsync(url);
				string data = await response.Content.ReadAsStringAsync();
				response.WriteLog("Api.Me", url, data);
				Bot = JsonSerializer.Deserialize<User>(data, JsonOptions);
			}
			return Bot;
		}

		public async Task<UpdateList> UpdatesAsync()
		{
			var url = "https://platform-api.max.ru/updates";
			//var data = await HttpClient.GetStringAsync(url);
			HttpResponseMessage response = await HttpClient.GetAsync(url);
			string data = await response.Content.ReadAsStringAsync();
			response.WriteLog("Api.UpdatesAsync", url, data, true);
			var list = JsonSerializer.Deserialize<UpdateList>(data, JsonOptions) ?? new UpdateList();
			foreach (var rawUpdate in list.RawUpdates)
			{
				try
				{
					var update = ConvertJsonToUpdate(rawUpdate.ToString());
					list.Updates.Add(update);
				}
				catch (Exception ex)
				{
					CLogger.WriteLog(ELogLevel.ERROR, ex);
				}
			}
			return list;
		}

		/// <summary>
		/// Ответ на callback
		/// Этот метод используется для отправки ответа после того, как пользователь нажал на кнопку. Ответом может быть обновленное сообщение и/или одноразовое уведомление для пользователя
		/// </summary>
		public async Task<SimpleQueryResult> AnswersAsync(string callbackId, string json)
		{
			string query = "";
			if (!string.IsNullOrEmpty(callbackId))
				query += $"callback_id={callbackId}";
			var url = $"https://platform-api.max.ru/answers?{query}";
			var response = await HttpClient.PostAsync(url, GetContent(json));
			response.WriteLog("Api.AnswersAsync", url, json);
			string data = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<SimpleQueryResult>(data, JsonOptions);
		}

		#region Utils
		private Update ConvertJsonToUpdate(string json)
		{
			var baseUpdate = JsonSerializer.Deserialize<Update>(json, JsonOptions) ?? new Update();
			switch (baseUpdate.UpdateType)
			{
				case UpdateType.bot_started:
					return JsonSerializer.Deserialize<BotStartedUpdate>(json, JsonOptions) ?? new Update();
				case UpdateType.bot_added:
					return JsonSerializer.Deserialize<BotAddedToChatUpdate>(json, JsonOptions) ?? new Update();
				case UpdateType.bot_removed:
					return JsonSerializer.Deserialize<BotRemovedFromChatUpdate>(json, JsonOptions) ?? new Update();
				case UpdateType.chat_title_changed:
					return JsonSerializer.Deserialize<ChatTitleChangedUpdate>(json, JsonOptions) ?? new Update();
				case UpdateType.message_callback:
					return JsonSerializer.Deserialize<MessageCallbackUpdate>(json, JsonOptions) ?? new Update();
				case UpdateType.message_created:
					return JsonSerializer.Deserialize<MessageCreatedUpdate>(json, JsonOptions) ?? new Update();
				case UpdateType.message_removed:
					return JsonSerializer.Deserialize<MessageRemovedUpdate>(json, JsonOptions) ?? new Update();
				case UpdateType.message_edited:
					return JsonSerializer.Deserialize<MessageEditedUpdate>(json, JsonOptions) ?? new Update();
				case UpdateType.user_added:
					return JsonSerializer.Deserialize<UserAddedToChatUpdate>(json, JsonOptions) ?? new Update();
				case UpdateType.user_removed:
					return JsonSerializer.Deserialize<UserRemovedFromChatUpdate>(json, JsonOptions) ?? new Update();
			}
			return baseUpdate;
		}

		public static HttpContent GetContent(string json)
		{
			return new StringContent(json, Encoding.UTF8, "application/json");
		}
		#endregion Utils
	}
}
