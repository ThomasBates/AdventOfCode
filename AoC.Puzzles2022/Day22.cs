using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day22 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 22;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day22Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day22(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		LoadFlatMap(input);

		var result = ProcessDataForPart1();

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadCubeMap(input);

		var result = ProcessDataForPart2();

		return result;
	}

	#endregion Solvers

	private class Stats
	{
		public int Min;
		public int Max;
		public int WallCount;
		public int Width => Max - Min + 1;
	}

	private readonly List<char[]> map = new();
	private readonly List<string> instructions = new();
	private readonly List<Stats> rowStats = new();
	private readonly List<Stats> colStats = new();

	private void LoadFlatMap(string input)
	{
		//  First Clear Data
		map.Clear();
		instructions.Clear();
		rowStats.Clear();
		colStats.Clear();

		var mapComplete = false;

		Helper.TraverseInputLines(input, line =>
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				mapComplete = true;
				return;
			}
			if (!mapComplete)
			{
				map.Add(line.ToCharArray());
			}
			else
				ParseInstructions(line);
		}, false);

		int maxWidth = 0;

		for (int r = 0; r < map.Count; r++)
		{
			maxWidth = Math.Max(maxWidth, map[r].Length);
			rowStats.Add(new Stats { Min = int.MaxValue, Max = int.MinValue, WallCount = 0 });
		}
		for (int c = 0; c < maxWidth; c++)
			colStats.Add(new Stats { Min = int.MaxValue, Max = int.MinValue, WallCount = 0 });

		for (int r = 0; r < map.Count; r++)
		{
			for (int c = 0; c < maxWidth; c++)
			{
				char t = c < map[r].Length ? map[r][c] : ' ';
				if (t == ' ') continue;

				if (t == '#')
				{
					rowStats[r].WallCount++;
					colStats[c].WallCount++;
				}

				rowStats[r].Min = Math.Min(rowStats[r].Min, c);
				rowStats[r].Max = Math.Max(rowStats[r].Max, c);
				colStats[c].Min = Math.Min(colStats[c].Min, r);
				colStats[c].Max = Math.Max(colStats[c].Max, r);
			}
		}

		for (int r = 0; r < rowStats.Count; r++)
			if (rowStats[r].WallCount == 0)
				logger.SendWarning(nameof(Day22), $"row {r} has no walls.");
		for (int c = 0; c < maxWidth; c++)
			if (colStats[c].WallCount == 0)
				logger.SendWarning(nameof(Day22), $"column {c} has no walls.");
	}

	private char[,,] cube;
	private int[,] topLeft;
	private Func<int, int, (int, int, int, int)>[,] edgeFunction = new Func<int, int, (int, int, int, int)>[6, 4];

	private void LoadCubeMap(string input)
	{
		//  magic hack
		bool isSampleData = (input.Length < 1000);
		int size = isSampleData ? 4 : 50;

		//  First Clear Data
		cube = new char[6, size, size];

		map.Clear();
		instructions.Clear();

		var mapComplete = false;

		Helper.TraverseInputLines(input, line =>
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				mapComplete = true;
				return;
			}
			if (!mapComplete)
			{
				map.Add(line.ToCharArray());
			}
			else
				ParseInstructions(line);
		}, false);

		if (isSampleData)
		{
			topLeft = new int[6, 2]
			{
				{0, 8},
				{4, 0},
				{4, 4},
				{4, 8},
				{8, 8},
				{8,12},
			};

			for (int face = 0; face < 6; face++)
				ReadCubeFace(face, topLeft[face, 0], topLeft[face, 1]);

			int max = size - 1;
			edgeFunction[0, 0] = (r, c) => (5, 2, max - r, max);
			edgeFunction[0, 1] = (r, c) => (3, 1, 0, c);
			edgeFunction[0, 2] = (r, c) => (2, 1, 0, r);
			edgeFunction[0, 3] = (r, c) => (1, 1, 0, max - c);

			edgeFunction[1, 0] = (r, c) => (2, 0, r, 0);
			edgeFunction[1, 1] = (r, c) => (4, 3, max, max - c);
			edgeFunction[1, 2] = (r, c) => (5, 3, max, max - r);
			edgeFunction[1, 3] = (r, c) => (0, 1, 0, max - c);

			edgeFunction[2, 0] = (r, c) => (3, 0, r, 0);
			edgeFunction[2, 1] = (r, c) => (4, 0, max - c, 0);
			edgeFunction[2, 2] = (r, c) => (1, 2, r, max);
			edgeFunction[2, 3] = (r, c) => (0, 0, c, 0);

			edgeFunction[3, 0] = (r, c) => (5, 1, 0, max - r);
			edgeFunction[3, 1] = (r, c) => (4, 1, 0, c);
			edgeFunction[3, 2] = (r, c) => (2, 2, r, max);
			edgeFunction[3, 3] = (r, c) => (0, 3, max, c);

			edgeFunction[4, 0] = (r, c) => (5, 0, r, 0);
			edgeFunction[4, 1] = (r, c) => (1, 3, max, max - c);
			edgeFunction[4, 2] = (r, c) => (2, 3, max, max - r);
			edgeFunction[4, 3] = (r, c) => (3, 3, max, c);

			edgeFunction[5, 0] = (r, c) => (0, 2, max - r, max);
			edgeFunction[5, 1] = (r, c) => (1, 0, max - c, 0);
			edgeFunction[5, 2] = (r, c) => (4, 2, r, max);
			edgeFunction[5, 3] = (r, c) => (3, 2, max - c, max);
		}
		else
		{
			topLeft = new int[6, 2]
			{
				{  0,  50},
				{  0, 100},
				{ 50,  50},
				{100,   0},
				{100,  50},
				{150,   0},
			};

			for (int face = 0; face < 6; face++)
				ReadCubeFace(face, topLeft[face, 0], topLeft[face, 1]);

			int max = size - 1;
			edgeFunction[0, 0] = (r, c) => (1, 0, r, 0);
			edgeFunction[0, 1] = (r, c) => (2, 1, 0, c);
			edgeFunction[0, 2] = (r, c) => (3, 0, max - r, 0);
			edgeFunction[0, 3] = (r, c) => (5, 0, c, 0);

			edgeFunction[1, 0] = (r, c) => (4, 2, max - r, max);
			edgeFunction[1, 1] = (r, c) => (2, 2, c, max);
			edgeFunction[1, 2] = (r, c) => (0, 2, r, max);
			edgeFunction[1, 3] = (r, c) => (5, 3, max, c);

			edgeFunction[2, 0] = (r, c) => (1, 3, max, r);
			edgeFunction[2, 1] = (r, c) => (4, 1, 0, c);
			edgeFunction[2, 2] = (r, c) => (3, 1, 0, r);
			edgeFunction[2, 3] = (r, c) => (0, 3, max, c);

			edgeFunction[3, 0] = (r, c) => (4, 0, r, 0);
			edgeFunction[3, 1] = (r, c) => (5, 1, 0, c);
			edgeFunction[3, 2] = (r, c) => (0, 0, max - r, 0);
			edgeFunction[3, 3] = (r, c) => (2, 0, c, 0);

			edgeFunction[4, 0] = (r, c) => (1, 2, max - r, max);
			edgeFunction[4, 1] = (r, c) => (5, 2, c, max);
			edgeFunction[4, 2] = (r, c) => (3, 2, r, max);
			edgeFunction[4, 3] = (r, c) => (2, 3, max, c);

			edgeFunction[5, 0] = (r, c) => (4, 3, max, r);
			edgeFunction[5, 1] = (r, c) => (1, 1, 0, c);
			edgeFunction[5, 2] = (r, c) => (0, 1, 0, r);
			edgeFunction[5, 3] = (r, c) => (3, 3, max, c);
		}
	}

	private void ReadCubeFace(int face, int top, int left)
	{
		int r0 = topLeft[face, 0];
		int c0 = topLeft[face, 1];
		for (int r = 0; r < cube.GetLength(1); r++)
		{
			for (int c = 0; c < cube.GetLength(2); c++)
			{
				cube[face, r, c] = map[r0 + r][c0 + c];
			}
		}
	}

	private void ParseInstructions(string line)
	{
		string number = "";
		foreach (char c in line)
		{
			switch (c)
			{
				case 'L':
					if (!string.IsNullOrEmpty(number))
						instructions.Add(number);
					number = "";
					instructions.Add("L");
					break;
				case 'R':
					if (!string.IsNullOrEmpty(number))
						instructions.Add(number);
					number = "";
					instructions.Add("R");
					break;
				default:
					number += c;
					break;
			}
		}
		if (!string.IsNullOrEmpty(number))
			instructions.Add(number);
	}

	private string ProcessDataForPart1()
	{
		int row = 0;
		int col = rowStats[row].Min;
		int dir = 0;

		foreach (var instruction in instructions)
		{
			switch (instruction)
			{
				case "R":
					dir = (dir + 1) % 4;
					logger.SendDebug(nameof(Day22), $"{instruction} => ({row},{col},{dir})");
					continue;
				case "L":
					dir = (dir + 3) % 4;
					logger.SendDebug(nameof(Day22), $"{instruction} => ({row},{col},{dir})");
					continue;
			}

			var count = int.Parse(instruction);
			if (count == 0)
			{
				logger.SendDebug(nameof(Day22), $"{instruction} => ({row},{col},{dir})");
				continue;
			}

			switch (dir)
			{
				case 0: // east
					{
						var stats = rowStats[row];
						if (stats.WallCount == 0)
						{
							count %= stats.Width;
							if (col + count > stats.Max)
								col += count - stats.Width;
							else
								col += count;
							break;
						}

						int c = col;
						for (int i = 0; i < count; i++)
						{
							int next = c + 1;
							if (next > stats.Max)
								next = stats.Min;
							if (map[row][next] == '#')
							{
								col = c;
								break;
							}
							c = next;
						}
						col = c;
					}
					break;
				case 1: // south
					{
						var stats = colStats[col];
						if (stats.WallCount == 0)
						{
							count %= stats.Width;
							if (row + count > stats.Max)
								row += count - stats.Width;
							else
								row += count;
							break;
						}

						int r = row;
						for (int i = 0; i < count; i++)
						{
							int next = r + 1;
							if (next > stats.Max)
								next = stats.Min;
							if (map[next][col] == '#')
							{
								row = r;
								break;
							}
							r = next;
						}
						row = r;
					}
					break;
				case 2: // west
					{
						var stats = rowStats[row];
						if (stats.WallCount == 0)
						{
							count %= stats.Width;
							if (col - count < stats.Min)
								col -= count - stats.Width;
							else
								col -= count;
							break;
						}

						int c = col;
						for (int i = 0; i < count; i++)
						{
							int next = c - 1;
							if (next < stats.Min)
								next = stats.Max;
							if (map[row][next] == '#')
							{
								col = c;
								break;
							}
							c = next;
						}
						col = c;
					}
					break;
				case 3: // north
					{
						var stats = colStats[col];
						if (stats.WallCount == 0)
						{
							count %= stats.Width;
							if (row - count < stats.Min)
								row -= count - stats.Width;
							else
								row -= count;
							break;
						}

						int r = row;
						for (int i = 0; i < count; i++)
						{
							int next = r - 1;
							if (next < stats.Min)
								next = stats.Max;
							if (map[next][col] == '#')
							{
								row = r;
								break;
							}
							r = next;
						}
						row = r;
					}
					break;
			}
			logger.SendDebug(nameof(Day22), $"{instruction} => ({row},{col},{dir})");
		}

		return ((row + 1) * 1000 + (col + 1) * 4 + dir).ToString();
	}

	private string ProcessDataForPart2()
	{
		int face = 0;
		int row = 0;
		int col = 0;
		int dir = 0;

		foreach (var instruction in instructions)
		{
			switch (instruction)
			{
				case "R":
					dir = (dir + 1) % 4;
					logger.SendDebug(nameof(Day22), $"{instruction} => ({face},{row},{col},{dir})");
					continue;
				case "L":
					dir = (dir + 3) % 4;
					logger.SendDebug(nameof(Day22), $"{instruction} => ({face},{row},{col},{dir})");
					continue;
			}

			var count = int.Parse(instruction);
			if (count == 0)
			{
				logger.SendDebug(nameof(Day22), $"{instruction} => ({face},{row},{col},{dir})");
				continue;
			}

			bool blocked = false;

			for (int i = 0; i < count & !blocked; i++)
			{
				switch (dir)
				{
					case 0: // east
						{
							int c = col + 1;
							if (c >= cube.GetLength(2))
							{
								var (newFace, newDir, newRow, newCol) = edgeFunction[face, dir](row, c);
								if (cube[newFace, newRow, newCol] == '#')
								{
									blocked = true;
									break;
								}
								(face, row, col, dir) = (newFace, newRow, newCol, newDir);
								break;
							}
							if (cube[face, row, c] == '#')
							{
								blocked = true;
								break;
							}
							col = c;
						}
						break;
					case 1: // south
						{
							int r = row + 1;
							if (r >= cube.GetLength(1))
							{
								var (newFace, newDir, newRow, newCol) = edgeFunction[face, dir](r, col);
								if (cube[newFace, newRow, newCol] == '#')
								{
									blocked = true;
									break;
								}
								(face, row, col, dir) = (newFace, newRow, newCol, newDir);
								break;
							}
							if (cube[face, r, col] == '#')
							{
								blocked = true;
								break;
							}
							row = r;
						}
						break;
					case 2: // west
						{
							int c = col - 1;
							if (c < 0)
							{
								var (newFace, newDir, newRow, newCol) = edgeFunction[face, dir](row, c);
								if (cube[newFace, newRow, newCol] == '#')
								{
									blocked = true;
									break;
								}
								(face, row, col, dir) = (newFace, newRow, newCol, newDir);
								break;
							}
							if (cube[face, row, c] == '#')
							{
								blocked = true;
								break;
							}
							col = c;
						}
						break;
					case 3: // north
						{
							int r = row - 1;
							if (r < 0)
							{
								var (newFace, newDir, newRow, newCol) = edgeFunction[face, dir](r, col);
								if (cube[newFace, newRow, newCol] == '#')
								{
									blocked = true;
									break;
								}
								(face, row, col, dir) = (newFace, newRow, newCol, newDir);
								break;
							}
							if (cube[face, r, col] == '#')
							{
								blocked = true;
								break;
							}
							row = r;
						}
						break;
				}
			}
			logger.SendDebug(nameof(Day22), $"{instruction} => ({face},{row},{col},{dir})");
		}

		int mapRow = topLeft[face, 0] + row + 1;
		int mapCol = topLeft[face, 1] + col + 1;

		return $"({mapRow},{mapCol},{dir}) => {mapRow * 1000 + mapCol * 4 + dir}";
	}
}
