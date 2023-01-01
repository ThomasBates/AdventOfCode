using AoC.Common.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AoC.Common.Helpers;

public class OptimizationHelper
{
	public static (TGenes, TFitness, int) RunGeneticOptimization<TGenes, TFitness>(
		ILogger logger,

		int populationSize,
		int eliteCount,
		int maxGenerations,
		int ageLimit,
		double mutationProbability,

		Func<TGenes> createGenes,
		Func<TGenes, TFitness> evaluateGenes,
		Func<TGenes, TGenes, bool, TGenes> mateGenes) 
		where TGenes : IEquatable<TGenes> 
		where TFitness : IComparable<TFitness>
	{
		int totalGenerations = 0;

		var random = new Random();

		var population = new List<(TGenes genes, int age, TFitness fitness)>();
		var seen = new List<TGenes>();

		//  Initialize population
		while (population.Count < populationSize)
		{
			var genes = createGenes();
			if (!seen.Contains(genes))
			{
				seen.Add(genes);
				population.Add((genes, 0, default));
			}
		}

		for (int generation = 0; generation < maxGenerations; generation++)
		{
			totalGenerations++;

			//  Evaluate population
			for (int i = 0; i < populationSize; i++)
			{
				var (genes, age, fitness) = population[i];
				if (age == 0)
				{
					fitness = evaluateGenes(genes);
					population[i] = (genes, age, fitness);
				}
			}

			//  Sort by fitness
			population = population.OrderByDescending(x => x.fitness).ToList();

			//  Output fittest
			logger.SendDebug("Genetic", $"After generation {generation + 1}");
			foreach (var (genes, age, fitness) in population)
				logger.SendVerbose("Genetic", $"  {genes} => {fitness} ({age})");
			logger.SendDebug("Genetic", $"  Best: {population[0].genes} => {population[0].fitness} ({population[0].age})");

			if (ageLimit > 0 && population[0].age > ageLimit)
				break;

			//  Mate
			if (generation < maxGenerations - 1)
			{
				var nextGen = new List<(TGenes genes, int age, TFitness fitness)>();

				for (int i = 0; i < eliteCount; i++)
				{
					var (genes, age, fitness) = population[i];
					age++;
					nextGen.Add((genes, age, fitness));
				}

				while (nextGen.Count < populationSize)
				{
					//  The r^4 here is to give mating preference to more fit individuals
					int i1 = (int)(Math.Pow(random.NextDouble(), 4) * populationSize);
					int i2 = (int)(Math.Pow(random.NextDouble(), 4) * populationSize);

					var parent1 = population[i1].genes;
					var parent2 = population[i2].genes;

					var doMutate = random.Next(101) < mutationProbability;
					TGenes child = mateGenes(parent1, parent2, doMutate);

					if (!seen.Contains(child))
					{
						seen.Add(child);
						nextGen.Add((child, 0, default));
					}
				}
				population = nextGen;
			}
		}

		var (bestGenes, _, bestFitness) = population[0];
		return (bestGenes, bestFitness, totalGenerations);
	}

