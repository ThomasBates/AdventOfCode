using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Text.RegularExpressions;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day16 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 16;

	public string Name => $"Day 16";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		//{"Example Inputs", Resources.Day16Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day16(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		LoadDataFromInputPart1(input);

		var result = ProcessDataForPart1("3:7:2:3:0:0:5:3:2:1");

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInputPart2(input);

		var result = ProcessDataForPart2(new Sue
		{
			children = 3,
			cats = 7,
			samoyeds = 2,
			pomeranians = 3,
			akitas = 0,
			vizslas = 0,
			goldfish = 5,
			trees = 3,
			cars = 2,
			perfumes = 1,
		});

		return result;
	}

	#endregion Solvers

	private readonly List<string> compounds = new()
	{
		"children",
		"cats",
		"samoyeds",
		"pomeranians",
		"akitas",
		"vizslas",
		"goldfish",
		"trees",
		"cars",
		"perfumes",
	};

	private readonly List<string> part1Sues = new();

	private void LoadDataFromInputPart1(string input)
	{
		//  First Clear Data
		part1Sues.Clear();

		InputHelper.TraverseInputLines(input, line =>
		{
			//  Sue 1: children: 1, cars: 8, vizslas: 7
			Match match = Regex.Match(line, @"Sue (\d+): ([a-z]+): (\d+), ([a-z]+): (\d+), ([a-z]+): (\d+)");

			if (!match.Success)
			{
				logger.SendError(nameof(Day13), $"Couldn't read line: {line}");
				return;
			}

			var compound1 = match.Groups[2].Value;
			var amount1 = match.Groups[3].Value;
			var compound2 = match.Groups[4].Value;
			var amount2 = match.Groups[5].Value;
			var compound3 = match.Groups[6].Value;
			var amount3 = match.Groups[7].Value;

			var sue = new StringBuilder();

			for (int i = 0; i < compounds.Count; i++)
			{
				var compound = compounds[i];

				if (i > 0)
					sue.Append(":");
				if (compound == compound1)
					sue.Append(amount1);
				else if (compound == compound2)
					sue.Append(amount2);
				else if (compound == compound3)
					sue.Append(amount3);
				else
					sue.Append(".*");
			}
			part1Sues.Add(sue.ToString());
		});

	}

	private string ProcessDataForPart1(string ticker)
	{
		logger.SendVerbose(nameof(Day16), $"Matching {ticker}");
	
		int? bestSue = null;
		for (int i = 0; i < part1Sues.Count; i++)
		{
			var sue = part1Sues[i];
			if (Regex.IsMatch(ticker, sue))
			{
				bestSue = i;
				logger.SendVerbose(nameof(Day16), $"Sue {i + 1}: {sue} matches");
				logger.SendDebug(nameof(Day16), $"Best Sue is Sue {i + 1}");
			}
			else
			{
				logger.SendVerbose(nameof(Day16), $"Sue {i + 1}: {sue} does not match");
			}
		}

		if (bestSue.HasValue)
			return $"{bestSue + 1}";
		else
			logger.SendDebug(nameof(Day16), $"Best Sue is not found.");

		return "";
	}

	private class Sue
	{
		public int? children;
		public int? cats;
		public int? samoyeds;
		public int? pomeranians;
		public int? akitas;
		public int? vizslas;
		public int? goldfish;
		public int? trees;
		public int? cars;
		public int? perfumes;

		public override string ToString()
		{
			return $"{(children.HasValue ? $"{children}" : "?")}:"
				+ $"{(cats.HasValue ? $"{cats}" : "?")}:"
				+ $"{(samoyeds.HasValue ? $"{samoyeds}" : "?")}:"
				+ $"{(pomeranians.HasValue ? $"{pomeranians}" : "?")}:"
				+ $"{(akitas.HasValue ? $"{akitas}" : "?")}:"
				+ $"{(vizslas.HasValue ? $"{vizslas}" : "?")}:"
				+ $"{(goldfish.HasValue ? $"{goldfish}" : "?")}:"
				+ $"{(trees.HasValue ? $"{trees}" : "?")}:"
				+ $"{(cars.HasValue ? $"{cars}" : "?")}:"
				+ $"{(perfumes.HasValue ? $"{perfumes}" : "?")}";
		}
	}

	private readonly List<Sue> part2Sues = new();

	private void LoadDataFromInputPart2(string input)
	{
		//  First Clear Data
		part2Sues.Clear();

		InputHelper.TraverseInputLines(input, line =>
		{
			//  Sue 1: children: 1, cars: 8, vizslas: 7
			Match match = Regex.Match(line, @"Sue (\d+): ([a-z]+): (\d+), ([a-z]+): (\d+), ([a-z]+): (\d+)");

			var sue = new Sue();
			UpdateSue(sue, match.Groups[2].Value, int.Parse(match.Groups[3].Value));
			UpdateSue(sue, match.Groups[4].Value, int.Parse(match.Groups[5].Value));
			UpdateSue(sue, match.Groups[6].Value, int.Parse(match.Groups[7].Value));

			part2Sues.Add(sue);
		});

		static void UpdateSue(Sue sue, string compound, int amount)
		{
			switch (compound)
			{
				case "children": sue.children = amount; break;
				case "cats": sue.cats = amount; break;
				case "samoyeds": sue.samoyeds = amount; break;
				case "pomeranians": sue.pomeranians = amount; break;
				case "akitas": sue.akitas = amount; break;
				case "vizslas": sue.vizslas = amount; break;
				case "goldfish": sue.goldfish = amount; break;
				case "trees": sue.trees = amount; break;
				case "cars": sue.cars = amount; break;
				case "perfumes": sue.perfumes = amount; break;
			}
		}
	}

	private string ProcessDataForPart2(Sue ticker)
	{
		logger.SendVerbose(nameof(Day16), $"Matching {ticker}");

		int? bestSue = null;
		for (int i = 0; i < part2Sues.Count; i++)
		{
			var sue = part2Sues[i];
			if ((sue.children == null || sue.children == ticker.children) &&
				(sue.cats == null || sue.cats > ticker.cats) &&
				(sue.samoyeds == null || sue.samoyeds == ticker.samoyeds) &&
				(sue.pomeranians == null || sue.pomeranians < ticker.pomeranians) &&
				(sue.akitas == null || sue.akitas == ticker.akitas) &&
				(sue.vizslas == null || sue.vizslas == ticker.vizslas) &&
				(sue.goldfish == null || sue.goldfish < ticker.goldfish) &&
				(sue.trees == null || sue.trees > ticker.trees) &&
				(sue.cars == null || sue.cars == ticker.cars) &&
				(sue.perfumes == null || sue.perfumes == ticker.perfumes))
			{
				bestSue = i;
				logger.SendVerbose(nameof(Day16), $"Sue {i + 1}: {sue} matches");
				logger.SendDebug(nameof(Day16), $"Best Sue is Sue {i + 1}");
			}
			else
			{
				logger.SendVerbose(nameof(Day16), $"Sue {i + 1}: {sue} does not match");
			}
		}

		if (bestSue.HasValue)
			return $"{bestSue + 1}";
		else
			logger.SendDebug(nameof(Day16), $"Best Sue is not found.");

		return "";
	}
}
