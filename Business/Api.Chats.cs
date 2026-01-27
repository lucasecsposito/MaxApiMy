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
	public class ApiChats
	{
		//public ApiChatsMembers Members = new ApiChatsMembers();
		public ApiChatsMembers Members(long chatId)
		{
			return new ApiChatsMembers(chatId);
		}

		/// <summary>
		/// Получение информации о групповом чате
		/// Возвращает информацию о групповом чате по его ID
		/// https://dev.max.ru/docs-api/methods/GET/chats/-chatId-
		/// </summary>
		public async Task<Chat> GetAsync(long chatId)
		{
			var url = $"https://platform-api.max.ru/chats/{chatId}";
			//var data = await HttpClient.GetStringAsync(url);
			HttpResponseMessage response = await Api.HttpClient.GetAsync(url);
			string data = await response.Content.ReadAsStringAsync();
			response.WriteLog("Api.Chats.GetAsync", url, data);
			Chat chat = JsonSerializer.Deserialize<Chat>(data, Api.JsonOptions);
			return chat;
		}
		/// <summary>
		/// Получение списка администраторов группового чата
		/// Возвращает список всех администраторов группового чата. Бот должен быть администратором в запрашиваемом чате
		/// https://dev.max.ru/docs-api/methods/GET/chats/-chatId-/members/admins
		/// </summary>
		public async Task<Users> Admins_GetAsync(long chatId)
		{
			var url = $"https://platform-api.max.ru/chats/{chatId}/members/admins";
			try
			{
				//throw new Exception("Моя ошибка");
				//var data = await Api.HttpClient.GetStringAsync(url);
				HttpResponseMessage response = await Api.HttpClient.GetAsync(url);
				string data = await response.Content.ReadAsStringAsync();
				response.WriteLog("Api.Chats.Admins_GetAsync", url, data);
				Users users = JsonSerializer.Deserialize<Users>(data, Api.JsonOptions);
				if (users != null && users.RawMembers != null)
					users.Members = ConvertRawMembers(users.RawMembers);
				return users;
			}
			catch (Exception ex)
			{
				CLogger.WriteLog(ELogLevel.ERROR, $"-> url: {url} {ex}");
			}
			return null;
		}

		#region Utils
		private List<User> ConvertRawMembers(IReadOnlyList<JsonElement> rawMembers)
		{
			var result = new List<User>();
			foreach (var rawMember in rawMembers)
				result.Add(JsonSerializer.Deserialize<User>(rawMember, Api.JsonOptions) ?? new User());
			return result;
		}
		#endregion Utils
	}
}
