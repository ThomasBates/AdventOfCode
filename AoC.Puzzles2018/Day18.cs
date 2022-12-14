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
	public class Day18 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2018;

		public int Day => 18;

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

		public Day18()
		{
			Inputs.Add("Example Inputs 01", Resources.Day18Inputs01);
			Inputs.Add("Example Inputs 02", Resources.Day18Inputs02);
			Inputs.Add("Puzzle Inputs", "");

			Solvers.Add("Solve Part 1", SolvePart1);
			Solvers.Add("Solve Part 2", SolvePart2);
		}

		#endregion Constructors

		char[,] _map;
		int _width;
		int _height;

		public string SolvePart1(string input)
		{
			var result = new StringBuilder();

			LoadDataFromInput(input);

			Visualize(result);

			for (int minute = 0; minute < 10; minute++)
			{
				ProcessMinute();

				Visualize(result);
			}   //	for minutes

			//Visualize(result);

			int openCount = 0;
			int treesCount = 0;
			int lumberCount = 0;

			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					if (_map[x, y] == '.')
						openCount++;
					if (_map[x, y] == '|')
						treesCount++;
					if (_map[x, y] == '#')
						lumberCount++;
				}
			}

			result.AppendLine($"After {10} minutes, there are {treesCount} wooded acres and {lumberCount} lumberyards.");
			result.AppendLine($"Result = {treesCount * lumberCount}.");


			return result.ToString();
		}

		private void ProcessMinute()
		{
			char[,] nextMap = new char[_width, _height];

			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					int openCount = 0;
					int treesCount = 0;
					int lumberCount = 0;

					for (int i = -1; i <= 1; i++)
					{
						for (int j = -1; j <= 1; j++)
						{
							if (i == 0 && j == 0)
								continue;
							if (x + i < 0 || x + i >= _width ||
								y + j < 0 || y + j >= _height)
								continue;
							if (_map[x + i, y + j] == '.')
								openCount++;
							if (_map[x + i, y + j] == '|')
								treesCount++;
							if (_map[x + i, y + j] == '#')
								lumberCount++;
						}
					}

					switch (_map[x, y])
					{
						case '.':
							if (treesCount >= 3)
								nextMap[x, y] = '|';
							else
								nextMap[x, y] = '.';
							break;
						case '|':
							if (lumberCount >= 3)
								nextMap[x, y] = '#';
							else
								nextMap[x, y] = '|';
							break;
						case '#':
							if (lumberCount >= 1 && treesCount >= 1)
								nextMap[x, y] = '#';
							else
								nextMap[x, y] = '.';
							break;
					}
				}   //	for y
			}   //	for x

			_map = nextMap;
		}

		public string SolvePart2(string input)
		{
			var result = new StringBuilder();

			LoadDataFromInput(input);

			var treesList = new List<int>();
			var lumberList = new List<int>();
			var resultList = new List<int>();

			int openCount = 0;
			int treesCount = 0;
			int lumberCount = 0;

			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					if (_map[x, y] == '.')
						openCount++;
					if (_map[x, y] == '|')
						treesCount++;
					if (_map[x, y] == '#')
						lumberCount++;
				}
			}

			result.AppendLine($"{0}: {treesCount} * {lumberCount} = {treesCount * lumberCount}");
			treesList.Add(treesCount);
			lumberList.Add(lumberCount);
			resultList.Add(treesCount * lumberCount);

			for (int minute = 1; minute <= 1000; minute++)
			{
				ProcessMinute();

				openCount = 0;
				treesCount = 0;
				lumberCount = 0;

				for (int x = 0; x < _width; x++)
				{
					for (int y = 0; y < _height; y++)
					{
						if (_map[x, y] == '.')
							openCount++;
						if (_map[x, y] == '|')
							treesCount++;
						if (_map[x, y] == '#')
							lumberCount++;
					}
				}

				result.AppendLine($"{minute}: {treesCount} * {lumberCount} = {treesCount * lumberCount}");
				treesList.Add(treesCount);
				lumberList.Add(lumberCount);
				resultList.Add(treesCount * lumberCount);

			}   //	for minutes

			//	Find periodicity in lists.
			//	Find when periodicity begins.
			//	Find element of periodicity that corresponds with future time.

			int minY = Math.Min(treesList.Min(r => r), lumberList.Min(r => r));
			int maxY = Math.Max(treesList.Max(r => r), lumberList.Max(r => r));
			int width = resultList.Count;
			int height = maxY - minY + 1;
			char[,] graph = new char[width, height];

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					graph[x, y] = '.';
				}
			}

			for (int m = 0; m < treesList.Count; m++)
			{
				int r = treesList[m];
				graph[m, r - minY] = 'X';
			}

			for (int m = 0; m < lumberList.Count; m++)
			{
				int r = lumberList[m];
				graph[m, r - minY] = '#';
			}

			for (int y = height - 1; y >= 0; y--)
			{
				for (int x = 0; x < width; x++)
				{
					result.Append(graph[x, y]);
				}
				result.AppendLine();
			}
			result.AppendLine();

			long future = 1000000000L;
			while (future > 1000)
			{
				future -= 28;
			}

			result.AppendLine($"Minute {future} has the same values as Minute 1,000,000,000.");
			result.AppendLine($"After {future} minutes, there are {treesList[(int)future]} wooded acres and {lumberList[(int)future]} lumberyards.");
			result.AppendLine($"Result = {resultList[(int)future]}.");

			return result.ToString();
		}

		private void LoadDataFromInput(string input)
		{
			List<string> lines = new List<string>();

			Helper.TraverseInputLines(input, line =>
			{
				lines.Add(line);
			});

			_width = lines.Max(l => l.Length);
			_height = lines.Count;

			_map = new char[_width, _height];

			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					_map[x, y] = lines[y][x];
				}
			}
		}

		void Visualize(StringBuilder result)
		{
			for (int y = 0; y < _height; y++)
			{
				for (int x = 0; x < _width; x++)
				{
					result.Append(_map[x, y]);
				}
				result.AppendLine();
			}
			result.AppendLine();
		}
	}
}
