using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using AoC.Common;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day06 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2022;

		public int Day => 6;

		public string Name => $"Day {Day:00}";

		public Dictionary<string, string> Inputs { get; } = new()
		{
			{"Example Inputs", Resources.Day06Inputs},
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

			Helper.TraverseInputLines(input, line =>
			{
				bool found = false;
				for (int i = 0; i < line.Length - 3; i++)
				{
					string word = line.Substring(i, 4);
					if (word[0] != word[1] &&
						word[0] != word[2] &&
						word[0] != word[3] &&
						word[1] != word[2] &&
						word[1] != word[3] &&
						word[2] != word[3])
					{
						found = true;
						output.AppendLine($"The answer is {i + 4}");
						break;
					}
				}
				if (!found)
					output.AppendLine($"The answer is not found");
			});

			return output.ToString();
		}

		public static string SolvePart2(string input)
		{
			var output = new StringBuilder();

			Helper.TraverseInputLines(input, line =>
			{
				bool found = false;
				for (int i = 0; i < line.Length - 3; i++)
				{
					string word = line.Substring(i, 14);

					bool match = false;
					for (int j = 0; j < 13 && !match; j++)
					{
						for (int k = j + 1; k < 14 && !match; k++)
						{
							if (word[j] == word[k])
							{
								match = true;
							}
						}
					}

					if (!match)
					{
						found = true;
						output.AppendLine($"The answer is {i + 14}");
						break;
					}
				}
				if (!found)
					output.AppendLine($"The answer is not found");
			});

			return output.ToString();
		}
	}
}
