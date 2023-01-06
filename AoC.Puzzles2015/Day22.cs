using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day22 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 22;

	public string Name => $"Day 22";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs (13 hp)", Resources.Day22Inputs13},
		{"Example Inputs (14 hp)", Resources.Day22Inputs14},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day22(ILogger logger)
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

		var result = ProcessDataForPart1BreadthFirst(data, false);

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart1BreadthFirst(data, true);

		return result.ToString();
	}

	#endregion Solvers

	private (int, int, int, int) LoadDataFromInput(string input)
	{
		//  First Clear Data
		int bossHP = 0;
		int bossDamage = 0;
		int heroHP = 50;
		int heroMana = 500;

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			int value = int.Parse(parts[1].Trim());

			switch (parts[0])
			{
				case "Hit Points":
					bossHP = value;
					break;
				case "Damage":
					bossDamage = value;
					break;
				case "Hero HP":
					heroHP = value;
					break;
				case "Mana":
					heroMana = value;
					break;
			}
		});

		return (bossHP, bossDamage, heroHP, heroMana);
	}

	private class State
	{
		public int heroHP; 
		public int bossHP;
		public int mana;
		public int shieldTimer;
		public int poisonTimer;
		public int rechargeTimer;
		public string spells = "";
		public int cost;
	}

	private class StateComparer : IComparer<State>
	{
		public static readonly StateComparer Instance = new();
		public int Compare(State x, State y)
		{
			return x.cost.CompareTo(y.cost);
		}
	}

	private int ProcessDataForPart1BreadthFirst((int bossHP, int bossDamage, int heroHP, int mana) data, bool hard)
	{
		int bestCost = int.MaxValue;
		var bestSpells = "";

		var states = new List<State>()
		{
			new State 
			{ 
				heroHP = data.heroHP, 
				bossHP = data.bossHP, 
				mana = data.mana
			}
		};

		while (states.Count > 0)
		{
			var state = states[0];
			states.RemoveAt(0);

			if (state.cost > bestCost)
				continue;

			var heroShield = 0;

			//  The boss's turn comes after the hero's turn. Skip boss's turn the first time.
			if (state.spells.Length > 0)
			{
				//  Boss's turn
				//  Lingering effects
				if (state.shieldTimer > 0)
				{
					heroShield = 7;
					state.shieldTimer--;
				}
				if (state.poisonTimer > 0)
				{
					state.bossHP -= 3;
					state.poisonTimer--;
				}
				if (state.rechargeTimer > 0)
				{
					state.mana += 101;
					state.rechargeTimer--;
				}

				//  if boss dies, then hero wins, compare cost.
				if (state.bossHP <= 0)
				{
					logger.SendDebug(nameof(Day22), $"Victory: {state.cost} - {state.spells}");
					if (state.cost < bestCost)
					{
						bestCost = state.cost;
						bestSpells = state.spells;
						logger.SendDebug(nameof(Day22), $"Best so far: {bestCost}");
					}
					continue;
				}

				//  Boss attacks
				state.heroHP -= (data.bossDamage - heroShield);

				//  if hero dies, cull tree.
				if (state.heroHP <= 0)
					continue;
			}

			//  Hero's turn
			//  Lingering effects
			if (hard)
				state.heroHP--;
			if (state.shieldTimer > 0)
			{
				heroShield = 7;
				state.shieldTimer--;
			}
			if (state.poisonTimer > 0)
			{
				state.bossHP -= 3;
				state.poisonTimer--;

				//  if boss dies, then hero wins, compare cost.
				if (state.bossHP <= 0)
				{
					logger.SendDebug(nameof(Day22), $"Victory: {state.cost} - {state.spells}");
					if (state.cost < bestCost)
					{
						bestCost = state.cost;
						bestSpells = state.spells;
						logger.SendDebug(nameof(Day22), $"Best so far: {bestCost}");
					}
					continue;
				}
			}
			if (state.rechargeTimer > 0)
			{
				state.mana += 101;
				state.rechargeTimer--;
			}

			//  Hero attacks
			if (state.mana >= 53)
			{
				AddStateSorted(state, "M", 53);
			}
			if (state.mana >= 73)
			{
				AddStateSorted(state, "D", 73);
			}
			if (state.mana >= 113 && state.shieldTimer == 0)
			{
				AddStateSorted(state, "S", 113);
			}
			if (state.mana >= 173 && state.poisonTimer == 0)
			{
				AddStateSorted(state, "P", 173);
			}
			if (state.mana >= 229 && state.rechargeTimer == 0)
			{
				AddStateSorted(state, "R", 229);
			}
		}

		return bestCost;

		void AddStateSorted(State state, string spell, int cost)
		{
			var newState = new State
			{
				heroHP = state.heroHP,
				bossHP = state.bossHP,
				mana = state.mana - cost,
				shieldTimer = state.shieldTimer,
				poisonTimer = state.poisonTimer,
				rechargeTimer = state.rechargeTimer,
				spells = state.spells + spell,
				cost = state.cost + cost,
			};

			switch (spell)
			{
				case "M":
					newState.bossHP -= 4;
					break;
				case "D":
					newState.bossHP -= 2;
					newState.heroHP += 2;
					break;
				case "S":
					newState.shieldTimer = 6;
					break;
				case "P":
					newState.poisonTimer = 6;
					break;
				case "R":
					newState.rechargeTimer = 5;
					break;
			}

			var index = states.BinarySearch(newState, StateComparer.Instance);
			if (index < 0) 
				index = ~index;
			states.Insert(index, newState);
		}
	}
}
