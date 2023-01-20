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
public class Day06 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 06;

	public string Name => $"Day 06";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day06Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day06(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day06), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day06), message);

	#endregion Helpers

	private byte[] LoadData(string input)
	{
		var data = new List<byte>();

		InputHelper.TraverseInputTokens(input, value =>
		{
			data.Add(byte.Parse(value));
		});

		return data.ToArray();
	}

	private int SolvePart1(byte[] banks)
	{
		var states = GetLoopStates(banks);

		return states.Count - 1;
	}

	private int SolvePart2(byte[] banks)
	{
		var states = GetLoopStates(banks);

		var last = states[states.Count - 1];

		for (var i = states.Count - 2; i >= 0; i--)
		{
			var state = states[i];
			if (Enumerable.SequenceEqual(state, last))
				return states.Count - 1 - i;
		}

		return 0;
	}

	private List<byte[]> GetLoopStates(byte[] banks)
	{
		var states = new List<byte[]> { banks.ToArray() };

		SendDebug($"[{string.Join(", ", banks.Select(b => $"{b,3}"))}]");

		while (true)
		{
			var blocks = banks.Max(b => b);
			var bank = 0;
			while (banks[bank] < blocks)
				bank++;
			banks[bank] = 0;
			for (var i = 0; i < blocks; i++)
			{
				bank++;
				if (bank >= banks.Length)
					bank = 0;
				banks[bank]++;
			}

			SendDebug($"[{string.Join(", ", banks.Select(b => $"{b,3}"))}]");

			var seen = states.Any(s => Enumerable.SequenceEqual(s, banks));

			states.Add(banks.ToArray());

			if (seen)
				return states;
		}
	}
}
