using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day16 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 16;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day16Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day16()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	private string SolvePart1(string input)
	{
		var output = new StringBuilder();

		LoadDataFromInput(input);

		ProcessDataForPart1(output);

		return output.ToString();
	}

	private string SolvePart2(string input)
	{
		var output = new StringBuilder();

		LoadDataFromInput(input);

		ProcessDataForPart2(output);

		return output.ToString();
	}

	private class Valve
	{
		public string Name;
		public int FlowRate;
		public List<Valve> Tunnels = new();
		public Dictionary<Valve, int> Distances = new();

		public override string ToString()
		{
			return $"{Name}({FlowRate})";
		}
	}

	private readonly List<Valve> allValves = new();

	private void LoadDataFromInput(string input, StringBuilder output = null)
	{
		allValves.Clear();

		Valve fromValve = null;

		GrammarHelper.ParseInput(null, input, Resources.Day16Grammar,
			null,
			null,
			(token, valueStack) =>
			{
				switch (token)
				{
					case "c_valve":
						{
							var valveName = valueStack.Pop();
							fromValve = allValves.FirstOrDefault(v => v.Name == valveName);
							if (fromValve == null)
							{
								fromValve = new Valve { Name = valveName };
								allValves.Add(fromValve);
							}
						}
						break;
					case "c_rate":
						fromValve.FlowRate = Convert.ToInt32(valueStack.Pop());
						break;
					case "c_tunnel":
						{
							var valveName = valueStack.Pop();
							Valve toValve = allValves.FirstOrDefault(v => v.Name == valveName);
							if (toValve == null)
							{
								toValve = new Valve { Name = valveName };
								allValves.Add(toValve);
							}
							fromValve.Tunnels.Add(toValve);
						}
						break;
					default:
						output?.AppendLine($"Unknown token: {token}");
						break;
				}
			});
	}

	private void ProcessDataForPart1(StringBuilder output)
	{
		var start = allValves.FirstOrDefault(v => v.Name == "AA");
		var timer = 30;
		var valves = allValves.Where(v => v.FlowRate > 0).ToList();

		var (valveOrder, value) = SolveRemaining(start, valves, timer);

		output.AppendLine(string.Join(", ", valveOrder));
		output.AppendLine($"Pressure released = {value}");
	}

	private (List<Valve>, int) SolveRemaining(Valve current, List<Valve> valves, int timer)
	{
		if (!valves.Any())
			return (new List<Valve>(), 0);

		Valve bestValve = null;
		int bestValue = 0;
		List<Valve> bestValveOrder = null;

		foreach (var valve in valves)
		{
			var pathLength = FindPathLength(current, valve);
			int remainingTime = timer - pathLength - 1;

			if (remainingTime < 0)
				continue;

			int value = remainingTime * valve.FlowRate;
			var remainingValves = valves.Where(v => v != valve).ToList();

			var (valveOrder, remainingValue) = SolveRemaining(valve, remainingValves, remainingTime);

			value += remainingValue;

			if (value > bestValue)
			{
				bestValve = valve;
				bestValue = value;
				bestValveOrder = valveOrder;
			}
		}

		if (bestValveOrder == null)
			return (new List<Valve>(), 0);

		bestValveOrder?.Insert(0, bestValve);

		return (bestValveOrder, bestValue);
	}

	private int FindPathLength(Valve source, Valve target)
	{
		if (source.Distances.TryGetValue(target, out var distance))
			return distance;

		var path = PathfindingHelper.FindPath(allValves,
			(valve) => valve.Tunnels,
			(source, target) => 1, 
			source, target);

		distance = path.Count();

		source.Distances[target] = distance;
		target.Distances[source] = distance;

		return distance;
	}

	private class ValveInfo
	{
		public Valve Valve;
		public int OpenedAt;
		public override string ToString()
		{
			return $"{Valve}@{OpenedAt}";
		}
	}

	private void ProcessDataForPart2(StringBuilder output)
	{
		var start1 = allValves.FirstOrDefault(v => v.Name == "AA");
		var start2 = start1;
		var timer1 = 26;
		var timer2 = 26;

		var valves = allValves.Where(v => v.FlowRate > 0).ToList();

		var (valveOrder1, valveOrder2, value) = SolveRemaining(start1, start2, valves, timer1, timer2, false, null);

		output.AppendLine(string.Join(", ", valveOrder1));
		output.AppendLine(string.Join(", ", valveOrder2));
		output.AppendLine($"Pressure released = {value}");
	}

	private string indent = "";

	private (List<ValveInfo>, List<ValveInfo>, int) SolveRemaining(Valve current1, Valve current2, List<Valve> valves, int timer1, int timer2, bool swapped, StringBuilder output)
	{
		output?.AppendLine($"{indent}SolveRemaining({current1}, {current2}, [{string.Join(",", valves)}], {timer1}, {timer2})");
		indent = $"  {indent}";

		if (!valves.Any())
		{
			indent = indent.Substring(2);
			output?.AppendLine($"{indent}SolveRemaining return [], [], 0");
			return (new List<ValveInfo>(), new List<ValveInfo>(), 0);
		}

		List<ValveInfo> valveOrder1 = null;
		List<ValveInfo> valveOrder2 = null;
		int remainingValue = 0;

		Valve bestValve = null;
		List<ValveInfo> bestValveOrder1 = null;
		List<ValveInfo> bestValveOrder2 = null;
		int bestValue = 0;
		int bestOpenTime = 0;

		foreach (var valve in valves)
		{
			var pathLength = FindPathLength(current1, valve);
			int remainingTime = timer1 - pathLength - 1;

			if (remainingTime < 0)
				continue;

			int value = remainingTime * valve.FlowRate;
			var remainingValves = valves.Where(v => v != valve).ToList();

			if (remainingTime >= timer2)
				(valveOrder1, valveOrder2, remainingValue) = SolveRemaining(valve, current2, remainingValves, remainingTime, timer2, false, output);
			else
				(valveOrder2, valveOrder1, remainingValue) = SolveRemaining(current2, valve, remainingValves, timer2, remainingTime, false, output);

			value += remainingValue;

			if (value > bestValue)
			{
				bestValve = valve;
				bestValue = value;
				bestValveOrder1 = valveOrder1;
				bestValveOrder2 = valveOrder2;
				bestOpenTime = remainingTime;
			}
		}

		if (bestValveOrder1 == null)
		{
			if (swapped)
			{
				indent = indent.Substring(2);
				output?.AppendLine($"{indent}SolveRemaining return [], [], 0");
				return (new List<ValveInfo>(), new List<ValveInfo>(), 0);
			}

			(valveOrder2, valveOrder1, var value) = SolveRemaining(current2, current1, valves, timer2, timer1, true, output);

			indent = indent.Substring(2);
			output?.AppendLine($"{indent}SolveRemaining return [{string.Join(",", valveOrder1)}], [{string.Join(",", valveOrder2)}], {value}");
			return (valveOrder1, valveOrder2, value);
		}

		bestValveOrder1?.Insert(0, new ValveInfo { Valve = bestValve, OpenedAt = bestOpenTime });

		indent = indent.Substring(2);
		output?.AppendLine($"{indent}SolveRemaining return [{string.Join(",", bestValveOrder1)}], [{string.Join(",", bestValveOrder2)}], {bestValue}");
		return (bestValveOrder1, bestValveOrder2, bestValue);
	}
}