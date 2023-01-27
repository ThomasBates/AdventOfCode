using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day22 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 22;

	public string Name => $"Day 22";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day22Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day22(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day22), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day22), message);

	#endregion Helpers

	private class Data
	{
		public Dictionary<Point, char> points = new();
		public Point current = Point.Empty;
		public Point direction = new(-1, 0);
		public int numInfections;
	}

	private Data LoadData(string input)
	{
		var data = new Data();

		var maxY = 0;
		var x = 0;
		InputHelper.TraverseInputLines(input, line =>
		{
			maxY = line.Length - 1;
			for (var y = 0; y < line.Length; y++)
				data.points.Add(new Point(x, y), line[y]);
			x++;
		});
		var maxX = x - 1;
		data.current = new Point(maxX / 2, maxY / 2);
		
		return data;
	}

	private object SolvePart1(Data data)
	{
		InfectMap2(data, 10000, part1: true);

		return data.numInfections;
	}

	private object SolvePart2(Data data)
	{
		InfectMap2(data, 10000000, part1: false);

		return data.numInfections;
	}

	private readonly Dictionary<Point, Point> left = new()
	{
		{ new Point(-1, 0), new Point(0, -1) },
		{ new Point( 0, 1), new Point(-1, 0) },
		{ new Point( 1, 0), new Point( 0, 1) },
		{ new Point( 0,-1), new Point( 1, 0) }
	};
	private readonly Dictionary<Point, Point> right = new()
	{
		{ new Point(-1, 0), new Point( 0, 1) },
		{ new Point( 0, 1), new Point( 1, 0) },
		{ new Point( 1, 0), new Point( 0,-1) },
		{ new Point( 0,-1), new Point(-1, 0) }
	};
	private readonly Dictionary<Point, Point> reverse = new()
	{
		{ new Point(-1, 0), new Point( 1, 0) },
		{ new Point( 0, 1), new Point( 0,-1) },
		{ new Point( 1, 0), new Point(-1, 0) },
		{ new Point( 0,-1), new Point( 0, 1) }
	};

	private void InfectMap2(Data data, int iterationCount, bool part1)
	{
		Visualize(data, 0);

		for (var i = 0; i < iterationCount; i++)
		{
			if (!data.points.TryGetValue(data.current, out var state))
			{
				state = '.';
				data.points[data.current] = state; 
			}

			data.direction = state switch
			{
				'.' => left[data.direction],
				'W' => data.direction,
				'#' => right[data.direction],
				'F' => reverse[data.direction],
				_ => data.direction
			};

			if (part1)
			{
				data.points[data.current] = state switch
				{
					'.' => '#',
					'#' => '.',
					_ => '.'
				};
				if (state == '.')
					data.numInfections++;
			}
			else
			{
				data.points[data.current] = state switch
				{
					'.' => 'W',
					'W' => '#',
					'#' => 'F',
					'F' => '.',
					_ => '.'
				};
				if (state == 'W')
					data.numInfections++;
			}

			data.current.Offset(data.direction);

			Visualize(data, i + 1);
		}

		void Visualize(Data data, int iteration)
		{
			if (logger.Severity > SeverityLevel.Verbose)
				return;

			var builder = new StringBuilder();
			builder.AppendLine($"{iteration}: ");

			var minX = data.points.Keys.Min(p => p.X) - 2;
			var maxX = data.points.Keys.Max(p => p.X) + 2;
			var minY = data.points.Keys.Min(p => p.Y) - 2;
			var maxY = data.points.Keys.Max(p => p.Y) + 2;

			for (var x = minX; x <= maxX; x++)
			{
				if (data.current.X == x && data.current.Y == minY)
					builder.Append('[');
				else
					builder.Append(' ');

				for (var y = minY; y <= maxY; y++)
				{
					var p = new Point(x, y);
					if (!data.points.TryGetValue(p, out var state))
						state = '.';
					builder.Append(state);

					if (data.current.X == x && data.current.Y == y)
						builder.Append(']');
					else if (data.current.X == x && data.current.Y == y + 1)
						builder.Append('[');
					else
						builder.Append(' ');
				}
				builder.AppendLine();
			}

			SendVerbose(builder.ToString());
		}
	}
}
