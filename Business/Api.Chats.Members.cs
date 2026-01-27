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
	public class ApiChatsMembers
	{
		public long ChatId { get; set; }

		public ApiChatsMembers(long chatId)
		{
			ChatId = chatId;
		}

		/// <summary>
		/// Добавление участников в групповой чат
		/// Добавляет участников в групповой чат. Для этого могут потребоваться дополнительные права
		/// </summary>
		public async Task<SimpleQueryResult> AddAsync(string json)
		{
			var url = $"https://platform-api.max.ru/chats/{ChatId}/members";
			var response = await Api.HttpClient.PostAsync(url, Api.GetContent(json));
			response.WriteLog("Api.Messages.AddAsync", url, json);
			string data = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<SimpleQueryResult>(data, Api.JsonOptions);
		}

		/// <summary>
		/// Удаление участника из группового чата
		/// Удаляет участника из группового чата. Для этого могут потребоваться дополнительные права
		/// </summary>
		public async Task<SimpleQueryResult> DeleteAsync(long userId, bool? block = null)
		{
			string query = "";
			query += $"user_id={userId}";
			if (block != null)
				query += $"&block={block}";
			var url = $"https://platform-api.max.ru/chats/{ChatId}/members?{query}";
			var response = await Api.HttpClient.DeleteAsync(url);
			response.WriteLog("Api.Chats.Members.DeleteAsync", url, "");
			string data = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<SimpleQueryResult>(data, Api.JsonOptions);
		}
	}
}
