using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day19 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 19;

	public string Name => $"Day 19";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day19Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day19(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day19), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day19), message);

	#endregion Helpers

	private char[,] LoadData(string input)
	{
		var data = new List<string>();

		InputHelper.TraverseInputLines(input, line =>
		{
			data.Add(line);
		});
		
		var height = data.Count;
		var width = data.Max(line => line.Length);
		var min = data.Min(line => line.Length);

		var map = new char[height, width];
		for (var x = 0; x < height; x++)
			for (var y = 0; y < width; y++)
				map[x, y] = y < data[x].Length ? data[x][y] : ' ';

		return map;
	}

	private string SolvePart1(char[,] map)
	{
		var (path, _) = FollowPath(map);
		return path;
	}

	private int SolvePart2(char[,] map)
	{
		var (_, count) = FollowPath(map);
		return count;
	}

	private (string path, int count) FollowPath(char[,] map)
	{
		var maxX = map.GetLength(0) - 1;
		var maxY = map.GetLength(1) - 1;

		Point start = Point.Empty;
		for (var y = 0; y <= maxY; y++)
			if (map[0, y] != ' ')
				start = new Point(0, y);
		var dir = new Point(1, 0);

		var path = new StringBuilder();
		var count = 0;
		var current = start;
		while (true)
		{
			count++;
			current.Offset(dir);

			var x = current.X;
			var y = current.Y;
			var c = map[x, y];

			if (c == ' ')
				return (path.ToString(), count);

			if (c >= 'A' && c <= 'Z')
				path.Append(c);

			if (c == '+')
			{
				var dirX = 0;
				var dirY = 0;
				if (dir.Y == 0)
					dirY = y < maxY && map[x, y + 1] != ' ' ? 1
						: y > 0 && map[x, y - 1] != ' ' ? -1 : 0;
				if (dir.X == 0)
					dirX = x < maxX && map[x + 1, y] != ' ' ? 1
						: x > 0 && map[x - 1, y] != ' ' ? -1 : 0;
				dir = new Point(dirX, dirY);
			}
		}
	}
}
