using Common;
using MaxApiMy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxApiMy.Common
{
	public static class ResponseHelper
	{
		public static void WriteLog(this HttpResponseMessage response, string methodName, string url, string json, bool onlyError = false)
		{
			if (!response.IsSuccessStatusCode)
				CLogger.WriteLog(ELogLevel.ERROR, $"-> {response.StatusCode} {response.RequestMessage.Method} {methodName} url: {url} json: {json}");
			if (response.IsSuccessStatusCode && !onlyError)
				CLogger.WriteLog(ELogLevel.INFO, $"-> {methodName} json: {json.Replace('\r', ' ').Replace('\n', ' ')}".TakeMax(157));
		}
	}
}
