using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

// TODO :: (AK) поместить это в библиотеку Common.dll?
namespace Common
{
	public static class ActionHelper
	{
		/// <summary>
		/// "Желтые,Красные;Черные" -> new {new { "Желтые", "Красные" }, new { "Черные" } }
		/// </summary>
		public static List<List<string>> ToDoubleList(this string text)
		{
			return (text ?? "").Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(cell => cell.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList()).ToList();
		}
		/// <summary>
		/// "Желтые{chr2}Красные{chr1}Черные" -> new {new { "Желтые", "Красные" }, new { "Черные" } }
		/// </summary>
		public static List<List<string>> ToDoubleList(this string text, char chr1, char chr2)
		{
			return (text ?? "").Split(new[] { chr1 }, StringSplitOptions.RemoveEmptyEntries).Select(cell => cell.Split(new[] { chr2 }, StringSplitOptions.RemoveEmptyEntries).ToList()).ToList();
		}
		public static int IndexOf(this List<List<string>> texts, string value)
		{
			var index = 0;
			foreach (var txts in texts)
				foreach (var text in txts)
				{
					if (text == value)
						return index;
					index++;
				}

			return -1;
		}
		/// <summary>
		/// "1,2,3,4,5" => "1,2", "3,4","5"
		/// </summary>
		public static List<List<string>> ToDoubleList(this string[] arr, int size)
		{
			return ToDoubleList(arr.ToList(), size);
		}
		/// <summary>
		/// "1,2,3,4,5" => "1,2", "3,4","5"
		/// </summary>
		public static List<List<string>> ToDoubleList(this List<string> arr, int size)
		{
			return arr.Select((s, i) => arr.Skip(i * size).Take(size).ToList()).Where(a => a.Any()).ToList();
		}
		/// <summary>
		/// "1,2,3,4,5" => "1,2", "3,4","5"
		/// </summary>
		public static List<List<string>> ToDoubleList(this List<int> arr, int size)
		{
			return arr.Select((s, i) => arr.Skip(i * size).Take(size).Select(n => n.ToString()).ToList()).Where(a => a.Any()).ToList();
		}

