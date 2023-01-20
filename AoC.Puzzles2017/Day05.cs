using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day05 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 05;

	public string Name => $"Day 05";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day05Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day05(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day05), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day05), message);

	#endregion Helpers

	private List<int> LoadData(string input)
	{
		var jumps = new List<int>();

		InputHelper.TraverseInputLines(input, line =>
		{
			jumps.Add(int.Parse(line));
		});
		
		return jumps;
	}

	private int SolvePart1(List<int> jumps)
	{
		var count = 0;

		var pc = 0;
		while(pc >= 0 && pc < jumps.Count)
		{
			count++;
			int jump = jumps[pc];

			jumps[pc]++;

			pc += jump;
		}

		return count;
	}

	private int SolvePart2(List<int> jumps)
	{
		var count = 0;

		var pc = 0;
		while (pc >= 0 && pc < jumps.Count)
		{
			count++;
			int jump = jumps[pc];

			if (jump >= 3)
				jumps[pc]--;
			else
				jumps[pc]++;

			pc += jump;
		}

		return count;
	}
}
