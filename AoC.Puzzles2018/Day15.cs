using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day15 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 15;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new Dictionary<string, string>
	{
		{"Example Inputs 01", Resources.Day15Inputs01 },
		{"Example Inputs 02", Resources.Day15Inputs02 },
		{"Example Inputs 03", Resources.Day15Inputs03 },
		{"Example Inputs 04", Resources.Day15Inputs04 },
		{"Example Inputs 05", Resources.Day15Inputs05 },
		{"Example Inputs 06", Resources.Day15Inputs06 },
		{"Example Inputs 07", Resources.Day15Inputs07 },
		{"Example Inputs 08", Resources.Day15Inputs08 },
		{"Example Inputs 09", Resources.Day15Inputs09 },
		{"Puzzle Inputs", "" }
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day15()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	private enum Race
	{
		Elf, Goblin
	}

	private class Player
	{
		public Race Race;
		public int Health;
		public int Strength;
		public Point Location;
		public override string ToString()
		{
			return $"{Race} ({Location.X},{Location.Y}) [{Health}]";
		}
	}

	private char[,] map;
	private int maxX, maxY;
	private readonly List<Player> players = new();

	private bool showVisualization = false;
	private int initialHealth = 200;
	private int elfAttackPower = 3;
	private int goblinAttackPower = 3;

	public string SolvePart1(string input)
	{
		var result = new StringBuilder();

		LoadMapFromInput(input);
		FindPlayers();

		int round = 0;
		while (true)
		{
			Visualize(round, result);

			bool finished = false;
			var playersInOrder = players.OrderBy(p => p.Location.Y * 1000 + p.Location.X).ToList();
			foreach (var player in playersInOrder.ToList())
			{
				//	In case they died after the round began.
				if (!players.Contains(player))
				{
					continue;
				}

				var targets = players.Where(p => p.Race != player.Race).ToList();
				if (targets.Count == 0)
				{
					finished = true;
					break;
				}

				if (AttackTarget(player, targets))
				{
					continue;
				}

				var adjacencies = FindAdjacencies(targets);
				if (adjacencies.Count == 0)
				{
					continue;
				}

				var paths = FindPaths(player.Location, adjacencies);
				if (paths.Count == 0)
				{
					continue;
				}

				var bestAdjacency = adjacencies.OrderBy(a => paths[a].Count * 1000000 + a.Y * 1000 + a.X).First();
				var bestPath = paths[bestAdjacency];
				if (bestPath.Count > 0)
				{
					player.Location = bestPath[0];
				}

				AttackTarget(player, targets);
			}

			if (finished)
			{
				break;
			}

			round++;
		}

		Visualize(round, result);

		int totalHealth = players.Sum(p => p.Health);

		result.AppendLine($"Combat ends after {round} full rounds.");
		result.AppendLine($"{players[0].Race}s win with {totalHealth} total hit points left.");
		result.AppendLine($"Outcome: {round} * {totalHealth} = {round * totalHealth}");

		return result.ToString();
	}

	public string SolvePart2(string input)
	{
		var result = new StringBuilder();

		elfAttackPower = 3;
		int round;
		while (true)
		{
			elfAttackPower++;

			LoadMapFromInput(input);
			FindPlayers();

			int initialElfCount = players.Where(p => p.Race == Race.Elf).Count();

			round = 0;
			while (true)
			{
				//Visualize(round, result);

				bool finished = false;
				var playersInOrder = players.OrderBy(p => p.Location.Y * 1000 + p.Location.X).ToList();
				foreach (var player in playersInOrder.ToList())
				{
					//	In case they died after the round began.
					if (!players.Contains(player))
					{
						continue;
					}

					var targets = players.Where(p => p.Race != player.Race).ToList();
					if (targets.Count == 0)
					{
						finished = true;
						break;
					}

					if (AttackTarget(player, targets))
					{
						continue;
					}

					var adjacencies = FindAdjacencies(targets);
					if (adjacencies.Count == 0)
					{
						continue;
					}

					var paths = FindPaths(player.Location, adjacencies);
					if (paths.Count == 0)
					{
						continue;
					}

					var bestAdjacency = adjacencies.OrderBy(a => paths[a].Count * 1000000 + a.Y * 1000 + a.X).First();
					var bestPath = paths[bestAdjacency];
					if (bestPath.Count > 0)
					{
						player.Location = bestPath[0];
					}

					AttackTarget(player, targets);
				}

				if (finished)
				{
					break;
				}

				round++;
			}

			int finalElfCount = players.Where(p => p.Race == Race.Elf).Count();
			if (finalElfCount == initialElfCount)
			{
				break;
			}
		}

		//Visualize(round, result);

		int totalHealth = players.Sum(p => p.Health);

		result.AppendLine($"Elf Attack Power = {elfAttackPower}");
		result.AppendLine($"Combat ends after {round} full rounds.");
		result.AppendLine($"{players[0].Race}s win with {totalHealth} total hit points left.");
		result.AppendLine($"Outcome: {round} * {totalHealth} = {round * totalHealth}");

		return result.ToString();
	}

	private enum InputState
	{
		None,
		Definitions,
		Inputs,
		Done
	}

	private void LoadMapFromInput(string input)
	{
		var lines = new List<string>();

		InputState state = InputState.Inputs;

		InputHelper.TraverseInputLines(input, line =>
		{
			if (string.Equals(line, "#DEFINITIONS"))
			{
				state = InputState.Definitions;
				return;
			}
			if (string.Equals(line, "#INPUTS"))
			{
				state = InputState.Inputs;
				return;
			}
			if (string.Equals(line, "#END"))
			{
				state = InputState.Done;
				return;
			}

			switch (state)
			{
				case InputState.None:
					break;
				case InputState.Definitions:
					var parts = line.Split(new char[] {}, StringSplitOptions.RemoveEmptyEntries);
					if (string.Equals(parts[0], "ShowVisualization"))
					{
						showVisualization = bool.Parse(parts[2]);
					}
					else if (string.Equals(parts[0], "InitialHealth"))
					{
						initialHealth = int.Parse(parts[2]);
					}
					else if (string.Equals(parts[0], "ElfAttackPower"))
					{
						elfAttackPower = int.Parse(parts[2]);
					}
					else if (string.Equals(parts[0], "GoblinAttackPower"))
					{
						goblinAttackPower = int.Parse(parts[2]);
					}
					break;
				case InputState.Inputs:
					lines.Add(line);
					break;
				case InputState.Done:
					break;
			}

		});

		maxX = lines.Max(l => l.Length);
		maxY = lines.Count;
		map = new char[maxX, maxY];

		for (int y = 0; y < maxY; y++)
		{
			var line = lines[y];
			for (int x = 0; x < maxY; x++)
			{
				map[x, y] = line[x];
			}
		}
	}

	private void FindPlayers()
	{
		players.Clear();

		for (int x = 0; x < maxY; x++)
		{
			for (int y = 0; y < maxY; y++)
			{
				char c = map[x, y];
				if (c == 'E')
				{
					players.Add(new Player
					{
						Race = Race.Elf,
						Health = initialHealth,
						Strength = elfAttackPower,
						Location = new Point(x, y),
					});
					map[x, y] = '.';
				}
				if (c == 'G')
				{
					players.Add(new Player
					{
						Race = Race.Goblin,
						Health = initialHealth,
						Strength = goblinAttackPower,
						Location = new Point(x, y),
					});
					map[x, y] = '.';
				}
			}
		}
	}

	private bool AttackTarget(Player player, List<Player> targets)
	{
		var attackable = targets.Where(t => Math.Abs(t.Location.X - player.Location.X) +
											Math.Abs(t.Location.Y - player.Location.Y) == 1)
								.OrderBy(t => t.Health * 1000000 + t.Location.Y * 1000 + t.Location.X)
								.FirstOrDefault();
		if (attackable != null)
		{
			attackable.Health -= player.Strength;
			if (attackable.Health <= 0)
			{
				players.Remove(attackable);
			}
			return true;
		}

		return false;
	}

	private List<Point> FindAdjacencies(List<Player> targets)
	{
		var result = new List<Point>();

		foreach (var target in targets)
		{
			TestAdjacency(target.Location.X, target.Location.Y - 1, result);
			TestAdjacency(target.Location.X - 1, target.Location.Y, result);
			TestAdjacency(target.Location.X + 1, target.Location.Y, result);
			TestAdjacency(target.Location.X, target.Location.Y + 1, result);
		}
		return result;
	}

	private void TestAdjacency(int x, int y, List<Point> adjacencies)
	{
		if ((x < 0) || (x >= maxX) || (y < 0) || (y >= maxY))
		{
			return;
		}

		if (map[x, y] != '.')
		{
			return;
		}

		if (players.Any(p => (p.Location.X == x) && (p.Location.Y == y)))
		{
			return;
		}

		if (adjacencies.Any(p => (p.X == x) && (p.Y == y)))
		{
			return;
		}

		adjacencies.Add(new Point(x, y));
	}

	private Dictionary<Point, List<Point>> FindPaths(Point source, List<Point> destinations)
	{
		var paths = new Dictionary<Point, List<Point>>();

		foreach (var destination in destinations.ToList())
		{
			var path = FindPath(source, destination);
			if (path != null)
			{
				paths.Add(destination, path);
			}
			else
			{
				destinations.Remove(destination);
			}
		}

		return paths;
	}

	private List<Point> FindPath(Point source, Point destination)
	{
		bool[,] nodes = new bool[maxX, maxY];
		bool[,] visited = new bool[maxX, maxY];
		int[,] distance = new int[maxX, maxY];
		Point?[,] prev = new Point?[maxX, maxY];

		for (int x = 0; x < maxY; x++)
		{
			for (int y = 0; y < maxY; y++)
			{
				if (map[x, y] != '.')
				{
					nodes[x, y] = false;
					continue;
				}

				if (players.Any(p => (p.Location.X == x) && (p.Location.Y == y)))
				{
					nodes[x, y] = false;
					continue;
				}

				nodes[x, y] = true;
				visited[x, y] = false;
				distance[x, y] = int.MaxValue;
				prev[x, y] = null;
			}
		}

		distance[source.X, source.Y] = 0;
		Point? current = new Point(source.X, source.Y);

		bool first = true;
		while (true)
		{
			if (first)
			{
				CheckNode(nodes, visited, distance, prev, current.Value, current.Value.X, current.Value.Y - 1, 1001);
				CheckNode(nodes, visited, distance, prev, current.Value, current.Value.X - 1, current.Value.Y, 1002);
				CheckNode(nodes, visited, distance, prev, current.Value, current.Value.X + 1, current.Value.Y, 1003);
				CheckNode(nodes, visited, distance, prev, current.Value, current.Value.X, current.Value.Y + 1, 1004);
				first = false;
			}
			else
			{
				CheckNode(nodes, visited, distance, prev, current.Value, current.Value.X, current.Value.Y - 1, 1000);
				CheckNode(nodes, visited, distance, prev, current.Value, current.Value.X - 1, current.Value.Y, 1000);
				CheckNode(nodes, visited, distance, prev, current.Value, current.Value.X + 1, current.Value.Y, 1000);
				CheckNode(nodes, visited, distance, prev, current.Value, current.Value.X, current.Value.Y + 1, 1000);
			}
			visited[current.Value.X, current.Value.Y] = true;

			if (current == destination)
			{
				return GetPath(prev, source, destination);
			}

			current = GetCurrent(nodes, visited, distance);

			if (!current.HasValue)
			{
				return null;
			}
		}
	}

	private void CheckNode(bool[,] nodes, bool[,] visited, int[,] distance, Point?[,] prev, Point current, int x, int y, int value)
	{
		if ((x < 0) || (x >= maxX) || (y < 0) || (y >= maxY))
		{
			return;
		}

		if (!nodes[x, y] || visited[x,y])
		{
			return;
		}

		int d = distance[current.X, current.Y] + value;
		if (d < distance[x, y])
		{
			distance[x, y] = d;
			prev[x, y] = new Point(current.X, current.Y);
		}
	}

	private Point? GetCurrent(bool[,] nodes, bool[,] visited, int[,] distance)
	{
		int minDistance = int.MaxValue;
		Point? result = null;

		for (int y = 0; y < maxY; y++)
		{
			for (int x = 0; x < maxY; x++)
			{
				if (!nodes[x,y] || visited[x,y])
				{
					continue;
				}

				if (distance[x, y] < minDistance)
				{
					minDistance = distance[x, y];
					result = new Point(x, y);
				}
			}
		}
		return result;
	}

	private List<Point> GetPath(Point?[,] prev, Point source, Point destination)
	{
		var path = new List<Point>();
		Point? current = destination;

		while (true)
		{
			if (current == source)
			{
				return path;
			}

			path.Insert(0, current.Value);

			current = prev[current.Value.X, current.Value.Y];
			if (!current.HasValue)
			{
				return null;
			}
		}
	}

	private void Visualize(int round, StringBuilder result)
	{
		if (!showVisualization)
		{
			return;
		}

		result.AppendLine($"After {round} round{(round==1?"":"s")}:");
		for (int y = 0; y < maxY; y++)
		{
			var line = new StringBuilder();
			var playerInfo = new StringBuilder();
			for (int x = 0; x < maxX; x++)
			{
				var player = players.Where(p => (p.Location.X == x) && (p.Location.Y == y)).FirstOrDefault();
				if (player != null)
				{
					string race = player.Race.ToString().Substring(0, 1);
					line.Append(race);
					if (playerInfo.Length > 0)
					{
						playerInfo.Append(", ");
					}
					playerInfo.Append($"{race}({player.Health})");
                    }
				else
				{
					line.Append(map[x, y]);
				}
			}
			result.AppendLine($"{line}   {playerInfo}");
		}
		result.AppendLine();
	}
}
