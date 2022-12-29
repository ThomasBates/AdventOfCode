using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day00 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 0;

	public string Name => "Day 00";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day00Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day00()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	public string SolvePart1(string input)
	{
		var result = new StringBuilder();

		LoadDataFromInput(input);

		//

		return result.ToString();
	}

	public string SolvePart2(string input)
	{
		var result = new StringBuilder();

		LoadDataFromInput(input);

		//

		return result.ToString();
	}

	private void LoadDataFromInput(string input)
	{
		InputHelper.TraverseInputTokens(input, value =>
		{
		});

		InputHelper.TraverseInputLines(input, line =>
		{
		});
	}
}
