using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	public class MessageStat
	{
		[JsonPropertyName("views")]
		public int Views { get; set; }
	}
}
