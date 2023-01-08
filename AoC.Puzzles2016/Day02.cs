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
public class Day02 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 02;

	public string Name => $"Day 02";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day02Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day02(ILogger logger)
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
		var data = new List<string>();

		InputHelper.TraverseInputLines(input, line =>
		{
			data.Add(line);
		});
		
		return data;
	}

	private readonly char[,] keypad1 = new[,]
	{
		{ '1', '2', '3' },
		{ '4', '5', '6' },
		{ '7', '8', '9' }
	};

	private string ProcessDataForPart1(List<string> data)
	{
		var result = new StringBuilder();

		int row = 1;
		int col = 1;

		foreach (var line in data)
		{
			foreach (char c in line)
			{
				switch (c)
				{
					case 'U': if (row > 0) row--; break;
					case 'D': if (row < 2) row++; break;
					case 'L': if (col > 0) col--; break;
					case 'R': if (col < 2) col++; break;
				}
			}

			result.Append(keypad1[row, col]);
		}

		return result.ToString();
	}

	private readonly char[,] keypad2 = new[,]
	{
		{ ' ', ' ', '1', ' ', ' ' },
		{ ' ', '2', '3', '4', ' ' },
		{ '5', '6', '7', '8', '9' },
		{ ' ', 'A', 'B', 'C', ' ' },
		{ ' ', ' ', 'D', ' ', ' ' }
	};

	private string ProcessDataForPart2(List<string> data)
	{
		var result = new StringBuilder();

		int row = 2;
		int col = 0;

		foreach (var line in data)
		{
			foreach (char c in line)
			{
				var (newRow, newCol) = c switch
				{
					'U' => (row - 1, col),
					'D' => (row + 1, col),
					'L' => (row, col - 1),
					'R' => (row, col + 1),
					_ => (row, col),
				};

				if (newRow < 0 || newRow > 4 || newCol < 0 || newCol > 4)
					continue;

				if (keypad2[newRow, newCol] == ' ')
					continue;
				
				(row, col) = (newRow, newCol);
			}

			result.Append(keypad2[row, col]);
		}

		return result.ToString();
	}
}
