using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day14 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 14;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day14Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day14()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	private readonly List<List<Point>> paths = new();
	private int minX;
	private int minY;	
	private int maxX;
	private int maxY;
	private int sizeX;
	private int sizeY;

	private char[,] map;

	private string SolvePart1(string input)
	{
		var output = new StringBuilder();

		LoadDataFromInput(input, output, false);

		SimulateFallingSand(output);

		return output.ToString();
	}

	private string SolvePart2(string input)
	{
		var output = new StringBuilder();

		LoadDataFromInput(input, output, true);

		SimulateFallingSand(output);

		return output.ToString();
	}

	private void LoadDataFromInput(string input, StringBuilder output, bool withFloor)
	{
		paths.Clear();
		minX = 500;
		maxX = 500;
		minY = 0;
		maxY = 0;

		InputHelper.TraverseInputLines(input, line =>
		{
			if (string.IsNullOrEmpty(line)) return;

			var path = new List<Point>();
			paths.Add(path);

			var parts = line.Split(new[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var part in parts)
			{
				var pp = part.Split(',');

				var point = new Point(int.Parse(pp[0]), int.Parse(pp[1]));
				path.Add(point);
				minX = Math.Min(minX, point.X);
				maxX = Math.Max(maxX, point.X);
				minY = Math.Min(minY, point.Y);
				maxY = Math.Max(maxY, point.Y);
			}
		});

		if (withFloor)
		{
			maxY += 2;
			minX = Math.Min(minX, 500 - (maxY - minY) - 2);
			maxX = Math.Max(maxX, 500 + (maxY - minY) + 2);

			var path = new List<Point>
			{
				new Point(minX, maxY),
				new Point(maxX, maxY)
			};

			paths.Add(path);
		}

		output.AppendLine($"({minX},{minY}) - ({maxX},{maxY})");

		sizeX = maxX - minX + 1;
		sizeY = maxY - minY + 1;

		map = new char[sizeX, sizeY];

		for (int x = 0; x < sizeX; x++)
		{
			for (int y = 0; y < sizeY; y++)
			{
				map[x, y] = '.';
			}
		}

		foreach (var path in paths)
		{
			Point? lastPoint = null;

			foreach (var point in path)
			{
				if (lastPoint.HasValue)
				{
					if (lastPoint.Value.X < point.X)
					{
						for (int x = lastPoint.Value.X; x <= point.X; x++)
						{
							map[x - minX, point.Y - minY] = '#';
						}
					}
					else if (lastPoint.Value.X > point.X)
					{
						for (int x = lastPoint.Value.X; x >= point.X; x--)
						{
							map[x - minX, point.Y - minY] = '#';
						}
					}
					else if (lastPoint.Value.Y < point.Y)
					{
						for (int y = lastPoint.Value.Y; y <= point.Y; y++)
						{
							map[point.X - minX, y - minY] = '#';
						}
					}
					else if (lastPoint.Value.Y > point.Y)
					{
						for (int y = lastPoint.Value.Y; y >= point.Y; y--)
						{
							map[point.X - minX, y - minY] = '#';
						}
					}
				}
				lastPoint = point;
			}
		}
		map[500 - minX, 0 - minY] = '+';

		for (int y = 0; y < sizeY; y++)
		{
			for (int x = 0; x < sizeX; x++)
			{
				output.Append(map[x,y]);
			}
			output.AppendLine();
		}
	}

	private void SimulateFallingSand(StringBuilder output)
	{

		var source = new Point(500 - minX, 0 - minY);

		int drops = 0;
		bool finished = false;
		while (!finished)
		{
			var drop = source;
			while (true)
			{
				if (drop.Y + 1 >= sizeY)
				{
					finished = true;
					break;
				}

				if (map[drop.X, drop.Y + 1] == '.')
				{
					drop.Y++;
					continue;
				}

				if (drop.X - 1 < 0)
				{
					finished = true;
					break;
				}

				if (map[drop.X - 1, drop.Y + 1] == '.')
				{
					drop.X--;
					drop.Y++;
					continue;
				}

				if (drop.X + 1 >= sizeX)
				{
					finished = true;
					break;
				}

				if (map[drop.X + 1, drop.Y + 1] == '.')
				{
					drop.X++;
					drop.Y++;
					continue;
				}

				if (drop == source)
				{
					map[drop.X, drop.Y] = 'o';
					drops++;

					finished = true;
					break;
				}

				map[drop.X, drop.Y] = 'o';
				drops++;
				break;
			}
		}

		output.AppendLine();

		for (int y = 0; y < sizeY; y++)
		{
			for (int x = 0; x < sizeX; x++)
			{
				output.Append(map[x, y]);
			}
			output.AppendLine();
		}

		output.AppendLine();
		output.AppendLine($"{drops} drops");
	}
}
