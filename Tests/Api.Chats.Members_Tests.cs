using Common;
using MaxApiMy.Business;
using MaxApiMy.Common;
using MaxApiMy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MaxApiMy.Tests
{
	public class ApiChatsMembers_Tests
	{
		public async Task AddAsync()
		{
			var api = new Api("f9LHodD0cOK5Hfc3N_eq0Vbku46BLikMjuawboLuRFsaJ_tbaLkjRFt6s6znDFtbHv6C38c_430qcHDzHoFu");
			//SimpleQueryResult result = await api.Chats.Members(-70406918203993).DeleteAsync(181315262);
			string[] userIds = ["181315262", "4857452", "5193556"];
			var obj = new { user_ids = userIds };
			//SimpleQueryResult result = await api.Chats.Members(-70406918203993).AddAsync(JsonSerializer.Serialize(obj, Api.JsonOptions));
			SimpleQueryResult result = await api.Chats.Members(-70504365908158).AddAsync(JsonSerializer.Serialize(obj, Api.JsonOptions));
			Console.WriteLine(result.Success);
		}
	}
}
