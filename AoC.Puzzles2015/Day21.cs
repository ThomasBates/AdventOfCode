using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day21 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 21;

	public string Name => $"Day 21";

	public Dictionary<string, string> Inputs { get; } = new()
	{
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

	private (int, int, int) LoadDataFromInput(string input)
	{
		//  First Clear Data
		int hp = 0;
		int damage = 0;
		int armor = 0;
		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			int value = int.Parse(parts[1].Trim());

			switch (parts[0])
			{
				case "Hit Points":
					hp = value;
					break;
				case "Damage":
					damage = value;
					break;
				case "Armor":
					armor = value;
					break;
			}
		});

		return (hp, damage, armor);
	}

	private List<(int cost, int damage)> weapons = new()
	{
		( 8, 4),
		(10, 5),
		(25, 6),
		(40, 7),
		(74, 8),
	};

	private List<(int cost, int protection)> armors = new()
	{
		(  0, 0),
		( 13, 1),
		( 31, 2),
		( 53, 3),
		( 75, 4),
		(102, 5),
	};

	private List<(int cost, int damage, int protection)> rings = new()
	{
		(  0, 0, 0),
		( 25, 1, 0),
		( 50, 2, 0),
		(100, 3, 0),
		( 20, 0, 1),
		( 40, 0, 2),
		( 80, 0, 3),
	};

	private int ProcessDataForPart1((int, int, int) boss)
	{
		int bestCost = int.MaxValue;

		foreach (var weapon in weapons)
		{
			foreach (var armor in armors)
			{
				foreach (var ring1 in rings)
				{
					foreach (var ring2 in rings)
					{
						if (ring1.cost == ring2.cost && ring1.cost != 0)
							continue;

						int cost = weapon.cost + armor.cost + ring1.cost + ring2.cost;
						int damage = weapon.damage + ring1.damage + ring2.damage;
						int protection = armor.protection + ring1.protection + ring2.protection;

						bool victory = SimulateBattle((100, damage, protection), boss);

						logger.SendDebug(nameof(Day21), $"({weapon.cost} + {armor.cost} + {ring1.cost} + {ring2.cost} = {cost}) => ({damage}, {protection}) => {(victory?"victory":"defeat")}");

						if (victory && cost < bestCost)
						{
							bestCost = cost;

							logger.SendDebug(nameof(Day21), $"({weapon.cost} + {armor.cost} + {ring1.cost} + {ring2.cost} = {cost}) => BEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
						}
					}
				}
			}
		}

		return bestCost;
	}

	private int ProcessDataForPart2((int, int, int) boss)
	{
		int bestCost = 0;

		foreach (var weapon in weapons)
		{
			foreach (var armor in armors)
			{
				foreach (var ring1 in rings)
				{
					foreach (var ring2 in rings)
					{
						if (ring1.cost == ring2.cost && ring1.cost != 0)
							continue;

						int cost = weapon.cost + armor.cost + ring1.cost + ring2.cost;
						int damage = weapon.damage + ring1.damage + ring2.damage;
						int protection = armor.protection + ring1.protection + ring2.protection;

						bool victory = SimulateBattle((100, damage, protection), boss);

						logger.SendDebug(nameof(Day21), $"({weapon.cost} + {armor.cost} + {ring1.cost} + {ring2.cost} = {cost}) => ({damage}, {protection}) => {(victory ? "victory" : "defeat")}");

						if (!victory && cost > bestCost)
						{
							bestCost = cost;

							logger.SendDebug(nameof(Day21), $"({weapon.cost} + {armor.cost} + {ring1.cost} + {ring2.cost} = {cost}) => BEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
						}
					}
				}
			}
		}

		return bestCost;
	}

	private bool SimulateBattle((int hp, int damage, int protection) player, (int hp, int damage, int protection) boss, bool verbose = false)
	{
		if (verbose)
			logger.SendVerbose("Battle", $"player = ({player.hp}, {player.damage}, {player.protection}), boss = ({boss.hp}, {boss.damage}, {boss.protection})");

		while (true)
		{
			boss.hp -= player.damage - boss.protection;
			if (verbose)
				logger.SendVerbose("Battle", $"  boss.hp = {boss.hp}");
			if (boss.hp <= 0)
				return true;

			player.hp -= boss.damage - player.protection;
			if (verbose)
				logger.SendVerbose("Battle", $"  player.hp = {player.hp}");
			if (player.hp <= 0)
				return false;
		}
	}
}
