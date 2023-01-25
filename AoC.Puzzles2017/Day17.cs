using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day17 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 17;

	public string Name => $"Day 17";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", "3"},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day17(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day17), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day17), message);

	#endregion Helpers

	private int LoadData(string input)
	{
		var steps = 0;

		InputHelper.TraverseInputTokens(input, value =>
		{
			steps = int.Parse(value);
		});
		
		return steps;
	}

	private int SolvePart1(int steps)
	{
		var ring = new LinkedList<int>();
		var current = ring.AddFirst(0);
		var first = current;

		for (var i = 1; i <= 2017; i++)
		{
			var realSteps = steps % ring.Count;
			for (var j = 0; j < realSteps; j++)
				current = current.Next ?? ring.First;
			if (current == first)
				SendDebug($"after zero = {i}");
			current = ring.AddAfter(current, i);
		}

		return (current.Next ?? ring.First).Value;
	}

	private int SolvePart2(int steps)
	{
		var ringSize = 1;
		var current = 0;
		var afterZero = 0;

		for (var i = 1; i <= 50000000; i++)
		{
			var realSteps = steps % ringSize;
			current = (current + realSteps) % ringSize;

			if (current == 0)
			{
				afterZero = i;
				SendDebug($"after zero = {i}");
			}

			ringSize++;
			current++;
		}

		return afterZero;
	}
}
