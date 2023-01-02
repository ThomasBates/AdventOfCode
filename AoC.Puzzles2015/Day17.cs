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
public class Day17 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 17;

	public string Name => $"Day 17";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day17Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day17(ILogger logger)
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

		var total = containers.Count == 5 ? 25 : 150;
		var solutions = FindAllSolutions(total);

		for (int i = 0; i < solutions.Count; i++)
		{
			var solution = solutions[i];
			logger.SendDebug(nameof(Day17), $"{i + 1}: {string.Join(", ", solution)}");
		}

		return solutions.Count.ToString();
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInput(input);

		var total = containers.Count == 5 ? 25 : 150;
		var solutions = FindAllSolutions(total);

		var minSolutionSize = solutions.Min(s => s.Count);
		var minSolutions = solutions.Where(s => s.Count == minSolutionSize).ToList();

		for (int i = 0; i < minSolutions.Count; i++)
		{
			var solution = minSolutions[i];
			logger.SendDebug(nameof(Day17), $"{i + 1}: {string.Join(", ", solution)}");
		}

		return minSolutions.Count.ToString();
	}

	#endregion Solvers

	private List<int> containers = new();

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		containers.Clear();

		InputHelper.TraverseInputTokens(input, value =>
		{
			containers.Add(int.Parse(value));
		});
		containers = containers.OrderByDescending(c => c).ToList();
	}

	private List<List<int>> FindAllSolutions(int total)
	{
		var solutions = new List<List<int>>();

		var combinations = new List<List<int>>
		{
			new List<int> { 0 },
			new List<int> { containers[0] }
		};

		while (combinations.Count > 0)
		{
			var combination = combinations[0];
			combinations.RemoveAt(0);

			logger.SendVerbose(nameof(Day17), $"==> combination: {string.Join(", ", combination)}");

			int i = combination.Count;
			if (i >= containers.Count)
				break;

			var sum = combination.Sum(x => x);
			var container = containers[i];
			var remaining = 0;
			for (int j = i + 1; j < containers.Count; j++)
				remaining += containers[j];

			logger.SendVerbose(nameof(Day17), $"  sum = {sum}, container = {container}, remaining = {remaining}");

			if (sum + container + remaining < total)
				continue;

			if (sum == total)
			{
				var solution = combination.Where(c => c > 0).ToList();
				solutions.Add(solution);
				logger.SendVerbose(nameof(Day17), $"  <== solution: {string.Join(", ", solution)}");
				continue;
			}
			else if (sum < total)
			{
				var newCombination = new List<int>(combination) { 0 };
				combinations.Add(newCombination);
				logger.SendVerbose(nameof(Day17), $"  <== combination: {string.Join(", ", newCombination)}");
			}

			if (sum + container == total)
			{
				var solution = combination.Where(c => c > 0).ToList();
				solution.Add(container);
				solutions.Add(solution);
				logger.SendVerbose(nameof(Day17), $"  <== solution: {string.Join(", ", solution)}");
			}
			else if (sum + container < total)
			{
				var newCombination = new List<int>(combination) { container };
				combinations.Add(newCombination);
				logger.SendVerbose(nameof(Day17), $"  <== combination: {string.Join(", ", newCombination)}");
			}
		}

		return solutions;
	}
}
