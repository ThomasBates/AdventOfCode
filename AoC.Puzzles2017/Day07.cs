using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day07 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

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

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day07), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day07), message);

	#endregion Helpers

	private class Input
	{
		public string Name { get; set; }
		public int Weight { get; set; }
		public List<string> Above { get; } = new();
	}

	private class Node
	{
		public string Name { get; set; }
		public int Weight { get; set; }
		public Node BelowNode { get; set; }
		public List<Node> AboveNodes { get; } = new();

		private int? totalWeight;
		public int TotalWeight
		{
			get
			{
				totalWeight ??= Weight + AboveNodes.Sum(n => n.TotalWeight);
				return totalWeight.Value;
			}
		}

		private bool? isBalanced;
		public bool IsBalanced
		{
			get
			{
				isBalanced ??= AboveNodes.Select(n => n.TotalWeight).Distinct().Count() < 2;
				return isBalanced.Value;
			}
		}

		public override string ToString() =>
			$"{Name} ({Weight}){(AboveNodes.Count == 0 ? "" : $" -> {string.Join(", ", AboveNodes.Select(n => $"{n.Name} ({n.TotalWeight})"))}")}";
	}

	private List<Input> LoadData(string input)
	{
		var programs = new List<Input>();
		Input current = null;

		GrammarHelper.ParseInput(logger, input, Resources.Day07Grammar,
			scopeControllerAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "s_node":
						current = new() { Name = valueStack.Pop() };
						programs.Add(current);
						break;
				}
			},
			typeCheckerAction: null,
			codeGeneratorAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "c_weight":
						current.Weight = int.Parse(valueStack.Pop());
						break;
					case "c_above":
						current.Above.Add(valueStack.Pop());
						break;
				}
			});
		
		return programs;
	}

	private string SolvePart1(List<Input> inputs)
	{
		Node baseProgram = BuildTree(inputs);

		return baseProgram.Name;
	}

	private int SolvePart2(List<Input> inputs)
	{
		Node baseProgram = BuildTree(inputs);

		return CheckBalance(baseProgram);
	}

	private Node BuildTree(List<Input> inputs)
	{
		var allNodes = new Dictionary<string, Node>();
		foreach (var input in inputs)
			allNodes.Add(input.Name, new Node { Name = input.Name, Weight = input.Weight });

		foreach (var input in inputs)
		{
			var node = allNodes[input.Name];
			foreach (var above in input.Above)
			{
				var aboveNode = allNodes[above];
				if (aboveNode.BelowNode != null)
				{
					logger.SendError(nameof(Day07), $"program {aboveNode.Name} is supported more than once.");
					continue;
				}

				aboveNode.BelowNode = node;
				node.AboveNodes.Add(aboveNode);
			}
		}

		var baseNode = allNodes[inputs[0].Name];
		while (true)
		{
			if (baseNode.BelowNode == null)
				return baseNode;
			baseNode = baseNode.BelowNode;
		}
	}

	private int CheckBalance(Node program)
	{
		SendDebug($"Checking {program}:\n\n    {string.Join("\n    ", program.AboveNodes)}\n");

		if (program.IsBalanced)
			return -1;

		foreach (var above in program.AboveNodes.Where(n => !n.IsBalanced))
		{
			var result = CheckBalance(above);
			if (result >= 0)
				return result;
		}

		var ordered = program.AboveNodes.OrderBy(n => n.TotalWeight).ToList();
		var difference = ordered[1].TotalWeight - ordered[0].TotalWeight;
		if (difference > 0)
			return ordered[0].Weight + difference;

		ordered = program.AboveNodes.OrderByDescending(n => n.TotalWeight).ToList();
		difference = ordered[0].TotalWeight - ordered[1].TotalWeight;
		if (difference > 0)
			return ordered[0].Weight - difference;

		return -1;
	}
}
