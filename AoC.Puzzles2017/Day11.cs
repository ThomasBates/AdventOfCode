using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day11 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 11;

	public string Name => $"Day 11";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day11Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day11(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day11), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day11), message);

	#endregion Helpers

	private List<string> LoadData(string input)
	{
		var lines = new List<string>();

		InputHelper.TraverseInputLines(input, line =>
		{
			lines.Add(line);
		});

		return lines;
	}

	private int SolvePart1(List<string> lines)
	{
		var distance = 0;
		foreach (var line in lines)
		{
			SendDebug(line);
			distance = 0;

			var steps = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			var nw = steps.Count(s => s == "nw");
			var n = steps.Count(s => s == "n");
			var ne = steps.Count(s => s == "ne");
			var se = steps.Count(s => s == "se");
			var s = steps.Count(s => s == "s");
			var sw = steps.Count(s => s == "sw");
			distance = GetDistance(nw, n, ne, se, s, sw);
			SendDebug($"{distance} steps");
		}
		return distance;
	}

	private int SolvePart2(List<string> lines)
	{
		var maxDistance = 0;
		foreach (var line in lines)
		{
			SendDebug(line);
			maxDistance = 0;
			var (nw, n, ne, se, s, sw) = (0, 0, 0, 0, 0, 0);

			var steps = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			foreach(var step in steps)
			{
				switch (step)
				{
					case "nw": nw++; break;
					case "n":  n++;  break;
					case "ne": ne++; break;
					case "se": se++; break;
					case "s":  s++;  break;
					case "sw": sw++; break;
				}
				var distance = GetDistance(nw, n, ne, se, s, sw);
				maxDistance = Math.Max(maxDistance, distance);
			}
			SendDebug($"{maxDistance} steps");
		}
		return maxDistance;
	}

	private int GetDistance(int nw, int n, int ne, int se, int s, int sw)
	{
		SendVerbose($"{nameof(GetDistance)}: start:  {nw} nw, {n} n, {ne} ne, {se} se, {s} s, {sw} sw");

		var min = Math.Min(nw, se);
		nw -= min;
		se -= min;
		min = Math.Min(n, s);
		n -= min;
		s -= min;
		min = Math.Min(ne, sw);
		ne -= min;
		sw -= min;
		SendVerbose($"{nameof(GetDistance)}: ready:  {nw} nw, {n} n, {ne} ne, {se} se, {s} s, {sw} sw");

		Adjust(ref ne, ref s,  ref se, ref nw);
		Adjust(ref se, ref sw, ref s,  ref n);
		Adjust(ref s,  ref nw, ref sw, ref ne);
		Adjust(ref sw, ref n,  ref nw, ref se);
		Adjust(ref nw, ref ne, ref n,  ref s);
		Adjust(ref n,  ref se, ref ne, ref sw);
		SendVerbose($"{nameof(GetDistance)}: adjust: {nw} nw, {n} n, {ne} ne, {se} se, {s} s, {sw} sw");

		return nw + n + ne + se + s + sw;

		static void Adjust(ref int t1, ref int t2, ref int a1, ref int a2)
		{
			var min = Math.Min(t1, t2);
			if (min > 0)
			{
				t1 -= min;
				t2 -= min;
				if (a1 > 0)
				{
					a1 += min;
				}
				else if (a2 >= min)
				{
					a2 -= min;
				}
				else
				{
					a1 = min - a2;
					a2 = 0;
				}
			}
		}
	}
}
