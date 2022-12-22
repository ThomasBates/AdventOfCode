using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day00 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

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

	[ImportingConstructor]
	public Day00(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataForPart1();

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataForPart2();

		return result;
	}

	#endregion Solvers

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data

		Helper.TraverseInputTokens(input, value =>
		{
		});

		Helper.TraverseInputLines(input, line =>
		{
		});

		Helper.ParseInput(logger, input, Resources.Day00Grammar,
			(token, valueStack) =>
			{
				switch (token)
				{
					case "s_scope":
						break;
					default:
						logger.Send(SeverityLevel.Error, "Parser", $"Unknown token: {token}");
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
						logger.Send(SeverityLevel.Error, "Parser", $"Unknown token: {token}");
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
						logger.Send(SeverityLevel.Error, "Parser", $"Unknown token: {token}");
						break;
				}
			});
	}

	private string ProcessDataForPart1()
	{
		return "";
	}

	private string ProcessDataForPart2()
	{
		return "";
	}
}
