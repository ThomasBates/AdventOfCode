using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day22 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 22;

	public string Name => "Day 22";

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

	private class Map
	{
		public int Depth;
		public Point Target;
		public Point Origin;

		private readonly Dictionary<Point, int> levels = new();
		private readonly Dictionary<Point, int> types = new();

		public int GetType(Point p)
		{
			if (!types.TryGetValue(p, out var type))
			{
				var level = GetLevel(p);
				type = level % 3;
				types[p] = type;
			}
			return type;
		}
		private int GetLevel(Point p)
		{
			if (!levels.TryGetValue(p, out var level))
			{
				var x = p.X;
				var y = p.Y;
				int index;
				if (x == 0 && y == 0)
					index = 0;
				else if (x == Target.X && y == Target.Y)
					index = 0;
				else if (y == 0)
					index = x * 16807;
				else if (x == 0)
					index = y * 48271;
				else
					index = GetLevel(new Point(x - 1, y)) * GetLevel(new Point(x, y - 1));

				level = (index + Depth) % 20183;
				levels[p] = level;
			}
			return level;
		}
	}

	private Map LoadData(string input)
	{
		var data = new Map { Origin = new Point(0, 0) };

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ':', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (parts[0] == "depth")
				data.Depth = int.Parse(parts[1]);
			else if (parts[0] == "target")
				data.Target = new Point(int.Parse(parts[1]), int.Parse(parts[2]));
		});

		return data;
	}

	private object SolvePart1(Map map)
	{
		var risk = 0;

		for (var x = 0; x <= map.Target.X; x++)
			for (var y = 0; y <= map.Target.Y; y++)
				risk += map.GetType(new Point(x, y));

		return risk;
	}

	private object SolvePart2(Map map)
	{
		(Point point, char gear) origin = (map.Origin, 't');
		(Point point, char gear) target = (map.Target, 't');

		var path = PathfindingHelper.FindPath(origin, target,
			getNeighbors: node =>
			{
				var neighbors = new List<(Point, char)>();

				var type = map.GetType(node.point);
				var newGear = (type, node.gear) switch
				{
					(0, 'c') => 't',
					(0, 't') => 'c',
					(1, 'c') => 'n',
					(1, 'n') => 'c',
					(2, 't') => 'n',
					(2, 'n') => 't',
					_ => throw new NotImplementedException()
				};
				neighbors.Add((node.point, newGear));

				AddNeighbor(dx: -1);
				AddNeighbor(dx: 1);
				AddNeighbor(dy: -1);
				AddNeighbor(dy: 1);

				return neighbors;

				void AddNeighbor(int dx = 0, int dy = 0)
				{
					var newPoint = node.point;
					newPoint.Offset(dx, dy);
					if (newPoint.X < 0 || newPoint.Y < 0)
						return;
					var newType = map.GetType(newPoint);
					var canAdd = (newType, node.gear) switch
					{
						(0, 'c') => true,
						(0, 't') => true,
						(1, 'c') => true,
						(1, 'n') => true,
						(2, 't') => true,
						(2, 'n') => true,
						_ => false
					};
					if (canAdd)
						neighbors.Add((newPoint, node.gear));
				}
			},
			getCost: (node1, node2) => (node1.point == node2.point && node1.gear != node2.gear) ? 7 : 1,
			getHeuristic: node => Math.Abs(node.point.X - target.point.X) + Math.Abs(node.point.Y - target.point.Y))
			.ToList();

		var total = 0;
		var node2 = path[0];
		SendDebug($"{node2.point}-{node2.gear}: time = {0}, total = {total}");
		for (var i = 1; i < path.Count; i++)
		{
			var node1 = node2;
			node2 = path[i];
			var length = (node1.point == node2.point && node1.gear != node2.gear) ? 7 : 1;
			total += length;
			SendDebug($"{node2.point}-{node2.gear}: time = {length}, total = {total}");
		}

		return total;
	}
}
