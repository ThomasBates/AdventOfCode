using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day18 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 18;

	public string Name => $"Day 18";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day18Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day18(ILogger logger)
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

		int steps = grid.Count == 6 ? 4 : 100;
		var result = ProcessData(steps, false);

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInput(input);

		int steps = grid.Count == 6 ? 5 : 100;
		var result = ProcessData(steps, true);

		return result;
	}

	#endregion Solvers

	private List<char[]> grid;

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		grid = new List<char[]>();

		InputHelper.TraverseInputLines(input, line =>
		{
			grid.Add(line.ToCharArray());
		});
	}

	private string ProcessData(int steps, bool doCorners)
	{
		if (doCorners)
		{
			grid[0][0] = '#';
			grid[0][grid[0].Length - 1] = '#';
			grid[grid.Count - 1][0] = '#';
			grid[grid.Count - 1][grid[0].Length - 1] = '#';
		}

		for (int step = 0; step < steps; step++)
			grid = DoGridStep(grid, doCorners);

		int result = grid.Sum(row => row.Count(c => c == '#'));
		return result.ToString();
	}

	private List<char[]> DoGridStep(List<char[]> grid, bool doCorners)
	{
		var next = new List<char[]>();

		for (int row = 0; row < grid.Count; row++)
		{
			next.Add(new char[grid[row].Length]);

			for (int col = 0; col < grid[row].Length; col++)
			{
				var cell = grid[row][col];
				int neighbors = 0;
				for (int r = row - 1; r <= row + 1; r++)
				{
					for (int c = col - 1; c <= col + 1; c++)
					{
						if (r == row && c == col)
							continue;
						if (r < 0 || r > grid.Count - 1 ||
							c < 0 || c > grid[r].Length - 1)
							continue;

						if (grid[r][c] == '#')
							neighbors++;
					}
				}

				if (cell == '#')
					next[row][col] = (neighbors == 2 || neighbors == 3) ? '#' : '.';
				else
					next[row][col] = neighbors == 3 ? '#' : '.';
			}
		}

		if (doCorners)
		{
			next[0][0] = '#';
			next[0][grid[0].Length - 1] = '#';
			next[grid.Count - 1][0] = '#';
			next[grid.Count - 1][grid[0].Length - 1] = '#';
		}

		return next;
	}
}
