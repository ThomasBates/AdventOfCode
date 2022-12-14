using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

		public static string GetInputText(int year, int day)
		{
			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AdventOfCode");

			Directory.CreateDirectory(path);

			var filename = Path.Combine(path, $"AoC-Input-{year}-{day:00}.txt");

			if (!File.Exists(filename))
			{
				using var sessionStream = new StreamReader(Path.Combine(path, $"adventofcode.com.session.txt"));
				var session = sessionStream.ReadToEnd();

				GetInputFileAsync(day, year, session, filename).ConfigureAwait(false).GetAwaiter().GetResult();
			}

			if (!File.Exists(filename))
				return string.Empty;

			using var fileStream = new StreamReader(filename);
			var inputText = fileStream.ReadToEnd();

			return inputText;
		}

		//  https://www.reddit.com/r/adventofcode/comments/r64bzw/best_way_of_getting_input_data_c
		//  https://www.reddit.com/r/adventofcode/comments/a2vonl/how_to_download_inputs_with_a_script
		static async Task GetInputFileAsync(int day, int year, string cookie, string filename)
		{
			if (!File.Exists(filename))
			{
				var uri = new Uri("https://adventofcode.com");
				var cookies = new CookieContainer();
				cookies.Add(uri, new Cookie("session", cookie));
				
				using var handler = new HttpClientHandler() { CookieContainer = cookies };
				using var client = new HttpClient(handler) { BaseAddress = uri };
				using var response = await client.GetAsync($"/{year}/day/{day}/input").ConfigureAwait(false);
				using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

				using var file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);

				await stream.CopyToAsync(file).ConfigureAwait(false);
			}
		}
	}
}
