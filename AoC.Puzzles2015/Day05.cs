using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AoC.Common;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day05 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 05;

	public string Name => $"Day 05";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day05Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day05(ILogger logger)
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

		var result = ProcessDataForPart1();

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataForPart2();

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

	private string ProcessDataForPart1()
	{
		int count = 0;
		
		foreach (var line in lines)
		{
			var test1 = line.Count(c => "aeiou".Contains(c)) >= 3;
			var test2 = false;
			for(int i=1; i < line.Length; i++)
			{
				if (line[i - 1] == line[i])
				{
					test2 = true;
					break;
				}
			}
			var test3 = !(
				line.Contains("ab") ||
				line.Contains("cd") ||
				line.Contains("pq") ||
				line.Contains("xy"));
			logger.Send(SeverityLevel.Debug, nameof(Day05), $"{line,-16} - {(test1 ? " " : "x")}{(test2 ? " " : "x")}{(test3 ? " " : "x")} - {(test1 && test2 && test3 ? "nice" : "naughty")}");

			if (test1 && test2 && test3)
				count++;
		}

		return count.ToString();
	}

	private string ProcessDataForPart2()
	{
		int count = 0;

		foreach (var line in lines)
		{
			var test1 = false;
			for (int i = 0; i < line.Length - 3; i++)
			{
				for (int j = i + 2; j < line.Length - 1; j++)
				{
					if (line[i] == line[j] && line[i + 1] == line[j+1])
					{
						test1 = true;
						break; 
					}
				}
			}

			var test2 = false;
			for (int i = 2; i < line.Length; i++)
			{
				if (line[i - 2] == line[i])
				{
					test2 = true;
					break;
				}
			}

			logger.Send(SeverityLevel.Debug, nameof(Day05), $"{line,-16} - {(test1 ? " " : "x")}{(test2 ? " " : "x")} - {(test1 && test2 ? "nice" : "naughty")}");

			if (test1 && test2)
				count++;
		}

		return count.ToString();
	}
}
