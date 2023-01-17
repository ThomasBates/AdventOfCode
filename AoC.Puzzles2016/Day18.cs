using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day18 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 18;

	public string Name => $"Day 18";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", ".^^.^.^^^^"},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day18(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day18), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day18), message);

	#endregion Helpers

	private string LoadData(string input)
	{
		var firstRow = "";

		InputHelper.TraverseInputLines(input, line =>
		{
			firstRow = line;
		});

		LoggerSendDebug(firstRow);

		return firstRow;
	}

	private int SolvePart1(string firstRow)
	{
		if (string.IsNullOrEmpty(firstRow))
			return 0;

		var safeCount = GetSafeCount(firstRow, firstRow.Length == 10 ? 10 : 40);

		return safeCount;
	}

	private int SolvePart2(string firstRow)
	{
		if (string.IsNullOrEmpty(firstRow))
			return 0;

		var safeCount = GetSafeCount(firstRow, firstRow.Length == 10 ? 10 : 400000);

		return safeCount;
	}

	private int GetSafeCount(string firstRow, int roomSize)
	{
		var safeCount = 0;

		var last = firstRow;
		LoggerSendVerbose(last);

		safeCount += last.Count(c => c == '.');

		for (int row=1; row<roomSize;row++)
		{
			var next = new StringBuilder();
			for (int i = 0; i < last.Length; i++)
			{
				var leftSafe = i <= 0 || last[i - 1] == '.';
				var rightSafe = i >= last.Length - 1 || last[i + 1] == '.';
				next.Append(leftSafe == rightSafe ? '.' : '^');
			}

			last = next.ToString();
			LoggerSendVerbose(last);

			safeCount += last.Count(c => c == '.');
		}

		return safeCount;
	}
}
