using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Text;

using AoC.Common;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018
{
	[Export(typeof(IPuzzle))]
	public class Day17 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2018;

		public int Day => 17;

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

		public Day17()
		{
			Inputs.Add("Example Inputs 01", Resources.Day17Inputs01);
			Inputs.Add("Puzzle Inputs", "");

			Solvers.Add("Solve Part 1", SolvePart1);
			Solvers.Add("Solve Part 1 (Take 2)", SolvePart1Take2);
			//Solvers.Add("Solve Part 2", SolvePart2);
		}

		#endregion Constructors

		Point _spring = new Point(500, 0);
		List<Point> _clayPoints = new List<Point>();

		int _minX;
		int _maxX;
		int _minY;
		int _maxY;
		int _width;
		int _height;
		char[,] _map;

		public string SolvePart1(string input)
		{
			var result = new StringBuilder();

			LoadDataFromInput(input);

			Visualize(result);

			// repeat until drops are running out the bottom.
			while (true)
			{
				Point drop = new Point(500, _minY);

				if (Fall(ref drop))
				{
					break;
				}

				//Visualize(result);
			}

			Visualize(result);

			int waterCount = 0;
			int wetSandCount = 0;
			for (int y = 0; y < _height; y++)
			{
				for (int x = 0; x < _width; x++)
				{
					if (_map[x, y] == '~')
					{
						waterCount++;
					}
					if (_map[x, y] == '|')
					{
						wetSandCount++;
					}
				}
			}

			result.AppendLine($"The water can reach {waterCount + wetSandCount} tiles.");
			result.AppendLine($"{waterCount} water tiles are left.");

			return result.ToString();
		}

		//	Courtesy of u/TangentialThinker
		public string SolvePart1Take2(string input)
		{
			var result = new StringBuilder();

			LoadDataFromInput(input);

			//Visualize(result);

			int curX = 500;
			int curY = 0;
			int ans = 0;
			bool done = false;

			// repeat until drops are running out the bottom.
			while (true)
			{
				if (done)
				{
					curX = 500;
					curY = 0;
					done = false;
				}

				while (Map(curX, curY + 1) == '.' && curY <= _maxY - 1)
				{
					curY++;
				}

				if (curY == _maxY)
				{
					// stop once we've reached the lower bound
					done = true;
					Map(curX, curY, '|');
					ans++;
					continue;
				}
				else if (curY <= _minY)
				{
					// we must be at the stream exiting the spring,
					// so we can safely exit
					// do not count tiles which are too high
					break;
				}

				// spread out horizontally
				if (Map(curX - 1, curY) == '.')
				{
					//	while there's something settled under, we can move sideways.
					while (Map(curX - 1, curY) == '.' &&
						(Map(curX, curY + 1) == '#' ||
						Map(curX, curY + 1) == '~'))
					{
						curX--;
					}
				}
				else if (Map(curX + 1, curY) == '.')
				{
					while (Map(curX + 1, curY) == '.' &&
						(Map(curX, curY + 1) == '#' ||
						Map(curX, curY + 1) == '~'))
					{
						curX++;
					}
				}

				if (Map(curX, curY + 1) != '.')
				{
					//	we can no longer move down. 
					//	tile must be at its final position.
					done = true;
					if (Map(curX, curY + 1) == '|' ||
						Map(curX + 1, curY) == '|' ||
						Map(curX - 1, curY) == '|')
					{
						//	if any neighbors below/sideways are unsettled, this tileis unsettled
						Map(curX, curY, '|');
						// we might have incorrectly set some tiles
						// previously as settled, fix this now
						// This works because we're setting tiles
						// in decreasing order of Y for each basin:
						// any errors will be fixed before affecting 
						// anything else
						int x = curX;
						while (Map(x - 1, curY) == '~')
						{
							Map(x - 1, curY, '|');
							x--;
						}
						x = curX;
						while (Map(x + 1, curY) == '~')
						{
							Map(x + 1, curY, '|');
							x++;
						}
					}
					else
					{
						Map(curX, curY, '~');
					}
					ans++;
				}

				//Visualize(result);
			}

			Visualize(result);

			int count = 0;
			for (int y = 0; y < _height; y++)
			{
				for (int x = 0; x < _width; x++)
				{
					if ((_map[x, y] == '~') ||
						(_map[x, y] == '|'))
					{
						count++;
					}
				}
			}

			result.AppendLine($"The water can reach {ans} (or {count}) tiles.");

			return result.ToString();
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

		bool Fall(ref Point drop)
		{
			Point below = drop;
			below.Offset(0, 1);

			while (true)
			{
				if (below.Y > _maxY)
				{
					return true;
				}

				switch (Map(below))
				{
					case '.':
						Map(below, '|');
						drop = below;
						below.Offset(0, 1);
						continue;
					case '|':
						drop = below;
						below.Offset(0, 1);
						continue;
					case '#':
						return Spread(ref drop);
					case '~':
						return Spread(ref drop);
				}
			}
		}

		private bool Spread(ref Point drop)
		{
			Point below = drop;
			below.Offset(0, 1);

			//	Mistake, should not be here.
			if ((Map(below) != '#') &&
				(Map(below) != '~'))
			{
				return Fall(ref drop);
			}

			//	Look left
			Point left = drop;
			left.Offset(-1, 0);
			bool leftFallOff = false;

			while (true)
			{
				below = left;
				below.Offset(0, 1);

				//	Found fall-off point.
				if ((Map(below) != '#') &&
					(Map(below) != '~'))
				{
					leftFallOff = true;
					break;
				}

				if (Map(left) == '.')
				{
					left.Offset(-1, 0);
					continue;
				}

				if (Map(left) == '|')
				{
					left.Offset(-1, 0);
					continue;
				}

				if (Map(left) == '#')
				{
					break;
				}
			}

			//	Look right
			Point right = drop;
			right.Offset(1, 0);
			bool rightFallOff = false;

			while (true)
			{
				below = right;
				below.Offset(0, 1);

				//	Found fall-off point.
				if ((Map(below) != '#') &&
					(Map(below) != '~'))
				{
					rightFallOff = true;
					break;
				}

				if (Map(right) == '.')
				{
					right.Offset(1, 0);
					continue;
				}

				if (Map(right) == '|')
				{
					right.Offset(1, 0);
					continue;
				}

				if (Map(right) == '#')
				{
					break;
				}
			}

			if (leftFallOff || rightFallOff)
			{
				if (!leftFallOff)
				{
					left.Offset(1, 0);
				}
				if (!rightFallOff)
				{
					right.Offset(-1, 0);
				}

				Point fill = left;
				while (fill.X <= right.X)
				{
					Map(fill, '|');
					fill.Offset(1, 0);
				}

				bool leftFall = true;
				bool rightFall = true;
				if (leftFallOff)
				{
					leftFall = Fall(ref left);
				}
				if (rightFallOff)
				{
					rightFall = Fall(ref right);
				}

				return leftFall && rightFall;
			}
			else
			{
				left.Offset(1, 0);
				right.Offset(-1, 0);
				while (left.X <= right.X)
				{
					Map(left, '~');
					left.Offset(1, 0);
				}

				return false;
			}
		}

		public string SolvePart2(string input)
		{
			var result = new StringBuilder();

			LoadDataFromInput(input);

			//

			return result.ToString();
		}

		private void LoadDataFromInput(string input)
		{
			_clayPoints.Clear();

			Helper.TraverseInputLines(input, line =>
			{
				string[] parts = line.Split(new char[] { '=', ',', ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);

				if (string.Equals(parts[0], "x"))
				{
					int x = int.Parse(parts[1]);
					int y1 = int.Parse(parts[3]);
					int y2 = int.Parse(parts[4]);
					for (int y = y1; y <= y2; y++)
					{
						_clayPoints.Add(new Point(x, y));
					}
				}
				else
				{
					int y = int.Parse(parts[1]);
					int x1 = int.Parse(parts[3]);
					int x2 = int.Parse(parts[4]);
					for (int x = x1; x <= x2; x++)
					{
						_clayPoints.Add(new Point(x, y));
					}
				}
			});

			_minX = _clayPoints.Min(p => p.X) - 1;
			_maxX = _clayPoints.Max(p => p.X) + 1;
			_minY = _clayPoints.Min(p => p.Y) - 1;
			_maxY = _clayPoints.Max(p => p.Y);

			//_minY = Math.Min(_minY, 0);

			_width = _maxX - _minX + 1;
			_height = _maxY - _minY + 1;

			_map = new char[_width, _height];
			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					_map[x, y] = '.';
				}
			}

			Map(500, _minY, '+');

			foreach (Point point in _clayPoints)
			{
				Map(point, '#');
			}
		}

		char Map(Point point)
		{
			if (point.X < _minX || point.X > _maxX || point.Y < _minY || point.Y > _maxY)
				return '.';
			return _map[point.X - _minX, point.Y - _minY];
		}

		void Map(Point point, char value)
		{
			if (point.X < _minX || point.X > _maxX || point.Y < _minY || point.Y > _maxY)
				return;
			_map[point.X - _minX, point.Y - _minY] = value;
		}

		char Map(int x, int y)
		{
			if (x < _minX || x > _maxX || y < _minY || y > _maxY)
				return '.';
			return _map[x - _minX, y - _minY];
		}

		void Map(int x, int y, char value)
		{
			if (x < _minX || x > _maxX || y < _minY || y > _maxY)
				return;
			_map[x - _minX, y - _minY] = value;
		}
	}
}
