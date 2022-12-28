using AoC.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AoC.Common;

public class Helper
{
	public static string GetInputText(int year, int day)
	{
		var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AdventOfCode");

		Directory.CreateDirectory(path);

		var filename = Path.Combine(path, $"Puzzle Inputs\\AoC-Input-{year}-12-{day:00}.txt");

		if (!File.Exists(filename))
		{
			using var sessionStream = new StreamReader(Path.Combine(path, $"Session\\adventofcode.com.session.txt"));
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
		using var stream = GenerateStreamFromString(input);
		using var reader = new StreamReader(stream);

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

	public static bool ParseInput(
		ILogger logger,
		string input,
		string grammarFile,
		Action<string, Stack<string>> scopeControllerAction,
		Action<string, Stack<string>> typeCheckerAction,
		Action<string, Stack<string>> codeGeneratorAction)
	{
		var valueStack = new Stack<string>();

		IL2Grammar grammar = new L2Grammar();
		grammar.OnLogMessageEmitted += HandleLogMessageEmitted;

		try
		{
			grammar.ReadGrammarDefinition(grammarFile);
		}
		catch (GrammarException ex)
		{
			logger?.SendError("Grammar", $"{ex.GetType().Name}: {ex.Message}");
			return false;
		}

		IParser parser = new L2Parser(grammar);
		parser.OnValueEmitted += HandleValueEmitted;
		parser.OnTokenEmitted += HandleTokenEmitted;
		parser.OnLogMessageEmitted += HandleLogMessageEmitted;

		try
		{
			parser.Parse(input);
		}
		catch (ParserException ex)
		{
			logger?.SendError("Parser", $"{ex.GetType().Name}: {ex.Message}");
			return false;
		}

		return true;

		void HandleValueEmitted(object sender, ParserEventArgs e)
		{
			valueStack.Push(e.Value);
		}
		void HandleTokenEmitted(object sender, ParserEventArgs e)
		{
			if (string.IsNullOrEmpty(e.Token)) return;

			switch (e.Token[0])
			{
				case 's': scopeControllerAction?.Invoke(e.Token, valueStack); break;
				case 't': typeCheckerAction?.Invoke(e.Token, valueStack); break;
				case 'c': codeGeneratorAction?.Invoke(e.Token, valueStack); break;
				default:
					logger?.SendError("Parser", $"Unknown token: {e.Token}");
					break;
			}
		}
		void HandleLogMessageEmitted(object sender, ParserLogEventArgs e)
		{
			if (logger == null)
				return;

			switch (e.Severity.ToLower())
			{
				case "error":
					logger.SendError(e.Category, e.Message);
					break;
				case "warning":
					logger.SendWarning(e.Category, e.Message);
					break;
				case "info":
					logger.SendInfo(e.Category, e.Message);
					break;
				case "debug":
					logger.SendDebug(e.Category, e.Message);
					break;
				case "verbose":
					logger.SendVerbose(e.Category, e.Message);
					break;
			}
		}
	}

	public static IEnumerable<TNode> FindPath<TNode>(
		IEnumerable<TNode> allNodes,
		Func<TNode, IEnumerable<TNode>> getNeighbors,
		Func<TNode, TNode, double> getDistance,
		TNode source,
		TNode target)
		where TNode : class
	{
		Dictionary<TNode, bool> visited = new();
		Dictionary<TNode, double> distance = new();
		Dictionary<TNode, TNode> prev = new();

		foreach (var node in allNodes)
		{
			visited[node] = false;
			distance[node] = double.MaxValue;
			prev[node] = default;
		}

		distance[source] = 0;
		TNode current = source;

		while (true)
		{
			foreach (var neighbor in getNeighbors(current))
			{
				if (visited[neighbor])
					continue;

				double d = distance[current] + getDistance(current, neighbor);
				if (d < distance[neighbor])
				{
					distance[neighbor] = d;
					prev[neighbor] = current;
				}
			}

			visited[current] = true;

			if (current == target)
			{
				return GetPath(prev, source, target);
			}

			current = GetCurrent(visited, distance);

			if (current == null)
			{
				return null;
			}
		}

		TNode GetCurrent(Dictionary<TNode, bool> visited, Dictionary<TNode, double> distance)
		{
			double minDistance = double.MaxValue;
			TNode result = null;

			foreach (var valve in visited.Keys)
			{
				if (visited[valve])
					continue;

				if (distance[valve] < minDistance)
				{
					minDistance = distance[valve];
					result = valve;
				}
			}

			return result;
		}

		List<TNode> GetPath(Dictionary<TNode, TNode> prev, TNode source, TNode target)
		{
			var path = new List<TNode>();
			TNode current = target;

			while (true)
			{
				if (current == source)
				{
					return path;
				}

				path.Insert(0, current);

				current = prev[current];
				if (current == null)
				{
					return null;
				}
			}
		}
	}
}
