using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	public class Users
	{
		[JsonPropertyName("members")]
		public List<JsonElement> RawMembers { get; set; } = new();

		[JsonIgnore]
		public List<User> Members { get; set; } = new();

		[JsonPropertyName("marker")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? Marker { get; set; }
	}
}
