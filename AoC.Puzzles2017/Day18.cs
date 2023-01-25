using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day18 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 18;

	public string Name => $"Day 18";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs (1)", Resources.Day18Inputs01},
		{"Example Inputs (2)", Resources.Day18Inputs02},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day18(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadProgram(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadProgram(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day18), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day18), message);

	#endregion Helpers

	private enum Op 
	{
		none,
		sndn, sndr, 
		setrn, setrr, 
		addrn, addrr, 
		mulrn, mulrr,
		modrn, modrr,
		rcvr,
		jgznn, jgznr, jgzrn, jgzrr
	}

	private List<(Op, int, int)> LoadProgram(string input)
	{
		var program = new List<(Op, int, int)>();

		const string numType = nameof(numType);
		const string regType = nameof(regType);

		GrammarHelper.ParseInput(logger, input, Resources.Day18Grammar,
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
					case "c_snd":
					case "c_rcv":
						{
							var xType = valueStack.Pop();
							var x = int.Parse(valueStack.Pop());

							var op = (token, xType) switch
							{
								("c_snd", numType) => Op.sndn,
								("c_snd", regType) => Op.sndr,
								("c_rcv", regType) => Op.rcvr,
								_ => throw new Exception()
							};

							program.Add((op, x, 0));
						}
						break;
					case "c_set":
					case "c_add":
					case "c_mul":
					case "c_mod":
						{
							var yType = valueStack.Pop();
							var y = int.Parse(valueStack.Pop());
							var xType = valueStack.Pop();
							var x = int.Parse(valueStack.Pop());

							var op = (token, yType) switch
							{
								("c_set", numType) => Op.setrn,
								("c_set", regType) => Op.setrr,
								("c_add", numType) => Op.addrn,
								("c_add", regType) => Op.addrr,
								("c_mul", numType) => Op.mulrn,
								("c_mul", regType) => Op.mulrr,
								("c_mod", numType) => Op.modrn,
								("c_mod", regType) => Op.modrr,
								_ => throw new Exception()
							};

							program.Add((op, x, y));
						}
						break;
					case "c_jgz":
						{
							var yType = valueStack.Pop();
							var y = int.Parse(valueStack.Pop());
							var xType = valueStack.Pop();
							var x = int.Parse(valueStack.Pop());

							var op = (xType, yType) switch
							{
								(numType, numType) => Op.jgznn,
								(numType, regType) => Op.jgznr,
								(regType, numType) => Op.jgzrn,
								(regType, regType) => Op.jgzrr,
								_ => throw new Exception()
							};

							program.Add((op, x, y));
						}
						break;
				}
			});

		foreach (var (op, arg1, arg2) in program)
			SendDebug($"{op,-5} {arg1} {arg2}");

		return program;
	}

	private class Process
	{
		public int ID;
		public int PC;
		public long[] Registers = new long[26];
		public Queue<long> SndQueue;
		public Queue<long> RcvQueue;
		public long LastSend;
		public int SendCount;
		public bool IsBlocking;
	}

	private long SolvePart1(List<(Op, int, int)> program)
	{
		var p = new Process
		{
			SndQueue = new(),
			RcvQueue = new()
		};

		while(true)
		{
			ClockProgram(p, program);

			if (p.IsBlocking)
				return p.LastSend;
		}
	}

	private long SolvePart2(List<(Op, int, int)> program)
	{
		var p0 = new Process
		{
			SndQueue = new(),
			RcvQueue = new()
		};
		var p1 = new Process
		{
			ID = 1,
			SndQueue = p0.RcvQueue,
			RcvQueue = p0.SndQueue
		};
		p0.Registers['p' - 'a'] = p0.ID;
		p1.Registers['p' - 'a'] = p1.ID;

		while (true)
		{
			ClockProgram(p0, program);
			ClockProgram(p1, program);

			if (p0.IsBlocking && p1.IsBlocking)
				return p1.SendCount;
		}
	}

	private void ClockProgram(Process process, List<(Op, int, int)> program)
	{
		if (process.PC < 0 || process.PC >= program.Count) 
			return;

		var (op, x, y) = program[process.PC];

		switch (op)
		{
			case Op.none:
				break;
			case Op.sndn:
				process.SndQueue.Enqueue(x);
				process.LastSend = x;
				process.SendCount++;
				process.PC++;
				break;
			case Op.sndr:
				process.SndQueue.Enqueue(process.Registers[x]);
				process.LastSend = process.Registers[x];
				process.SendCount++;
				process.PC++;
				break;
			case Op.setrn:
				process.Registers[x] = y;
				process.PC++;
				break;
			case Op.setrr:
				process.Registers[x] = process.Registers[y];
				process.PC++;
				break;
			case Op.addrn:
				process.Registers[x] = process.Registers[x] + y;
				process.PC++;
				break;
			case Op.addrr:
				process.Registers[x] = process.Registers[x] + process.Registers[y];
				process.PC++;
				break;
			case Op.mulrn:
				process.Registers[x] = process.Registers[x] * y;
				process.PC++;
				break;
			case Op.mulrr:
				process.Registers[x] = process.Registers[x] * process.Registers[y];
				process.PC++;
				break;
			case Op.modrn:
				process.Registers[x] = process.Registers[x] % y;
				process.PC++;
				break;
			case Op.modrr:
				process.Registers[x] = process.Registers[x] % process.Registers[y];
				process.PC++;
				break;
			case Op.rcvr:
				if (process.RcvQueue.Count > 0)
				{
					process.IsBlocking = false;
					process.Registers[x] = process.RcvQueue.Dequeue();
					process.PC++;
				}
				else
				{
					process.IsBlocking = true;
				}
				break;
			case Op.jgznn:
				if (x > 0)
					process.PC += y;
				else
					process.PC++;
				break;
			case Op.jgznr:
				if (x > 0)
					process.PC += (int)process.Registers[y];
				else
					process.PC++;
				break;
			case Op.jgzrn:
				if (process.Registers[x] > 0)
					process.PC += y;	
				else
					process.PC++;
				break;
			case Op.jgzrr:
				if (process.Registers[x] > 0)
					process.PC += (int)process.Registers[y];
				else
					process.PC++;
				break;
		}

		SendVerbose($"process {process.ID}: {op,-5} {x,3} {y,6} pc = {process.PC,2} [{string.Join(", ", process.Registers)}]");
	}
}
