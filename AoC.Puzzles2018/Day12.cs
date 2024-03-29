﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day12 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 12;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day12Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day12()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	private class Rule
	{
		public string Pattern;
		public string NextGen;
	}

	private void LoadDataFromInput(string input, List<string> states, List<Rule> rules)
	{
		bool initialState = false;
		states.Clear();
		rules.Clear();

		InputHelper.TraverseInputLines(input, line =>
		{
			if (!initialState)
			{
				string[] parts = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

				states.Add(parts[2]);

				initialState = true;
			}
			else
			{
				string[] parts = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

				rules.Add(new Rule { Pattern = parts[0], NextGen = parts[2] });
			}
		});
	}

	public string SolvePart1(string input)
	{
		var result = new StringBuilder();

		var states = new List<string>();
		var rules = new List<Rule>();

		LoadDataFromInput(input, states, rules);

		string lastGen = ".........." + states[0] + "..............................";
		result.AppendLine($" 0: {lastGen}");
		for (int gen = 1; gen <= 20; gen++)
		{
			string nextGen = lastGen;

			foreach (var rule in rules)
			{
				int index = lastGen.IndexOf(rule.Pattern);
				while (index >= 0)
				{
					nextGen = nextGen.Substring(0, index + 2) + rule.NextGen + nextGen.Substring(index + 3);
					index = lastGen.IndexOf(rule.Pattern, index + 1);
				}
			}

			states.Add(nextGen);
			lastGen = nextGen;
			result.AppendLine($"{gen,2}: {lastGen}");
		}

		int sum = Sum(lastGen);

		result.AppendLine($"The sum of the numbers of the pots that contain a plant is {sum}.");
		return result.ToString();
	}

	public string SolvePart2(string input)
	{
		var result = new StringBuilder();

		var states = new List<string>();
		var rules = new List<Rule>();

		LoadDataFromInput(input, states, rules);


		string lastGen = ".........." + states[0] + "..........";
		int lastSum = Sum(lastGen);
		int lastDelta = lastSum;
		int deltaCount = 0;
		result.AppendLine($" 0: {lastGen}: {lastSum}");
		for (int gen = 1; gen <= 1000; gen++)
		{
			string nextGen = lastGen + ".";

			foreach (var rule in rules)
			{
				int index = lastGen.IndexOf(rule.Pattern);
				while (index >= 0)
				{
					nextGen = nextGen.Substring(0, index + 2) + rule.NextGen + nextGen.Substring(index + 3);
					index = lastGen.IndexOf(rule.Pattern, index + 1);
				}
			}

			lastGen = nextGen;

			int sum = Sum(lastGen);
			int delta = sum - lastSum;
			lastSum = sum;

			if (delta == lastDelta)
			{
				deltaCount++;
			}
			else
			{
				deltaCount = 0;
			}
			lastDelta = delta;

			result.AppendLine($"{gen,2}: {lastGen}: {lastSum}, {delta}, {deltaCount}");
			//result.AppendLine($"{gen,2}: {lastSum}, {delta}");

			if (deltaCount >= 10)
			{
				int offset = sum - gen * delta;
				long finalSum = 50000000000 * delta + offset;
				result.AppendLine($"Final Sum is {finalSum}.");
				break;
			}
		}

		return result.ToString();
	}

	int Sum(string state)
	{
		int sum = 0;
		for (int i = 0; i < state.Length; i++)
		{
			int value = i - 10;
			char plant = state[i];
			if (plant == '#')
			{
				sum += value;
			}
		}
		return sum;
	}
}
