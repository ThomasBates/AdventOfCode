using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day22 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 22;

	public string Name => $"Day 22";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{ "Example Inputs", Resources.Day22Inputs },
		{ "Puzzle Inputs",  ""}
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

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day22), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day22), message);

	#endregion Helpers

	private class StorageNode
	{
		public int X;
		public int Y;
		public int Size;
		public int Used;
		public int Avail;
		public int Percent;

		public StorageNode(int x, int y, int s, int u, int a, int p)
		{
			this.X = x;
			this.Y = y;
			this.Size = s;
			this.Used = u;
			this.Avail = a;
			this.Percent = p;
		}

		public override string ToString() => $"({X,2},{Y,2}) [{Size,3}, {Used,3}, {Avail,3}, {Percent,3}]";
	}

	private List<StorageNode> LoadData(string input)
	{
		var data = new List<StorageNode>();

		InputHelper.TraverseInputLines(input, line =>
		{
			if (!line.StartsWith("/"))
				return;

			Match match = Regex.Match(line, @"/dev/grid/node-x(\d+)-y(\d+)\s+(\d+)T\s+(\d+)T\s+(\d+)T\s+(\d+)%");

			if (!match.Success)
			{
				logger.SendError(nameof(Day22), $"Could not match line: {line}");
				return;
			}

			var x = int.Parse(match.Groups[1].Value);
			var y = int.Parse(match.Groups[2].Value);
			var s = int.Parse(match.Groups[3].Value);
			var u = int.Parse(match.Groups[4].Value);
			var a = int.Parse(match.Groups[5].Value);
			var p = int.Parse(match.Groups[6].Value);

			data.Add(new StorageNode(x, y, s, u, a, p));
		});

		LoggerSendDebug($"{data.Count} nodes.");
		foreach (var node in data)
		{
			LoggerSendVerbose(node.ToString());
		}
		LoggerSendDebug("");

		return data;
	}

	private int SolvePart1(List<StorageNode> data)
	{
		var count = 0;
		var adjacent = 0;

		foreach (var A in data ) 
		{
			var a = A.ToString();
			if (A.Used == 0)
			{
				LoggerSendVerbose($"{a} is empty");
				continue;
			}

			foreach (var B in data)
			{
				var b = B.ToString();

				if (A.X == B.X && A.Y == B.Y)
				{
					LoggerSendVerbose($"{a} and {b} are the same");
					continue;
				}

				if (A.Used > B.Avail)
				{
					LoggerSendVerbose($"{a} too big for {b}");
					continue;
				}

				count++;

				if ((B.X == A.X && B.Y == A.Y - 1) ||
					(B.X == A.X && B.Y == A.Y + 1) ||
					(B.X == A.X - 1 && B.Y == A.Y) ||
					(B.X == A.X + 1 && B.Y == A.Y))
				{
					LoggerSendDebug($"{a} and {b} are viable and adjacent");
					adjacent++;
					continue;
				}

				LoggerSendDebug($"{a} and {b} are viable, but not adjacent");
			}
		}

		LoggerSendDebug($"there are {count} viable pairs");
		LoggerSendDebug($"there are {adjacent} viable and adjacent pairs");

		return count;
	}

	private object SolvePart2(List<StorageNode> data)
	{
		var maxX = data.Max(node => node.X);
		var maxY = data.Max(node => node.Y);
		var empty = data.FirstOrDefault(node => node.Used == 0);
		var hash = GetHash(empty.X, empty.Y, maxX, 0);

		var queue = new Queue<(int,int)>();
		queue.Enqueue((hash, 0));
		var seen = new HashSet<int>() { hash };

		var blocks = data
			.Where(node => node.Used > 200)
			.Select(node => new Point(node.X, node.Y))
			.ToList();

		var steps = 0;
		while (queue.Count > 0)
		{
			var state = queue.Dequeue();

			steps = TryMove(state, 0, -1);			
			if (steps > 0) 
				return steps;
			steps = TryMove(state, 0, +1);
			if (steps > 0)
				return steps;
			steps = TryMove(state, -1, 0);
			if (steps > 0)
				return steps;
			steps = TryMove(state, +1, 0);
			if (steps > 0)
				return steps;
		}

		return 0;

		int TryMove((int, int) state, int dx, int dy)
		{
			var (hash, steps) = state;

			var (ex, ey, gx, gy) = UnrollHash(hash);

			int ex2 = ex + dx;
			int ey2 = ey + dy;

			if (ex2 < 0 || ex2 > maxX || ey2 < 0 || ey2 > maxY)
				return 0;
			if (blocks.Contains(new Point(ex2, ey2)))
				return 0;
			if (ex2 == gx && ey2 == gy)
				(gx, gy) = (ex, ey);

			if (gx == 0 && gy == 0)
				return steps + 1;

			var newHash = GetHash(ex2, ey2, gx, gy);
			if (seen.Contains(newHash))
				return 0;

			var newState = (newHash, steps + 1);

			seen.Add(newHash);
			queue.Enqueue(newState);
			return 0;
		}

		int GetHash(int ex, int ey, int gx, int gy)
			=> ex * 1000000 + ey * 10000 + gx * 100 + gy;
		(int ex, int ey, int gx, int gy) UnrollHash(int hash)
			=> ((hash / 1000000), (hash / 10000) % 100, (hash / 100) % 100, hash % 100);
	}
}
