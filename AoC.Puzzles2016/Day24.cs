using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
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

	private class Point
	{
		public int Row;
		public int Col;
		public char Type;

		public override int GetHashCode() => Row * 1000 + Col;
	}

	private class Node
	{
		public char Name;
		public Dictionary<char, int> Neighbors = new();
	}

	private List<Point> GetStops(List<char[]> map)
	{
		var stops = new List<Point>();

		for (var row = 0; row < map.Count; row++)
		{
			for (var col = 0; col < map[row].Length; col++)
			{
				char c = map[row][col];
				if (c >= '0' && c <= '9')
					stops.Add(new Point { Type = c, Row = row, Col = col });
			}
		}

		var ordered = stops.OrderBy(stop => stop.Type).ToList();

		foreach(var stop in ordered)
			LoggerSendDebug($"{stop.Type} is at ({stop.Row,3}, {stop.Col,3})");

		return ordered;
	}

	private List<Node> CreateGraph(List<char[]> map, List<Point> stops)
	{
		var nodes = new List<Node>();

		foreach (var stop in stops)
			nodes.Add(new Node { Name = stop.Type });

		foreach (var stop in stops)
		{
			var node = nodes.FirstOrDefault(node => node.Name == stop.Type);

			foreach (var neighbor in stops)
			{
				if (neighbor.Type == stop.Type)
					continue;

				if (node.Neighbors.ContainsKey(neighbor.Type))
					continue;

				var distance = GetDistance(map, stop, neighbor);

				node.Neighbors.Add(neighbor.Type, distance);
				var neighborNode = nodes.FirstOrDefault(node => node.Name == neighbor.Type);
				neighborNode.Neighbors.Add(stop.Type, distance);
			}
		}

		foreach (var node in nodes)
			foreach (var entry in node.Neighbors)
				LoggerSendDebug($"{node.Name} to {entry.Key} is {entry.Value,3} steps");

		return nodes;
	}

	private int GetDistance(List<char[]> map, Point origin, Point target)
	{
		var points = new Dictionary<int, Point> 
		{
			{ origin.GetHashCode(), origin },
			{ target.GetHashCode(), target }
		};

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

		void AddNeighbor(Point point, List<Point> neighbors, int dRow, int dCol)
		{
			int row = point.Row + dRow;
			int col = point.Col + dCol;
			int hash = row * 1000 + col;
			if (!points.TryGetValue(hash, out var neighbor))
			{
				neighbor = new Point { Row = row, Col = col, Type = map[row][col] };
				points.Add(hash, neighbor);
			}
			if (neighbor.Type != '#')
				neighbors.Add(neighbor);
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
