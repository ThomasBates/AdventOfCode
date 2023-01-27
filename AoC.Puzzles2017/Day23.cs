using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day23 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 23;

	public string Name => $"Day 23";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		//{"Example Inputs", Resources.Day23Inputs},
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
		Solvers.Add("Solve Part 2 (optimized)", _ => OptimizedProgram(1).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day23), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day23), message);

	#endregion Helpers

	private enum Op
	{
		none,
		setrn, setrr,
		subrn, subrr,
		mulrn, mulrr,
		jnznn, jnznr, jnzrn, jnzrr
	}

	private class Data
	{
		public List<(Op, int, int)> Program = new();
	}

	private Data LoadData(string input)
	{
		var data = new Data();

		const string numType = nameof(numType);
		const string regType = nameof(regType);

		GrammarHelper.ParseInput(logger, input, Resources.Day23Grammar,
			scopeControllerAction: null,
			typeCheckerAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "t_number":
						valueStack.Push(numType);
						break;
					case "t_register":
						var register = valueStack.Pop();
						var registerValue = register[0] - 'a';
						valueStack.Push(registerValue.ToString());
						valueStack.Push(regType);
						break;
				}
			},
			codeGeneratorAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "c_set":
					case "c_sub":
					case "c_mul":
						{
							var yType = valueStack.Pop();
							var y = int.Parse(valueStack.Pop());
							var xType = valueStack.Pop();
							var x = int.Parse(valueStack.Pop());

							var op = (token, yType) switch
							{
								("c_set", numType) => Op.setrn,
								("c_set", regType) => Op.setrr,
								("c_sub", numType) => Op.subrn,
								("c_sub", regType) => Op.subrr,
								("c_mul", numType) => Op.mulrn,
								("c_mul", regType) => Op.mulrr,
								_ => throw new Exception()
							};

							data.Program.Add((op, x, y));
						}
						break;
					case "c_jnz":
						{
							var yType = valueStack.Pop();
							var y = int.Parse(valueStack.Pop());
							var xType = valueStack.Pop();
							var x = int.Parse(valueStack.Pop());

							var op = (xType, yType) switch
							{
								(numType, numType) => Op.jnznn,
								(numType, regType) => Op.jnznr,
								(regType, numType) => Op.jnzrn,
								(regType, regType) => Op.jnzrr,
								_ => throw new Exception()
							};

							data.Program.Add((op, x, y));
						}
						break;
				}
			});

		foreach (var (op, arg1, arg2) in data.Program)
			SendDebug($"{op,-5} {arg1} {arg2}");

		return data;
	}

	private class Process
	{
		public int ID;
		public int PC;
		public long[] Registers = new long[8];
		public int MulCount;
	}

	private int SolvePart1(Data data)
	{
		var process = new Process();

		while (ClockProgram(process, data.Program));

		return process.MulCount;
	}

	private int SolvePart2(Data data)
	{
		var process = new Process();
		process.Registers[0] = 1;

		while (ClockProgram(process, data.Program)) ;

		return process.MulCount;
	}

	private bool ClockProgram(Process process, List<(Op, int, int)> program)
	{
		if (process.PC < 0 || process.PC >= program.Count)
			return false;

		var (op, x, y) = program[process.PC];

		switch (op)
		{
			case Op.none:
				break;
			case Op.setrn:
				process.Registers[x] = y;
				process.PC++;
				break;
			case Op.setrr:
				process.Registers[x] = process.Registers[y];
				process.PC++;
				break;
			case Op.subrn:
				process.Registers[x] = process.Registers[x] - y;
				process.PC++;
				break;
			case Op.subrr:
				process.Registers[x] = process.Registers[x] - process.Registers[y];
				process.PC++;
				break;
			case Op.mulrn:
				process.Registers[x] = process.Registers[x] * y;
				process.MulCount++;
				process.PC++;
				break;
			case Op.mulrr:
				process.Registers[x] = process.Registers[x] * process.Registers[y];
				process.MulCount++;
				process.PC++;
				break;
			case Op.jnznn:
				if (x != 0)
					process.PC += y;
				else
					process.PC++;
				break;
			case Op.jnznr:
				if (x != 0)
					process.PC += (int)process.Registers[y];
				else
					process.PC++;
				break;
			case Op.jnzrn:
				if (process.Registers[x] != 0)
					process.PC += y;
				else
					process.PC++;
				break;
			case Op.jnzrr:
				if (process.Registers[x] != 0)
					process.PC += (int)process.Registers[y];
				else
					process.PC++;
				break;
		}

		SendVerbose($"process {process.ID}: {op,-5} {x,3} {y,6} pc = {process.PC,2} [{string.Join(", ", process.Registers)}] mul = {process.MulCount}");

		return true;
	}

	private int OptimizedProgram(int a)
	{
		var h = 0;

		int b = 84;
		int c = 84;
		if (a != 0)
		{
			b = 108400;
			c = 125400; // b+17000
		}

		while (true)
		{
			int f = 1;
			int d = 2;
			do  //  b-2 times
			{
				int e = b / d;
				if (e < d)
					break;
				if (d * e == b)
					f = 0;
				if (f == 0)
					break;
				d += 1;
			} while (d != b);
			if (f == 0)
				h += 1;
			if (b == c)
				return h;
			b += 17;
		}
	}
}
