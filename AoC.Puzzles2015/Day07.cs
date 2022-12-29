using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day07 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 07;

	public string Name => $"Day 07";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day07Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day07(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		var ok = LoadDataFromInput(input);

		if (!ok)
			return "";

		var result = ProcessDataForPart1();

		return result;
	}

	private string SolvePart2(string input)
	{
		var ok = LoadDataFromInput(input);

		if (!ok)
			return "";

		var result = ProcessDataForPart2();

		return result;
	}

	#endregion Solvers

	private class Gate
	{
		public string Name { get; set; }
		public string Input1 { get; set; }
		public string Input2 { get; set; }
		public ushort Wire1 { get; set; }
		public ushort Wire2 { get; set; }
		public Gate Gate1 { get; set; }
		public Gate Gate2 { get; set; }

		public Func<ushort, ushort, ushort> Operation { get; set; }

		private ushort? output;
		public ushort Output
		{
			get
			{
				output ??= Operation(Gate1?.Output ?? Wire1, Gate2?.Output ?? Wire2);
				return output.Value;
			}
		}

		public void Reset()
		{
			output = null;
		}
	}

	private readonly Dictionary<string, Gate> allGates = new();

	private bool LoadDataFromInput(string input)
	{
		//  First Clear Data
		allGates.Clear();

		var ok = GrammarHelper.ParseInput(logger, input, Resources.Day07Grammar,
			null,
			null,
			(token, valueStack) =>
			{
				switch (token)
				{
					case "c_set":
						{
							var gate = new Gate
							{
								Name = valueStack.Pop(),
								Input1 = valueStack.Pop(),
								Operation = (input1, input2) => input1
							};
							allGates[gate.Name] = gate;
						}
						break;
					case "c_not":
						{
							var gate = new Gate
							{
								Name = valueStack.Pop(),
								Input1 = valueStack.Pop(),
								Operation = (input1, input2) => (ushort)(0xffff - input1)
							};
							allGates[gate.Name] = gate;
						}
						break;
					case "c_and":
						{
							var gate = new Gate
							{
								Name = valueStack.Pop(),
								Input2 = valueStack.Pop(),
								Input1 = valueStack.Pop(),
								Operation = (input1, input2) => (ushort)(input1 & input2)
							};
							allGates[gate.Name] = gate;
						}
						break;
					case "c_or":
						{
							var gate = new Gate
							{
								Name = valueStack.Pop(),
								Input2 = valueStack.Pop(),
								Input1 = valueStack.Pop(),
								Operation = (input1, input2) => (ushort)(input1 | input2)
							};
							allGates[gate.Name] = gate;
						}
						break;
					case "c_lshift":
						{
							var gate = new Gate
							{
								Name = valueStack.Pop(),
								Input2 = valueStack.Pop(),
								Input1 = valueStack.Pop(),
								Operation = (input1, input2) => (ushort)(input1 << input2)
							};
							allGates[gate.Name] = gate;
						}
						break;
					case "c_rshift":
						{
							var gate = new Gate
							{
								Name = valueStack.Pop(),
								Input2 = valueStack.Pop(),
								Input1 = valueStack.Pop(),
								Operation = (input1, input2) => (ushort)(input1 >> input2)
							};
							allGates[gate.Name] = gate;
						}
						break;
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			});

		if (!ok)
			return false;

		foreach (var gate in allGates.Values)
		{
			if (!string.IsNullOrEmpty(gate.Input1))
			{
				if (ushort.TryParse(gate.Input1, out var wire1))
					gate.Wire1 = wire1;
				else if (allGates.TryGetValue(gate.Input1, out var gate1))
					gate.Gate1 = gate1;
			}

			if (!string.IsNullOrEmpty(gate.Input2))
			{
				if (ushort.TryParse(gate.Input2, out var wire2))
					gate.Wire2 = wire2;
				else if (allGates.TryGetValue(gate.Input2, out var gate2))
					gate.Gate2 = gate2;
			}
		}

		return true;
	}

	private string ProcessDataForPart1()
	{
		ushort result = 0;

		if (allGates.TryGetValue("a", out var a))
		{
			result = a.Output;
		}

		foreach (var gate in allGates.Values.OrderBy(g => g.Name).ToList())
		{
			logger.SendDebug(nameof(Day07), $"{gate.Name}: {gate.Output}");
		}

		return result.ToString();
	}

	private string ProcessDataForPart2()
	{
		if (!allGates.TryGetValue("a", out var a))
			return "";

		ushort result = a.Output;

		foreach (var gate in allGates.Values.OrderBy(g => g.Name).ToList())
		{
			logger.SendDebug(nameof(Day07), $"{gate.Name}: {gate.Output}");
		}

		foreach (var gate in allGates.Values)
			gate.Reset();

		if (allGates.TryGetValue("b", out var b))
			b.Wire1 = result;

		result = a.Output;

		foreach (var gate in allGates.Values.OrderBy(g => g.Name).ToList())
		{
			logger.SendDebug(nameof(Day07), $"{gate.Name}: {gate.Output}");
		}

		return result.ToString();
	}
}
