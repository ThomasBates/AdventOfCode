using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day25 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 25;

	public string Name => $"Day 25";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day25Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day25(ILogger logger)
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

	private (int, int) LoadDataFromInput(string input)
	{
		//  First Clear Data
		int row = 0;
		int col = 0;

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(' ');
			row = int.Parse(parts[16].Substring(0, parts[16].Length - 1));
			col = int.Parse(parts[18].Substring(0, parts[18].Length - 1));
		});

		return (row, col);
	}

	private object ProcessDataForPart1((int row, int col) data)
	{
		var codeIndex = 0;
		int step = 0;
		for (int c = 0; c < data.col; c++)
		{
			step++;
			codeIndex += step;
		}

		for (int r = 1; r < data.row; r++)
		{
			codeIndex += step;
			step++;
		}
		var codeInterval = codeIndex / 100;
		if (codeInterval == 0)
			codeInterval = 1;
		int percent = 0;

		logger.SendDebug(nameof(Day25), $"codeIndex = {codeIndex}, codeInterval = {codeInterval}");

		long code = 20151125;

		for (int i = 2; i <= codeIndex; i++)
		{
			code = (code * 252533) % 33554393;

			if (i % codeInterval == 0)
				logger.SendDebug(nameof(Day25), $"code {i} = {code} ({++percent}%)");
		}

		return code;
	}

	private object ProcessDataForPart2(object data)
	{
		return "Merry Christmas";
	}
}
