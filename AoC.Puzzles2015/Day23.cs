using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day23 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

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

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		var program = LoadDataFromInput(input);

		var result = RunProgram(program, 0, 0);

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var program = LoadDataFromInput(input);

		var result = RunProgram(program, 1, 0);

		return result.ToString();
	}

	#endregion Solvers

	private List<(string op, string args)> LoadDataFromInput(string input)
	{
		//  First Clear Data
		var program = new List<(string op, string args)>();

		InputHelper.TraverseInputLines(input, line =>
		{
			program.Add((line.Substring(0, 3), line.Substring(4)));
		});
		
		return program;
	}

	private long RunProgram(List<(string op, string args)> program, int a, int b)
	{
		var registers = new Dictionary<string, long> { { "a", a }, { "b", b } };
		int pc = 0;

		while (pc < program.Count)
		{
			var (op, args) = program[pc];
			switch(op)
			{
				case "hlf":
					registers[args] = registers[args] / 2;
					pc++;
					break;
				case "tpl":
					registers[args] = registers[args] * 3;
					pc++;
					break;
				case "inc":
					registers[args] = registers[args] + 1;
					pc++;
					break;
				case "jmp":
					pc += int.Parse(args);
					break;
				case "jie":
					{
						var reg = args.Substring(0, 1);
						var offset = int.Parse(args.Substring(3));
						if (registers[reg] % 2 == 0)
							pc += offset;
						else
							pc++;
					}
					break;
				case "jio":
					{
						var reg = args.Substring(0, 1);
						var offset = int.Parse(args.Substring(3));
						if (registers[reg] == 1)
							pc += offset;
						else
							pc++;
					}
					break;
			}

			logger.SendVerbose(nameof(Day23), $"{op} {args,-8} pc = {pc,2}, a = {registers["a"],4}, b = {registers["b"],4}");
		}

		return registers["b"];
	}
}
