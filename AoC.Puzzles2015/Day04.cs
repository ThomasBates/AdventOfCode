using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using AoC.Common;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day04 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 04;

	public string Name => $"Day 04";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day04Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day04(ILogger logger)
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

	private List<string> lines = new();

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		lines.Clear();

		Helper.TraverseInputLines(input, line =>
		{
			lines.Add(line);
		});
	}

	private string ProcessDataForPart1()
	{
		int result = 0;
		foreach (var key in lines)
		{
			int number = 1;
			while (true)
			{
				var input = $"{key}{number}";

				// Use input string to calculate MD5 hash
				using var md5 = System.Security.Cryptography.MD5.Create();
				
				byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				if (hashBytes[0] == 0x00 &&
					hashBytes[1] == 0x00 &&
					hashBytes[2] < 0x10)
				{
					break;
				}

				number++;
			}
			result = number;

			logger.Send(SeverityLevel.Debug, nameof(Day03), $"{key}{result}");
		}

		return result.ToString();
	}

	private string ProcessDataForPart2()
	{
		int result = 0;
		foreach (var key in lines)
		{
			int number = 1;
			while (true)
			{
				var input = $"{key}{number}";

				// Use input string to calculate MD5 hash
				using var md5 = System.Security.Cryptography.MD5.Create();

				byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				if (hashBytes[0] == 0x00 &&
					hashBytes[1] == 0x00 &&
					hashBytes[2] == 0x00)
				{
					break;
				}

				number++;
			}
			result = number;

			logger.Send(SeverityLevel.Debug, nameof(Day03), $"{key}{result}");
		}

		return result.ToString();
	}
}
