using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day20 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 20;

	public string Name => $"Day 20";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day20Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day20(ILogger logger)
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

		var result = ProcessDataTake2(data, perElf: 10, elfLimit: 0);

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataTake2(data, perElf: 11, elfLimit: 50);

		return result.ToString();
	}

	#endregion Solvers

	private int LoadDataFromInput(string input)
	{
		//  First Clear Data
		int result = 0;
		InputHelper.TraverseInputTokens(input, value =>
		{
			result = int.Parse(value);
		});

		return result;
	}

	//  Brute force: check every house
	private int ProcessDataTake1(int minPresents, int perElf, int elfLimit)
	{
		int houseNumber = 1;

		while (true)
		{
			var presentCount = CountPresents(houseNumber, perElf, elfLimit);

			logger.SendDebug(nameof(Day20), $"House {houseNumber} got {presentCount} presents.");

			if (presentCount >= minPresents)
				return houseNumber;
			houseNumber++;
		}
	}

	private int ProcessDataTake2(int minPresentCount, int perElf, int elfLimit)
	{
		int baseHouseNumber = 1;
		int houseFactor = 1;
		int lowerBound = 1;
		int upperBound = -1;
		int lowerPresentCount = 0;

		while (upperBound < 0)
		{
			baseHouseNumber *= houseFactor;
			houseFactor++;

			for (int i=1; i < houseFactor;i++)
			{
				int houseNumber = baseHouseNumber * i;

				var presentCount = CountPresents(houseNumber, perElf, elfLimit);

				logger.SendDebug(nameof(Day20), $"House {houseNumber} got {presentCount} presents.");

				if (presentCount >= minPresentCount)
				{
					upperBound = houseNumber;
					break;
				}

				lowerBound = houseNumber;
				lowerPresentCount = presentCount;
			}
		}

		for (int houseNumber = lowerBound + 1; houseNumber < upperBound; houseNumber++)
		{
			var presentCount = CountPresents(houseNumber, perElf, elfLimit);

			if (presentCount > lowerPresentCount)
			{
				logger.SendDebug(nameof(Day20), $"House {houseNumber} got {presentCount} presents.");
				lowerPresentCount = presentCount;
			}

			if (presentCount >= minPresentCount)
				return houseNumber;
		}

		logger.SendDebug(nameof(Day20), $"Second loop found nothing.");
		return upperBound;
	}

	private int CountPresents(int houseNumber, int perElf, int elfLimit)
	{
		int presentCount = 0;
		for (int i = 1; i <= houseNumber; i++)
		{
			if (houseNumber % i == 0)
			{
				if (i > houseNumber / i)
					break;
				int visit = houseNumber / i;
				if (elfLimit == 0 || visit <= elfLimit)
					presentCount += perElf * i;

				if (i >= houseNumber / i)
					break;
				visit = i;
				if (elfLimit == 0 || visit <= elfLimit)
					presentCount += perElf * (houseNumber / i);
			}
		}
		return presentCount;
	}
}
