using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day13 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 13;

	public string Name => $"Day 13";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day13Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day13(ILogger logger)
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

	private readonly Dictionary<string, Dictionary<string, int>> guests = new();

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		guests.Clear();

		InputHelper.TraverseInputLines(input, line =>
		{
			Match match = Regex.Match(line, @"([A-Za-z]+) would (lose|gain) (\d+) happiness units by sitting next to ([A-Za-z]+).");

			if (!match.Success) 
			{
				logger.SendError(nameof(Day13), $"Couldn't read line: {line}");
				return;
			}

			var guest = match.Groups[1].Value;
			var sign = match.Groups[2].Value == "gain" ? 1 : -1;
			var happiness = int.Parse(match.Groups[3].Value);
			var friend = match.Groups[4].Value;

			happiness *= sign;

			logger.SendDebug(nameof(Day13), $"{guest} => {friend} => {happiness}");

			if (!guests.TryGetValue(guest, out var friends))
			{
				friends = new Dictionary<string, int>();
				guests[guest] = friends;
			}
			if (!friends.ContainsKey(friend))
			{
				friends[friend] = happiness;
			}
		});
	}

	private int bestHappiness;
	private List<string> bestPath;

	private string ProcessDataForPart1()
	{
		var path = new List<string>();

		bestHappiness = 0;
		bestPath = null;

		FindBestHappiness(path);

		logger.SendDebug(nameof(Day09), $"{bestHappiness}: {string.Join(" -> ", bestPath)}");

		return bestHappiness.ToString();
	}

	private string ProcessDataForPart2()
	{
		var me = new Dictionary<string, int>();
		guests["me"] = me;
		foreach (var entry in guests)
		{
			entry.Value["me"] = 0;
			me[entry.Key] = 0;
		}

		var path = new List<string>();

		bestHappiness = 0;
		bestPath = null;

		FindBestHappiness(path);

		logger.SendDebug(nameof(Day09), $"{bestHappiness}: {string.Join(" -> ", bestPath)}");

		return bestHappiness.ToString();
	}

	private void FindBestHappiness(List<string> path)
	{
		var availableGuests = guests.Keys.Where(l => !path.Contains(l)).ToList();

		if (availableGuests.Count == 0)
		{
			int happiness = 0;
			for (int i = 0; i < path.Count; i++)
			{
				var guest = path[i];
				var friend = i + 1 < path.Count ? path[i + 1] : path[0];
				happiness += guests[guest][friend];
				happiness += guests[friend][guest];
			}

			if (happiness > bestHappiness)
			{
				bestHappiness = happiness;
				bestPath = new List<string>(path);

				logger.SendDebug(nameof(Day09), $"{bestHappiness}: {string.Join(" -> ", bestPath)}");
			}
			return;
		}

		foreach (var guest in availableGuests)
		{
			var newPath = new List<string>(path) { guest };
			FindBestHappiness(newPath);
		}
	}
}
