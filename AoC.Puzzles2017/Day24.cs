using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Security.Cryptography;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day24 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

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

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day24), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day24), message);

	#endregion Helpers

	private class Component
	{
		public int Port1;
		public int Port2;
		public override string ToString() => $"{Port1}/{Port2}";
	}

	private class Data
	{
		public List<Component> Components = new();
	}

	private Data LoadData(string input)
	{
		var data = new Data();

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split('/');
			data.Components.Add(new Component
			{
				Port1 = int.Parse(parts[0]),
				Port2 = int.Parse(parts[1])
			});
		});
		
		return data;
	}

	private object SolvePart1(Data data)
	{
		var bridge = FindBestBridge(data.Components, new List<Component>(), 0, doLongest: false);
		var strength = GetBridgeStrength(bridge);
		return strength;
	}

	private object SolvePart2(Data data)
	{
		var bridge = FindBestBridge(data.Components, new List<Component>(), 0, doLongest: true);
		var strength = GetBridgeStrength(bridge);
		return strength;
	}

	private List<Component> FindBestBridge(List<Component> components, List<Component> bridge, int lastPort, bool doLongest)
	{
		List<Component> bestBridge = bridge;
		var bestLength = bridge.Count;
		var bestStrength = GetBridgeStrength(bridge);

		foreach (var component in components)
		{
			if (bridge.Contains(component))
				continue;
			if (component.Port1 != lastPort && component.Port2 != lastPort)
				continue;
			var nextPort = component.Port1 == lastPort ? component.Port2 : component.Port1;
			var nextBridge = new List<Component>(bridge) { component };
			var testBridge = FindBestBridge(components, nextBridge, nextPort, doLongest);
			if (testBridge == null)
				continue;
			var testLength = testBridge.Count;
			var testStrength = GetBridgeStrength(testBridge);

			if ((doLongest &&
				 ((testLength > bestLength) ||
				  (testLength == bestLength && testStrength > bestStrength))) ||
				(testStrength > bestStrength))
			{
				bestLength = testLength;
				bestStrength = testStrength;
				bestBridge = testBridge;
			}
		}

		return bestBridge;
	}

	private int GetBridgeStrength(List<Component> bridge)
	{
		if (bridge == null) 
			return 0;
		var strength = 0;
		foreach (var component in bridge)
			strength += component.Port1 + component.Port2;
		return strength;
	}
}
