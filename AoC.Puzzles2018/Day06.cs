using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AoC.IO;
using AoC.IO.SegmentList;
using AoC.Parser;
using AoC.Puzzle;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018
{
	[Export(typeof(IPuzzle))]
	public class Day06 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2018;

		public int Day => 6;

		public string Name => $"Day {Day:00}";

		public Dictionary<string, string> Inputs
		{
			get;
		} = new Dictionary<string, string>();

		public Dictionary<string, Func<string, string>> Solvers
		{
			get;
		} = new Dictionary<string, Func<string, string>>();

		#endregion IPuzzle Properties

		#region Constructors

		public Day06()
		{
			Inputs.Add("Example Inputs", Resources.Day06Inputs);
			Inputs.Add("Puzzle Inputs", "");

			Solvers.Add("Solve Part 1", SolvePart1);
			Solvers.Add("Solve Part 2", SolvePart2);
		}

		#endregion Constructors

		public string SolvePart1(string input)
		{
			List<Point> points = new List<Point>();

			Helper.TraverseInputLines(input, line =>
			{
				string[] coords = line.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);

				points.Add(new Point(int.Parse(coords[0]), int.Parse(coords[1])));
			});

			HashSet<int> infinitePoints = new HashSet<int>();

			//	Find "infinite" points.
			for (int i = -1000; i < 1000; i++)
			{
				infinitePoints.Add(GetNearestPointIndex(new Point(i, -1000), points));
				infinitePoints.Add(GetNearestPointIndex(new Point(i, 1000), points));
				infinitePoints.Add(GetNearestPointIndex(new Point(-1000, i), points));
				infinitePoints.Add(GetNearestPointIndex(new Point(1000, i), points));
			}

			int minX = points.OrderBy(point => point.X).First().X;
			int maxX = points.OrderByDescending(point => point.X).First().X;
			int minY = points.OrderBy(point => point.Y).First().Y;
			int maxY = points.OrderByDescending(point => point.Y).First().Y;

			var nearestPointTally = new Dictionary<int, int>();
			for (int i = 0; i < points.Count; i++)
			{
				if (!infinitePoints.Contains(i))
				{
					nearestPointTally.Add(i, 0);
				}
			}

			for (int x = minX; x <= maxX; x++)
			{
				for (int y = minY; y <= maxY; y++)
				{
					int nearestPoint = GetNearestPointIndex(new Point(x, y), points);
					if (nearestPointTally.ContainsKey(nearestPoint))
					{
						nearestPointTally[nearestPoint]++;
					}
				}
			}

			var ppp = nearestPointTally.OrderByDescending(p => p.Value).First();


			return $"The size of the largest area is {ppp.Value} at point {ppp.Key}.";
		}

		public string SolvePart2(string input)
		{
			List<Point> points = new List<Point>();

			Helper.TraverseInputLines(input, line =>
			{
				string[] coords = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

				points.Add(new Point(int.Parse(coords[0]), int.Parse(coords[1])));
			});

			int minX = points.OrderBy(point => point.X).First().X;
			int maxX = points.OrderByDescending(point => point.X).First().X;
			int minY = points.OrderBy(point => point.Y).First().Y;
			int maxY = points.OrderByDescending(point => point.Y).First().Y;

			int regionSize = 0;

			for (int x = minX; x <= maxX; x++)
			{
				for (int y = minY; y <= maxY; y++)
				{
					int totalDistance = 0;
					foreach (var point in points)
					{
						totalDistance += Distance(point, new Point(x, y));
					}
					if (totalDistance < 10000)
					{
						regionSize++;
					}
				}
			}

			return $"The size of the region is {regionSize}.";
		}

		private int GetNearestPointIndex(Point point, List<Point> points)
		{
			var distances = new Dictionary<int, int>();

			for (int index = 0; index < points.Count; index++)
			{
				int distance = Distance(point, points[index]);
				distances.Add(index, distance);
			}

			var minDistances = distances.OrderBy(d => d.Value).ToArray();
			if (minDistances[0].Value == minDistances[1].Value)
			{
				return -1;
			}

			return minDistances[0].Key;
		}

		int Distance(Point a, Point b)
		{
			return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
		}
	}
}
