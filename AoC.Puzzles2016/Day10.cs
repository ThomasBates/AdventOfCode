using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day10 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 10;

	public string Name => $"Day 10";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day10Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day10(ILogger logger)
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

	private Dictionary<int, (int low, int high, List<int> values)> LoadDataFromInput(string input)
	{
		var data = new object();

		var robots = new Dictionary<int, (int low, int high, List<int> values)>();
		
		GrammarHelper.ParseInput(logger, input, Resources.Day10Grammar,
			null,
			null,
			(token, valueStack) =>
			{
				switch (token)
				{
					case "c_value":
						{
							int bot = int.Parse(valueStack.Pop());
							int value = int.Parse(valueStack.Pop());
							if (!robots.TryGetValue(bot, out var robot))
							{
								robot = (0, 0, new List<int>());
								robots[bot] = robot;
							}
							else
							{ }
							robot.values.Add(value);
						}
						break;
					case "c_bot":
						{
							int high = int.Parse(valueStack.Pop());
							int low = int.Parse(valueStack.Pop());
							int bot = int.Parse(valueStack.Pop());

							if (robots.TryGetValue(bot, out var robot))
								robots[bot] = (low, high, robot.values);
							else
								robots[bot] = (low, high, new List<int>());
						}
						break;
					case "c_output":
						int number = int.Parse(valueStack.Pop());

						valueStack.Push($"{number + 1000000}");
						break;
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			});
		
		return robots;
	}

	private int ProcessDataForPart1(Dictionary<int, (int low, int high, List<int> values)> robots)
	{
		return ProcessData(robots, part1: true);
	}

	private int ProcessDataForPart2(Dictionary<int, (int low, int high, List<int> values)> robots)
	{
		return ProcessData(robots, part1: false);
	}

	private int ProcessData(Dictionary<int, (int low, int high, List<int> values)> robots, bool part1)
	{
		int winner = -1;
		int lowTest = robots.Count < 10 ? 2 : 17;
		int highTest = robots.Count < 10 ? 5 : 61;

		var outputs = new Dictionary<int, int>();

		while (true)
		{
			var robotEntries = robots.Where(r => r.Value.values.Count == 2);
			if (!robotEntries.Any())
				break;
			var robotEntry = robotEntries.First();

			var (lowBot, highBot, values) = robotEntry.Value;
			var lowValue = values.Min(v => v);
			var highValue = values.Max(v => v);

			logger.SendDebug(nameof(Day10), $"bot {robotEntry.Key}: {lowValue} => robot {lowBot}, {highValue} => robot {highBot}");

			if (lowValue == lowTest && highValue == highTest)
			{
				winner = robotEntry.Key;
				logger.SendDebug(nameof(Day10), $"^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
			}

			if (lowBot >= 1000000)
			{

				outputs[lowBot - 1000000] = lowValue;
			}
			else
			{
				if (robots[lowBot].values.Count > 1)
				{ }
				robots[lowBot].values.Add(values.Min(v => v));
			}
			if (highBot >= 1000000)
			{
				outputs[highBot - 1000000] = highValue;
			}
			else
			{
				if (robots[highBot].values.Count > 1)
				{ }
				robots[highBot].values.Add(values.Max(v => v));
			}
			values.Clear();
		}

		var product = outputs[0] * outputs[1] * outputs[2];
		logger.SendDebug(nameof(Day10), $"{outputs[0]} * {outputs[1]} * {outputs[2]} = {product}");

		return part1 ? winner : product;
	}
}