	//  https://learn.microsoft.com/en-us/archive/msdn-magazine/2011/april/msdn-magazine-natural-algorithms-use-bee-colony-algorithms-to-solve-impossible-problems
	public static (TBee, TQuality, int) RunBeehiveOptimization<TBee, TQuality>(
		ILogger logger,

		int populationSize,
		double activeRatio,
		double inactiveRatio,
		int maxCycles,
		int maxVisits,
		int stasisThreshold,

		Func<TBee> createBee,
		Func<TBee, TQuality> evaluateBee,
		Func<TBee, TBee> createNeighborBee) 
		where TBee : IEquatable<TBee>
		where TQuality : IComparable<TQuality>
	{
		const double persuasionProbability = 0.90;
		const double mistakeProbability = 0.01;
		
		int activeCount = (int)(populationSize * activeRatio);
		int inactiveCount = (int)(populationSize * inactiveRatio);
		int scoutCount = populationSize - activeCount - inactiveCount;

		int totalCycles = 0;

		var random = new Random();

		var actives = new List<(TBee bee, int visits, TQuality quality)>();
		var inactives = new List<(TBee bee, int visits, TQuality quality)>();
		var scouts = new List<(TBee bee, int visits, TQuality quality)>();

		TBee bestBee = createBee();
		TQuality bestQuality = evaluateBee(bestBee);

		while (actives.Count < activeCount)
		{
			var bee = createBee();
			var quality = evaluateBee(bee);
			actives.Add((bee, 0, quality));
			if (bestQuality.CompareTo(quality) < 0)
			{
				bestQuality = quality;
				bestBee = bee;
			}
		}

		while (inactives.Count < inactiveCount)
		{
			var bee = createBee();
			var quality = evaluateBee(bee);
			inactives.Add((bee, 0, quality));
			if (bestQuality.CompareTo(quality) < 0)
			{
				bestQuality = quality;
				bestBee = bee;
			}
		}

		while (scouts.Count < scoutCount)
		{
			var bee = createBee();
			var quality = evaluateBee(bee);
			scouts.Add((bee, 0, quality));
			if (bestQuality.CompareTo(quality) < 0)
			{
				bestQuality = quality;
				bestBee = bee;
			}
		}

		int cyclesWithoutUpdating = 0;

		for (int cycle = 0; cycle < maxCycles; cycle++)
		{
			totalCycles++;

			var updatedBest = false;

			//  Process active bees
			for (int i = 0; i < actives.Count; i++)
			{
				var (bee, visits, quality) = actives[i];
				var neighborBee = createNeighborBee(bee);
				var neighborQuality = evaluateBee(neighborBee);
				var doSwitch = neighborQuality.CompareTo(quality) > 0;
				if (random.NextDouble() < mistakeProbability)
					doSwitch = !doSwitch;
				var maxVisitsExceeded = false;
				if (doSwitch)
				{
					bee = neighborBee;
					quality = neighborQuality;
					actives[i] = (bee, 0, quality);
				}
				else
				{
					visits++;
					actives[i] = (bee, visits, quality);
					if (visits > maxVisits)
						maxVisitsExceeded = true;
				}
				if (maxVisitsExceeded)
				{
					var inactive = random.Next(inactives.Count);
					var (inactiveBee, _, inactiveQuality) = inactives[inactive];
					actives[i] = (inactiveBee, 0, inactiveQuality);
					inactives[inactive] = (bee, 0, quality);
				}
				else if (doSwitch)
				{
					if (bestQuality.CompareTo(quality) < 0)
					{
						bestQuality = quality;
						bestBee = bee;
						updatedBest = true;
					}
					//  Waggle
					for (int j = 0; j < inactives.Count; j++)
					{
						var (_, _, inactiveQuality) = inactives[j];
						if (quality.CompareTo(inactiveQuality) > 0 &&
							random.NextDouble() < persuasionProbability)
						{
							inactives[j] = (bee, 0, quality);
						}
					}
				}
			}

			//  Process scout bees
			for (int i=0; i<scouts.Count; i++)
			{
				var (_, _, quality) = scouts[i];

				var randomBee = createBee();
				var randomQuality = evaluateBee(randomBee);

				if (randomQuality.CompareTo(quality) > 0)
				{
					var bee = randomBee;
					quality = randomQuality;
					scouts[i] = (bee, 0, quality);

					if (bestQuality.CompareTo(quality) < 0)
					{
						bestQuality = quality;
						bestBee = bee;
						updatedBest = true;
					}
					//  Waggle
					for (int j = 0; j < inactives.Count; j++)
					{
						var (_, _, inactiveQuality) = inactives[j];
						if (quality.CompareTo(inactiveQuality) > 0 &&
							random.NextDouble() < persuasionProbability)
						{
							inactives[j] = (bee, 0, quality);
						}
					}
				}
			}

			if (updatedBest)
			{
				cyclesWithoutUpdating = 0;
			}
			else
			{
				cyclesWithoutUpdating++;
				if (cyclesWithoutUpdating > stasisThreshold)
					break;
			}

			logger.SendDebug("Beehive", $"After {cycle + 1} cycles:");
			foreach (var (bee, visited, quality) in actives)
				logger.SendVerbose("Genetic", $"  active: {bee} => {quality} ({visited})");
			foreach (var (bee, visited, quality) in scouts)
				logger.SendVerbose("Genetic", $"  scout: {bee} => {quality}");
			foreach (var (bee, visited, quality) in inactives)
				logger.SendVerbose("Genetic", $"  inactive: {bee} => {quality}");
			logger.SendDebug("Genetic", $"  Best: {bestBee} => {bestQuality} ({cyclesWithoutUpdating})");
		}

		return (bestBee, bestQuality, totalCycles);
	}

	public static (TAnt, TScore, int, int) RunAntColonyOptimization<TAnt, TScore>()
		where TAnt: IEquatable<TAnt>
		where TScore:IComparable<TScore>
	{
		return (default, default, 0, 0);
	}

	public static (TSolution, TScore, int, int) RunSimulatedAnnealingOptimization<TSolution, TScore>()
		where TSolution : IEquatable<TSolution>
		where TScore : IComparable<TScore>
	{
		return (default, default, 0, 0);
	}
}
