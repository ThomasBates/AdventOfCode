using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;

using AoC.Common;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day21 : IPuzzle
{
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

	public Day21()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		var output = new StringBuilder();

		LoadDataFromInput(input, output);

		ProcessDataForPart1(output);

		return output.ToString();
	}

	private string SolvePart2(string input)
	{
		var output = new StringBuilder();

		LoadDataFromInput(input, output);

		ProcessDataForPart2(output);

		return output.ToString();
	}

	#endregion Solvers

	private readonly Dictionary<string, string> monkeys = new();

	private void LoadDataFromInput(string input, StringBuilder output = null)
	{
		//  First Clear Data
		monkeys.Clear();

		Helper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			monkeys[parts[0].Trim()] = parts[1].Trim();
		});
	}

	private void ProcessDataForPart1(StringBuilder output = null)
	{
		var answer = Evaluate("root");

		output?.AppendLine($"answer = {answer}");
	}

	string indent = "";

	private long Evaluate(string name, StringBuilder output = null)
	{
		var job = monkeys[name];
		output?.AppendLine($"{indent}{name}: {job}");
		indent = $"  {indent}";

		var parts = job.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length== 1)
		{
			indent = indent.Substring(2);
			output?.AppendLine($"{indent}{name} = {parts[0]}");
			return long.Parse(parts[0]);
		}

		var (name1, op, name2) = (parts[0], parts[1], parts[2]);

		var p1 = Evaluate(name1, output);
		var p2 = Evaluate(name2, output);

		var result = op switch
		{
			"+" => p1 + p2,
			"-" => p1 - p2,
			"*" => p1 * p2,
			"/" => p1 / p2,
			_ => throw new Exception()
		};
		indent = indent.Substring(2);
		output?.AppendLine($"{indent}{name} = {result}");
		return result;
	}

	private void ProcessDataForPart2(StringBuilder output = null)
	{
		var job = monkeys["root"];
		var parts = job.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		var (name1, name2) = (parts[0], parts[2]);

		var found = FindHuman(name1);

		var value = Evaluate(found ? name2 : name1);
		long humn = Solve(found ? name1 : name2, value);
		output?.AppendLine($"humn = {humn}");
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

	private long Solve(string name, long value, StringBuilder output = null)
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
			var p2 = Evaluate(name2, output);
			var p1 = op switch
			{
				"+" => value - p2,
				"-" => value + p2,
				"*" => value / p2,
				"/" => value * p2,
				_ => throw new Exception()
			};

			return Solve(name1, p1, output);
		}
		else
		{
			var p1 = Evaluate(name1, output);
			var p2 = op switch
			{
				"+" => value - p1,
				"-" => p1 - value,
				"*" => value / p1,
				"/" => p1 / value,
				_ => throw new Exception()
			};

			return Solve(name2, p2, output);
		}
	}
}
