using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day05 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 5;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day05Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day05()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	public string SolvePart1(string input)
	{
		var result = new StringBuilder();

		InputHelper.TraverseInputTokens(input, value =>
		{
			string sequence = value;

			int cDiff = Math.Abs('A' - 'a');
			int index = 0;
			while (index < sequence.Length - 1)
			{
				char c1 = sequence[index];
				char c2 = sequence[index + 1];

				if (Math.Abs(c1 - c2) == cDiff)
				{
					sequence = sequence.Remove(index, 2);
					if (index > 0)
					{
						index--;
					}
				}
				else
				{
					index++;
				}
			}

			result.AppendLine($"There are {sequence.Length} units remaining.");
		});

		return result.ToString();
	}

	public string SolvePart1_Take1(string input)
	{
		var result = new StringBuilder();

		InputHelper.TraverseInputTokens(input, value =>
		{
			bool finished = false;

			string sequence = value;

			while (!finished)
			{
				bool reaction = false;

				for (char c = 'a'; c <= 'z'; c++)
				{
					char C = (char)(c + 'A' - 'a');

					string pair = $"{c}{C}";
					if (sequence.Contains(pair))
					{
						sequence = sequence.Replace(pair, "");
						reaction = true;
					}

					pair = $"{C}{c}";
					if (sequence.Contains(pair))
					{
						sequence = sequence.Replace(pair, "");
						reaction = true;
					}
				}

				finished = !reaction;
			}

			result.AppendLine(sequence);
		});

		return result.ToString();
	}

	public string SolvePart2(string input)
	{
		var result = new StringBuilder();

		InputHelper.TraverseInputTokens(input, value =>
		{
			int minLength = int.MaxValue;
			char minType = ' ';

			for (char cType = 'a'; cType <= 'z'; cType++)
			{
				string sequence = value;
				string sType = $"{cType}";
				sequence = sequence.Replace(sType, "");
				sType = $"{(char)(cType + 'A' - 'a')}";
				sequence = sequence.Replace(sType, "");

				bool finished = false;
				while (!finished)
				{
					bool reaction = false;

					for (char c = 'a'; c <= 'z'; c++)
					{
						char C = (char)(c + 'A' - 'a');

						string pair = $"{c}{C}";
						if (sequence.Contains(pair))
						{
							sequence = sequence.Replace(pair, "");
							reaction = true;
						}

						pair = $"{C}{c}";
						if (sequence.Contains(pair))
						{
							sequence = sequence.Replace(pair, "");
							reaction = true;
						}
					}

					finished = !reaction;
				}

				int length = sequence.Length;
				if (length < minLength)
				{
					minLength = length;
					minType = cType;
				}
			}

			result.AppendLine($"After removing type '{minType}', there are {minLength} units remaining.");
		});

		return result.ToString();
	}
}
