using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day24 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 24;

	public string Name => $"Day 24";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day24Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day24(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart1(data);

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart2(data);

		return result.ToString();
	}

	#endregion Solvers

	private List<int> LoadDataFromInput(string input)
	{
		//  First Clear Data
		var packages = new List<int>();

		InputHelper.TraverseInputTokens(input, value =>
		{
			packages.Add(int.Parse(value));
		});

		return packages.OrderByDescending(p => p).ToList();
	}

	private long ProcessDataForPart1(List<int> packages)
	{
		var total = packages.Sum();
		var bagSize = total / 3;

		var solutions = FindSolutions(packages, bagSize);

		var bestQE = long.MaxValue;
		foreach (var solution in solutions)
		{
			var qe = solution.Aggregate(1L, (qe, p) => qe * p);
			logger.SendDebug(nameof(Day24), $"{string.Join(" ", solution)} => QE = {qe}.");
			if (bestQE > qe)
			{
				bestQE = qe;
			}
		}

		return bestQE;
	}

	private long ProcessDataForPart2(List<int> packages)
	{
		var total = packages.Sum();
		var bagSize = total / 4;

		var solutions = FindSolutions(packages, bagSize);

		var bestQE = long.MaxValue;
		foreach (var solution in solutions)
		{
			var qe = solution.Aggregate(1L, (qe, p) => qe * p);
			logger.SendDebug(nameof(Day24), $"{string.Join(" ", solution)} => QE = {qe}.");
			if (bestQE > qe)
			{
				bestQE = qe;
			}
		}

		return bestQE;
	}

	private List<List<int>> FindSolutions(List<int> packages, int total, bool findAnySolution = false)
	{
		var solutions = new List<List<int>>();

		var combinations = new List<List<int>>()
		{
			new List<int> { 0 },
			new List<int> { packages[0] }
		};

		while (combinations.Count > 0)
		{
			var combination = combinations[0];
			combinations.RemoveAt(0);

			logger.SendVerbose(nameof(Day24), $"==> combination: {string.Join(", ", combination)}");

			int i = combination.Count;
			if (i >= packages.Count)
				break;

			if (solutions.Count > 0 && combination.Count(c => c > 0) >= solutions[0].Count)
				continue;

			var sum = combination.Sum(x => x);
			var package = packages[i];
			var remaining = 0;
			for (int j = i + 1; j < packages.Count; j++)
				remaining += packages[j];

			logger.SendVerbose(nameof(Day24), $"  sum = {sum}, package = {package}, remaining = {remaining}");

			if (sum + package + remaining < total)
				continue;

			if (sum < total)
			{
				var newCombination = new List<int>(combination) { 0 };
				combinations.Add(newCombination);
				logger.SendVerbose(nameof(Day24), $"  combination <== {string.Join(", ", newCombination)}");
			}
			if (sum + package < total)
			{
				var newCombination = new List<int>(combination) { package };
				combinations.Add(newCombination);
				logger.SendVerbose(nameof(Day24), $"  combination <== {string.Join(", ", newCombination)}");
			}

			if (sum + package == total)
			{
				var solution = combination.Where(c => c > 0).ToList();
				solution.Add(package);
				if (findAnySolution)
				{
					solutions.Add(solution);
					logger.SendDebug(nameof(Day24), $"  solution <== {string.Join(", ", solution)} => any");
					return solutions;
				}
				if (solutions.Count == 0)
				{
					solutions.Add(solution);
					logger.SendDebug(nameof(Day24), $"  solution <== {string.Join(", ", solution)} => first");
				}
				else if (solution.Count == solutions[0].Count)
				{
					solutions.Add(solution);
					logger.SendDebug(nameof(Day24), $"  solution <== {string.Join(", ", solution)} => same length as first");
				}
				else if (solution.Count < solutions[0].Count)
				{
					solutions.Clear();
					solutions.Add(solution);
					logger.SendDebug(nameof(Day24), $"  solution <== {string.Join(", ", solution)} => shorter: new first");
				}
				else
				{
					logger.SendDebug(nameof(Day24), $"  solution <== {string.Join(", ", solution)} => too long");
				}
			}
		}

		return solutions;
	}
}
