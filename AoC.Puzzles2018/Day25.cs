using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Common.Types;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day25 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 25;

	public string Name => "Day 25";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs (1)", Resources.Day25Inputs01},
		{"Example Inputs (2)", Resources.Day25Inputs02},
		{"Example Inputs (3)", Resources.Day25Inputs03},
		{"Example Inputs (4)", Resources.Day25Inputs04},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day25(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day25), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day25), message);

	#endregion Helpers

	private class Data
	{
		public List<Point4D> Points = new();
		public Dictionary<Point4D, List<Point4D>> Neighbors = new();
		public List<List<Point4D>> Constellations = new();
	}

	private Data LoadData(string input)
	{
		var data = new Data();

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
			data.Points.Add(new Point4D(
				int.Parse(parts[0]),
				int.Parse(parts[1]),
				int.Parse(parts[2]),
				int.Parse(parts[3])));
		});

		return data;
	}

	private object SolvePart1(Data data)
	{
		ConnectPoints(data);
		var count = FindConstellations(data);
		return count;
	}

	private object SolvePart2(Data data)
	{
		ConnectPoints(data);
		FindConstellations2(data);
		return data.Constellations.Count;
	}

	private void ConnectPoints(Data data)
	{
		foreach (var point in data.Points)
			data.Neighbors[point] = new List<Point4D>();

		for (var i = 0; i < data.Points.Count; i++)
		{
			var point1 = data.Points[i];
			for (var j = i + 1; j < data.Points.Count; j++)
			{
				var point2 = data.Points[j];
				if (point1.ManhattanDistance(point2) <= 3)
				{
					data.Neighbors[point1].Add(point2);
					data.Neighbors[point2].Add(point1);
				}
			}
		}
	}

	//  From AoC.Puzzles2017.Day12 - 1.53s
	private int FindConstellations(Data data)
	{
		var count = 0;
		var available = data.Points.ToList();
		while (available.Any())
		{
			count++;

			var origin = available.First();
			available.Remove(origin);
			foreach (var target in available.ToList())
			{
				var path = PathfindingHelper.FindPath(origin, target,
					getNeighbors: (current) => data.Neighbors[current]);

				if (path != null)
					available.Remove(target);
			}
		}
		return count;
	}

	//  From AoC.Puzzles2017.Day14 - 0.11s
	private void FindConstellations2(Data data)
	{
		List<Point4D> constellation = null;
		var available = data.Points.ToList();
		while (available.Any())
		{
			var start = available.First();

			constellation = new List<Point4D>();
			data.Constellations.Add(constellation);

			constellation.Add(start);
			available.Remove(start);

			ExtendConstellation(start);
		}

		void ExtendConstellation(Point4D point)
		{
			foreach (var neighbor in data.Neighbors[point])
			{
				if (!available.Contains(neighbor))
					continue;

				constellation.Add(neighbor);
				available.Remove(neighbor);

				ExtendConstellation(neighbor);
			}
		}
	}
}
