using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day21 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 21;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day21Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day21(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataForPart1();

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataForPart2();

		return result;
	}

	#endregion Solvers

	private readonly Dictionary<string, string> monkeys = new();

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		monkeys.Clear();

		Helper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			monkeys[parts[0].Trim()] = parts[1].Trim();
		});
	}

	private string ProcessDataForPart1()
	{
		var result = Evaluate("root");

		return $"{result}";
	}

	string indent = "";

	private long Evaluate(string name)
	{
		var job = monkeys[name];
		logger.Send(SeverityLevel.Debug, nameof(Day21), $"{indent}{name}: {job}");
		indent = $"  {indent}";

		var parts = job.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length== 1)
		{
			indent = indent.Substring(2);
			logger.Send(SeverityLevel.Debug, nameof(Day21), $"{indent}{name} = {parts[0]}");
			return long.Parse(parts[0]);
		}

		var (name1, op, name2) = (parts[0], parts[1], parts[2]);

		var p1 = Evaluate(name1);
		var p2 = Evaluate(name2);

		var result = op switch
		{
			"+" => p1 + p2,
			"-" => p1 - p2,
			"*" => p1 * p2,
			"/" => p1 / p2,
			_ => throw new Exception()
		};
		indent = indent.Substring(2);
		logger.Send(SeverityLevel.Debug, nameof(Day21), $"{indent}{name} = {result}");
		return result;
	}

	private string ProcessDataForPart2()
	{
		var job = monkeys["root"];
		var parts = job.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		var (name1, name2) = (parts[0], parts[2]);

		var found = FindHuman(name1);

		var value = Evaluate(found ? name2 : name1);
		long humn = Solve(found ? name1 : name2, value);
		logger.Send(SeverityLevel.Debug, nameof(Day21), $"humn = {humn}");

		return humn.ToString();
	}

	private bool FindHuman(string name)
	{
		if (name == "humn")
			return true;

		var job = monkeys[name];
		var parts = job.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length == 1)
			return false;

		var (name1, name2) = (parts[0], parts[2]);

		return FindHuman(name1) || FindHuman(name2);
	}

	private long Solve(string name, long value)
	{
		if (name == "humn")
			return value;

		var job = monkeys[name];
		var parts = job.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		if (parts.Length == 1)
			throw new Exception();

		var (name1, op, name2) = (parts[0], parts[1], parts[2]);

		var found = FindHuman(name1);

		if (found)
		{
			var p2 = Evaluate(name2);
			var p1 = op switch
			{
				"+" => value - p2,
				"-" => value + p2,
				"*" => value / p2,
				"/" => value * p2,
				_ => throw new Exception()
			};

			return Solve(name1, p1);
		}
		else
		{
			var p1 = Evaluate(name1);
			var p2 = op switch
			{
				"+" => value - p1,
				"-" => p1 - value,
				"*" => value / p1,
				"/" => p1 / value,
				_ => throw new Exception()
			};

			return Solve(name2, p2);
		}
	}
}
