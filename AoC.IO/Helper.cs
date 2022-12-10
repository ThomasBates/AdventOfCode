using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.IO
{
    public class Helper
    {
		public static Stream GenerateStreamFromString(string s)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		public static void TraverseInputLines(string input, Action<string> action, bool ignoreEmptyLines = true)
		{
			using (var stream = GenerateStreamFromString(input))
			using (var reader = new StreamReader(stream))
			{
				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine();
					if (ignoreEmptyLines && String.IsNullOrWhiteSpace(line))
					{
						continue;
					}

					action(line);
				}
			}
		}

		public static void TraverseInputTokens(string input, Action<string> action)
		{
			var stream = GenerateStreamFromString(input);
			var reader = new StreamReader(stream);
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine();
				if (String.IsNullOrWhiteSpace(line))
				{
					continue;
				}

				string[] values = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string value in values)
				{
					if (String.IsNullOrWhiteSpace(value))
					{
						continue;
					}
					action(value);
				}
			}
		}
	}
}
