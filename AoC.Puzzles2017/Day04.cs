using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day04 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 04;

	public string Name => $"Day 04";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs (1)", Resources.Day04Inputs01},
		{"Example Inputs (2)", Resources.Day04Inputs02},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day04(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day04), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day04), message);

	#endregion Helpers

	private List<List<string>> LoadData(string input)
	{
		var data = new List<List<string>>();

		InputHelper.TraverseInputLines(input, line =>
		{
			var phrase = new List<string>();
			data.Add(phrase);

			var words = line.Split(' ');
			foreach (var word in words)
				phrase.Add(word);
		});
		
		return data;
	}

	private int SolvePart1(List<List<string>> data)
	{
		var count = 0;

		foreach (var phrase in data)
		{
			var valid = true;
			for (var i = 0; i < phrase.Count - 1 && valid; i++)
			{
				for (var j = i + 1; j < phrase.Count && valid; j++)
				{
					if (phrase[i] == phrase[j])
						valid = false;
				}
			}

			SendDebug($"{(valid ? "valid:    " : "not valid:")} {string.Join(" ", phrase)}");

			if (valid)
				count++;
		}

		return count;
	}

	private int SolvePart2(List<List<string>> data)
	{
		var count = 0;

		foreach (var phrase in data)
		{
			var valid = true;
			for (var i = 0; i < phrase.Count - 1 && valid; i++)
			{
				for (var j = i + 1; j < phrase.Count && valid; j++)
				{
					if (IsAnagram(phrase[i], phrase[j]))
						valid = false;
				}
			}

			SendDebug($"{(valid ? "valid:    " : "not valid:")} {string.Join(" ", phrase)}");

			if (valid)
				count++;
		}

		return count;
	}

	private bool IsAnagram(string word1, string word2)
	{
		var ordered1 = word1.ToCharArray().OrderBy(c => c).ToArray();
		var ordered2 = word2.ToCharArray().OrderBy(c => c).ToArray();
		return Enumerable.SequenceEqual(ordered1, ordered2);
	}
}
