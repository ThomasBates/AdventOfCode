using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day11 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 11;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day11Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day11()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	public string SolvePart1(string input)
	{
		var result = new StringBuilder();

		InputHelper.TraverseInputLines(input, line =>
		{
			if (int.TryParse(line, out int serialNumber))
			{
				var grid = new int[301, 301];
				for (int x = 1; x <= 300; x++)
				{
					for (int y = 1; y <= 300; y++)
					{
						int rackId = x + 10;
						int power = rackId * y;
						power += serialNumber;
						power *= rackId;
						power = (power / 100) % 10;
						power -= 5;
						grid[x, y] = power;
					}
				}

				int maxPower = 0;
				int maxX = 0;
				int maxY = 0;
				for (int x = 1; x <= 300 - 2; x++)
				{
					for (int y = 1; y <= 300 - 2; y++)
					{
						int power = grid[x + 0, y + 0] + grid[x + 1, y + 0] + grid[x + 2, y + 0]
								  + grid[x + 0, y + 1] + grid[x + 1, y + 1] + grid[x + 2, y + 1]
								  + grid[x + 0, y + 2] + grid[x + 1, y + 2] + grid[x + 2, y + 2];

						if (power > maxPower)
						{
							maxPower = power;
							maxX = x;
							maxY = y;
						}
					}
				}

				result.AppendLine($"Max Power at ({maxX},{maxY})");
			}
		});

		return result.ToString();
	}

	public string SolvePart2(string input)
	{
		var result = new StringBuilder();

		InputHelper.TraverseInputLines(input, line =>
		{
			if (int.TryParse(line, out int serialNumber))
			{
				var grid = new int[301, 301];
				for (int x = 1; x <= 300; x++)
				{
					for (int y = 1; y <= 300; y++)
					{
						int rackId = x + 10;
						int power = rackId * y;
						power += serialNumber;
						power *= rackId;
						power = (power / 100) % 10;
						power -= 5;
						grid[x, y] = power;
					}
				}

				int maxPower = 0;
				int maxX = 0;
				int maxY = 0;
				int maxSize = 0;

				for (int x = 1; x <= 300; x++)
				{
					for (int y = 1; y <= 300; y++)
					{
						int maxSide = Math.Min(30, Math.Min(301 - x, 301 - y));
						for (int side = 1; side <= maxSide; side++)
						{
							int power = 0;
							for (int i = 0; i < side; i++)
							{
								for (int j = 0; j < side; j++)
								{
									power += grid[x + i, y + j];
								}
							}

							if (power > maxPower)
							{
								maxPower = power;
								maxX = x;
								maxY = y;
								maxSize = side;
							}
						}
					}
				}

				result.AppendLine($"Max Power at ({maxX},{maxY},{maxSize})");
			}
		});

		return result.ToString();
	}
}
