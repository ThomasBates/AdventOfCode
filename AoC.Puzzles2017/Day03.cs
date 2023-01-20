using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Text;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day03 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 03;

	public string Name => $"Day 03";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", "1024"},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day03(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day03), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day03), message);

	#endregion Helpers

	private int LoadData(string input)
	{
		var startSquare = 1;

		InputHelper.TraverseInputTokens(input, value =>
		{
			startSquare = int.Parse(value);
		});
		
		return startSquare;
	}

	private int SolvePart1(int startSquare)
	{
		if (startSquare == 1)
			return 0;

		int root = (int)Math.Ceiling(Math.Sqrt((double)startSquare)) - 1;
		if (root % 2 == 0)
			root--;
		int ring = 1 + root / 2;

		int left = startSquare - (root * root);
		int steps = left % (root + 1);

		return ring + Math.Abs(steps - ring);
	}

	private int SolvePart2(int minValue)
	{
		var grid = new int[100, 100];
		var value = 1;
		int x = 50;
		int y = 50;
		grid[x, y] = value;
		var ring = 0;

		int minX = x;
		int maxX = x;
		int minY = y;
		int maxY = y;

		while (true)
		{
			// new ring
			ring++;
			x++;
			y++;
			for (var i = 0; i < ring * 2; i++)
			{
				y--;
				value = SumNeighbors(x, y);
				grid[x, y] = value;
				if (value > minValue)
				{
					VisualizeGrid();
					return value;
				}
			}
			for (var i = 0; i < ring * 2; i++)
			{
				x--;
				value = SumNeighbors(x, y);
				grid[x, y] = value;
				if (value > minValue)
				{
					VisualizeGrid();
					return value;
				}
			}
			for (var i = 0; i < ring * 2; i++)
			{
				y++;
				value = SumNeighbors(x, y);
				grid[x, y] = value;
				if (value > minValue)
				{
					VisualizeGrid();
					return value;
				}
			}
			for (var i = 0; i < ring * 2; i++)
			{
				x++;
				value = SumNeighbors(x, y);
				grid[x, y] = value;
				if (value > minValue)
				{
					VisualizeGrid();
					return value;
				}
			}
		}

		int SumNeighbors(int x, int y)
		{
			minX = Math.Min(minX, x);
			maxX = Math.Max(maxX, x);
			minY = Math.Min(minY, y);
			maxY = Math.Max(maxY, y);

			var sum = 0;
			for (var dx = -1; dx <= 1; dx++)
				for (var dy = -1; dy <= 1; dy++)
					if (dx!=0 || dy!=0)
						sum += grid[x + dx, y + dy];
			return sum;
		}

		void VisualizeGrid()
		{
			for (var y=minY; y<=maxY; y++)
			{
				var line = new StringBuilder();
				for (var x=minX;x<=maxX;x++)
				{
					if ((x,y)==(50,50))
					{
						line.Append(" O");
						continue;
					}
					var v = grid[x, y];
					if (v == value)
						line.Append(" X");
					else if (v==0)
						line.Append(" .");
					else
						line.Append($" {(int)Math.Log10((double)v) + 1}");
				}
				SendDebug(line.ToString());
			}
		}
	}
}
