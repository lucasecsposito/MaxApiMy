using Common;
using MaxApiMy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxApiMy.Common
{
	public static class TaskExtensions
	{
		public static async void FireAndForget(this Task task, Action<Exception>? onError = null)
		{
			try
			{
				await task;
			}
			catch (Exception ex)
			{
				//CLogger.WriteLog(ELogLevel.ERROR, $"ProcessCallbackInGroup Bot: @{Bot.Username} {Bot.Name} ChatId: {ChatId} Ex: {ex}");
				Console.WriteLine(ex);
				//onError?.Invoke(ex);
			}
		}
	}
}