		public static List<List<string>> ToDoubleList(this List<string> arr)
		{
			return new[] { arr }.ToList();
		}
		/// <summary>
		/// "1,2,3,4,5" => "1,2", "[3],4","5"
		/// </summary>
		public static List<List<string>> ToDoubleList(this string[] arr, int size, string selected)
		{
			return ToDoubleList(arr.ToList(), size, selected);
		}
		/// <summary>
		/// "1,2,3,4,5" => "1,2", "3,4","5"
		/// </summary>
		public static List<List<string>> ToDoubleList(this string text, int size)
		{
			return text.ToList().ToDoubleList(size, Guid.NewGuid().ToString());
		}
		/// <summary>
		/// "1,2,3,4,5" => "1,2", "3,4","5"
		/// </summary>
		public static List<List<string>> ToDoubleList(this string text, char separator, int size)
		{
			return text.ToList(separator).ToDoubleList(size, Guid.NewGuid().ToString());
		}
		/// <summary>
		/// "1,2,3,4,5" => "1,2", "[3],4","5"
		/// </summary>
		public static List<List<string>> ToDoubleList(this List<string> arr, int size, string selected)
		{
			return arr.Select((s, i) => arr.Skip(i * size).Take(size).Select(it => it == selected ? $"[{it}]" : it).ToList()).Where(a => a.Any()).ToList();
		}
		/// <summary>
		/// "1,2,3,4,5" => "1,2", "[3],4","5"
		/// </summary>
		public static List<List<string>> ToDoubleList(this List<int> arr, int size, int selected)
		{
			return arr.Select((s, i) => arr.Skip(i * size).Take(size).Select(n => n.ToString()).Select(it => it == selected.ToString() ? $"[{it}]" : it).ToList()).Where(a => a.Any()).ToList();
		}
		/// <summary>
		/// "1,2,3,4,5" => "1,2", "[3],4","5"
		/// </summary>
		public static List<List<string>> ToDoubleList(this string text, int size, string selected)
		{
			return text.ToList().ToDoubleList(size, selected);
		}
		/// <summary>
		/// "1,2", "[3],4","5" => "1,2;[3],4,5" 
		/// </summary>
		public static string ToDoubleCsv(this List<List<string>> arr)
		{
			var lines = new List<string>();
			foreach (var item in arr)
				lines.Add(string.Join(",", item));
			return string.Join(";", lines);
		}
		/// <summary>
		/// "key1:value1#key2:value2" => [key1,value1], [key2,value2]
		/// </summary>
		public static Dictionary<string, string> ToDictionary(this string text)
		{
			return text.ToDictionary('#', ':');
		}
		/// <summary>
		/// "key1{chr2}value1{chr1}key2{chr2}value2" => [key1,value1], [key2,value2]
		/// </summary>
		public static Dictionary<string, string> ToDictionary(this string text, char chr1, char chr2)
		{
			if (string.IsNullOrEmpty(text))
				return new Dictionary<string, string>();
			var lines = text.Split(chr1);
			Dictionary<string, string> result = new Dictionary<string, string>();
			foreach (var line in lines)
			{
				var cells = line.Split(chr2);
				if (!result.ContainsKey(cells[0]))
					result.Add(cells[0], cells[1]);
			}
			return result;
		}
		public static Dictionary<string, int> ToDictionaryStringInt(this string text, char chr1, char chr2)
		{
			if (string.IsNullOrEmpty(text))
				return new Dictionary<string, int>();
			var lines = text.Split(chr1);
			Dictionary<string, int> result = new Dictionary<string, int>();
			foreach (var line in lines)
			{
				var cells = line.Split(chr2);
				if (!result.ContainsKey(cells[0]))
					result.Add(cells[0], cells[1].GetInt(0));
			}
			return result;
		}
		public static Dictionary<int, string> ToDictionaryIntString(this string text, char chr1, char chr2)
		{
			if (string.IsNullOrEmpty(text))
				return new Dictionary<int, string>();
			var lines = text.Split(chr1);
			Dictionary<int, string> result = new Dictionary<int, string>();
			foreach (var line in lines)
			{
				var cells = line.Split(chr2);
				if (!result.ContainsKey(cells[0].GetInt(0)))
					result.Add(cells[0].GetInt(0), cells[1]);
			}
			return result;
		}
		public static Dictionary<int, int> ToDictionaryIntInt(this string text, char chr1, char chr2)
		{
			if (string.IsNullOrEmpty(text))
				return new Dictionary<int, int>();
			var lines = text.Split(chr1);
			Dictionary<int, int> result = new Dictionary<int, int>();
			foreach (var line in lines)
			{
				var cells = line.Split(chr2);
				if (!result.ContainsKey(cells[0].GetInt(0)))
					result.Add(cells[0].GetInt(0), cells[1].GetInt(0));
			}
			return result;
		}
		public static Dictionary<long, int> ToDictionaryLongInt(this string text, char chr1, char chr2)
		{
			if (string.IsNullOrEmpty(text))
				return new Dictionary<long, int>();
			var lines = text.Split(chr1);
			Dictionary<long, int> result = new Dictionary<long, int>();
			foreach (var line in lines)
			{
				var cells = line.Split(chr2);
				if (!result.ContainsKey(cells[0].GetInt(0)))
					result.Add(cells[0].GetLong(0), cells[1].GetInt(0));
			}
			return result;
		}
		/// <summary>
		/// "key1:value1{chr}key2:value2" => [key1,value1], [key2,value2]
		/// Key1, Key2 всегда фиксированной длины keyLength
		/// </summary>
		public static Dictionary<string, string> ToDictionary(this string text, int keyLength, char chr)
		{
			var lines = text.Split(chr);
			var result = new Dictionary<string, string>();
			foreach (var line in lines)
			{
				var key = line.Substring(0, Math.Min(line.Length, keyLength));
				var value = line.Substring(Math.Min(line.Length, keyLength + 1));
				if (!result.ContainsKey(key))
					result.Add(key, value);
			}
			return result;
		}

		public static string DictionaryToString(this Dictionary<long, int> dic)
		{
			var lines = new List<string>();
			foreach (var item in dic)
				lines.Add(string.Join(",", item));
			return string.Join(";", lines);
		}

