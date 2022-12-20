using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using AoC.IO;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day19 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 19;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day19Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day19()
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

		ProcessDataForPart1b(output);

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

	private class Blueprint
	{
		public int ID;
		public int OreRobot_OreCost;
		public int ClayRobot_OreCost;
		public int ObsidianRobot_OreCost;
		public int ObsidianRobot_ClayCost;
		public int GeodeRobot_OreCost;
		public int GeodeRobot_ObsidianCost;
	}

	private List<Blueprint> blueprints = new();

	private void LoadDataFromInput(string input, StringBuilder output = null)
	{
		blueprints.Clear();
		int nextID = 1;
		Helper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(' ');
			blueprints.Add(new Blueprint
			{
				ID = nextID++,
				OreRobot_OreCost = int.Parse(parts[6]),
				ClayRobot_OreCost = int.Parse(parts[12]),
				ObsidianRobot_OreCost = int.Parse(parts[18]),
				ObsidianRobot_ClayCost = int.Parse(parts[21]),
				GeodeRobot_OreCost = int.Parse(parts[27]),
				GeodeRobot_ObsidianCost = int.Parse(parts[30])
			});
		});
	}

	private struct Inventory
	{
		public int Ore;
		public int Clay;
		public int Obsidian;
		public int Geodes;
		public int OreRobots;
		public int ClayRobots;
		public int ObsidianRobots;
		public int GeodeRobots;
	}

	private class State
	{
		public int Ore;
		public int Clay;
		public int Obsidian;
		public int Geodes;
		public int OreRobots;
		public int ClayRobots;
		public int ObsidianRobots;
		public int GeodeRobots;
		public int Time;

		public State(int ore, int clay, int obsidian, int geodes, int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, int time)
		{
			Ore = ore;
			Clay = clay;
			Obsidian = obsidian;
			Geodes = geodes;
			OreRobots = oreRobots;
			ClayRobots = clayRobots;
			ObsidianRobots = obsidianRobots;
			GeodeRobots = geodeRobots;
			Time = time;
		}

		public (int, int, int, int, int, int, int, int, int) Decompose()
		{
			return (Ore, Clay, Obsidian, Geodes, OreRobots, ClayRobots, ObsidianRobots, GeodeRobots, Time);
		}

		public override int GetHashCode()
		{
			return Ore * 100000000 +
				Clay * 10000000 +
				Obsidian * 1000000 +
				Geodes * 100000 +
				OreRobots * 10000 +
				ClayRobots * 1000 +
				ObsidianRobots * 100 +
				GeodeRobots * 10 +
				Time;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is State other))
				return false;
			return this.Ore == other.Ore &&
				this.Clay == other.Clay &&
				this.Obsidian == other.Obsidian &&
				this.Geodes == other.Geodes &&
				this.OreRobots == other.OreRobots &&
				this.ClayRobots == other.ClayRobots &&
				this.ObsidianRobots == other.ObsidianRobots &&
				this.GeodeRobots == other.GeodeRobots &&
				this.Time == other.Time;
		}
	}

	private enum ChoiceType
	{
		None,
		BuildNothing,
		BuildOreRobot,
		BuildClayRobot,
		BuildObsidianRobot,
		BuildGeodeRobot,
	}

	//  Take 1. forward depth-first-search recursion
	private void ProcessDataForPart1a(StringBuilder output = null)
	{
		int bestResult = 0;
		Blueprint bestBluePrint = null;
		IEnumerable<ChoiceType> bestChoices = null;

		output.AppendLine(DateTime.Now.ToString());

		int total = 0;
		foreach (var b in blueprints)
		{
			output.AppendLine($"Blueprint {b.ID}: ore ${b.OreRobot_OreCost}, clay ${b.ClayRobot_OreCost}, obs ${b.ObsidianRobot_OreCost} + ${b.ObsidianRobot_ClayCost}, geode ${b.GeodeRobot_OreCost} + ${b.GeodeRobot_ObsidianCost}");

			var (blueprintResult, blueprintChoices) = SolveBlueprint1a(b, 24);

			output.AppendLine($"{blueprintResult} geodes");
			int timer = 1;
			foreach (var choice in blueprintChoices)
				output.AppendLine($"{timer++}: {choice}");
			output.AppendLine();

			if (blueprintResult > bestResult)
			{
				bestResult = blueprintResult;
				bestBluePrint = b;
				bestChoices = blueprintChoices;
			}
			// sum ID*geodes for all. Answer is not 2234.
			total += b.ID * blueprintResult;
		}

		output.AppendLine();
		output.AppendLine($"Total = {total}");
		output.AppendLine();
		output.AppendLine("Best:");
		output.AppendLine($"Blueprint {bestBluePrint.ID} produced {bestResult} geodes");
		output.AppendLine(DateTime.Now.ToString());
	}

	//  Take 2. https://github.com/jonathanpaulson/AdventOfCode/blob/master/2022/19.py
	private void ProcessDataForPart1b(StringBuilder output = null)
	{
		//int bestResult = 0;
		//Blueprint bestBluePrint = null;
		//IEnumerable<ChoiceType> bestChoices = null;

		output.AppendLine(DateTime.Now.ToString());

		int total = 0;
		foreach (var b in blueprints)
		{
			output.AppendLine($"Blueprint {b.ID}: ore ${b.OreRobot_OreCost}, clay ${b.ClayRobot_OreCost}, obs ${b.ObsidianRobot_OreCost} + ${b.ObsidianRobot_ClayCost}, geode ${b.GeodeRobot_OreCost} + ${b.GeodeRobot_ObsidianCost}");

			var blueprintResult = SolveBlueprint1b(b, 24, output);

			output.AppendLine($"{blueprintResult} geodes");
			//int timer = 1;
			//foreach (var choice in blueprintChoices)
			//	output.AppendLine($"{timer++}: {choice}");
			//output.AppendLine();

			//if (blueprintResult > bestResult)
			//{
			//	bestResult = blueprintResult;
			//	bestBluePrint = b;
			//	bestChoices = blueprintChoices;
			//}
			// sum ID*geodes for all. Answer is not 2234.
			total += b.ID * blueprintResult;
		}

		output.AppendLine();
		output.AppendLine($"Total = {total}");
		//output.AppendLine();
		//output.AppendLine("Best:");
		//output.AppendLine($"Blueprint {bestBluePrint.ID} produced {bestResult} geodes");
		output.AppendLine(DateTime.Now.ToString());
	}

	private void ProcessDataForPart2(StringBuilder output = null)
	{
		//int bestResult = 0;
		//Blueprint bestBluePrint = null;
		//IEnumerable<ChoiceType> bestChoices = null;

		output.AppendLine(DateTime.Now.ToString());

		int total = 1;
		for (int i = 0; i < 3; i++)
		{
			if (i > blueprints.Count - 1)
				break;
			var b = blueprints[i];
			output.AppendLine($"Blueprint {b.ID}: ore ${b.OreRobot_OreCost}, clay ${b.ClayRobot_OreCost}, obs ${b.ObsidianRobot_OreCost} + ${b.ObsidianRobot_ClayCost}, geode ${b.GeodeRobot_OreCost} + ${b.GeodeRobot_ObsidianCost}");

			var blueprintResult = SolveBlueprint1b(b, 32, output);

			output.AppendLine($"{blueprintResult} geodes");
			//int timer = 1;
			//foreach (var choice in blueprintChoices)
			//	output.AppendLine($"{timer++}: {choice}");
			//output.AppendLine();

			//if (blueprintResult > bestResult)
			//{
			//	bestResult = blueprintResult;
			//	bestBluePrint = b;
			//	bestChoices = blueprintChoices;
			//}
			// sum ID*geodes for all. Answer is not 2234.
			total *= blueprintResult;
		}

		output.AppendLine();
		output.AppendLine($"Total = {total}");
		//output.AppendLine();
		//output.AppendLine("Best:");
		//output.AppendLine($"Blueprint {bestBluePrint.ID} produced {bestResult} geodes");
		output.AppendLine(DateTime.Now.ToString());
	}

	private (int, IEnumerable<ChoiceType>) SolveBlueprint1a(Blueprint blueprint, int timer, StringBuilder output = null)
	{
		return TryChoice(blueprint, new Inventory { OreRobots = 1 }, ChoiceType.BuildNothing, 1);
	}

	private int SolveBlueprint1b(Blueprint blueprint, int timer, StringBuilder output = null)
	{
		int best = 0;
		var state = new State(0, 0, 0, 0, 1, 0, 0, 0, timer);
		var deque = new List<State>() { state };
		var seen = new HashSet<State>();
		while (deque.Count > 0)
		{
			state = deque[0];
			deque.RemoveAt(0);

			var (ore, clay, obsidian, geodes, oreRobots, clayRobots, obsidianRobots, geodeRobots, time) = state.Decompose();

			best = Math.Max(best, state.Geodes);
			if (time == 0)
				continue;

			int maxOreCost = Math.Max(Math.Max(Math.Max(blueprint.OreRobot_OreCost, blueprint.ClayRobot_OreCost), blueprint.ObsidianRobot_OreCost), blueprint.GeodeRobot_OreCost);
			if (oreRobots >= maxOreCost)
				oreRobots = maxOreCost;
			if (clayRobots >= blueprint.ObsidianRobot_ClayCost)
				clayRobots = blueprint.ObsidianRobot_ClayCost;
			if (obsidianRobots >= blueprint.GeodeRobot_ObsidianCost)
				obsidianRobots = blueprint.GeodeRobot_ObsidianCost;
			if (ore >= maxOreCost * time - oreRobots * (time - 1))
				ore = maxOreCost * time - oreRobots * (time - 1);
			if (clay >= blueprint.ObsidianRobot_ClayCost * time - clayRobots * (time - 1))
				clay = blueprint.ObsidianRobot_ClayCost * time - clayRobots * (time - 1);
			if (obsidian >= blueprint.GeodeRobot_ObsidianCost * time - obsidianRobots * (time - 1))
				obsidian = blueprint.GeodeRobot_ObsidianCost * time - obsidianRobots * (time - 1);

			state = new State(ore, clay, obsidian, geodes, oreRobots, clayRobots, obsidianRobots, geodeRobots, time);

			if (seen.Contains(state))
				continue;
			seen.Add(state);

			if (seen.Count % 1000000 == 0)
				output.AppendLine($"{time}, {best}, {seen.Count}");

			deque.Add(new State(ore + oreRobots, clay + clayRobots, obsidian + obsidianRobots, geodes + geodeRobots, oreRobots, clayRobots, obsidianRobots, geodeRobots, time - 1));

			if (time == 1)
				continue;

			if (ore >= blueprint.GeodeRobot_OreCost &&
				obsidian >= blueprint.GeodeRobot_ObsidianCost)
			{
				deque.Add(new State(ore - blueprint.GeodeRobot_OreCost + oreRobots, clay + clayRobots, obsidian - blueprint.GeodeRobot_ObsidianCost + obsidianRobots, geodes + geodeRobots, oreRobots, clayRobots, obsidianRobots, geodeRobots + 1, time - 1));
				continue;
			}
			if (ore >= blueprint.OreRobot_OreCost)
			{
				deque.Add(new State(ore - blueprint.OreRobot_OreCost + oreRobots, clay + clayRobots, obsidian + obsidianRobots, geodes + geodeRobots, oreRobots + 1, clayRobots, obsidianRobots, geodeRobots, time - 1));
			}
			if (ore >= blueprint.ClayRobot_OreCost)
			{
				deque.Add(new State(ore - blueprint.ClayRobot_OreCost + oreRobots, clay + clayRobots, obsidian + obsidianRobots, geodes + geodeRobots, oreRobots, clayRobots + 1, obsidianRobots, geodeRobots, time - 1));
			}
			if (ore >= blueprint.ObsidianRobot_OreCost &&
				clay >= blueprint.ObsidianRobot_ClayCost)
			{
				deque.Add(new State(ore - blueprint.ObsidianRobot_OreCost + oreRobots, clay - blueprint.ObsidianRobot_ClayCost + clayRobots, obsidian + obsidianRobots, geodes + geodeRobots, oreRobots, clayRobots, obsidianRobots + 1, geodeRobots, time - 1));
			}
		}
		return best;
	}

	private (int, IEnumerable<ChoiceType>) TryChoice(Blueprint blueprint, Inventory inventory, ChoiceType choice, int timer)
	{
		var resultChoices = new List<ChoiceType> { choice };


		// step 1: if building a robot, remove resources from inventory.
		switch (choice)
		{
			case ChoiceType.BuildOreRobot:
				inventory.Ore -= blueprint.OreRobot_OreCost;
				break;
			case ChoiceType.BuildClayRobot:
				inventory.Ore -= blueprint.ClayRobot_OreCost;
				break;
			case ChoiceType.BuildObsidianRobot:
				inventory.Ore -= blueprint.ObsidianRobot_OreCost;
				inventory.Clay -= blueprint.ObsidianRobot_ClayCost;
				break;
			case ChoiceType.BuildGeodeRobot:
				inventory.Ore -= blueprint.GeodeRobot_OreCost;
				inventory.Obsidian -= blueprint.GeodeRobot_ObsidianCost;
				break;
			default:
				break;
		}


		// step 2: existing robots add resources to inventory.
		inventory.Ore += inventory.OreRobots;
		inventory.Clay += inventory.ClayRobots;
		inventory.Obsidian += inventory.ObsidianRobots;
		inventory.Geodes += inventory.GeodeRobots;


		// step 3: if building a robot, add robot to inventory.
		switch (choice)
		{
			case ChoiceType.BuildOreRobot:
				inventory.OreRobots++;
				break;
			case ChoiceType.BuildClayRobot:
				inventory.ClayRobots++;
				break;
			case ChoiceType.BuildObsidianRobot:
				inventory.ObsidianRobots++;
				break;
			case ChoiceType.BuildGeodeRobot:
				inventory.GeodeRobots++;
				break;
			default:
				break;
		}

		timer++;
		if (timer > 24)
			return (inventory.Geodes, resultChoices);


		// step 4: identify possible choices.
		var possibleChoices = new List<ChoiceType> { ChoiceType.BuildNothing };

		if (timer < 24)
		{
			if (inventory.Ore >= blueprint.OreRobot_OreCost)
				possibleChoices.Add(ChoiceType.BuildOreRobot);

			if (inventory.Ore >= blueprint.ClayRobot_OreCost)
				possibleChoices.Add(ChoiceType.BuildClayRobot);

			if (inventory.Ore >= blueprint.ObsidianRobot_OreCost &&
				inventory.Clay >= blueprint.ObsidianRobot_ClayCost)
			{
				possibleChoices.Add(ChoiceType.BuildObsidianRobot);
			}

			if (inventory.Ore >= blueprint.GeodeRobot_OreCost &&
				inventory.Obsidian >= blueprint.GeodeRobot_ObsidianCost)
			{
				possibleChoices.Add(ChoiceType.BuildGeodeRobot);
			}
		}


		// step 5: apply rules
		// https://www.reddit.com/r/adventofcode/comments/zpihwi/comment/j0w8ujl/?utm_source=share&utm_medium=web2x&context=3
		if (possibleChoices.Contains(ChoiceType.BuildGeodeRobot))
		{
			possibleChoices = new List<ChoiceType> { ChoiceType.BuildGeodeRobot };
		}

		if (possibleChoices.Contains(ChoiceType.BuildOreRobot) && inventory.ClayRobots > 0)
		{
			possibleChoices.Remove(ChoiceType.BuildOreRobot);
		}


		// step 6: try each choice
		int bestResult = -1;
		ChoiceType bestChoice;
		IEnumerable<ChoiceType> bestChoices = null;

		foreach (var possibleChoice in possibleChoices)
		{
			var (choiceResult, choiceChoices) = TryChoice(blueprint, inventory, possibleChoice, timer);
			if (choiceResult > bestResult)
			{
				bestResult = choiceResult;
				bestChoice = possibleChoice;
				bestChoices = choiceChoices;
			}
		}

		resultChoices.AddRange(bestChoices);
		return (bestResult, resultChoices);
	}
}
