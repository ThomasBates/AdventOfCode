using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.RegularExpressions;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day14 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 14;

	public string Name => $"Day 14";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day14Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day14(ILogger logger)
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

	private readonly Dictionary<string, (int, int, int)> allReindeer = new();

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		allReindeer.Clear();

		InputHelper.TraverseInputLines(input, line =>
		{
			Match match = Regex.Match(line, @"([A-Za-z]+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds.");

			if (!match.Success)
			{
				logger.SendError(nameof(Day13), $"Couldn't read line: {line}");
				return;
			}

			var reindeer = match.Groups[1].Value;
			var speed = int.Parse(match.Groups[2].Value);
			var duration = int.Parse(match.Groups[3].Value);
			var rest = int.Parse(match.Groups[4].Value);

			logger.SendDebug(nameof(Day14), $"{reindeer}: speed = {speed}, duration = {duration}, rest time = {rest}, average speed = {speed * duration * 1.0 / (duration + rest)}");

			allReindeer[reindeer] = (speed, duration, rest);
		});
	}

	private string ProcessDataForPart1()
	{
		var totalTime = allReindeer.Count < 9 ? 1000 : 2503;

		var bestDistance = 0;
		var bestReindeer = "";

		foreach(var entry in allReindeer)
		{
			var reindeer = entry.Key;
			var (speed, duration, rest) = entry.Value;

			var distance = CalcDistanceAtTime(speed, duration, rest, totalTime);

			logger.SendDebug(nameof(Day14), $"{reindeer} ran {distance} km");

			if (distance > bestDistance)
			{
				bestDistance = distance;
				bestReindeer = reindeer;
			}
		}

		return bestDistance.ToString();
	}

	private string ProcessDataForPart2()
	{
		var totalTime = allReindeer.Count < 9 ? 1000 : 2503;

		var scores = new Dictionary<string, int>();
		foreach (var reindeer in allReindeer.Keys)
			scores[reindeer] = 0;

		for (int t = 0; t < totalTime; t++)
		{
			var distances = new Dictionary<string, int>();
			var bestDistance = 0;
			foreach (var entry in allReindeer)
			{
				var reindeer = entry.Key;
				var (speed, duration, rest) = entry.Value;

				var distance = CalcDistanceAtTime(speed, duration, rest, t+1);
				distances[reindeer] = distance;

				if (distance > bestDistance)
					bestDistance = distance;
			}

			foreach (var reindeer in allReindeer.Keys)
			{
				if (distances[reindeer] == bestDistance)
					scores[reindeer]++;
			}

			logger.SendDebug(nameof(Day14), $"After {t + 1} second{(t > 0 ? "s" : "")}:");
			foreach (var reindeer in allReindeer.Keys)
				logger.SendDebug(nameof(Day14), $"{reindeer}: {distances[reindeer]} km => {scores[reindeer]} points.");
		}

		var bestScore = 0;
		var bestReindeer = "";

		foreach (var entry in scores)
		{
			var reindeer = entry.Key;
			var score = entry.Value;

			logger.SendDebug(nameof(Day14), $"{reindeer} has {score} points");

			if (score > bestScore)
			{
				bestScore = score;
				bestReindeer = reindeer;
			}
		}

		logger.SendDebug(nameof(Day14), $"{bestReindeer} wins with {bestScore} points");

		return bestScore.ToString();
	}

	private int CalcDistanceAtTime(int speed, int duration, int rest, int totalTime)
	{
		int spurts = totalTime / (duration + rest);
		int time = spurts * (duration + rest);
		int distance = spurts * speed * duration;

		int remainingTime = totalTime - time;
		int runningTime = Math.Min(remainingTime, duration);
		distance += speed * runningTime;

		return distance;
	}
}
