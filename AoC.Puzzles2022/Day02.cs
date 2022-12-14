using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using AoC.IO;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day02 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2022;

		public int Day => 2;

		public string Name => $"Day {Day:00}";

		public Dictionary<string, string> Inputs { get; } = new()
		{
			{"Example Inputs", Resources.Day02Inputs},
			{"Puzzle Inputs", ""}
		};

		public Dictionary<string, Func<string, string>> Solvers { get; } = new()
		{
			{ "Part 1", SolvePart1 },
			{ "Part 2", SolvePart2 }
		};

		#endregion IPuzzle Properties

		/*
		you	me	result	score
		1	1	3		4
		1	2	6		8
		1	3	0		3
		2	1	0		1
		2	2	3		5
		2	3	6		9
		3	1	6		7
		3	2	0		2
		3	3	3		6
		*/
		public static string SolvePart1(string input)
		{
			int[,] scores = new int[3, 3] { { 4, 8, 3 }, { 1, 5, 9 }, { 7, 2, 6 } };

			return Solve(input, scores);
		}

		/*
		you	result	me	score
		1	1		3	3
		1	2		1	4
		1	3		2	8
		2	1		1	1
		2	2		2	5
		2	3		3	9
		3	1		2	2
		3	2		3	6
		3	3		1	7
		*/
		public static string SolvePart2(string input)
		{
			int[,] scores = new int[3, 3] { { 3, 4, 8 }, { 1, 5, 9 }, { 2, 6, 7 } };

			return Solve(input, scores);
		}

		private static string Solve(string input, int[,] scores)
		{
			var output = new StringBuilder();

			int total = 0;
			int you = -1;
			int me = -1;
			Helper.TraverseInputTokens(input, value =>
			{
				switch (value)
				{
					case "A": you = 0; break;
					case "B": you = 1; break;
					case "C": you = 2; break;

					case "X": me = 0; break;
					case "Y": me = 1; break;
					case "Z": me = 2; break;
				}

				output.Append(value + " ");

				if (you >= 0 && me >= 0)
				{
					int score = scores[you, me];
					output.AppendLine($" {score}");
					total += score;
					me = -1;
				}
			});

			output.AppendLine($"The total score is {total}.");

			return output.ToString();
		}
	}
}
