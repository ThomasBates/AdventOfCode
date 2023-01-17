using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day19 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 19;

	public string Name => $"Day 19";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", "5"},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day19(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day19), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day19), message);

	#endregion Helpers

	private int LoadData(string input)
	{
		var circleSize = 0;

		InputHelper.TraverseInputLines(input, line =>
		{
			circleSize = int.Parse(line);
		});
		
		return circleSize;
	}

	private int SolvePart1(int circleSize)
	{
		for (int i=1;i<=100;i++)
		{
			var n = SolveCircleDirect1(i);
			LoggerSendDebug($"{i,3} => {n,3}");
		}

		return SolveCircleCalculated1(circleSize);
	}

	private int SolvePart2(int circleSize)
	{
		for (int i = 1; i <= 100; i++)
		{
			var n = SolveCircleDirect2(i);
			LoggerSendDebug($"{i,3} => {n,3}");
		}

		return SolveCircleCalculated2(circleSize);
	}

	private int SolveCircleDirect1(int circleSize)
	{
		var ring = new LinkedList<int>();
		for (int i = 0; i < circleSize; i++)
			ring.AddLast(i + 1);

		var current = ring.First;
		while (ring.Count > 1)
		{
			var victim = current.Next ?? ring.First;
			ring.Remove(victim);
			current = current.Next ?? ring.First;
		}
		return current.Value;
	}

	private int SolveCircleCalculated1(int circleSize)
	{
		int power = 1;
		while (true)
		{
			if (power * 2 > circleSize)
				break;
			power *= 2;
		}

		return 1 + (circleSize - power) * 2;
	}

	private int SolveCircleDirect2(int circleSize)
	{
		var ring = new LinkedList<int>();
		for (int i = 0; i < circleSize; i++)
			ring.AddLast(i + 1);

		var current = ring.First;
		while (ring.Count > 1)
		{
			var victim = current;
			for (int i = 0; i < ring.Count / 2; i++)
				victim = victim.Next ?? ring.First;
			ring.Remove(victim);

			current = current.Next ?? ring.First;
		}
		return current.Value;
	}

	private int SolveCircleCalculated2(int circleSize)
	{
		int power = 1;
		while (true)
		{
			if (power * 3 > circleSize)
				break;
			power *= 3;
		}

		if (circleSize == power)
			return circleSize;

		if (circleSize <= power * 2)
			return circleSize - power;

		return circleSize * 2 - power * 3;
	}
}
