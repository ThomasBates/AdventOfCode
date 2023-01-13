using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day00 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 00;

	public string Name => $"Day 00";

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

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day00), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day00), message);

	#endregion Helpers

	#region Solvers

	private string SolvePart1(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart1(data);

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart2(data);

		return result.ToString();
	}

	#endregion Solvers

	private object LoadDataFromInput(string input)
	{
		var data = new object();

		InputHelper.TraverseInputTokens(input, value =>
		{
		});

		InputHelper.TraverseInputLines(input, line =>
		{
		});

		GrammarHelper.ParseInput(logger, input, Resources.Day00Grammar,
			scopeControllerAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "s_scope":
						break;
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			},
			typeCheckerAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "t_type":
						break;
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			},
			codeGeneratorAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "c_code":
						break;
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			});
		
		return data;
	}

	private object ProcessDataForPart1(object data)
	{
		return null;
	}

	private object ProcessDataForPart2(object data)
	{
		return null;
	}
}
