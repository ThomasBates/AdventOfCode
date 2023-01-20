using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day00 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

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

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day00), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day00), message);

	#endregion Helpers

	private object LoadData(string input)
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
				}
			},
			typeCheckerAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "t_type":
						break;
				}
			},
			codeGeneratorAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "c_code":
						break;
				}
			});
		
		return data;
	}

	private object SolvePart1(object data)
	{
		return new object();
	}

	private object SolvePart2(object data)
	{
		return new object();
	}
}
