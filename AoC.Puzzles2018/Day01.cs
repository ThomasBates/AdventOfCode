using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018
{
	[Export(typeof(IPuzzle))]
	public class Day01 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2018;

		public int Day => 1;

		public string Name => $"Day {Day:00}";

		public Dictionary<string, string> Inputs
		{
			get;
		} = new Dictionary<string, string>();

		public Dictionary<string, Func<string, string>> Solvers
		{
			get;
		} = new Dictionary<string, Func<string, string>>();

		#endregion IPuzzle Properties

		#region Constructors

		public Day01()
		{
			Inputs.Add("Example Inputs", Resources.Day01Inputs);
			Inputs.Add("Puzzle Inputs", "");

			Solvers.Add("Part 1", SolvePart1);
			Solvers.Add("Part 2", SolvePart2);
		}

		#endregion Constructors

		public string SolvePart1(string input)
		{
			int frequency = 0;

			Helper.TraverseInputTokens(input, value =>
			{
				int frequencyShift;
				if (int.TryParse(value, out frequencyShift))
				{
					frequency += frequencyShift;
				}
			});

			return $"The end frequency is {frequency}.";
		}

		public string SolvePart2(string input)
		{
			List<int> frequencyShifts = new List<int>();

			Helper.TraverseInputTokens(input, value =>
			{
				int frequencyShift;
				if (int.TryParse(value, out frequencyShift))
				{
					frequencyShifts.Add(frequencyShift);
				}
			});

			int frequency = 0;
			int traversal = 0;

			if (frequencyShifts.Count > 0)
			{
				HashSet<int> frequencies = new HashSet<int>();
				frequencies.Add(frequency);
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
}
