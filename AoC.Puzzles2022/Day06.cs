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
	public class Day06 : IPuzzle
	{
		#region IPuzzle Properties

		public string Name => "Day 06";

		public Dictionary<string, string> Inputs { get; } = new Dictionary<string, string>()
		{
			{"Example Inputs", Resources.Day06ExampleInputs},
			{"Puzzle Inputs",  Resources.Day06PuzzleInputs}
		};

		public Dictionary<string, Func<string, string>> Solvers { get; } = new Dictionary<string, Func<string, string>>()
		{
			{ "Part 1", SolvePart1 },
			{ "Part 2", SolvePart2 }
		};

		#endregion IPuzzle Properties

		public static string SolvePart1(string input)
		{
			StringBuilder output = new StringBuilder();

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
			StringBuilder output = new StringBuilder();

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
