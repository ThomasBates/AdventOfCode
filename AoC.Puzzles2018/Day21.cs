using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day21 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 21;

	public string Name => "Day 21";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day21(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2b(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day21), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day21), message);

	#endregion Helpers

	private class Instruction
	{
		public string OpCode;
		public int[] Parameters;
		public override string ToString() =>
			$"{OpCode} {string.Join(" ", Parameters)}";
	}

	private class Data
	{
		public Dictionary<string, Action<int[], int[]>> operations;
		public int IPRegister;
		public List<Instruction> program = new();
	}

	private class Process
	{
		public int[] Registers = new int[6];
	}

	private Data LoadData(string input)
	{
		var data = new Data();

		InputHelper.TraverseInputLines(input, line =>
		{
			string[] parts = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
			if (string.Equals(parts[0], "#ip"))
			{
				data.IPRegister = int.Parse(parts[1]);
				return;
			}
			data.program.Add(new Instruction
			{
				OpCode = parts[0],
				Parameters = new int[]
				{
					int.Parse(parts[1]),
					int.Parse(parts[2]),
					int.Parse(parts[3])
				}
			});
		});

		data.operations = new Dictionary<string, Action<int[], int[]>>
		{
			{ "addr", ADDR },
			{ "addi", ADDI },
			{ "mulr", MULR },
			{ "muli", MULI },
			{ "banr", BANR },
			{ "bani", BANI },
			{ "borr", BORR },
			{ "bori", BORI },
			{ "setr", SETR },
			{ "seti", SETI },
			{ "gtir", GTIR },
			{ "gtri", GTRI },
			{ "gtrr", GTRR },
			{ "eqir", EQIR },
			{ "eqri", EQRI },
			{ "eqrr", EQRR }
		};
		return data;
	}

	private object SolvePart1(Data data)
	{
		var bestCount = 1000000;	//	???
		var maxKey = 1000;			//	???

		for (var key = 0; key <= maxKey; key++)
		{
			var process = new Process();
			process.Registers[0] = key;

			for (var i = 0; i < bestCount; i++)
			{
				var halted = ClockProgram(data, process);
				if (halted)
				{
					bestCount = i;
					SendDebug($"key {key,4}: count = {i}");
					break;
				}
			}
		}

		return bestCount;
	}

	private object SolvePart1b(Data data)
	{
		var key0 = Decompiled(part1: true);
		return key0;
	}

	private object SolvePart2b(Data data)
	{
		var key = Decompiled(part1: false);
		return key;
	}

	private long Decompiled(bool part1)
	{
		var keys = new List<long>();

		long a = 0;
		long d = 0;
		do
		{
			long b = d | 0x10000;
			d = 10373714;
			while (true)
			{
				long f = b & 0xff;
				d += f;
				d &= 0xffffff;
				d *= 65899;
				d &= 0xffffff;
				if (b < 256)
					break;
				b = b / 256;
			}
			SendDebug($"key = {d,8}");
			if (part1)
				break;
			if (keys.Contains(d))
			{
				d = keys[keys.Count - 1];
				break;
			}
			keys.Add(d);
		} while (d != a);

		return d;
	}

	private bool ClockProgram(Data data, Process process)
	{
		int ip = process.Registers[data.IPRegister];
		if (ip < 0 || ip >= data.program.Count)
			return true;

		var instruction = data.program[ip];
		var operation = data.operations[instruction.OpCode];
		operation(process.Registers, instruction.Parameters);

		process.Registers[data.IPRegister]++;
		return false;
	}

	#region Operations

	private void ADDR(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]] + registers[parameters[1]];
	}

	private void ADDI(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]] + parameters[1];
	}

	private void MULR(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]] * registers[parameters[1]];
	}

	private void MULI(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]] * parameters[1];
	}

	private void BANR(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]] & registers[parameters[1]];
	}

	private void BANI(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]] & parameters[1];
	}

	private void BORR(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]] | registers[parameters[1]];
	}

	private void BORI(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]] | parameters[1];
	}

	private void SETR(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]];
	}

	private void SETI(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = parameters[0];
	}

	private void GTIR(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = parameters[0] > registers[parameters[1]] ? 1 : 0;
	}

	private void GTRI(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]] > parameters[1] ? 1 : 0;
	}

	private void GTRR(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]] > registers[parameters[1]] ? 1 : 0;
	}

	private void EQIR(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = parameters[0] == registers[parameters[1]] ? 1 : 0;
	}

	private void EQRI(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]] == parameters[1] ? 1 : 0;
	}

	private void EQRR(int[] registers, int[] parameters)
	{
		registers[parameters[2]] = registers[parameters[0]] == registers[parameters[1]] ? 1 : 0;
	}

	#endregion Operations
}
