using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Text;

using AoC.IO;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day17 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 17;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day17Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day17()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	private string SolvePart1(string input)
	{
		var output = new StringBuilder();

		ProcessDataForPart1(input, 2022, output);

		return output.ToString();
	}

	private string SolvePart2(string input)
	{
		var output = new StringBuilder();

		ProcessDataForPart2(input, output);

		return output.ToString();
	}


	private readonly List<char[,]> rockShapes = new()
	{
		new char[4,1] { { '@' }, { '@' }, { '@' }, { '@' } },
		new char[3,3] { { '.', '@', '.' },{ '@', '@', '@' },{ '.', '@', '.' } },
		new char[3,3] { { '@', '.', '.' },{ '@', '.', '.' },{ '@', '@', '@' } },
		new char[1,4] { { '@', '@', '@', '@' }},
		new char[2,2] { { '@', '@' },{ '@', '@' }}
	};

	private void ProcessDataForPart1(string moves1, int rockCount, StringBuilder output = null)
	{
		var moves = moves1.ToCharArray();

		char[,] room = new char[9, 5300];

		room[0, 0] = '+';
		room[8, 0] = '+';

		for (int y = 1; y < 5300; y++)
		{
			room[0, y] = '|';
			room[8, y] = '|';
		}
		for (int x = 1; x < 8; x++)
		{
			for (int y = 1; y < 5300; y++)
			{
				room[x, y] = '.';
			}
			room[x, 0] = '_';
		}

		int moveIndex = 0;
		int towerHeight = 0;
		for (int rockIndex = 0; rockIndex < rockCount; rockIndex++)
		{
			int rockType = rockIndex % 5;
			var rockShape = rockShapes[rockType];
			var rockPosition = new Point(3, towerHeight + 4);

			//VisualizeRoom(room, rockShape, rockPosition, output);

			bool canDrop = true;
			while (canDrop)
			{
				if (moveIndex >= moves.Length)
					moveIndex = 0;
				int move = moves[moveIndex] switch
				{
					'<' => -1,
					'>' => 1,
					_ => 0
				};

				if (move == 0)
				{
					output?.AppendLine($"moves[{moveIndex}] = char({(int)moves[moveIndex]}) <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
					moveIndex++;
					continue;
				}

				output?.Append(moves[moveIndex]);

				bool canMove = true;
				for (int rx = 0; rx < rockShape.GetLength(0) && canMove; rx++)
				{
					for (int ry = 0; ry < rockShape.GetLength(1) && canMove; ry++)
					{
						if (rockShape[rx, ry] == '.')
							continue;

						if (room[rockPosition.X + rx + move, rockPosition.Y + ry] != '.')
							canMove = false;
					}
				}
				if (canMove)
					rockPosition.Offset(move, 0);

				for (int rx = 0; rx < rockShape.GetLength(0) && canDrop; rx++)
				{
					for (int ry = 0; ry < rockShape.GetLength(1) && canDrop; ry++)
					{
						if (rockShape[rx, ry] == '.')
							continue;

						if (room[rockPosition.X + rx, rockPosition.Y + ry - 1] != '.')
							canDrop = false;
					}
				}
				if (canDrop)
				{
					rockPosition.Offset(0, -1);

					moveIndex++;
					continue;
				}

				for (int rx = 0; rx < rockShape.GetLength(0); rx++)
				{
					for (int ry = 0; ry < rockShape.GetLength(1); ry++)
					{
						if (rockShape[rx, ry] == '.')
							continue;

						room[rockPosition.X + rx, rockPosition.Y + ry] = '#';
					}
				}
				moveIndex++;
			}
			output.AppendLine();
			towerHeight = Math.Max(towerHeight, rockPosition.Y + rockShape.GetLength(1) - 1);
			output.AppendLine($"Rock {rockIndex + 1}: Tower height = {towerHeight}");
		}
	}

	private void VisualizeRoom(char[,] room, char[,] rockShape, Point rockPosition, StringBuilder output)
	{
		if (output == null)
			return;

		output.AppendLine();
		for (int y = rockPosition.Y + rockShape.GetLength(1) - 1; y >= 0; y--)
		{
			for (int x = 0; x < 9; x++)
			{
				int rx = x - rockPosition.X;
				int ry = y - rockPosition.Y;
				if (rx >= 0 && rx < rockShape.GetLength(0) &&
					ry >= 0 && ry < rockShape.GetLength(1) &&
					rockShape[rx,ry] != '.')
				{
					output.Append(rockShape[rx, ry]);
					continue;
				}

				output.Append(room[x, y]);
			}

			output.AppendLine();
		}
		output.AppendLine();
	}

	private void ProcessDataForPart2(string moves, StringBuilder output = null)
	{
	}
}
