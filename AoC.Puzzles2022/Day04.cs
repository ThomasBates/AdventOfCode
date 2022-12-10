using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AoC.IO;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day04 : IPuzzle
	{
		#region IPuzzle Properties

		public string Name => "Day 04";

		public Dictionary<string, string> Inputs { get; } = new Dictionary<string, string>()
		{
			{"Example Inputs", Resources.Day04ExampleInputs},
			{"Puzzle Inputs",  Resources.Day04PuzzleInputs}
		};

		public Dictionary<string, Func<string, string>> Solvers { get; } = new Dictionary<string, Func<string, string>>()
		{
			{ "Part 1 (split)", SolvePart1_Split },
			{ "Part 1 (regex)", SolvePart1_Regex },
			{ "Part 2 (split)", SolvePart2_Split },
			{ "Part 2 (regex)", SolvePart2_Regex },
		};

		#endregion IPuzzle Properties

		public static string SolvePart1_Split(string input)
		{
			StringBuilder output = new StringBuilder();

			int total = 0;

			Helper.TraverseInputLines(input, line =>
			{
				var values = line.Split(',', '-');

				output.Append(string.Join(", ", values));

				int min1 = int.Parse(values[0]);
				int max1 = int.Parse(values[1]);
				int min2 = int.Parse(values[2]);
				int max2 = int.Parse(values[3]);

				if ((min1 <= min2 && max2 <= max1) ||
					(min2 <= min1 && max1 <= max2))
				{
					total++;
					output.AppendLine("  <<====");
				}
				else
					output.AppendLine();
			});

			output.AppendLine($"The answer is {total}");

			return output.ToString();
		}

		public static string SolvePart1_Regex(string input)
		{
			StringBuilder output = new StringBuilder();

			int total = 0;

			Helper.TraverseInputLines(input, line =>
			{
				Match match = Regex.Match(line, @"(\d+)-(\d+),(\d+)-(\d+)");

				int min1 = int.Parse(match.Groups[1].Value);
				int max1 = int.Parse(match.Groups[2].Value);
				int min2 = int.Parse(match.Groups[3].Value);
				int max2 = int.Parse(match.Groups[4].Value);

				output.Append($"{min1}-{max1},{min2}-{max2}");

				if ((min1 <= min2 && max2 <= max1) ||
					(min2 <= min1 && max1 <= max2))
				{
					total++;
					output.AppendLine("  <<====");
				}
				else
					output.AppendLine();
			});

			output.AppendLine($"The answer is {total}");

			return output.ToString();
		}

		public static string SolvePart2_Split(string input)
		{
			StringBuilder output = new StringBuilder();

			int total = 0;

			Helper.TraverseInputLines(input, line =>
			{
				var values = line.Split(',', '-');

				output.Append(string.Join(", ", values));

				int min1 = int.Parse(values[0]);
				int max1 = int.Parse(values[1]);
				int min2 = int.Parse(values[2]);
				int max2 = int.Parse(values[3]);

				if (min1 <= max2 && min2 <= max1)
				{
					total++;
					output.AppendLine("  <<====");
				}
				else
					output.AppendLine();
			});

			output.AppendLine($"The answer is {total}");

			return output.ToString();
		}

		public static string SolvePart2_Regex(string input)
		{
			StringBuilder output = new StringBuilder();

			int total = 0;

			Helper.TraverseInputLines(input, line =>
			{
				Match match = Regex.Match(line, @"(\d+)-(\d+),(\d+)-(\d+)");

				int min1 = int.Parse(match.Groups[1].Value);
				int max1 = int.Parse(match.Groups[2].Value);
				int min2 = int.Parse(match.Groups[3].Value);
				int max2 = int.Parse(match.Groups[4].Value);

				output.Append($"{min1}-{max1},{min2}-{max2}");

				if (min1 <= max2 && min2 <= max1)
				{
					total++;
					output.AppendLine("  <<====");
				}
				else
					output.AppendLine();
			});

			output.AppendLine($"The answer is {total}");

			return output.ToString();
		}
	}
}
