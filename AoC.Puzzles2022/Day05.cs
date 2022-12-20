using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Text.RegularExpressions;

using AoC.Common;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day05 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2022;

		public int Day => 5;

		public string Name => $"Day {Day:00}";

		public Dictionary<string, string> Inputs { get; } = new()
		{
			{"Example Inputs", Resources.Day05Inputs},
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
			var stacks = new List<Stack<char>>();

			var output = new StringBuilder();

			var stackLines = new Stack<string>();
			bool stackReady = false;
			Helper.TraverseInputLines(input, line =>
			{
				if (!stackReady)
				{
					if (!string.IsNullOrWhiteSpace(line))
					{
						stackLines.Push(line);
					}
					else
					{
						line = stackLines.Pop();

						//  1. parse " 1  2  3  4  5  ... "
						var stackNumbers = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						int numStacks = stackNumbers.Length;
						for (int i = 0; i < numStacks; i++)
							stacks.Add(new Stack<char>());
						output.AppendLine($"{numStacks} stacks");

						//  2. parse stack contents from bottom to top.
						while (stackLines.Count > 0)
						{
							line = stackLines.Pop();

							for (int i = 0; i < numStacks; i++)
							{
								int pos = i * 4 + 1;
								if (line.Length < pos)
									break;
								char c = line[pos];
								output.Append($"<{c}> ");
								if (c == ' ')
									continue;
								stacks[i].Push(c);
							}
							output.AppendLine();
						}

						stackReady = true;
					}
				}
				else
				{
					Match match = Regex.Match(line, @"move \s?(\d+)\s? from \s?(\d+)\s? to \s?(\d+)\s?");

					int number = int.Parse(match.Groups[1].Value);
					int source = int.Parse(match.Groups[2].Value);
					int target = int.Parse(match.Groups[3].Value);

					output.AppendLine($"Moving {number} from {source} to {target}.");

					var sourceStack = stacks[source - 1];
					var targetStack = stacks[target - 1];

					if (sourceStack.Count < number)
					{
						output.AppendLine($"Stack {source} has {sourceStack.Count}. Need {number}.");
						return;
					}

					for (int i = 0; i < number; i++)
					{
						char c = sourceStack.Pop();
						targetStack.Push(c);
					}
				}
			}, false);

			output.Append("The answer is '");

			foreach (var stack in stacks)
			{
				if (stack.Count == 0)
				{
					output.Append(" ");
					continue;
				}

				char c = stack.Pop();
				output.Append(c);
			}

			output.AppendLine($"'");

			return output.ToString();
		}

		public static string SolvePart2(string input)
		{
			var stacks = new List<Stack<char>>();

			var output = new StringBuilder();

			var stackLines = new Stack<string>();
			bool stackReady = false;
			Helper.TraverseInputLines(input, line =>
			{
				if (!stackReady)
				{
					if (!string.IsNullOrWhiteSpace(line))
					{
						stackLines.Push(line);
					}
					else
					{
						line = stackLines.Pop();

						//  1. parse " 1  2  3  4  5  ... "
						var stackNumbers = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						int numStacks = stackNumbers.Length;
						for (int i = 0; i < numStacks; i++)
							stacks.Add(new Stack<char>());
						output.AppendLine($"{numStacks} stacks");

						//  2. parse stack contents from bottom to top.
						while (stackLines.Count > 0)
						{
							line = stackLines.Pop();

							for (int i = 0; i < numStacks; i++)
							{
								int pos = i * 4 + 1;
								if (line.Length < pos)
									break;
								char c = line[pos];
								output.Append($"<{c}> ");
								if (c == ' ')
									continue;
								stacks[i].Push(c);
							}
							output.AppendLine();
						}

						stackReady = true;
					}
				}
				else
				{
					Match match = Regex.Match(line, @"move \s?(\d+)\s? from \s?(\d+)\s? to \s?(\d+)\s?");

					int number = int.Parse(match.Groups[1].Value);
					int source = int.Parse(match.Groups[2].Value);
					int target = int.Parse(match.Groups[3].Value);

					output.AppendLine($"Moving {number} from {source} to {target}.");

					var sourceStack = stacks[source - 1];
					var targetStack = stacks[target - 1];

					if (sourceStack.Count < number)
					{
						output.AppendLine($"Stack {source} has {sourceStack.Count}. Need {number}.");
						return;
					}

					var tempStack = new Stack<char>();
					for (int i = 0; i < number; i++)
					{
						char c = sourceStack.Pop();
						tempStack.Push(c);
					}
					for (int i = 0; i < number; i++)
					{
						char c = tempStack.Pop();
						targetStack.Push(c);
					}
				}
			}, false);

			output.Append("The answer is '");

			foreach (var stack in stacks)
			{
				if (stack.Count == 0)
				{
					output.Append(" ");
					continue;
				}

				char c = stack.Pop();
				output.Append(c);
			}

			output.AppendLine($"'");

			return output.ToString();
		}
	}
}
