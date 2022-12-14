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
	internal class Day20 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2018;

		public int Day => 20;

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

		public Day20()
		{
			Inputs.Add("Example Inputs 01", Resources.Day20Inputs01);
			Inputs.Add("Example Inputs 02", Resources.Day20Inputs02);
			Inputs.Add("Example Inputs 03", Resources.Day20Inputs03);
			Inputs.Add("Example Inputs 04", Resources.Day20Inputs04);
			Inputs.Add("Example Inputs 05", Resources.Day20Inputs05);
			Inputs.Add("Puzzle Inputs", "");

			Solvers.Add("Solve Part 1", SolvePart1);
			//Solvers.Add("Solve Part 2", SolvePart2);
		}

		#endregion Constructors

		List<string> _allPaths;

		string _paths;
		char[,] _map;
		int[,] _steps;
		int _minX, _maxX, _minY, _maxY;

		public string SolvePart1(string input)
		{
			var result = new StringBuilder();

			LoadDataFromInput(input);

			//_allPaths = new List<string>();
			//FindAllPaths(_paths);

			//foreach (string path in _allPaths)
			//{
			//	result.AppendLine(path);
			//}
			//result.AppendLine("---------");

			BuildMap(result, _paths);
			DisplayMap(result);

			return result.ToString();
		}

		private void FindAllPaths(string _paths)
		{
			int openIndex = -1;
			int closeIndex = -1;
			for (int i = 0; i < _paths.Length; i++)
			{
				char c = _paths[i];

				if (c == '(')
				{
					openIndex = i;
				}
				if (c == ')')
				{
					closeIndex = i;
					break;
				}
			}

			if (openIndex < 0 && closeIndex < 0)
			{
				if (!_allPaths.Contains(_paths))
				{
					_allPaths.Add(_paths);
				}
				return;
			}

			int startIndex = openIndex + 1;
			for (int i = openIndex + 1; i <= closeIndex; i++)
			{
				char c = _paths[i];
				if (c == '|' || c==')')
				{
					if (i == startIndex)
					{
						string path = _paths.Substring(0, openIndex)
									+ _paths.Substring(closeIndex + 1);
						FindAllPaths(path);
					}
					else
					{
						string path = _paths.Substring(0, openIndex)
									+ _paths.Substring(startIndex, i - startIndex)
									+ _paths.Substring(closeIndex + 1);
						FindAllPaths(path);
					}
					startIndex = i + 1;
				}
			}
		}

		public string SolvePart2(string input)
		{
			var result = new StringBuilder();

			LoadDataFromInput(input);

			//

			return result.ToString();
		}

		private void BuildMap(StringBuilder result, string paths)
		{
			_map = new char[400, 400];
			_steps = new int[400, 400];
			_minX = int.MaxValue;
			_maxX = int.MinValue;
			_minY = int.MaxValue;
			_maxY = int.MinValue;

			int x = 200;
			int y = 200;
			Map(x, y, 'X');
			BuildRoom(x,y);

			StringBuilder path = new StringBuilder();
			path.Append(paths[0]);

			//int pathIndex = 1;
			//FollowPath(paths, pathIndex, x, y, path, result);
			//FollowPath2(paths, x, y, path, result);
			FollowPath3(x, y, paths);

			int longWalks = 0;
			int maxSteps = 0;
			for (y = _minY; y <= _maxY; y++)
			{
				for (x = _minX; x <= _maxX; x++)
				{
					if (_map[x, y] == '?')
					{
						_map[x, y] = '#';
					}
					if (_steps[x, y] > maxSteps)
					{
						maxSteps = _steps[x, y];
					}
					if (_steps[x, y] >= 1000)
					{
						longWalks++;
					}
				}
			}

			result.AppendLine($"Longest route is {maxSteps}");
			result.AppendLine($"{longWalks} rooms are at least 1000 doors away.");
		}

		private void DisplayMap(StringBuilder result)
		{
			for (int y = _minY; y <= _maxY; y++)
			{
				for (int x = _minX; x <= _maxX; x++)
				{
					if (_map[x, y] == (char)0)
					{
						result.Append(' ');
					}
					else
					{
						result.Append(_map[x, y]);
					}
				}
				result.AppendLine();
			}
			result.AppendLine();
		}

		private void FollowPath3(int x, int y, string path)
		{
			int steps = _steps[x, y];
			for (int i = 0; i < path.Length; i++)
			{
				char c = path[i];
				if (c == '(')
				{
					string remaining = path.Substring(i);
					BranchPath3(x, y, remaining);
					return;
				}
				switch (c)
				{
					case 'W':
						Map(--x, y, '|', '?');
						BuildRoom(--x, y);
						Steps(x, y, ++steps);
						break;
					case 'N':
						Map(x, --y, '-', '?');
						BuildRoom(x, --y);
						Steps(x, y, ++steps);
						break;
					case 'E':
						Map(++x, y, '|', '?');
						BuildRoom(++x, y);
						Steps(x, y, ++steps);
						break;
					case 'S':
						Map(x, ++y, '-', '?');
						BuildRoom(x, ++y);
						Steps(x, y, ++steps);
						break;
					case '(':
						BranchPath3(x, y, path.Substring(i));
						return;
					case '$':
						return;
				}
			}
		}

		private void BranchPath3(int x, int y, string path)
		{
			string options = string.Empty;
			string remainder = string.Empty;

			int level = 0;
			for (int i = 1; i < path.Length; i++)
			{
				char c = path[i];
				if (c == '(')
				{
					level++;
				}
				else if (c == ')')
				{
					level--;
				}
				if (level < 0)
				{
					options = path.Substring(1, i - 1);
					remainder = path.Substring(i + 1);
					break;
				}
			}

			int px = x;
			int py = y;

			while (true)
			{
				level = 0;
				bool optionFound = false;
				for (int i = 0; i < options.Length; i++)
				{
					char c = options[i];
					if (c == '(')
					{
						level++;
					}
					else if (c == ')')
					{
						level--;
					}
					else if (c == '|')
					{
						if (level == 0)
						{
							optionFound = true;
							string option = options.Substring(0, i);
							options = options.Substring(i + 1);
							//FollowPath3(x, y, option + remainder);
							FollowPath3(x, y, option);
							break;
						}
					}
				}

				if (!optionFound)
				{
					//FollowPath3(x, y, options + remainder);
					FollowPath3(px, py, options);
					break;
				}
			}

			FollowPath3(x, y, remainder);
		}

		private void FollowPath2(string paths, int x, int y, StringBuilder path, StringBuilder result)
		{
			int i;
			for (i=0; i<paths.Length; i++)
			{
				char c = paths[i];
				if (c == '(')
					break;
				switch (c)
				{
					case 'W':
						Map(--x, y, '|', '?');
						BuildRoom(--x, y);
						//path.Append(c);
						break;
					case 'N':
						Map(x, --y, '-', '?');
						BuildRoom(x, --y);
						//path.Append(c);
						break;
					case 'E':
						Map(++x, y, '|', '?');
						BuildRoom(++x, y);
						//path.Append(c);
						break;
					case 'S':
						Map(x, ++y, '-', '?');
						BuildRoom(x, ++y);
						//path.Append(c);
						break;
				}
			}

			int px = x;
			int py = y;

			if (i == paths.Length)
			{
				return;
			}

			int j = i + 1;
			int level = 0;
			while (level >= 0)
			{
				char c = paths[j];
				if (c == '(')
				{
					level++;
				}
				if (c == ')')
				{
					level--;
				}
				j++;
			}

			string branchPath = paths.Substring(i + 1, j - i - 2);

			while (true)
			{
				int k;
				level = 0;
				for (k = 0; k < branchPath.Length; k++)
				{
					char c = branchPath[k];
					if (c == '(')
					{
						level++;
					}
					if (c == ')')
					{
						level--;
					}
					if (c == '|' && level == 0)
					{
						break;
					}
				}

				FollowPath2(branchPath.Substring(0, k), x, y, path, result);

				if (k == branchPath.Length)
				{
					break;
				}

				branchPath = branchPath.Substring(k + 1);
			}

			if (j < paths.Length - 1)
			{
				FollowPath2(paths.Substring(j + 1), px, py, path, result);
			}
		}

		private void FollowPath(string paths, int pathIndex, int x, int y, StringBuilder path, StringBuilder result)
		{
			while (true)
			{
				char c = paths[pathIndex];
				switch (c)
				{
					case 'W':
						Map(--x, y, '|', '?');
						BuildRoom(--x, y);
						//path.Append(c);
						break;
					case 'N':
						Map(x, --y, '-', '?');
						BuildRoom(x, --y);
						//path.Append(c);
						break;
					case 'E':
						Map(++x, y, '|', '?');
						BuildRoom(++x, y);
						//path.Append(c);
						break;
					case 'S':
						Map(x, ++y, '-', '?');
						BuildRoom(x, ++y);
						//path.Append(c);
						break;
					case '(':
						BranchPath(paths, ++pathIndex, x, y, path, result);
						return;
					case '|':
						//	find closing ')'.
						int level = 0;
						while (level >= 0)
						{
							c = paths[++pathIndex];
							if (c == '(')
							{
								level++;
							}
							if (c == ')')
							{
								level--;
							}
						}
						break;
					case ')':
						//	no need to do anything.
						break;
					case '$':
						//path.Append(c);
						//result.AppendLine(path.ToString());
						return;
				}
				pathIndex++;
			}
		}

		//	Already processed the leading '('.
		private void BranchPath(string paths, int pathIndex, int x, int y, StringBuilder path, StringBuilder result)
		{
			StringBuilder branchPath = new StringBuilder(path.ToString());
			FollowPath(paths, pathIndex, x, y, branchPath, result);

			int level = 0;
			while (true)
			{
				switch (paths[pathIndex])
				{
					case '|':
						if (level == 0)
						{
							branchPath = new StringBuilder(path.ToString());
							FollowPath(paths, pathIndex + 1, x, y, branchPath, result);
						}
						break;
					case '(':
						level++;
						break;
					case ')':
						level--;
						if (level < 0)
						{
							return;
						}
						break;
				}
				pathIndex++;
			}
		}

		private void BuildRoom(int x, int y)
		{
			Map(x - 1, y - 1, '#');
			Map(x - 1, y + 1, '#');
			Map(x + 1, y - 1, '#');
			Map(x + 1, y + 1, '#');
			Map(x, y - 1, '?');
			Map(x, y + 1, '?');
			Map(x - 1, y, '?');
			Map(x + 1, y, '?');
			Map(x, y, '.');
		}

		char Map(int x, int y)
		{
			return _map[x, y + 1];
		}

		void Map(int x, int y, char c, char t = (char)0)
		{
			if (x < 0 || x >= 400 || y < 0 || y >= 400)
			{
				return;
			}

			if (_map[x, y] == t)
			{
				_map[x, y] = c;
			}
			_minX = Math.Min(_minX, x);
			_maxX = Math.Max(_maxX, x);
			_minY = Math.Min(_minY, y);
			_maxY = Math.Max(_maxY, y);
		}

		int Steps(int x, int y)
		{
			if (x < 0 || x >= 400 || y < 0 || y >= 400)
			{
				return 0;
			}

			return _steps[x, y];
		}

		void Steps(int x, int y, int steps)
		{
			if (x < 0 || x >= 400 || y < 0 || y >= 400)
			{
				return;
			}

			if (_steps[x, y] == 0)
			{
				_steps[x, y] = steps;
			}
		}

		private void LoadDataFromInput(string input)
		{
			Helper.TraverseInputLines(input, line =>
			{
				_paths = line;
			});
		}
	}
}
