using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Text;
using AoC.IO;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day12 : IPuzzle
	{
		#region IPuzzle Properties

		public string Name => "Day 12";

		public Dictionary<string, string> Inputs { get; } = new Dictionary<string, string>()
		{
			{"Example Inputs", Resources.Day12ExampleInputs},
			{"Puzzle Inputs",  Resources.Day12PuzzleInputs}
		};

		public Dictionary<string, Func<string, string>> Solvers { get; } = new Dictionary<string, Func<string, string>>();

		#endregion IPuzzle Properties

		public Day12()
		{
			Solvers.Add("Part 1", SolvePart1);
			Solvers.Add("Part 2", SolvePart2);
		}

		private readonly List<char[]> map = new List<char[]>();
		private readonly Dictionary<Point, List<Point>> transitions = new Dictionary<Point, List<Point>>();
		private readonly Dictionary<Point, List<Point>> reverse = new Dictionary<Point, List<Point>>();
		private readonly List<Point> allStarts = new List<Point>();

		private int _maxX;
		private int _maxY;
		private Point start = Point.Empty;
		private Point end = Point.Empty;

		private string SolvePart1(string input)
		{
			StringBuilder output = new StringBuilder();

			map.Clear();
			transitions.Clear();
			reverse.Clear();
			allStarts.Clear();

			Helper.TraverseInputLines(input, line =>
			{
				map.Add(line.ToCharArray());
			});

			LoadMap();

			var path = FindPath(start, end);

			if (path == null)
			{
				output.AppendLine("Path length = 0");
			}
			else
			{
				output.AppendLine($"Path length = {path.Count}");

				foreach (var step in path)
				{
					output.AppendLine(step.ToString());
					map[step.X][step.Y] = char.ToUpper(map[step.X][step.Y]);
				}

				foreach(var line in map)
					output.AppendLine(new string(line));
			}

			return output.ToString();
		}

		private string SolvePart2(string input)
		{
			StringBuilder output = new StringBuilder();

			map.Clear();
			transitions.Clear();
			reverse.Clear();
			allStarts.Clear();

			Helper.TraverseInputLines(input, line =>
			{
				map.Add(line.ToCharArray());
			});

			LoadMap();

			var path = FindPathDown(end, allStarts);

			if (path == null)
			{
				output.AppendLine("No path");
			}
			else
			{
				output.AppendLine($"Path length = {path.Count}");

				foreach (var step in path)
				{
					output.AppendLine(step.ToString());
					map[step.X][step.Y] = char.ToUpper(map[step.X][step.Y]);
				}

				foreach (var line in map)
					output.AppendLine(new string(line));
			}

			return output.ToString();
		}


		private void LoadMap()
		{
			Size[] directions = new Size[] { new Size(0, 1), new Size(0, -1), new Size(1, 0), new Size(-1, 0) };

			int _maxX = map.Count;
			int _maxY = map[0].Length;

			for (int row = 0; row < _maxX; row++)
			{
				for (int col = 0; col < _maxY; col++)
				{
					var location = new Point(row, col);
					transitions.Add(location, new List<Point>());
					reverse.Add(location, new List<Point>());

					char height = map[row][col];

					if (height == 'S')
					{
						start = location;
						height = 'a';
					}
					if (height == 'E')
					{
						end = location;
						height = 'z';
					}
					if (height == 'a')
						allStarts.Add(location);

					foreach (var dir in directions)
					{
						var neighbor = location + dir;
						if (neighbor.X < 0 || neighbor.Y < 0)
							continue;
						if (neighbor.X >= _maxX || neighbor.Y >= _maxY)
							continue;

						var neighborHeight = map[neighbor.X][neighbor.Y];
						if (neighborHeight == 'S') neighborHeight = 'a';
						if (neighborHeight == 'E') neighborHeight = 'z';

						if (neighborHeight < height + 2)
							transitions[location].Add(neighbor);
						if (neighborHeight > height - 2)
							reverse[location].Add(neighbor);
					}
				}
			}
		}

		private List<Point> FindPath(Point source, Point destination)
		{
			_maxX = map.Count;
			_maxY = map[0].Length;

			bool[,] visited = new bool[_maxX, _maxY];
			int[,] distance = new int[_maxX, _maxY];
			Point?[,] prev = new Point?[_maxX, _maxY];

			for (int x = 0; x < _maxX; x++)
			{
				for (int y = 0; y < _maxY; y++)
				{
					visited[x, y] = false;
					distance[x, y] = int.MaxValue;
					prev[x, y] = null;
				}
			}

			distance[source.X, source.Y] = 0;
			Point? current = new Point(source.X, source.Y);

			while (true)
			{
				foreach (var neighbor in transitions[current.Value])
					CheckNode(visited, distance, prev, current.Value, neighbor);

				visited[current.Value.X, current.Value.Y] = true;

				if (current == destination)
				{
					return GetPath(prev, source, destination);
				}

				current = GetCurrent(visited, distance);

				if (!current.HasValue)
				{
					return null;
				}
			}
		}

		private List<Point> FindPathDown(Point source, List<Point> destinations)
		{
			_maxX = map.Count;
			_maxY = map[0].Length;

			bool[,] visited = new bool[_maxX, _maxY];
			int[,] distance = new int[_maxX, _maxY];
			Point?[,] prev = new Point?[_maxX, _maxY];

			for (int x = 0; x < _maxX; x++)
			{
				for (int y = 0; y < _maxY; y++)
				{
					visited[x, y] = false;
					distance[x, y] = int.MaxValue;
					prev[x, y] = null;
				}
			}

			distance[source.X, source.Y] = 0;
			Point? current = new Point(source.X, source.Y);

			while (true)
			{
				foreach (var neighbor in reverse[current.Value])
					CheckNode(visited, distance, prev, current.Value, neighbor);

				visited[current.Value.X, current.Value.Y] = true;

				if (destinations.Contains(current.Value))
				{
					return GetPath(prev, source, current.Value);
				}

				current = GetCurrent(visited, distance);

				if (!current.HasValue)
				{
					return null;
				}
			}
		}

		private void CheckNode(bool[,] visited, int[,] distance, Point?[,] prev, Point current, Point neighbor)
		{
			if (visited[neighbor.X, neighbor.Y])
				return;

			int d = distance[current.X, current.Y] + 1;
			if (d < distance[neighbor.X, neighbor.Y])
			{
				distance[neighbor.X, neighbor.Y] = d;
				prev[neighbor.X, neighbor.Y] = current;
			}
		}

		private Point? GetCurrent(bool[,] visited, int[,] distance)
		{
			int minDistance = int.MaxValue;
			Point? result = null;

			for (int y = 0; y < _maxY; y++)
			{
				for (int x = 0; x < _maxX; x++)
				{
					if (visited[x, y])
						continue;

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

	}
}
