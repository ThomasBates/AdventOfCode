﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day08 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 8;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day08Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new()
	{
		{ "Part 1", SolvePart1 },
		{ "Part 2", SolvePart2 }
	};

	#endregion IPuzzle Properties

	public static string SolvePart1(string input)
	{
		var output = new StringBuilder();

		var forest = new List<string>();

		InputHelper.TraverseInputLines(input, line =>
		{
			forest.Add(line);
		});

		int numVisible = 0;
		for (int row = 0; row < forest.Count; row++)
		{
			for (int col = 0; col < forest[row].Length; col++)
			{
				char tree = forest[row][col];
				bool visible =
					row == 0 || row == forest.Count - 1 ||
					col == 0 || col == forest[row].Length - 1;

				if (!visible)
				{
					visible = true;
					for (int i = row - 1; i >= 0 & visible; i--)
					{
						if (forest[i][col] >= tree)
							visible = false;
					}
				}

				if (!visible)
				{
					visible = true;
					for (int i = row + 1; i < forest.Count & visible; i++)
					{
						if (forest[i][col] >= tree)
							visible = false;
					}
				}

				if (!visible)
				{
					visible = true;
					for (int i = col - 1; i >= 0 & visible; i--)
					{
						if (forest[row][i] >= tree)
							visible = false;
					}
				}

				if (!visible)
				{
					visible = true;
					for (int i = col + 1; i < forest[row].Length & visible; i++)
					{
						if (forest[row][i] >= tree)
							visible = false;
					}
				}

				if (visible)
				{
					numVisible++;
					output.Append(tree);
				}
				else
				{
					output.Append('.');
				}
			}
			output.AppendLine();
		}

		output.AppendLine($"The answer is {numVisible}");

		return output.ToString();
	}

	public static string SolvePart2(string input)
	{
		var output = new StringBuilder();

		var forest = new List<string>();

		InputHelper.TraverseInputLines(input, line =>
		{
			forest.Add(line);
		});

		int bestScore = 0;
		for (int row = 0; row < forest.Count; row++)
		{
			for (int col = 0; col < forest[row].Length; col++)
			{
				char tree = forest[row][col];

				int score1 = 0;
				for (int i = row - 1; i >= 0; i--)
				{
					score1++;
					if (forest[i][col] >= tree)
						break;
				}

				int score2 = 0;
				for (int i = row + 1; i < forest.Count; i++)
				{
					score2++;
					if (forest[i][col] >= tree)
						break;
				}

				int score3 = 0;
				for (int i = col - 1; i >= 0; i--)
				{
					score3++;
					if (forest[row][i] >= tree)
						break;
				}

				int score4 = 0;
				for (int i = col + 1; i < forest[row].Length; i++)
				{
					score4++;
					if (forest[row][i] >= tree)
						break;
				}

				int score = score1 * score2 * score3 * score4;

				bestScore = Math.Max(bestScore, score);
			}
		}

		output.AppendLine($"The answer is {bestScore}");

		return output.ToString();
	}
}
