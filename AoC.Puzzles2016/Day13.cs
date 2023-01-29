using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day13 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 13;

	public string Name => $"Day 13";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day13Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day13(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day13), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day13), message);

	#endregion Helpers

	#region Solvers

	private string SolvePart1(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart1(data);

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart2(data);

		return result.ToString();
	}

	#endregion Solvers

	private int LoadDataFromInput(string input)
	{
		int favoriteNumber = 0;

		InputHelper.TraverseInputTokens(input, value =>
		{
			favoriteNumber = int.Parse(value);
		});
		
		return favoriteNumber;
	}

	private class Node : IEquatable<Node>
	{
		public int X;
		public int Y;
		public bool IsSpace;
		public int Steps = int.MaxValue;
		public Node(int x, int y, bool isSpace)
		{
			X = x;
			Y = y;
			IsSpace = isSpace;
		}
		public bool Equals(Node other)
		{
			return X == other.X && Y == other.Y;
		}
		public override string ToString() => $"({X},{Y})";
	}

	private int ProcessDataForPart1(int favoriteNumber)
	{
		var seen = new Dictionary<long, Node>();

		var origin = CreateNode(seen, favoriteNumber, 1, 1);
		var target = favoriteNumber == 10 
			? CreateNode(seen, favoriteNumber, 7, 4)
			: CreateNode(seen, favoriteNumber, 31, 39);

		var path = FindPath(origin, target, seen, favoriteNumber);

		return path?.Count() ?? 0;
	}

	private object ProcessDataForPart2(int favoriteNumber)
	{
		var seen = new Dictionary<long, Node>();

		var origin = CreateNode(seen, favoriteNumber, 1, 1);
		var target = favoriteNumber == 10
			? CreateNode(seen, favoriteNumber, 7, 4)
			: CreateNode(seen, favoriteNumber, 31, 39);

		var path = FindPath(origin, target, seen, favoriteNumber);

		return seen.Values.Count(node => node.IsSpace && node.Steps <= 50);
	}

	private IEnumerable<Node> FindPath(Node origin, Node target, Dictionary<long, Node> seen, int favoriteNumber)
	{
		var path = PathfindingHelper.FindPath(origin, target,
			getNeighbors: (node) =>
			{
				var neighbors = new List<Node>();

				AddNeighbor(dy: -1);
				AddNeighbor(dy: +1);
				AddNeighbor(dx: -1);
				AddNeighbor(dx: +1);

				LoggerSendVerbose($"{node} => {string.Join(", ", neighbors)}");

				return neighbors;

				void AddNeighbor(int dx = 0, int dy = 0)
				{
					int x = node.X + dx;
					int y = node.Y + dy;

					if (x < 0 || y < 0)
						return;

					var neighbor = CreateNode(seen, favoriteNumber, x, y);

					if (neighbor.IsSpace)
						neighbors.Add(neighbor);
				}
			},
			setCost: (node, distance) => 
			{ 
				node.Steps = (int)distance;

				LoggerSendVerbose($"{node} => {distance} steps");
			});

		var nodes = seen.Values.OrderBy(n => n.Y).ThenBy(n => n.X).ToList();
		foreach (var node in nodes)
			LoggerSendVerbose($"{node}:{(node.IsSpace?".":"#")} {node.Steps}");

		return path;
	}

	private Node CreateNode(Dictionary<long, Node> seen, int favoriteNumber, int x, int y)
	{
		long hash = x * 10000 + y;

		if (!seen.TryGetValue(hash, out var node))
		{
			long n = x * x + 3 * x + 2 * x * y + y + y * y + favoriteNumber;
			long count = 0;
			while (n != 0)
			{
				count += n & 1L;
				n >>= 1;
			}
			var isSpace = count % 2 == 0;
			node = new Node(x, y, isSpace);
			seen[hash] = node;
		}
		return node;
	}
}
