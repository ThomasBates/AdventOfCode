using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using AoC.IO;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day01 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2022;

		public int Day => 1;

		public string Name => $"Day {Day:00}";

		public Dictionary<string, string> Inputs { get; } = new()
		{
			{"Example Inputs", Resources.Day01Inputs},
			{"Puzzle Inputs", ""}
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

			int total = 0;
			int max = 0;
			Helper.TraverseInputLines(input, value =>
			{
				output.AppendLine(value);
				if (int.TryParse(value, out var calories))
				{
					total += calories;
				}
				else
				{
					output.AppendLine($"Total = {total}");
					max = Math.Max(max, total);
					total = 0;
				}
			}, false);

			output.AppendLine($"Max = {max}");

			//return $"The end frequency is {frequency}.";
			return output.ToString();
		}

		private class Elf
		{
			public List<int> Calories = new();
			public int Total;
		}

		public static string SolvePart2(string input)
		{
			var elves = new List<Elf>();
			var currentElf = new Elf();
			elves.Add(currentElf);

			var output = new StringBuilder();

			Helper.TraverseInputLines(input, value =>
			{
				output.AppendLine(value);
				if (int.TryParse(value, out var calories))
				{
					currentElf.Calories.Add(calories);
					currentElf.Total += calories;
				}
				else
				{
					output.AppendLine($"Total = {currentElf.Total}");
					currentElf = new Elf();
					elves.Add(currentElf);
				}
			}, false);

			var orderedElves = elves.OrderByDescending(elf => elf.Total).ToList();

			int top3 = 0;
			for (int i = 0; i < 3; i++)
			{
				output.AppendLine($"{i + 1}: {orderedElves[i].Total}");
				top3 += orderedElves[i].Total;
			}

			output.AppendLine($"Top 3 = {top3}");

			//return $"The end frequency is {frequency}.";
			return output.ToString();
		}
	}
}
