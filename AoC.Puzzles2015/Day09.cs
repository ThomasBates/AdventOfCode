using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day09 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 09;

	public string Name => $"Day 09";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day09Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day09(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataForPart1();

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataForPart2();

		return result;
	}

	#endregion Solvers

	private readonly Dictionary<string, Dictionary<string, int>> locations = new();

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		locations.Clear();

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(' ');
			var from = parts[0];
			var to = parts[2];
			var distance = int.Parse(parts[4]);

			var neighbors = new Dictionary<string, int>();

			if (!locations.TryGetValue(from, out neighbors))
			{
				neighbors = new Dictionary<string, int>();
				locations[from] = neighbors;
			}
			if (!neighbors.ContainsKey(to))
			{
				neighbors[to] = distance;
			}

			if (!locations.TryGetValue(to, out neighbors))
			{
				neighbors = new Dictionary<string, int>();
				locations[to] = neighbors;
			}
			if (!neighbors.ContainsKey(from))
			{
				neighbors[from] = distance;
			}
		});
	}

	private int bestDistance;
	private List<string> bestPath;

	private string ProcessDataForPart1()
	{
		var path = new List<string>();

		bestDistance = int.MaxValue;
		bestPath = null;

		FindShortestPath(path);

		logger.SendDebug(nameof(Day09), $"{bestDistance}: {string.Join(" -> ", bestPath)}");

		return bestDistance.ToString();
	}

	private void FindShortestPath(List<string> path)
	{
		var availableLocations = locations.Keys.Where(l => !path.Contains(l)).ToList();

		if (availableLocations.Count == 0)
		{
			int distance = 0;
			for (int i=1; i<path.Count; i++)
			{
				var from = path[i - 1];
				var to = path[i];
				distance += locations[from][to];
			}

			if (distance < bestDistance)
			{
				bestDistance = distance;
				bestPath = new List<string>(path);

				logger.SendDebug(nameof(Day09), $"{bestDistance}: {string.Join(" -> ", bestPath)}");
			}
			return;
		}

		foreach (var location in availableLocations)
		{
			var newPath = new List<string>(path) { location };
			FindShortestPath(newPath);
		}
	}

	private string ProcessDataForPart2()
	{
		var path = new List<string>();

		bestDistance = 0;
		bestPath = null;

		FindLongestPath(path);

		logger.SendDebug(nameof(Day09), $"{bestDistance}: {string.Join(" -> ", bestPath)}");

		return bestDistance.ToString();
	}

	private void FindLongestPath(List<string> path)
	{
		var availableLocations = locations.Keys.Where(l => !path.Contains(l)).ToList();

		if (availableLocations.Count == 0)
		{
			int distance = 0;
			for (int i = 1; i < path.Count; i++)
			{
				var from = path[i - 1];
				var to = path[i];
				distance += locations[from][to];
			}

			if (distance > bestDistance)
			{
				bestDistance = distance;
				bestPath = new List<string>(path);

				logger.SendDebug(nameof(Day09), $"{bestDistance}: {string.Join(" -> ", bestPath)}");
			}
			return;
		}

		foreach (var location in availableLocations)
		{
			var newPath = new List<string>(path) { location };
			FindLongestPath(newPath);
		}
	}
}
