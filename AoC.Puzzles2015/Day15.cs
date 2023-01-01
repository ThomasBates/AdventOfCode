using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day15 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 15;

	public string Name => $"Day 15";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day15Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day15(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 1 (beehive)", SolvePart1Beehive);
		Solvers.Add("Solve Part 1 (genetic)", SolvePart1Genetic);
		Solvers.Add("Solve Part 2", SolvePart2);
		Solvers.Add("Solve Part 2 (beehive)", SolvePart2Beehive);
		Solvers.Add("Solve Part 2 (genetic)", SolvePart2Genetic);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataUsingBruteForce(false);

		return result;
	}

	private string SolvePart1Genetic(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataUsingGeneticAlgorithm(false);

		return result;
	}

	private string SolvePart1Beehive(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataUsingBeehiveAlgorithm(false);

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataUsingBruteForce(true);

		return result;
	}

	private string SolvePart2Genetic(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataUsingGeneticAlgorithm(true);

		return result;
	}

	private string SolvePart2Beehive(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataUsingBeehiveAlgorithm(true);

		return result;
	}

	#endregion Solvers

	private readonly List<(string, int, int, int, int, int)> ingredients = new();

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		ingredients.Clear();

		InputHelper.TraverseInputLines(input, line =>
		{
			//  Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3
			Match match = Regex.Match(line, @"([A-Za-z]+): capacity (-?\d+), durability (-?\d+), flavor (-?\d+), texture (-?\d+), calories (-?\d+)");

			if (!match.Success)
			{
				logger.SendError(nameof(Day13), $"Couldn't read line: {line}");
				return;
			}

			var name = match.Groups[1].Value;
			var capacity = int.Parse(match.Groups[2].Value);
			var durability = int.Parse(match.Groups[3].Value);
			var flavor = int.Parse(match.Groups[4].Value);
			var texture = int.Parse(match.Groups[5].Value);
			var calories = int.Parse(match.Groups[6].Value);

			logger.SendDebug(nameof(Day15), $"{name}: capacity {capacity}, durability {durability}, flavor {flavor}, texture {texture}, calories {calories}");

			ingredients.Add((name, capacity, durability, flavor, texture, calories));
		});
	}

	private string ProcessDataUsingBruteForce(bool doCalories)
	{
		var recipe = new List<int>();
		int bestScore = 0;
		List<int> bestRecipe = null;
		int totalEvaluations = 0;

		FindBestRecipe(recipe, doCalories);

		logger.SendInfo(nameof(Day15), $"Best: ({string.Join(", ", bestRecipe)}) => {bestScore}");
		logger.SendInfo(nameof(Day15), $"{totalEvaluations} total evaluations");

		return bestScore.ToString();

		void FindBestRecipe(List<int> recipe, bool doCalories)
		{
			var used = recipe.Sum();
			var availableAmount = 100 - used;

			if (recipe.Count == ingredients.Count - 1)
			{
				recipe.Add(availableAmount);

				int score = CalcRecipeScore(recipe, doCalories);
				totalEvaluations++;

				if (score > bestScore)
				{
					bestScore = score;
					bestRecipe = new List<int>(recipe);

					logger.SendDebug(nameof(Day15), $"({string.Join(", ", bestRecipe)}) => {bestScore}");
				}
				return;
			}

			for (int i = 0; i <= availableAmount; i++)
			{
				var newRecipe = new List<int>(recipe) { i };
				FindBestRecipe(newRecipe, doCalories);
			}
		}
	}

	private readonly Random random = new();
	private readonly Dictionary<string, int> evaluations = new();

	private string ProcessDataUsingGeneticAlgorithm(bool doCalories)
	{
		evaluations.Clear();

		var (bestGenes, bestFitness, totalGenerations) = OptimizationHelper.RunGeneticOptimization(logger,
			populationSize: 20,
			eliteCount: 3,
			maxGenerations: 100,
			ageLimit: 10,
			mutationProbability: 0.5,

			createGenes: CreateRandomName,
			evaluateGenes: (name) => EvaluateName(name, doCalories),
			mateGenes: (parent1, parent2, doMutate) =>
			{
				var parent1Genes = parent1.Split(',').ToList().ConvertAll(v => int.Parse(v));
				var parent2Genes = parent2.Split(',').ToList().ConvertAll(v => int.Parse(v));

				var childGenes = new List<int>();

				for (int gene = 0; gene < parent1Genes.Count; gene++)
					childGenes.Add(random.Next(2) > 0 ? parent1Genes[gene] : parent2Genes[gene]);

				if (doMutate)
				{
					for (int gene = 0; gene < childGenes.Count; gene++)
						childGenes[gene] += random.Next(4) - 2;
				}
				var childName = string.Join(",", childGenes);
				return childName;
			});

		var bestRecipe = NameToRecipe(bestGenes);
		logger.SendInfo(nameof(Day15), $"Best: {bestGenes} ({string.Join(", ", bestRecipe)}) => {bestFitness}");
		logger.SendInfo(nameof(Day15), $"{totalGenerations} total generations");
		logger.SendInfo(nameof(Day15), $"{evaluations.Count} total evaluations");

		return bestFitness.ToString();
	}

	private string ProcessDataUsingBeehiveAlgorithm(bool doCalories)
	{
		evaluations.Clear();

		var (bestBee, bestQuality, totalCycles) = OptimizationHelper.RunBeehiveOptimization(logger,
			populationSize: 20,
			activeRatio: 0.75,
			inactiveRatio: 0.10,
			maxCycles: 100,
			maxVisits: ingredients.Count * 2,
			stasisThreshold: 50,

			createBee: CreateRandomName,
			evaluateBee: (name) => EvaluateName(name, doCalories),
			createNeighborBee: (name) =>
			{
				var vector = name.Split(',').ToList().ConvertAll(v => int.Parse(v));
				int k = random.Next(vector.Count);
				vector[k] += random.Next(2) * 2 - 1;
				var neighborName = string.Join(",", vector);
				return neighborName;
			});

		var bestRecipe = NameToRecipe(bestBee);
		logger.SendInfo(nameof(Day15), $"Best: {bestBee} ({string.Join(", ", bestRecipe)}) => {bestQuality}");
		logger.SendInfo(nameof(Day15), $"{totalCycles} total cycles");
		logger.SendInfo(nameof(Day15), $"{evaluations.Count} total evaluations");

		return bestQuality.ToString();
	}

	private string CreateRandomName()
	{
		var vector = new List<int>();
		for (int i = 0; i < ingredients.Count - 1; i++)
			vector.Add(random.Next(101));
		vector = vector.OrderBy(x => x).ToList();
		var name = string.Join(",", vector);
		return name;
	}

	private int EvaluateName(string name, bool doCalories)
	{
		if (evaluations.TryGetValue(name, out var value))
			return value;

		var recipe = NameToRecipe(name);
		value = CalcRecipeScore(recipe, doCalories);
		evaluations[name] = value;
		return value;
	}

	private List<int> NameToRecipe(string name)
	{
		var vector = name.Split(',').ToList().ConvertAll(v => int.Parse(v));

		var recipe = new List<int>() { vector[0] };
		for (int i = 1; i < vector.Count; i++)
		{
			recipe.Add(vector[i] - vector[i - 1]);
		}
		recipe.Add(100 - vector[vector.Count - 1]);

		return recipe;
	}

	private int CalcRecipeScore(List<int> recipe, bool doCalories)
	{
		int totalCapacity = 0;
		int totalDurability = 0;
		int totalFlavor = 0;
		int totalTexture = 0;
		int totalCalories = 0;

		for (int i = 0; i < recipe.Count; i++)
		{
			var (_, capacity, durability, flavor, texture, calories) = ingredients[i];
			var amount = recipe[i];

			totalCapacity += amount * capacity;
			totalDurability += amount * durability;
			totalFlavor += amount * flavor;
			totalTexture += amount * texture;
			totalCalories += amount * calories;
		}

		totalCapacity = Math.Max(totalCapacity, 0);
		totalDurability = Math.Max(totalDurability, 0);
		totalFlavor = Math.Max(totalFlavor, 0);
		totalTexture = Math.Max(totalTexture, 0);
		totalCalories = Math.Max(totalCalories, 0);

		var caloryFactor = (doCalories && totalCalories != 500) ? 0 : 1;

		return totalCapacity * totalDurability * totalFlavor * totalTexture * caloryFactor;
	}
}
