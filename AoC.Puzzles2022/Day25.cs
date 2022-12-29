using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day25 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 25;

	public string Name => $"Day 25";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day25Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day25(ILogger logger)
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

	private readonly List<string> snafus = new();

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		snafus.Clear();

		InputHelper.TraverseInputLines(input, line =>
		{
			snafus.Add(line);
		});
	}

	private string ProcessDataForPart1()
	{
		long sum = 0;
		foreach (var snafu in snafus)
		{
			var dec = SnafuToDecimal(snafu);
			var back = DecimalToSnafu(dec);
			logger.SendDebug(nameof(Day25), $"{snafu} => {dec} => {back}");
			sum += dec;
		}
		var snafuSum = DecimalToSnafu(sum);
		logger.SendDebug(nameof(Day25), $"{sum} => {snafuSum}");

		return snafuSum;
	}

	private string ProcessDataForPart2()
	{
		string snafuSum = "0";
		foreach (var snafu in snafus)
		{
			var sum = AddSnafu(snafuSum, snafu);

			logger.SendDebug(nameof(Day25), $"{snafuSum} + {snafu} = {sum}");

			snafuSum = sum;
		}
		return snafuSum;
	}

	private long SnafuToDecimal(string snafu)
	{
		long result = 0;
		long placePower = 1;
		for (int i = snafu.Length - 1; i >= 0; i--)
		{
			char digit = snafu[i];
			int digitValue = digit switch
			{
				'=' => -2,
				'-' => -1,
				'0' => 0,
				'1' => 1,
				'2' => 2,
				_ => throw new ArgumentOutOfRangeException()
			};
			result += digitValue * placePower;
			placePower *= 5;
		}

		return result;
	}

	private string DecimalToSnafu(long dec)
	{
		var result = new List<char>();

		while (dec != 0)
		{
			long digit = dec % 5;
			dec /= 5;

			switch (digit)
			{
				case 0:
					result.Insert(0, '0');
					break;
				case 1:
					result.Insert(0, '1');
					break;
				case 2:
					result.Insert(0, '2');
					break;
				case 3:
					result.Insert(0, '=');
					dec++;
					break;
				case 4:
					result.Insert(0, '-');
					dec++;
					break;
			}
		}
		return new string(result.ToArray());
	}

	private readonly char[,] addTable = new char[,]
	{

		{ '-', '=', '=', '-', '0' },
		{ '-', '=', '-', '-', '1' },
		{ '-', '=', '0', '-', '2' },
		{ '-', '=', '1', '0', '=' },
		{ '-', '=', '2', '0', '-' },

		{ '-', '-', '=', '-', '1' },
		{ '-', '-', '-', '-', '2' },
		{ '-', '-', '0', '0', '=' },
		{ '-', '-', '1', '0', '-' },
		{ '-', '-', '2', '0', '0' },

		{ '-', '0', '=', '-', '2' },
		{ '-', '0', '-', '0', '=' },
		{ '-', '0', '0', '0', '-' },
		{ '-', '0', '1', '0', '0' },
		{ '-', '0', '2', '0', '1' },

		{ '-', '1', '=', '0', '=' },
		{ '-', '1', '-', '0', '-' },
		{ '-', '1', '0', '0', '0' },
		{ '-', '1', '1', '0', '1' },
		{ '-', '1', '2', '0', '2' },

		{ '-', '2', '=', '0', '-' },
		{ '-', '2', '-', '0', '0' },
		{ '-', '2', '0', '0', '1' },
		{ '-', '2', '1', '0', '2' },
		{ '-', '2', '2', '1', '=' },

		{ '0', '=', '=', '-','1' },
		{ '0', '=', '-', '-','2' },
		{ '0', '=', '0', '0','=' },
		{ '0', '=', '1', '0','-' },
		{ '0', '=', '2', '0','0' },

		{ '0', '-', '=', '-','2' },
		{ '0', '-', '-', '0','=' },
		{ '0', '-', '0', '0','-' },
		{ '0', '-', '1', '0','0' },
		{ '0', '-', '2', '0','1' },

		{ '0', '0', '=', '0','=' },
		{ '0', '0', '-', '0','-' },
		{ '0', '0', '0', '0','0' },
		{ '0', '0', '1', '0','1' },
		{ '0', '0', '2', '0','2' },

		{ '0', '1', '=', '0','-' },
		{ '0', '1', '-', '0','0' },
		{ '0', '1', '0', '0','1' },
		{ '0', '1', '1', '0','2' },
		{ '0', '1', '2', '1','=' },

		{ '0', '2', '=', '0','0' },
		{ '0', '2', '-', '0','1' },
		{ '0', '2', '0', '0','2' },
		{ '0', '2', '1', '1','=' },
		{ '0', '2', '2', '1','-' },

		{ '1', '=', '=', '-','2' },
		{ '1', '=', '-', '0','=' },
		{ '1', '=', '0', '0','-' },
		{ '1', '=', '1', '0','0' },
		{ '1', '=', '2', '0','1' },

		{ '1', '-', '=', '0','=' },
		{ '1', '-', '-', '0','-' },
		{ '1', '-', '0', '0','0' },
		{ '1', '-', '1', '0','1' },
		{ '1', '-', '2', '0','2' },

		{ '1', '0', '=', '0','-' },
		{ '1', '0', '-', '0','0' },
		{ '1', '0', '0', '0','1' },
		{ '1', '0', '1', '0','2' },
		{ '1', '0', '2', '1','=' },

		{ '1', '1', '=', '0','0' },
		{ '1', '1', '-', '0','1' },
		{ '1', '1', '0', '0','2' },
		{ '1', '1', '1', '1','=' },
		{ '1', '1', '2', '1','-' },

		{ '1', '2', '=', '0','1' },
		{ '1', '2', '-', '0','2' },
		{ '1', '2', '0', '1','=' },
		{ '1', '2', '1', '1','-' },
		{ '1', '2', '2', '1','0' },
	};

	private string AddSnafu(string snafu1, string snafu2)
	{
		var result = new List<char>();
		char carry = '0';
		for (int i = 0; i < Math.Max(snafu1.Length, snafu2.Length); i++)
		{
			var digit1 = i < snafu1.Length ? snafu1[snafu1.Length - 1 - i] : '0';
			var digit2 = i < snafu2.Length ? snafu2[snafu2.Length - 1 - i] : '0';

			for (int j = 0; j < addTable.GetLength(0); j++)
			{
				if (addTable[j, 0] == carry &&
					addTable[j, 1] == digit1 &&
					addTable[j, 2] == digit2)
				{
					carry = addTable[j, 3];
					var digit = addTable[j, 4];
					result.Insert(0, digit);
					break;
				}
			}
		}

		return new string(result.ToArray());
	}
}
