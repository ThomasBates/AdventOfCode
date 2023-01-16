using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day15 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 15;

	public string Name => $"Day 15";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day15Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day15(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day15), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day15), message);

	#endregion Helpers

	private List<(int, int, int)> LoadData(string input)
	{
		var discs = new List<(int, int, int)>();

		InputHelper.TraverseInputLines(input, line =>
		{
			var match = Regex.Match(line, @"Disc #(\d+) has (\d+) positions; at time=0, it is at position (\d+).");

			if (!match.Success)
			{
				logger.SendError(nameof(Day15), $"Cannot match line: {line}");
				return;
			}

			discs.Add((
				int.Parse(match.Groups[1].Value),
				int.Parse(match.Groups[2].Value),
				int.Parse(match.Groups[3].Value)));
		});

		foreach (var (discNumber, positions, initialPosition) in discs)
			LoggerSendDebug($"Disc #{discNumber} has {positions,2} positions; at time=0, it is at position {initialPosition,2}.");
		LoggerSendDebug("");

		return discs;
	}

	private int SolvePart1(List<(int, int, int)> discs)
	{
		if (discs.Count == 0)
			return 0;

		var starts = new List<int>();
		var periods = new List<int>();

		foreach (var (discNumber, positions, initialPosition) in discs)
		{
			var start = positions - initialPosition - discNumber;
			if (start < 0)
				start += positions;
			starts.Add(start);
			periods.Add(positions);
			LoggerSendVerbose($"Disc #{discNumber} => {start,3}");
		}

		while (true)
		{
			var min = starts.Min();

			if (starts.All(s => s == min))
				return min;
			
			var index = starts.IndexOf(min);

			starts[index] += periods[index];

			LoggerSendVerbose($"Disc #{index + 1} => {starts[index],3}");
		}
	}

	private int SolvePart2(List<(int, int, int)> discs)
	{
		discs.Add((discs.Count + 1, 11, 0));

		foreach (var (discNumber, positions, initialPosition) in discs)
			LoggerSendDebug($"Disc #{discNumber} has {positions,2} positions; at time=0, it is at position {initialPosition,2}.");
		LoggerSendDebug("");

		return SolvePart1(discs);
	}
}
