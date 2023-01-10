using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day11 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 11;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day11Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day11(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		var monkeys = ReadMonkeysWithParserHelper(input);

		if (monkeys == null)
			return "";

		var result = PlayMonkeyGame(monkeys, rounds: 20, mitigateWorry: true);

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var monkeys = ReadMonkeysWithParserHelper(input);

		if (monkeys == null)
			return "";

		var result = PlayMonkeyGame(monkeys, rounds: 10000, mitigateWorry: false);

		return result.ToString();
	}

	#endregion Solvers

	private class Monkey
	{
		public Queue<ulong> ItemsQueue = new();
		public string Operation;
		public bool IsOperandOld;
		public ulong Operand;
		public ulong Test;
		public int TrueTarget;
		public int FalseTarget;
		public long Inspections;
	}

	private List<Monkey> ReadMonkeysWithParserHelper(string input)
	{
		var monkeys = new List<Monkey>();

		Monkey monkey = null;

		var ok = GrammarHelper.ParseInput(logger, input, Resources.Day11Grammar,
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
		if (!ok)
			return null;

		return monkeys;
	}

	//  See comment at https://github.com/jonathanpaulson/AdventOfCode/blob/master/2022/11.py
	private long PlayMonkeyGame(List<Monkey> monkeys, int rounds, bool mitigateWorry)
	{
		ulong mod = 1;
		foreach (var monkey in monkeys)
			mod *= monkey.Test;
		logger.SendDebug(nameof(Day11), $"mod = {mod}");

		ulong maxItem = 0;
		for (int round = 1; round <= rounds; round++)
		{
			//logger.SendVerbose(nameof(Day11), $"round {round}");
			for (int i = 0; i < monkeys.Count; i++)
			{
				//logger.SendVerbose(nameof(Day11), $"  monkey {i}:");

				var monkey = monkeys[i];
				while (monkey.ItemsQueue.Count > 0)
				{
					monkey.Inspections++;
					var output = new StringBuilder();

					ulong item = monkey.ItemsQueue.Dequeue();
					output.Append($"    item = {item}");

					ulong operand = monkey.IsOperandOld ? item : monkey.Operand;
					if (monkey.Operation == "*")
						item *= operand;
					else
						item += operand;

					output.Append($", {monkey.Operation} {monkey.Operand} = {item}");

					if (mitigateWorry)
					{
						maxItem = Math.Max(maxItem, item);
						item /= 3;
						output.Append($", div 3 = {item}");
					}
					else
					{
						maxItem = Math.Max(maxItem, item);
						item %= mod;
						output.Append($", % {mod} = {item}");
					}

					ulong test = item % monkey.Test;
					output.Append($", % {monkey.Test} = {test}");

					if (test == 0)
					{
						monkeys[monkey.TrueTarget].ItemsQueue.Enqueue(item);
						//logger.SendVerbose(nameof(Day11), $"    {output}: throw to monkey {monkey.TrueTarget}");
					}
					else
					{
						monkeys[monkey.FalseTarget].ItemsQueue.Enqueue(item);
						//logger.SendVerbose(nameof(Day11), $"    {output}: throw to monkey {monkey.FalseTarget}");
					}
				}
			}

			if (round == 1 || round == 20 || round % 1000 == 0)
			{
				logger.SendDebug(nameof(Day11), $"== After round {round} ==");
				for (int i = 0; i < monkeys.Count; i++)
				{
					var monkey = monkeys[i];
					logger.SendDebug(nameof(Day11), $"Monkey {i} inspected items {monkey.Inspections} times.");
				}
				logger.SendDebug(nameof(Day11), "");
			}
		}

		var ordered = monkeys.OrderByDescending(m => m.Inspections).ToList();
		long result = ordered[0].Inspections * ordered[1].Inspections;
		logger.SendDebug(nameof(Day11), $"{ordered[0].Inspections} * {ordered[1].Inspections} = {result}");

		logger.SendDebug(nameof(Day11), $"maxItem = {maxItem}");
		return result;
	}
}
