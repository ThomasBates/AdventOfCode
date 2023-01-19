using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day23 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 23;

	public string Name => $"Day 23";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day23Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day23(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day23), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day23), message);

	#endregion Helpers

	private enum Op 
	{
		cpynn, cpyrn, cpynr, cpyrr, 
		incn, incr, 
		decn, decr, 
		jnznn, jnzrn, jnznr, jnzrr, 
		tgln, tglr 
	}

	private List<(Op, int, int)> LoadData(string input)
	{
		var program = new List<(Op, int, int)>();

		GrammarHelper.ParseInput(logger, input, Resources.Day23Grammar,
			scopeControllerAction: null,
			typeCheckerAction: null,
			codeGeneratorAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "c_cpy":
						var target = valueStack.Pop();
						var source = valueStack.Pop();
						var cpyOp = Op.cpynr;
						if (!int.TryParse(source, out var sourceValue))
						{
							sourceValue = source[0] - 'a';
							cpyOp = Op.cpyrr;
						}
						var targetValue = target[0] - 'a';
						program.Add((cpyOp, sourceValue, targetValue));
						break;
					case "c_inc":
						var incReg = valueStack.Pop();
						var incValue = incReg[0] - 'a';
						program.Add((Op.incr, incValue, 0));
						break;
					case "c_dec":
						var decReg = valueStack.Pop();
						var decValue = decReg[0] - 'a';
						program.Add((Op.decr, decValue, 0));
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
					case "c_tgl":
						{
							var count = valueStack.Pop();
							var tglOp = Op.tgln;
							if (!int.TryParse(count, out var countValue))
							{
								countValue = count[0] - 'a';
								tglOp = Op.tglr;
							}
							program.Add((tglOp, countValue, 0));
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
		var result = RunProgram(program, new[] { 7, 0, 0, 0 });

		return result;
	}

	private int SolvePart2(List<(Op, int, int)> program)
	{
		var result = RunProgram(program, new[] { 12, 0, 0, 0 });

		return result;
	}

	private int RunProgram(List<(Op, int, int)> program, int[] registers)
	{
		if (registers.Length < 4)
			return 0;

		int pc = 0;

		while (pc < program.Count)
		{
			var (op, arg1, arg2) = program[pc];
			switch (op)
			{
				case Op.cpynn:
					pc++;
					break;
				case Op.cpyrn:
					pc++;
					break;
				case Op.cpynr:
					registers[arg2] = arg1;
					pc++;
					break;
				case Op.cpyrr:
					registers[arg2] = registers[arg1];
					pc++;
					break;
				case Op.incn:
					pc++;
					break;
				case Op.incr:
					registers[arg1]++;
					pc++;
					break;
				case Op.decn:
					pc++;
					break;
				case Op.decr:
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
				case Op.tgln:
					Toggle(pc + arg1);
					pc++;
					break;
				case Op.tglr:
					Toggle(pc + registers[arg1]);
					pc++;
					break;
			}

			//LoggerSendVerbose($"{op,-5} {arg1,-3} {arg2,-3} pc = {pc,2}, [{string.Join(", ", registers)}]");
		}

		return registers[0];

		void Toggle(int i)
		{
			if (i >= program.Count)
				return;

			var (op, arg1, arg2) = program[i];

			var newOp = op switch
			{
				Op.cpynn => Op.jnznn,
				Op.cpyrn => Op.jnzrn,
				Op.cpynr => Op.jnznr,
				Op.cpyrr => Op.jnzrr,
				Op.incn => Op.decn,
				Op.incr => Op.decr,
				Op.decn => Op.incn,
				Op.decr => Op.incr,
				Op.jnznn => Op.cpynn,
				Op.jnzrn => Op.cpynn,
				Op.jnznr => Op.cpynr,
				Op.jnzrr => Op.cpyrr,
				Op.tgln => Op.incn,
				Op.tglr => Op.incr,
				_ => op
			};

			program[i] = (newOp, arg1, arg2);
		}
	}
}
