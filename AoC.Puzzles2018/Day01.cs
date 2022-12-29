using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day01 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 1;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day01Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day01()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	public string SolvePart1(string input)
	{
		int frequency = 0;

		InputHelper.TraverseInputTokens(input, value =>
		{
			if (int.TryParse(value, out int frequencyShift))
			{
				frequency += frequencyShift;
			}
		});

		return $"The end frequency is {frequency}.";
	}

	public string SolvePart2(string input)
	{
		var frequencyShifts = new List<int>();

		InputHelper.TraverseInputTokens(input, value =>
		{
			if (int.TryParse(value, out int frequencyShift))
			{
				frequencyShifts.Add(frequencyShift);
			}
		});

		int frequency = 0;
		int traversal = 0;

		if (frequencyShifts.Count > 0)
		{
			var frequencies = new HashSet<int> { frequency };
			bool found = false;
			while (!found)
			{
				traversal++;
				foreach (int frequencyShift in frequencyShifts)
				{
					frequency += frequencyShift;
					if (frequencies.Contains(frequency))
					{
						found = true;
						break;
					}
					frequencies.Add(frequency);
				}
			}
		}

		return $"The first repeated frequency is {frequency}.\n" +
				$"It was found after {traversal} traversals of the dataset.";
	}
}
