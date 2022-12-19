using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using AoC.IO;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day00 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 0;

	public string Name => $"Day {Day:00}";

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

	#region Solvers

	private string SolvePart1(string input)
	{
		var output = new StringBuilder();

		LoadDataFromInput(input, output);

		ProcessDataForPart1(output);

		return output.ToString();
	}

	private string SolvePart2(string input)
	{
		var output = new StringBuilder();

		LoadDataFromInput(input, output);

		ProcessDataForPart2(output);

		return output.ToString();
	}

	#endregion Solvers

	private void LoadDataFromInput(string input, StringBuilder output = null)
	{
		Helper.TraverseInputTokens(input, value =>
		{
		});

		Helper.TraverseInputLines(input, line =>
		{
		});

		Helper.ParseInput(input, Resources.Day00Grammar,
			(token, valueStack) =>
			{
				switch (token)
				{
					case "s_scope":
						break;
					default:
						output?.AppendLine($"Unknown token: {token}");
						break;
				}
			},
			(token, valueStack) =>
			{
				switch (token)
				{
					case "t_type":
						break;
					default:
						output?.AppendLine($"Unknown token: {token}");
						break;
				}
			},
			(token, valueStack) =>
			{
				switch (token)
				{
					case "c_code":
						break;
					default:
						output?.AppendLine($"Unknown token: {token}");
						break;
				}
			},
			(severity, category, message) =>
			{
				output?.AppendLine($"[{severity,-7}] - [{category,-15}] - {message}");
			});

	}

	private void ProcessDataForPart1(StringBuilder output = null)
	{
	}

	private void ProcessDataForPart2(StringBuilder output = null)
	{
	}
}
