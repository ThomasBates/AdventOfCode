using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;
using Microsoft.Win32;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day12 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 12;

	public string Name => $"Day 12";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day12Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day12(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day12), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day12), message);

	#endregion Helpers

	#region Solvers

	private string SolvePart1(string input)
	{
		var program = LoadDataFromInput(input);

		var result = RunProgram(program, new[] { 0, 0, 0, 0 });

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var program = LoadDataFromInput(input);

		var result = RunProgram(program, new[] { 0, 0, 1, 0 });

		return result.ToString();
	}

	#endregion Solvers

	private enum Operation { cpyn, cpyr, inc, dec, jnznn, jnzrn, jnznr, jnzrr }

	private class Instruction
	{
		public Operation Op;
		public int[] Args;
	}

	private List<Instruction> LoadDataFromInput(string input)
	{
		var program = new List<Instruction>();

		GrammarHelper.ParseInput(logger, input, Resources.Day12Grammar,
			scopeControllerAction: null,
			typeCheckerAction: null,
			codeGeneratorAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "c_cpy":
						var target = valueStack.Pop();
						var source = valueStack.Pop();
						var cpyOp = Operation.cpyn;
						if (!int.TryParse(source, out var sourceValue))
						{
							sourceValue = source[0] - 'a';
							cpyOp = Operation.cpyr;
						}
						var targetValue = target[0] - 'a';
						program.Add(new Instruction
						{
							Op = cpyOp,
							Args = new[] { sourceValue, targetValue }
						});
						break;
					case "c_inc":
						var incReg = valueStack.Pop();
						var incValue = incReg[0] - 'a';
						program.Add(new Instruction 
						{
							Op = Operation.inc, 
							Args = new int[] { incValue } 
						});
						break;
					case "c_dec":
						var decReg = valueStack.Pop();
						var decValue = decReg[0] - 'a';
						program.Add(new Instruction
						{
							Op = Operation.dec,
							Args = new int[] { decValue }
						});
						break;
					case "c_jnz":
						var jump = valueStack.Pop();
						var test = valueStack.Pop();
						var testReg = false;
						var jumpReg = false;
						if (!int.TryParse(test, out var testValue))
						{
							testValue = test[0] - 'a';
							testReg = true;
						}
						if (!int.TryParse(jump, out var jumpValue))
						{
							jumpValue = jump[0] - 'a';
							jumpReg = true;
						}
						var jnzOp = (testReg, jumpReg) switch
						{
							(false, false) => Operation.jnznn,
							(true, false) => Operation.jnzrn,
							(false, true) => Operation.jnznr,
							(true, true) => Operation.jnzrr,
						};
						program.Add(new Instruction
						{
							Op = jnzOp,
							Args = new[] { testValue, jumpValue }
						});
						break;
				}
			});

		foreach (var instruction in program)
			LoggerSendDebug($"{instruction.Op,-5} {string.Join(" ", instruction.Args),-5}");

		return program;
	}

	private int RunProgram(List<Instruction> program, int[] registers)
	{
		if (registers.Length < 4)
			return 0;

		int pc = 0;

		while (pc < program.Count)
		{
			var instruction = program[pc];
			switch (instruction.Op)
			{
				case Operation.cpyn:
					registers[instruction.Args[1]] = instruction.Args[0];
					pc++;
					break;
				case Operation.cpyr:
					registers[instruction.Args[1]] = registers[instruction.Args[0]];
					pc++;
					break;
				case Operation.inc:
					registers[instruction.Args[0]]++;
					pc++;
					break;
				case Operation.dec:
					registers[instruction.Args[0]]--;
					pc++;
					break;
				case Operation.jnznn:
					if (instruction.Args[0] != 0)
						pc += instruction.Args[1];
					else
						pc++;
					break;
				case Operation.jnzrn:
					if (registers[instruction.Args[0]] != 0)
						pc += instruction.Args[1];
					else
						pc++;
					break;
				case Operation.jnznr:
					if (instruction.Args[0] != 0)
						pc += registers[instruction.Args[1]];
					else
						pc++;
					break;
				case Operation.jnzrr:
					if (registers[instruction.Args[0]] != 0)
						pc += registers[instruction.Args[1]];
					else
						pc++;
					break;
			}

			//LoggerSendVerbose($"{instruction.Op,-5} {string.Join(" ", instruction.Args),-5} pc = {pc,2}, [{string.Join(", ", registers)}]");
		}

		return registers[0];
	}
}
