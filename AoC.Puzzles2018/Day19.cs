﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day19 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 19;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs 01", Resources.Day19Inputs01},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day19()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
		Solvers.Add("Solve Test 1", SolveTest01);
		Solvers.Add("Solve Test 2", SolveTest02);

		operations = new Dictionary<string, Action<int[], int[]>>
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
	}

	#endregion Constructors

	private readonly Dictionary<string, Action<int[], int[]>> operations;

	private class Instruction
	{
		public string OpCode;
		public int[] Parameters;
		public override string ToString()
		{
			return $"{OpCode} {Parameters[0]} {Parameters[1]} {Parameters[2]}";
		}
	}

	private int _IPRegister;
	private List<Instruction> _program;

	private readonly PerformanceTimer timer1 = new();
	private readonly PerformanceTimer timer2 = new();


	public string SolvePart1(string input)
	{
		timer1.Reset();
		timer2.Reset();
		var result = new StringBuilder();

		LoadDataFromInput(input);

		int[] registers = new int[6] { 0, 0, 0, 0, 0, 0 };
		timer1.Start();
		RunProgram(registers, result);
		timer1.Stop();
		result.AppendLine(timer1.Show("SolvePart1"));
		result.AppendLine(timer2.Show("RunProgram"));
		result.AppendLine($"Register 0 = {registers[0]}");

		return result.ToString();
	}

	public string SolvePart2(string input)
	{
		var result = new StringBuilder();

		LoadDataFromInput(input);

		int[] registers = new int[6] { 1, 0, 0, 0, 0, 0 };
		RunProgram(registers, result);

		result.AppendLine($"Register 0 = {registers[0]}");

		return result.ToString();
	}

	public string SolveTest01(string input)
	{
		var result = new StringBuilder();

		int a = 0;
		int f = 876;
		for (int b = 1; b <= f; b++)
		{
			for (int c = 1; c <= f; c++)
			{
				if (b * c == f)
				{
					a += b;
					result.AppendLine($"{b} * {c} = {f}");
				}
			}
		}

		result.AppendLine($"a = {a}");
		return result.ToString();
	}

	public string SolveTest02(string input)
	{
		var result = new StringBuilder();

		int a = 0;
		//int f = 876;
		int f = 10551276;

		int b = 1;
		int c = f;

		while (b <= c)
		{
			if (b * c == f)
			{
				a += b;
				a += c;
				result.AppendLine($"{b} * {c} = {f}");
			}
			b++;
			while (b * c > f)
			{
				c--;
			}


		}

		result.AppendLine($"a = {a}");
		return result.ToString();
	}


	private void RunProgram(int[] registers, StringBuilder result)
	{
		long clock = 0;
		while (true)
		{
			timer2.Start();
			string before = RegistersToString(registers);

			int ip = registers[_IPRegister];
			if (ip < 0 || ip >= _program.Count)
			{
				timer2.Stop();
				break;
			}

			var instruction = _program[ip];
			var operation = operations[instruction.OpCode];
			operation(registers, instruction.Parameters);

			string after = RegistersToString(registers);

			//if (lastA != registers[0])
			//{
			//	string line = $"{clock} - ip={ip} {before} {instruction} {after}";
			//	result.AppendLine(line);
			//	lastA = registers[0];
			//	//result.AppendLine($"a = {registers[0]}");
			//}

			result.AppendLine($"{clock} - ip={ip} {before} {instruction} {after}");

			registers[_IPRegister]++;
			clock++;
			timer2.Stop();
			if (clock > 1000)
				break;
		}
		result.AppendLine($"{clock} - {RegistersToString(registers)}");
	}

	string RegistersToString(int[] registers)
	{
		return string.Format("[{0}, {1}, {2}, {3}, {4}, {5}]",
			registers[0],
			registers[1],
			registers[2],
			registers[3],
			registers[4],
			registers[5]);
	}

	private void LoadDataFromInput(string input)
	{
		_program = new List<Instruction>();

		InputHelper.TraverseInputLines(input, line =>
		{
			string[] parts = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
			if (line[0] == '#')
			{
				if (string.Equals(parts[0], "#ip"))
				{
					_IPRegister = int.Parse(parts[1]);
				}
				return;
			}

			_program.Add(new Instruction
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