		#region ConcurrentDictionary
		/// <summary>
		/// "key1:value1#key2:value2" => [key1,value1], [key2,value2]
		/// </summary>
		public static ConcurrentDictionary<string, string> ToConcurrentDictionary(this string text)
		{
			return text.ToConcurrentDictionary('#', ':');
		}
		/// <summary>
		/// "key1{chr2}value1{chr1}key2{chr2}value2" => [key1,value1], [key2,value2]
		/// </summary>
		public static ConcurrentDictionary<string, string> ToConcurrentDictionary(this string text, char chr1, char chr2)
		{
			if (string.IsNullOrEmpty(text))
				return new ConcurrentDictionary<string, string>();
			var lines = text.Split(chr1);
			ConcurrentDictionary<string, string> result = new ConcurrentDictionary<string, string>();
			foreach (var line in lines)
			{
				var cells = line.Split(chr2);
				if (!result.ContainsKey(cells[0]))
					result.AddUpdate(cells[0], cells[1]);
			}
			return result;
		}
		public static ConcurrentDictionary<string, int> ToConcurrentDictionaryStringInt(this string text, char chr1, char chr2)
		{
			if (string.IsNullOrEmpty(text))
				return new ConcurrentDictionary<string, int>();
			var lines = text.Split(chr1);
			ConcurrentDictionary<string, int> result = new ConcurrentDictionary<string, int>();
			foreach (var line in lines)
			{
				var cells = line.Split(chr2);
				if (!result.ContainsKey(cells[0]))
					result.AddUpdate(cells[0], cells[1].GetInt(0));
			}
			return result;
		}
		public static ConcurrentDictionary<int, string> ToConcurrentDictionaryIntString(this string text, char chr1, char chr2)
		{
			if (string.IsNullOrEmpty(text))
				return new ConcurrentDictionary<int, string>();
			var lines = text.Split(chr1);
			ConcurrentDictionary<int, string> result = new ConcurrentDictionary<int, string>();
			foreach (var line in lines)
			{
				var cells = line.Split(chr2);
				if (!result.ContainsKey(cells[0].GetInt(0)))
					result.AddUpdate(cells[0].GetInt(0), cells[1]);
			}
			return result;
		}
		public static ConcurrentDictionary<int, int> ToConcurrentDictionaryIntInt(this string text, char chr1, char chr2)
		{
			if (string.IsNullOrEmpty(text))
				return new ConcurrentDictionary<int, int>();
			var lines = text.Split(chr1);
			ConcurrentDictionary<int, int> result = new ConcurrentDictionary<int, int>();
			foreach (var line in lines)
			{
				var cells = line.Split(chr2);
				if (!result.ContainsKey(cells[0].GetInt(0)))
					result.AddUpdate(cells[0].GetInt(0), cells[1].GetInt(0));
			}
			return result;
		}
		public static ConcurrentDictionary<long, int> ToConcurrentDictionaryLongInt(this string text, char chr1, char chr2)
		{
			if (string.IsNullOrEmpty(text))
				return new ConcurrentDictionary<long, int>();
			var lines = text.Split(chr1);
			ConcurrentDictionary<long, int> result = new ConcurrentDictionary<long, int>();
			foreach (var line in lines)
			{
				var cells = line.Split(chr2);
				if (!result.ContainsKey(cells[0].GetInt(0)))
					result.AddUpdate(cells[0].GetLong(0), cells[1].GetInt(0));
			}
			return result;
		}
		/// <summary>
		/// "key1:value1{chr}key2:value2" => [key1,value1], [key2,value2]
		/// Key1, Key2 всегда фиксированной длины keyLength
		/// </summary>
		public static ConcurrentDictionary<string, string> ToConcurrentDictionary(this string text, int keyLength, char chr)
		{
			var lines = text.Split(chr);
			var result = new ConcurrentDictionary<string, string>();
			foreach (var line in lines)
			{
				var key = line.Substring(0, Math.Min(line.Length, keyLength));
				var value = line.Substring(Math.Min(line.Length, keyLength + 1));
				if (!result.ContainsKey(key))
					result.AddUpdate(key, value);
			}
			return result;
		}

		public static string ConcurrentDictionaryToString(this ConcurrentDictionary<long, int> dic)
		{
			var lines = new List<string>();
			foreach (var item in dic)
				lines.Add(string.Join(",", item));
			return string.Join(";", lines);
		}
		#endregion ConcurrentDictionary
	}

}
