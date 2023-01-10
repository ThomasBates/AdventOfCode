using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day09 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 09;

	public string Name => $"Day 09";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs (1)", Resources.Day09Inputs01},
		{"Example Inputs (2)", Resources.Day09Inputs02},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day09(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart1(data);

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart2(data);

		return result.ToString();
	}

	#endregion Solvers

	private List<string> LoadDataFromInput(string input)
	{
		var lines = new List<string>();

		InputHelper.TraverseInputLines(input, line =>
		{
			lines.Add(line);
		});
		
		return lines;
	}

	private int ProcessDataForPart1(List<string> lines)
	{
		int count = 0;

		foreach (var line in lines)
		{
			var newLine = new StringBuilder();
			int pos = 0;
			while (pos < line.Length)
			{
				char c = line[pos];
				if (c != '(')
				{
					newLine.Append(c);
					pos++;
					continue;
				}

				var x = line.IndexOf("x", pos);
				var close = line.IndexOf(')', pos);
				var numChars = int.Parse(line.Substring(pos + 1, x - pos - 1));
				var repeat = int.Parse(line.Substring(x + 1, close - x - 1));
				var segment = line.Substring(close + 1, numChars);
				for (int i = 0; i < repeat; i++)
					newLine.Append(segment);
				pos = close + numChars + 1;
			}

			if (line.Length < 100)
				logger.SendDebug(nameof(Day09), $"{line} => {newLine}");

			count = newLine.Length;
		}

		return count;
	}

	private long ProcessDataForPart2(List<string> lines)
	{
		long count = 0;

		foreach (var line in lines)
		{

			if (line.Length < 100)
				logger.SendDebug(nameof(Day09), line);

			// preprocess line
			var list = new LinkedList<(long length, int count, int repeat)>();
			int pos = 0;
			while (pos < line.Length)
			{
				var open = line.IndexOf('(', pos);
				if (open < 0)
				{
					list.AddLast((line.Length - pos, -1, -1));
					break;
				}
				if (open > pos)
				{
					list.AddLast((open - pos, -1, -1));
				}
				var x = line.IndexOf("x", open);
				var close = line.IndexOf(')', open);
				var numChars = int.Parse(line.Substring(open + 1, x - open - 1));
				var repeat = int.Parse(line.Substring(x + 1, close - x - 1));
				list.AddLast((close - open + 1, numChars, repeat));
				pos = close + 1;
			}

			if (line.Length < 100)
				logger.SendDebug(nameof(Day09), $"  => {string.Join("", list)}");
			else
				logger.SendDebug(nameof(Day09), $"  => list.Count = {list.Count}");
			var updateTime = DateTime.Now.AddSeconds(10);

			while (list.Count > 1)
			{
				if (DateTime.Now > updateTime)
				{
					logger.SendDebug(nameof(Day09), $"  => list.Count = {list.Count}");
					updateTime = DateTime.Now.AddSeconds(10);
				}

				var first = list.First;
				var marker = first.Value.count >= 0 ? first : first.Next;

				var sublist = new LinkedList<(long length, int count, int repeat)>();
				var next = marker.Next;

				var length = 0L;
				while (length < marker.Value.count && next != null)
				{
					if (length + next.Value.length <= marker.Value.count)
					{

						sublist.AddLast(next.Value);
						length += next.Value.length;
						next = next.Next;
					}
					else if (next.Value.count < 0) // !marker
					{
						next.Value = (next.Value.length - (marker.Value.count - length), -1, -1);
						sublist.AddLast((marker.Value.count - length, -1, -1));
						length = marker.Value.count;
					}
					else // marker
					{
						throw new Exception();
					}
				}

				if (next == null)
				{
					list.Clear();
				}
				else
				{
					while (next.Previous != null)
					{
						list.Remove(next.Previous);
					}
				}

				for (int i = 0; i < marker.Value.repeat; i++)
				{
					for (var bla = sublist.Last; bla != null; bla = bla.Previous)
					{
						if (list.Count == 0 || list.First.Value.count >= 0 || bla.Value.count >= 0)
							list.AddFirst(bla.Value);
						else
							list.First.Value = (list.First.Value.length + bla.Value.length, -1, -1);
					}
				}
				if (first != marker)
				{
					if (list.Count == 0 || list.First.Value.count >= 0)
						list.AddFirst(first.Value);
					else
						list.First.Value = (list.First.Value.length + first.Value.length, -1, -1);
				}
			}

			logger.SendDebug(nameof(Day09), $"    => {string.Join("", list)}");

			count = list.First.Value.length;
		}

		return count;
	}
}
