using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day11 : IPuzzle
	{
		#region IPuzzle Properties

		public string Name => "Day 11";

		public Dictionary<string, string> Inputs { get; } = new Dictionary<string, string>()
		{
			{"Example Inputs", Resources.Day11ExampleInputs},
			{"Puzzle Inputs",  Resources.Day11PuzzleInputs}
		};

		public Dictionary<string, Func<string, string>> Solvers { get; } = new Dictionary<string, Func<string, string>>()
		{
			{ "Part 1", SolvePart1 },
			{ "Part 2", SolvePart2 }
		};

		#endregion IPuzzle Properties

		private class Monkey
		{
			public Queue<ulong> ItemsQueue = new Queue<ulong>();
			public string Operation;
			public bool IsOperandOld;
			public ulong Operand;
			public ulong Test;
			public int TrueTarget;
			public int FalseTarget;
			public long Inspections;
		}

		private static string SolvePart1(string input)
		{
			StringBuilder output = new StringBuilder();

			var monkeys = new List<Monkey>();

			ReadMonkeysWithParserHelper(input, monkeys, output);

			PlayMonkeyGame(monkeys, 20, true, output, true);

			return output.ToString();
		}

		private static string SolvePart2(string input)
		{
			StringBuilder output = new StringBuilder();

			var monkeys = new List<Monkey>();

			ReadMonkeysWithParserHelper(input, monkeys, output);

			output.Clear();

			PlayMonkeyGame(monkeys, 10000, false, output, false);

			return output.ToString();
		}

		private static void ReadMonkeysWithParserHelper(string input, List<Monkey> monkeys, StringBuilder output)
		{
			Monkey monkey = null;

			ParserHelper.RunParser(input, output, Resources.Day11Grammar,
				(token, valueStack) =>
				{
					switch (token)
					{
						case "s_monkey":
							monkey = new Monkey();
							monkeys.Add(monkey);
							break;
					}
				},
				null,
				(token, valueStack) =>
				{
					switch (token)
					{
						case "c_starting":
							ulong item = ulong.Parse(valueStack.Pop());
							monkey.ItemsQueue.Enqueue(item);
							break;
						case "c_times":
							monkey.Operation = "*";
							break;
						case "c_plus":
							monkey.Operation = "+";
							break;
						case "c_opNumber":
							monkey.Operand = ulong.Parse(valueStack.Pop());
							break;
						case "c_opOld":
							monkey.IsOperandOld = true;
							break;
						case "c_test":
							monkey.Test = ulong.Parse(valueStack.Pop());
							break;
						case "c_trueTarget":
							monkey.TrueTarget = int.Parse(valueStack.Pop());
							break;
						case "c_falseTarget":
							monkey.FalseTarget = int.Parse(valueStack.Pop());
							break;
					}
				});
		}

		private static void PlayMonkeyGame(List<Monkey> monkeys, int rounds, bool mitigateWorry, StringBuilder output, bool verbose)
		{
			//  See comment at https://github.com/jonathanpaulson/AdventOfCode/blob/master/2022/11.py
			ulong mod = 1;
			foreach (var monkey in monkeys)
				mod *= monkey.Test;
			output.AppendLine($"mod = {mod}");

			ulong maxItem = 0;
			for (int round = 1; round <= rounds; round++)
			{
				if (verbose)
					output.AppendLine($"round {round}");
				for (int i = 0; i < monkeys.Count; i++)
				{
					if (verbose)
						output.AppendLine($"  monkey {i}:");

					var monkey = monkeys[i];
					while (monkey.ItemsQueue.Count > 0)
					{
						monkey.Inspections++;

						ulong item = monkey.ItemsQueue.Dequeue();
						if (verbose)
							output.Append($"    item = {item}");

						ulong operand = monkey.IsOperandOld ? item : monkey.Operand;
						if (monkey.Operation == "*")
						{
							item *= operand;
						}
						else
							item += operand;
						if (verbose)
							output.Append($", {monkey.Operation} {monkey.Operand} = {item}");

						if (mitigateWorry)
						{
							maxItem = Math.Max(maxItem, item);
							item /= 3;
							if (verbose)
								output.Append($", div 3 = {item}");
						}
						else
						{
							maxItem = Math.Max(maxItem, item);
							item %= mod;
							if (verbose)
								output.Append($", % {mod} = {item}");
						}

						ulong test = item % monkey.Test;
						if (verbose)
							output.Append($", % {monkey.Test} = {test}");

						if (test == 0)
						{
							monkeys[monkey.TrueTarget].ItemsQueue.Enqueue(item);
							if (verbose)
								output.AppendLine($": throw to monkey {monkey.TrueTarget}");
						}
						else
						{
							monkeys[monkey.FalseTarget].ItemsQueue.Enqueue(item);
							if (verbose)
								output.AppendLine($": throw to monkey {monkey.FalseTarget}");
						}
					}
				}

				if (round == 1 || round == 20 || round % 1000 == 0)
				{
					output.AppendLine($"== After round {round} ==");
					for (int i = 0; i < monkeys.Count; i++)
					{
						var monkey = monkeys[i];
						output.AppendLine($"Monkey {i} inspected items {monkey.Inspections} times.");
					}
					output.AppendLine();
				}
			}

			var ordered = monkeys.OrderByDescending(m => m.Inspections).ToList();
			long result = ordered[0].Inspections * ordered[1].Inspections;
			output.AppendLine($"{ordered[0].Inspections} * {ordered[1].Inspections} = {result}");

			output.AppendLine($"maxItem = {maxItem}");
		}
	}
}
