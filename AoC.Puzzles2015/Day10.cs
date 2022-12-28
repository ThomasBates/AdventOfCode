using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using AoC.Common;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day10 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 10;

	public string Name => $"Day 10";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day10Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day10(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessData(40);

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessData(50);

		return result;
	}

	#endregion Solvers

	private List<string> lines = new();

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		lines.Clear();

		Helper.TraverseInputLines(input, line =>
		{
			lines.Add(line);
		});
	}

	private string ProcessData(int count)
	{
		int result = 0;

		foreach (var line in lines)
		{
			if (lines.Count > 1)
			{
				var newLine = LookAndSay(line);
				logger.SendDebug(nameof(Day10), $"{line} => {newLine}");
				continue;
			}

			var text = line;
			for (int i = 0; i < count; i++)
			{
				var newText = LookAndSay(text);
				logger.SendDebug(nameof(Day10), $"{i + 1,-2}: {text.Length} => {newText.Length} ({(newText.Length * 1.0 / text.Length)})");
				text = newText;
			}
			result = text.Length;
		}

		return result.ToString();
	}

	private string LookAndSay(string line)
	{
		var result = new StringBuilder();
		var count = 1;
		char last = line[0];
		for (int i = 1; i < line.Length; i++)
		{
			char c = line[i];
			if (c == last)
			{
				count++;
			}
			else
			{
				result.Append($"{count}{last}");
				last = c;
				count = 1;
			}
		}
		result.Append($"{count}{last}");
		return result.ToString();
	}
}
