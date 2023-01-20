using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day24 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 24;

	public string Name => $"Day 24";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day24Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day24(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day24), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day24), message);

	#endregion Helpers

	private List<char[]> LoadData(string input)
	{
		var map = new List<char[]>();

		InputHelper.TraverseInputLines(input, line =>
		{
			map.Add(line.ToCharArray());
		});
		
		return map;
	}

	private int SolvePart1(List<char[]> map)
	{
		var stops = GetStops(map);
		var graph = CreateGraph(map, stops);
		var path = FindBestPathUsingBruteForce(graph, doReturn: false);
		var length = GetPathLength(path, doReturn: false);

		return length;
	}

	private int SolvePart2(List<char[]> map)
	{
		var stops = GetStops(map);
		var graph = CreateGraph(map, stops);
		var path = FindBestPathUsingBruteForce(graph, doReturn: true);
		var length = GetPathLength(path, doReturn: true);

		return length;
	}

	private class Node
	{
		public char Name;
		public Dictionary<char, int> Neighbors = new();
	}

	private List<(char,Point)> GetStops(List<char[]> map)
	{
		var stops = new List<(char c, Point p)>();

		for (var x = 0; x < map.Count; x++)
		{
			for (var y = 0; y < map[x].Length; y++)
			{
				char c = map[x][y];
				if (c >= '0' && c <= '9')
					stops.Add((c, new Point(x, y)));
			}
		}

		var ordered = stops.OrderBy(stop => stop.c).ToList();

		foreach(var stop in ordered)
			LoggerSendDebug($"{stop.c} is at ({stop.p.X,3}, {stop.p.Y,3})");

		return ordered;
	}

	private List<Node> CreateGraph(List<char[]> map, List<(char c, Point p)> stops)
	{
		var nodes = new List<Node>();

		foreach (var stop in stops)
			nodes.Add(new Node { Name = stop.c });

		foreach (var origin in stops)
		{
			var originNode = nodes.FirstOrDefault(node => node.Name == origin.c);

			foreach (var target in stops)
			{
				if (target.c == origin.c)
					continue;

				if (originNode.Neighbors.ContainsKey(target.c))
					continue;

				var distance = GetDistance(map, origin.p, target.p);

				originNode.Neighbors.Add(target.c, distance);
				var targetNode = nodes.FirstOrDefault(node => node.Name == target.c);
				targetNode.Neighbors.Add(origin.c, distance);
			}
		}

		foreach (var node in nodes)
			foreach (var entry in node.Neighbors)
				LoggerSendDebug($"{node.Name} to {entry.Key} is {entry.Value,3} steps");

		return nodes;
	}

	private int GetDistance(List<char[]> map, Point origin, Point target)
	{
		var path = PathfindingHelper.FindPath(origin, target, 
			getNeighbors: (point) =>
			{
				var neighbors = new List<Point>();

				AddNeighbor(point, neighbors, 0, -1);
				AddNeighbor(point, neighbors, 0, +1);
				AddNeighbor(point, neighbors, -1, 0);
				AddNeighbor(point, neighbors, +1, 0);

				return neighbors;
			}, 
			getDistance: (p1, p2) => 1);

		return path.Count();

		void AddNeighbor(Point point, List<Point> neighbors, int dx, int dy)
		{
			int x = point.X + dx;
			int y = point.Y + dy;
			if (map[x][y] != '#')
				neighbors.Add(new Point(x, y));
		}
	}

	private List<Node> FindBestPathUsingBruteForce(List<Node> graph, bool doReturn)
	{
		int bestLength = int.MaxValue;
		List<Node> bestPath = null;

		FindBestPath(new List<Node> { graph[0] });

		return bestPath;

		void FindBestPath(List<Node> path)
		{
			if (path.Count == graph.Count)
			{
				var length = GetPathLength(path, doReturn);
				if (length < bestLength)
				{
					bestLength = length;
					bestPath = path;

					LoggerSendDebug($"{string.Join("-", bestPath.Select(node => node.Name))}{(doReturn ? $"-{path[0].Name}":"")} => {bestLength}");
				}
				return;
			}

			foreach (var node in graph)
			{
				if (path.Contains(node))
					continue;
				var newPath = new List<Node>(path) { node };
				FindBestPath(newPath);
			}
		}
	}

	private int GetPathLength(List<Node> path, bool doReturn)
	{
		var length = 0;
		for (var i = 1; i < path.Count; i++)
			length += path[i - 1].Neighbors[path[i].Name];

		if (doReturn)
			length += path[0].Neighbors[path[path.Count - 1].Name];

		return length;
	}
}
