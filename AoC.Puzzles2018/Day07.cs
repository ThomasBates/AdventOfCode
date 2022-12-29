using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day07 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 7;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day07Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day07()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	public string SolvePart1(string input)
	{
		var prerequisites = new Dictionary<string, List<string>>();

		InputHelper.TraverseInputLines(input, line =>
		{
			string[] values = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

			string step1 = values[1];
			string step2 = values[7];

			if (!prerequisites.ContainsKey(step1))
			{
				prerequisites.Add(step1, new List<string>());
			}
			if (!prerequisites.ContainsKey(step2))
			{
				prerequisites.Add(step2, new List<string>());
			}
			prerequisites[step2].Add(step1);
		});

		var completed = new List<string>();

		var result = new StringBuilder();

		while (true)
		{
			var availableSteps = prerequisites.Where(s => s.Value.Count == 0).OrderBy(s => s.Key).ToList();
			if (availableSteps.Count == 0)
			{
				break;
			}
			var nextStep = availableSteps[0];

			result.Append(nextStep.Key);
			prerequisites.Remove(nextStep.Key);

			foreach (var entry in prerequisites)
			{
				if (entry.Value.Contains(nextStep.Key))
				{
					entry.Value.Remove(nextStep.Key);
				}
			}
		}

		return $"The step order is {result}.";
	}

	private class Elf
	{
		public string step;
		public int endTime;
	}

	public string SolvePart2(string input)
	{
		var prerequisites = new Dictionary<string, List<string>>();

		InputHelper.TraverseInputLines(input, line =>
		{
			string[] values = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

			string step1 = values[1];
			string step2 = values[7];

			if (!prerequisites.ContainsKey(step1))
			{
				prerequisites.Add(step1, new List<string>());
			}
			if (!prerequisites.ContainsKey(step2))
			{
				prerequisites.Add(step2, new List<string>());
			}
			prerequisites[step2].Add(step1);
		});

		var stepOrder = new StringBuilder();
		var result = new StringBuilder();

		bool finished = false;
		int time = 0;
		var elves = new List<Elf>();
		for (int i = 0; i < 5; i++)
		{
			var elf = new Elf { step = null, endTime = 0 };
			elves.Add(elf);
		}

		while (!finished)
		{
			//	For completed tasks, remove from prerequisite steps, and free the elf.
			foreach (var elf in elves)
			{
				if ((elf.step != null) && (elf.endTime == time))
				{
					foreach (var entry in prerequisites)
					{
						entry.Value.Remove(elf.step);
					}
					stepOrder.Append(elf.step);
					elf.step = null;
					elf.endTime = 0;
				}
			}

			var availableSteps = prerequisites.Where(s => s.Value.Count == 0).OrderBy(s => s.Key).ToList();

			foreach (var nextStep in availableSteps)
			{
				//	for available tasks, assign to available elves, remove from assignable tasks.
				foreach (var elf in elves)
				{
					if (elf.step == null)
					{
						elf.step = nextStep.Key;
						elf.endTime = time + (elf.step[0] - 'A' + 60 + 1);

						prerequisites.Remove(nextStep.Key);
						break;
					}
				}
			}

			finished = true;
			result.Append($"{time} ");
			foreach (var elf in elves)
			{
				if (elf.step == null)
				{
					result.Append(". ");
				}
				else
				{
					result.Append($"{elf.step} ");
					finished = false;
				}
			}
			result.AppendLine(stepOrder.ToString());

			time++;
		}

		return result.ToString();
	}
}
