using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day24 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 24;

	public string Name => "Day 24";

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

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day24), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day24), message);

	#endregion Helpers

	private class Group
	{
		public string ArmyName;
		public int ID;
		public int Units;
		public int HitPoints;
		public List<string> Weaknesses = new();
		public List<string> Immunities = new();
		public string AttackType;
		public int Damage;
		public int Initiative;
		public int Power => Units * Damage;
		public override string ToString() => $"{ArmyName} group {ID}";
		public Group Clone() => new()
		{
			ArmyName = ArmyName,
			ID = ID,
			Units = Units,
			HitPoints = HitPoints,
			Weaknesses = Weaknesses.ToList(),
			Immunities = Immunities.ToList(),
			AttackType = AttackType,
			Damage = Damage,
			Initiative = Initiative
		};
		
	}
	private class Army
	{
		public string Name;
		public List<Group> Groups = new();
		public int Units => Groups.Sum(g => g.Units);
		public override string ToString() => Name;
		public Army Clone() => new()
		{
			Name = Name,
			Groups = Groups.Select(g => g.Clone()).ToList()
		};
		
	}

	private class Data
	{
		public Army ImmuneSystem = new() { Name = "Immune System" };
		public Army Infection = new() { Name = "Infection" };
	}

	private Data LoadData(string input)
	{
		var data = new Data();

		Army army = null;
		Group group = null;
		List<string> special = null;

		GrammarHelper.ParseInput(logger, input, Resources.Day24Grammar,
			scopeControllerAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "s_immuneSystem":
						army = data.ImmuneSystem;
						break;
					case "s_infection":
						army = data.Infection;
						break;
					case "s_newGroup":
						group = new Group();
						army.Groups.Add(group);
						group.ArmyName = army.Name;
						group.ID = army.Groups.Count;
						break;
					case "s_weakness":
						special = group.Weaknesses;
						break;
					case "s_immunity":
						special = group.Immunities;
						break;
				}
			},
			typeCheckerAction: null,
			codeGeneratorAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "c_units":
						group.Units = int.Parse(valueStack.Pop());
						break;
					case "c_hitPoints":
						group.HitPoints = int.Parse(valueStack.Pop());
						break;
					case "c_specialType":
						special.Add(valueStack.Pop());
						break;
					case "c_damage":
						group.Damage = int.Parse(valueStack.Pop());
						break;
					case "c_attackType":
						group.AttackType = valueStack.Pop();
						break;
					case "c_initiative":
						group.Initiative = int.Parse(valueStack.Pop());
						break;
				}
			});

		return data;
	}

	private int SolvePart1(Data data)
	{
		var winner = Fight(data.ImmuneSystem, data.Infection);
		return winner.Units;
	}

	private int SolvePart2(Data data)
	{
		int min = 0;
		int max = 0x00010000;

		DoFight(min);
		DoFight(max);
		while (max - min > 1)
		{
			var mid = (min + max) >> 1;
			var (midOK, _) = DoFight(mid);
			if (midOK)
				max = mid;
			else
				min = mid;
		}

		var (_, units) = DoFight(max);

		return units;

		(bool, int) DoFight(int boost)
		{
			var immuneSystem = data.ImmuneSystem.Clone();
			var infection = data.Infection.Clone();

			foreach (var group in immuneSystem.Groups)
				group.Damage += boost;

			var winner = Fight(immuneSystem, infection);

			if (winner == null)
				SendDebug($"boost = {boost}: draw");
			else
				SendDebug($"boost = {boost}: winner = {winner} with {winner.Units} units");

			return (winner == immuneSystem, winner?.Units ?? 0);
		}
	}

	private Army Fight(Army army1, Army army2)
	{
		var attacks = new List<(Group attacker, Group defender)>();
		var totalKills = 0;
		while (true)
		{
			SendVerbose();
			ShowState(army1);
			ShowState(army2);

			attacks.Clear();

			SendVerbose();
			SelectTargets(army2, army1);
			SelectTargets(army1, army2);

			SendVerbose();
			Attack();

			if (totalKills == 0)
				return null;
			if (army1.Groups.Count == 0)
				return army2;
			if (army2.Groups.Count == 0)
				return army1;
		}

		void ShowState(Army army)
		{
			SendVerbose($"{army.Name}:");
			foreach (var group in army.Groups)
				SendVerbose($"Group {group.ID} contains {group.Units} units");
		}

		void SelectTargets(Army attacking, Army defending)
		{
			var attackable = defending.Groups.OrderByDescending(g => g.Power).ThenByDescending(g => g.Initiative).ToList();
			var ordered = attacking.Groups.OrderByDescending(g => g.Power).ThenByDescending(g => g.Initiative);
			foreach (var attacker in ordered)
			{
				int mostDamage = 0;
				Group bestTarget = null;
				foreach (var defender in attackable)
				{
					var damage = GetDamage(attacker, defender);
					if (damage > mostDamage)
					{
						mostDamage = damage;
						bestTarget = defender;
					}
				}
				if (bestTarget != null)
				{
					SendVerbose($"{attacking.Name} group {attacker.ID} would deal defending group {bestTarget.ID} {mostDamage} damage");
					attacks.Add((attacker, bestTarget));
					attackable.Remove(bestTarget);
				}
			}
		}

		void Attack()
		{
			attacks = attacks.OrderByDescending(a => a.attacker.Initiative).ToList();
			totalKills = 0;
			foreach (var (attacker, defender) in attacks)
			{
				var damage = GetDamage(attacker, defender);
				var units = Math.Min(defender.Units, damage / defender.HitPoints);
				defender.Units -= units;
				totalKills += units;
				SendVerbose($"{attacker.ArmyName} group {attacker.ID} attacks defending group {defender.ID}, killing {units} units");
			}

			ClearDeadGroups(army1);
			ClearDeadGroups(army2);
		}

		int GetDamage(Group attacker, Group defender)
		{
			if (defender.Immunities.Contains(attacker.AttackType))
				return 0;
			if (defender.Weaknesses.Contains(attacker.AttackType))
				return attacker.Power * 2;
			return attacker.Power;
		}

		void ClearDeadGroups(Army army)
			=> army.Groups = army.Groups.Where(g => g.Units > 0).ToList();
	}
}
