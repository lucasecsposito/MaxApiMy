using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxApiMy.Data
{
	/// <summary>
	/// MessageResponse represents the response wrapper when a message is sent
	/// </summary>
	public class MessageResponse
	{
		[System.Text.Json.Serialization.JsonPropertyName("message")]
		public Message Message { get; set; } = new();
	}
}
