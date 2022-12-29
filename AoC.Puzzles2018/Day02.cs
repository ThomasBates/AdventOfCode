using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day02 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 2;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day02Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day02(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	public string SolvePart1(string input)
	{
		int doubles = 0;
		int triples = 0;

		InputHelper.TraverseInputTokens(input, value =>
		{
			var charCount = new Dictionary<char, int>();
			foreach (char c in value)
			{
				if (!charCount.Keys.Contains(c))
					charCount.Add(c, 0);
				charCount[c]++;
			}

			bool hasDouble = false;
			bool hasTriple = false;

			foreach (int count in charCount.Values)
			{
				if (count == 2)
					hasDouble = true;
				if (count == 3)
					hasTriple = true;
			}

			if (hasDouble)
				doubles++;
			if (hasTriple)
				triples++;
		});

		int checksum = doubles * triples;

		return $"The checksum is {checksum}.";
	}

	public string SolvePart2(string input)
	{
		var boxIDs = new List<string>();

		InputHelper.TraverseInputTokens(input, value => boxIDs.Add(value));

		var result = new StringBuilder();

		foreach (string id1 in boxIDs)
		{
			foreach (string id2 in boxIDs)
			{
				if (String.Equals(id1, id2))
					continue;

				if (id1.Length != id2.Length)
					continue;

				var common = new StringBuilder();
				int diffCount = 0;
				for (int i = 0; i < id1.Length; i++)
				{
					if (id1[i] == id2[i])
					{
						common.Append(id1[i]);
					}
					else
					{
						diffCount++;
						if (diffCount > 1)
							break;
					}
				}

				if (diffCount == 1)
				{
					logger.SendDebug(nameof(Day02), $"The common letters are {common}.");
					return common.ToString();
				}
			}
		}

		logger.SendDebug(nameof(Day02),	"Can't find the boxes, boss.");

		return "";
	}
}
