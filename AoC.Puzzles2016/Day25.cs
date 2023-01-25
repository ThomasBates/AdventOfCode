using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day25 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 25;

	public string Name => $"Day 25";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{ "Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day25(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadProgram(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadProgram(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day25), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day25), message);

	#endregion Helpers

	private enum Op { cpyn, cpyr, inc, dec, jnznn, jnzrn, jnznr, jnzrr, outn, outr }

	private List<(Op, int, int)> LoadProgram(string input)
	{
		var program = new List<(Op, int, int)>();

		GrammarHelper.ParseInput(logger, input, Resources.Day25Grammar,
			scopeControllerAction: null,
			typeCheckerAction: null,
			codeGeneratorAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "c_cpy":
						var target = valueStack.Pop();
						var source = valueStack.Pop();
						var cpyOp = Op.cpyn;
						if (!int.TryParse(source, out var sourceValue))
						{
							sourceValue = source[0] - 'a';
							cpyOp = Op.cpyr;
						}
						var targetValue = target[0] - 'a';
						program.Add((cpyOp, sourceValue, targetValue));
						break;
					case "c_inc":
						var incReg = valueStack.Pop();
						var incValue = incReg[0] - 'a';
						program.Add((Op.inc, incValue, 0));
						break;
					case "c_dec":
						var decReg = valueStack.Pop();
						var decValue = decReg[0] - 'a';
						program.Add((Op.dec, decValue, 0));
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
							(false, false) => Op.jnznn,
							(true, false) => Op.jnzrn,
							(false, true) => Op.jnznr,
							(true, true) => Op.jnzrr,
						};
						program.Add((jnzOp, testValue, jumpValue));
						break;
					case "c_out":
						{
							var output = valueStack.Pop();
							var outOp = Op.outn;
							if (!int.TryParse(output, out var outputValue))
							{
								outputValue = output[0] - 'a';
								outOp = Op.outr;
							}
							program.Add((outOp, outputValue, 0));
						}
						break;
				}
			});

		foreach (var (op, arg1, arg2) in program)
			LoggerSendDebug($"{op,-5} {arg1} {arg2}");

		return program;
	}

	private int SolvePart1(List<(Op, int, int)> program)
	{
		int signalType = 0;
		while (true)
		{
			var isClock = RunProgram(program, signalType, true);
			if (isClock) 
				return signalType;
			signalType++;
		}
	}

	private int SolvePart2(List<(Op, int, int)> program)
	{
		int signalType = 0;
		while (true)
		{
			var isClock = RunProgram(program, signalType, false);
			if (isClock)
				return signalType;
			signalType++;
		}
	}

	private bool RunProgram(List<(Op, int, int)> program, int signalType, bool showShort)
	{
		var registers = new int[4] { signalType, 0, 0, 0 };

		int pc = 0;

		var output = new List<int>();
		var states = new List<int[]>();

		var isClock = true;
		var isRepeating = false;
		var doContinue = true;

		while (pc < program.Count && doContinue)
		{
			var (op, arg1, arg2) = program[pc];
			switch (op)
			{
				case Op.cpyn:
					registers[arg2] = arg1;
					pc++;
					break;
				case Op.cpyr:
					registers[arg2] = registers[arg1];
					pc++;
					break;
				case Op.inc:
					registers[arg1]++;
					pc++;
					break;
				case Op.dec:
					registers[arg1]--;
					pc++;
					break;
				case Op.jnznn:
					if (arg1 != 0)
						pc += arg2;
					else
						pc++;
					break;
				case Op.jnzrn:
					if (registers[arg1] != 0)
						pc += arg2;
					else
						pc++;
					break;
				case Op.jnznr:
					if (arg1 != 0)
						pc += registers[arg2];
					else
						pc++;
					break;
				case Op.jnzrr:
					if (registers[arg1] != 0)
						pc += registers[arg2];
					else
						pc++;
					break;
				case Op.outn:
					SendOutput(arg1);
					pc++;
					break;
				case Op.outr:
					SendOutput(registers[arg1]);
					pc++;
					break;
			}

			//LoggerSendVerbose($"{op,-5} {arg1,-3} {arg2,-3} pc = {pc,2}, [{string.Join(", ", registers)}]");
		}

		LoggerSendDebug($"Signal {signalType,3} => {string.Join("", output)} {(isRepeating ? "repeating" : "")}");

		return isClock && isRepeating;

		void SendOutput(int outputValue)
		{
			LoggerSendVerbose($"output state = [{string.Join(", ", registers)}]");

			if (!isRepeating && states.Any(state => Enumerable.SequenceEqual(state, registers)))
			{
				isRepeating = true;
				doContinue = false;
				return;
			}

			output.Add(outputValue);

			if (isClock && output.Count>1)
			{
				var out0 = output[0];
				var out1 = output[1];

				if (out0 == out1)
				{
					isClock = false;
					if (showShort)
					{
						doContinue = false;
						return;
					}
				}

				for (var i = 0; i < output.Count; i++)
				{
					if (i % 2 == 0 && output[i] != out0 ||
						i % 2 == 1 && output[i] != out1)
					{
						isClock = false;
						if (showShort)
						{
							doContinue = false;
							return;
						}
					}
				}
			}

			if (output.Count >= 100)
			{
				doContinue = false;
				return;
			}

			states.Add(registers.ToArray());
		}
	}
}
