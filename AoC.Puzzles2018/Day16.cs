using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day16 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 16;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		//{"Example Inputs", Resources.Day16Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day16()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);

		operations = new List<Action<int[], int[]>>
		{
			ADDR, ADDI,
			MULR, MULI,
			BANR, BANI,
			BORR, BORI,
			SETR, SETI,
			GTIR, GTRI, GTRR,
			EQIR, EQRI, EQRR
		};
	}

	#endregion Constructors

	private readonly List<Action<int[], int[]>> operations;
	private readonly Dictionary<int, int> operationsMap = new();

	private class Sample
	{
		public int[] RegistersBefore;
		public int[] Instruction;
		public int[] RegistersAfter;
	}

	private readonly List<Sample> samples = new();
	private readonly List<int[]> instructions = new();


	public string SolvePart1(string input)
	{
		var result = new StringBuilder();

		LoadDataFromInput(input);

		int sampleCount = 0;
		foreach (var sample in samples)
		{
			int matchCount = 0;
			foreach (var operation in operations)
			{
				int[] registers = new int[4];
				Array.Copy(sample.RegistersBefore, registers, 4);

				operation(registers, sample.Instruction);

				bool match = registers.SequenceEqual(sample.RegistersAfter);
				if (match)
				{
					matchCount++;
				}
			}
			if (matchCount >= 3)
			{
				sampleCount++;
			}
		}

		result.AppendLine($"{sampleCount} samples behave like three or more opcodes.");

		return result.ToString();
	}

	public string SolvePart2(string input)
	{
		var result = new StringBuilder();

		LoadDataFromInput(input);

		DetermineOpCodes();

		int[] registers = new int[4] { 0, 0, 0, 0 };

		foreach (var instruction in instructions)
		{
			var operation = operations[operationsMap[instruction[0]]];
			operation(registers, instruction);
		}

		Display("Registers: ", registers, result);
		
		return result.ToString();
	}

	private void Display(string caption, int[] vector, StringBuilder result)
	{
		result.Append(caption);
		for (int i = 0; i < 4; i++)
		{
			result.Append($" {vector[i]}");
		}
		result.AppendLine();
	}

	private void LoadDataFromInput(string input)
	{
		samples.Clear();

		Sample sample = null;

		InputHelper.TraverseInputLines(input, line =>
		{
			string[] parts = line.Split(new char[] { ':', '[', ']', ',', ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			if ((parts.Length == 5) && (string.Equals(parts[0], "Before")))
			{
				sample = new Sample
				{
					RegistersBefore = new int[4],
					Instruction = new int[4],
					RegistersAfter = new int[4]
				};

				for (int i = 0; i < 4; i++)
				{
					sample.RegistersBefore[i] = int.Parse(parts[i + 1]);
				}
			}
			else if (parts.Length == 4)
			{
				if (sample != null)
				{
					for (int i = 0; i < 4; i++)
					{
						sample.Instruction[i] = int.Parse(parts[i]);
					}
				}
				else
				{
					int[] instruction = new int[4];
					for (int i = 0; i < 4; i++)
					{
						instruction[i] = int.Parse(parts[i]);
					}
					instructions.Add(instruction);
				}
			}
			else if ((parts.Length == 5) && (string.Equals(parts[0], "After")))
			{
				for (int i = 0; i < 4; i++)
				{
					sample.RegistersAfter[i] = int.Parse(parts[i + 1]);
				}
				samples.Add(sample);
				sample = null;
			}
		});
	}

	private void DetermineOpCodes()
	{
		operationsMap.Clear();

		List<KeyValuePair<int, int>> possibilities = new();

		foreach (var sample in samples)
		{
			for (int opIndex = 0; opIndex < operations.Count; opIndex++)
			{
				var operation = operations[opIndex];

				int[] registers = new int[4];
				Array.Copy(sample.RegistersBefore, registers, 4);

				operation(registers, sample.Instruction);

				bool match = registers.SequenceEqual(sample.RegistersAfter);

				if (match)
				{
					int opCode = sample.Instruction[0];
					if (!possibilities.Any(p => (p.Key == opCode) && (p.Value == opIndex)))
					{
						possibilities.Add(new KeyValuePair<int, int>(opCode, opIndex));
					}
				}
			}
		}

		while (possibilities.Count > 0)
		{
			var single = possibilities.Where(p1 => possibilities.Count(p2 => p2.Key == p1.Key) == 1).ToList();

			if (single.Count != 1)
			{
				return;
			}

			int opCode = single[0].Key;
			int opIndex = single[0].Value;

			operationsMap.Add(opCode, opIndex);

			foreach (var possibility in possibilities.Where(p => p.Value == opIndex).ToList())
			{
				possibilities.Remove(possibility);
			}
		}
	}

	#region Operations

	private void ADDR(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int regB = instruction[2];
		int valB = registers[regB];

		int result = valA + valB;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void ADDI(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int valB = instruction[2];

		int result = valA + valB;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void MULR(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int regB = instruction[2];
		int valB = registers[regB];

		int result = valA * valB;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void MULI(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int valB = instruction[2];

		int result = valA * valB;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void BANR(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int regB = instruction[2];
		int valB = registers[regB];

		int result = valA & valB;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void BANI(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int valB = instruction[2];

		int result = valA & valB;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void BORR(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int regB = instruction[2];
		int valB = registers[regB];

		int result = valA | valB;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void BORI(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int valB = instruction[2];

		int result = valA | valB;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void SETR(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int result = valA;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void SETI(int[] registers, int[] instruction)
	{
		int valA = instruction[1];

		int result = valA;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void GTIR(int[] registers, int[] instruction)
	{
		int valA = instruction[1];

		int regB = instruction[2];
		int valB = registers[regB];

		int result = valA > valB ? 1 : 0;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void GTRI(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int valB = instruction[2];

		int result = valA > valB ? 1 : 0;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void GTRR(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int regB = instruction[2];
		int valB = registers[regB];

		int result = valA > valB ? 1 : 0;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void EQIR(int[] registers, int[] instruction)
	{
		int valA = instruction[1];

		int regB = instruction[2];
		int valB = registers[regB];

		int result = valA == valB ? 1 : 0;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void EQRI(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int valB = instruction[2];

		int result = valA == valB ? 1 : 0;

		int regC = instruction[3];
		registers[regC] = result;
	}

	private void EQRR(int[] registers, int[] instruction)
	{
		int regA = instruction[1];
		int valA = registers[regA];

		int regB = instruction[2];
		int valB = registers[regB];

		int result = valA == valB ? 1 : 0;

		int regC = instruction[3];
		registers[regC] = result;
	}

	#endregion Operations
}
