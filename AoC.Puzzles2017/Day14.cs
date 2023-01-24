using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Text;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day14 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 14;

	public string Name => $"Day 14";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{ "Example Inputs", "flqrgnkx" },
		{ "Puzzle Inputs",  "" }
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day14(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day14), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day14), message);

	#endregion Helpers

	private string LoadData(string input)
	{
		var key = "";

		InputHelper.TraverseInputTokens(input, value =>
		{
			key += value;
		});

		return key;
	}

	private int SolvePart1(string key)
	{
		var count = 0;

		for (int x = 0; x < 128; x++)
		{
			var rowCount = 0;
			var hash = HashHelper.GetKnotHash($"{key}-{x}");
			foreach (char c in hash)
			{
				var v = c >= 'a' ? c - 'a' + 10 : c - '0';
				while (v != 0)
				{
					if ((v & 1) == 1)
						rowCount++;
					v >>= 1;
				}
			}
			SendDebug($"row {x,3}: {hash} => {rowCount}");
			count += rowCount;
		}

		return count;
	}

	private int SolvePart2(string key)
	{
		var map = new int?[128, 128];

		for (var x = 0; x < 128; x++)
		{
			var row = new StringBuilder();
			var hash = HashHelper.GetKnotHash($"{key}-{x}");
			for (var y = 0; y < 128; y++)
			{
				var c = hash[y / 4];
				var v = c >= 'a' ? c - 'a' + 10 : c - '0';
				var mask = 1 << (3 - (y % 4));
				if ((v & mask) != 0)
					map[x, y] = 0;
			}
		}

		VisualizeMap();

		var region = 0;
		while (true)
		{
			var start = FindStart();
			if (!start.HasValue)
			{
				VisualizeMap();
				return region;
			}

			region++;

			ExtendRegion(start.Value);
		}

		Point? FindStart()
		{
			for (var x = 0; x < 128; x++)
				for (var y = 0; y < 128; y++)
					if (map[x, y].HasValue && map[x, y].Value == 0)
						return new Point(x, y);
			return null;
		}

		void ExtendRegion(Point point, int dx = 0, int dy = 0)
		{
			var x = point.X + dx;
			var y = point.Y + dy;

			if (x < 0 || x >= 128 || y < 0 || y >= 128)
				return;

			var currentRegion = map[x, y];
			if (!currentRegion.HasValue || currentRegion != 0)
				return;

			map[x, y] = region;
			var newPoint = new Point(x, y);
			ExtendRegion(newPoint, dx: 1);
			ExtendRegion(newPoint, dx: -1);
			ExtendRegion(newPoint, dy: 1);
			ExtendRegion(newPoint, dy: -1);
		}

		void VisualizeMap()
		{
			var grid = new StringBuilder("\n");

			for (var x = 0; x < 128; x++)
			{
				for (var y = 0; y < 128; y++)
				{
					if (map[x, y].HasValue)
						grid.Append($"{map[x, y].Value,4} ");
					else
						grid.Append($"   . ");
				}
				grid.AppendLine();
			}

			SendDebug(grid.ToString());
		}
	}
}
