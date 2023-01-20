using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day01 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 01;

	public string Name => $"Day 01";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day01Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day01(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day01), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day01), message);

	#endregion Helpers

	private List<string> LoadData(string input)
	{
		var data = new List<string>();

		InputHelper.TraverseInputLines(input, line =>
		{
			data.Add(line);
		});
		
		return data;
	}

	private int SolvePart1(List<string> data)
	{
		var sum = 0;
		foreach (var digits in data)
		{
			sum = 0;
			for (var i = 0; i < digits.Length; i++)
			{
				var j = i == 0 ? digits.Length - 1 : i - 1;
				if (digits[i] == digits[j])
					sum += digits[i] - '0';
			}
			SendDebug($"{sum} <= {digits}");
		}
		return sum;
	}

	private int SolvePart2(List<string> data)
	{
		var sum = 0;
		foreach (var digits in data)
		{
			sum = 0;
			for (var i = 0; i < digits.Length; i++)
			{
				var j = (i + digits.Length / 2) % digits.Length;
				if (digits[i] == digits[j])
					sum += digits[i] - '0';
			}
			SendDebug($"{sum} <= {digits}");
		}
		return sum;
	}
}
