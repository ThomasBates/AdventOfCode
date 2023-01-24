using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day15 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

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

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day15), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day15), message);

	#endregion Helpers

	private List<int> LoadData(string input)
	{
		var data = new List<int>();

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			data.Add(int.Parse(parts[parts.Length - 1]));
		});

		return data;
	}

	private class Generator
	{
		private readonly int factor;
		private readonly int modulus;
		private long value;

		public Generator(int factor, int value, int modulus = 1)
		{
			this.factor = factor;
			this.value = value;
			this.modulus = modulus;
		}
		public int GetNextValue()
		{
			while (true)
			{
				value *= factor;
				value %= 2147483647;
				if (value % modulus == 0)
					return (int)value;
			}
		}
	}

	private int SolvePart1(List<int> data)
	{
		var count = 0;

		var A = new Generator(16807, data[0]);
		var B = new Generator(48271, data[1]);

		for (var i = 0; i < 40000000; i++)
		{
			var a = A.GetNextValue();
			var b = B.GetNextValue();
			if ((a & 0xffff) == (b & 0xffff))
				count++;

			if (i < 5)
			{
				SendDebug($"A = {a,10} => {Convert.ToString(a, 2).PadLeft(32, '0')}");
				SendDebug($"B = {b,10} => {Convert.ToString(b, 2).PadLeft(32, '0')}");
				SendDebug();
			}
		}

		return count;
	}

	private int SolvePart2(List<int> data)
	{
		var count = 0;

		var A = new Generator(16807, data[0], 4);
		var B = new Generator(48271, data[1], 8);

		for (var i = 0; i < 5000000; i++)
		{
			var a = A.GetNextValue();
			var b = B.GetNextValue();
			if ((a & 0xffff) == (b & 0xffff))
				count++;

			if (i < 5 || i == 1055)
			{
				SendDebug($"A = {a,10} => {Convert.ToString(a, 2).PadLeft(32, '0')}");
				SendDebug($"B = {b,10} => {Convert.ToString(b, 2).PadLeft(32, '0')}");
				SendDebug();
			}
		}

		return count;
	}
}
