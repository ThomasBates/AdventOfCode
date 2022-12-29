using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day06 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

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
		LoadDataFromInput(input, true);

		var result = CountTheLights();

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInput(input, false);

		var result = CountTheLights();

		return result;
	}

	#endregion Solvers

	private readonly int[,] grid = new int[1000, 1000];

	private void LoadDataFromInput(string input, bool part1)
	{
		//  First Clear Data
		for (int x = 0; x < grid.GetLength(0); x++)
			for (int y = 0; y < grid.GetLength(1); y++)
				grid[x, y] = 0;

		int line = 0;
		GrammarHelper.ParseInput(null, input, Resources.Day06Grammar,
			null,
			null,
			(token, valueStack) =>
			{
				switch (token)
				{
					case "c_turnon":
						{
							line++;
							int y2 = int.Parse(valueStack.Pop());
							int x2 = int.Parse(valueStack.Pop());
							int y1 = int.Parse(valueStack.Pop());
							int x1 = int.Parse(valueStack.Pop());

							if (x2 < x1)
							{
								logger.SendWarning(nameof(Day06), $"line {line}: swapped {x2} and {x1}");
								(x1, x2) = (x2, x1);
							}

							if (y2 < y1)
							{
								logger.SendWarning(nameof(Day06), $"line {line}: swapped {y2} and {y1}");
								(y1, y2) = (y2, y1);
							}

							for (int x = x1; x <= x2; x++)
								for (int y = y1; y <= y2; y++)
									if (part1)
										grid[x, y] = 1;
									else
										grid[x, y]++;
						}
						break;
					case "c_turnoff":
						{
							line++;
							int y2 = int.Parse(valueStack.Pop());
							int x2 = int.Parse(valueStack.Pop());
							int y1 = int.Parse(valueStack.Pop());
							int x1 = int.Parse(valueStack.Pop());

							if (x2 < x1)
							{
								logger.SendWarning(nameof(Day06), $"line {line}: swapped {x2} and {x1}");
								(x1, x2) = (x2, x1);
							}

							if (y2 < y1)
							{
								logger.SendWarning(nameof(Day06), $"line {line}: swapped {y2} and {y1}");
								(y1, y2) = (y2, y1);
							}

							for (int x = x1; x <= x2; x++)
								for (int y = y1; y <= y2; y++)
									if (part1)
										grid[x, y] = 0;
									else if (grid[x, y] > 0)
										grid[x, y]--;
						}
						break;
					case "c_toggle":
						{
							line++;
							int y2 = int.Parse(valueStack.Pop());
							int x2 = int.Parse(valueStack.Pop());
							int y1 = int.Parse(valueStack.Pop());
							int x1 = int.Parse(valueStack.Pop());

							if (x2 < x1)
							{
								logger.SendWarning(nameof(Day06), $"line {line}: swapped {x2} and {x1}");
								(x1, x2) = (x2, x1);
							}

							if (y2 < y1)
							{
								logger.SendWarning(nameof(Day06), $"line {line}: swapped {y2} and {y1}");
								(y1, y2) = (y2, y1);
							}

							for (int x = x1; x <= x2; x++)
								for (int y = y1; y <= y2; y++)
									if (part1)
										grid[x, y] = 1 - grid[x, y];
									else
										grid[x, y] += 2;
						}
						break;
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			});

		VisualizeGrid();
	}

	private void VisualizeGrid()
	{
		var line = new StringBuilder();
		for (int y = 0; y < grid.GetLength(1); y++)
		{
			for (int x = 0; x < grid.GetLength(0); x++)
				if (grid[x, y] < 1)
					line.Append(' ');
				else if (grid[x, y] < 27)
					line.Append($"{(char)('a' + grid[x, y] - 1)}");
				else if (grid[x, y] < 53)
					line.Append($"{(char)('A' + grid[x, y] - 27)}");

			logger.SendDebug(nameof(Day06), line.ToString());
			line.Clear();
		}
	}

	private string CountTheLights()
	{
		int count = 0;
		int max = 0;
		for (int x = 0; x < grid.GetLength(0); x++)
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				max = Math.Max(max, grid[x, y]);
				count += grid[x, y];
			}

		logger.SendDebug(nameof(Day06), $"max = {max}");
		logger.SendDebug(nameof(Day06), $"count = {count}");

		return count.ToString();
	}
}
