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
public class Day08 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 08;

	public string Name => $"Day 08";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day08Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day08(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day08), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day08), message);

	#endregion Helpers

	private class Instruction
	{
		public string register;
		public int amount;
		public string testRegister;
		public string testOp;
		public int testValue;
	}

	private List<Instruction> LoadData(string input)
	{
		var instructions = new List<Instruction>();

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			instructions.Add(new Instruction
			{
				register = parts[0],
				amount = (parts[1] == "inc" ? 1 : -1) * int.Parse(parts[2]),
				testRegister = parts[4],
				testOp = parts[5],
				testValue = int.Parse(parts[6])
			});
		});

		return instructions;
	}

	private int SolvePart1(List<Instruction> instructions)
	{
		return RunInstructions(instructions, part1: true);
	}

	private int SolvePart2(List<Instruction> instructions)
	{
		return RunInstructions(instructions, part1: false);
	}

	private int RunInstructions(List<Instruction> instructions, bool part1)
	{
		var registers = new Dictionary<string, int>();
		var maxValue = int.MinValue;

		foreach (var instruction in instructions)
		{
			if (!registers.TryGetValue(instruction.testRegister, out var regValue))
				regValue = 0;

			var doMod = instruction.testOp switch
			{
				"==" => regValue == instruction.testValue,
				"!=" => regValue != instruction.testValue,
				"<=" => regValue <= instruction.testValue,
				">=" => regValue >= instruction.testValue,
				"<" => regValue < instruction.testValue,
				">" => regValue > instruction.testValue,
				_ => false
			};
			if (!doMod)
				continue;

			if (!registers.TryGetValue(instruction.register, out regValue))
				regValue = 0;

			var newValue = regValue + instruction.amount;
			registers[instruction.register] = newValue;
			maxValue = Math.Max(maxValue, newValue);
		}

		var ordered = registers.OrderByDescending(e => e.Value).ToList();

		SendDebug($"Registers:\n\n    {string.Join("\n    ", ordered)}\n");

		return part1 ? ordered[0].Value : maxValue;
	}
}
