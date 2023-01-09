using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day06 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 06;

	public string Name => $"Day 06";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day06Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day06(ILogger logger)
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

		var result = ProcessData(data, part1: true);

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessData(data, part1: false);

		return result.ToString();
	}

	#endregion Solvers

	private List<char[]> LoadDataFromInput(string input)
	{
		var data = new List<char[]>();

		InputHelper.TraverseInputLines(input, line =>
		{
			data.Add(line.ToCharArray());
		});
		
		return data;
	}

	private string ProcessData(List<char[]> data, bool part1)
	{
		var message = new StringBuilder();

		for (int col = 0; col < data[0].Length; col++)
		{
			var counts = new Dictionary<char, int>();

			for (int row = 0; row < data.Count; row++)
			{
				char c = data[row][col];
				if (!counts.TryGetValue(c, out var count))
					count = 0;
				counts[c] = count + 1;
			}

			var ordered = part1
				? counts.OrderByDescending(c => c.Value).ThenBy(c => c.Key).ToList()
				: counts.OrderBy(c => c.Value).ThenBy(c => c.Key).ToList();
			var first = ordered.FirstOrDefault();
			message.Append(first.Key);

			logger.SendDebug(nameof(Day06), $"{message,-10} <== {string.Join(",", ordered)}");
		}

		return message.ToString();
	}
}
