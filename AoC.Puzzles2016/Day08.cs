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
public class Day08 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 08;

	public string Name => $"Day 08";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day08Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day08(ILogger logger)
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

	private char[,] LoadDataFromInput(string input)
	{
		char[,] grid;

		if (input.Length < 1000)
			grid = new char[7, 3];
		else
			grid = new char[50, 6];

		for (int x = 0; x < grid.GetLength(0); x++)
			for (int y = 0; y < grid.GetLength(1); y++)
				grid[x, y] = '.';

		var ok = GrammarHelper.ParseInput(logger, input, Resources.Day08Grammar,
			null, 
			null,
			(token, valueStack) =>
			{
				switch (token)
				{
					case "c_rect":
						{
							var h = int.Parse(valueStack.Pop());
							var w = int.Parse(valueStack.Pop());
							for (int x = 0; x < w; x++)
								for (int y = 0; y < h; y++)
									grid[x, y] = '#';
						}
						break;
					case "c_rotateRow":
						{
							var shift = int.Parse(valueStack.Pop());
							var y = int.Parse(valueStack.Pop());
							var row = new char[grid.GetLength(0)];
							for (int x = 0; x < grid.GetLength(0); x++)
								row[x] = grid[x, y];
							for (int rowX = 0; rowX < grid.GetLength(0); rowX++)
							{
								var gridX = (rowX + shift) % grid.GetLength(0);
								grid[gridX, y] = row[rowX];
							}
						}
						break;
					case "c_rotateCol":
						{
							var shift = int.Parse(valueStack.Pop());
							var x = int.Parse(valueStack.Pop());
							var col = new char[grid.GetLength(1)];
							for (int y = 0; y < grid.GetLength(1); y++)
								col[y] = grid[x, y];
							for (int colY = 0; colY < grid.GetLength(1); colY++)
							{
								var gridY = (colY + shift) % grid.GetLength(1);
								grid[x, gridY] = col[colY];
							}
						}
						break;
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			});

		if (!ok)
			return null;
		
		return grid;
	}

	private int ProcessDataForPart1(char[,] grid)
	{
		int count = 0;
		for (int x = 0; x < grid.GetLength(0); x++)
			for (int y = 0; y < grid.GetLength(1); y++)
				if (grid[x, y] == '#')
					count++;

		return count;
	}

	private int ProcessDataForPart2(char[,] grid)
	{
		int count = 0;
		for (int y = 0; y < grid.GetLength(1); y++)
		{
			var line = new StringBuilder();
			for (int x = 0; x < grid.GetLength(0); x++)
			{
				if (grid[x, y] == '#')
				{
					line.Append("##");
					count++;
				}
				else
				{
					line.Append("  ");
				}
			}
			logger.SendInfo(nameof(Day08), line.ToString());
		}

		return count;
	}
}
