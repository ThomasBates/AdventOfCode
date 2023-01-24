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
public class Day13 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 13;

	public string Name => $"Day 13";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day13Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day13(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day13), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day13), message);

	#endregion Helpers

	private List<(int, int, int)> LoadData(string input)
	{
		var layers = new List<(int, int, int)>();

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			var depth = int.Parse(parts[0]);
			var range = int.Parse(parts[1]);
			var period = (range - 1) * 2;
			layers.Add((depth, range, period));
		});

		return layers;
	}

	private int SolvePart1(List<(int depth, int range, int period)> layers)
	{
		return layers.Sum(l => (l.depth % l.period == 0) ? (l.depth * l.range) : 0);
	}

	private int SolvePart2(List<(int depth, int range, int period)> layers)
	{
		var delay = 0;

		while (true)
		{
			delay++;

			if (!layers.Any(l => (delay + l.depth) % l.period == 0))
				return delay;
		}
	}
}
