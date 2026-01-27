using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	public class UpdateList
	{
		[JsonPropertyName("updates")]
		public List<JsonElement> RawUpdates { get; set; } = new();

		[JsonIgnore]
		public List<Update> Updates { get; set; } = new();

		[JsonPropertyName("marker")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? Marker { get; set; }
	}
}
