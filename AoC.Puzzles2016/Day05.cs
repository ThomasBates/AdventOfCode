using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day05 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 05;

	public string Name => $"Day 05";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day05Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day05(ILogger logger)
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

	private string LoadDataFromInput(string input)
	{
		string doorID = null;

		InputHelper.TraverseInputTokens(input, value =>
		{
			doorID = value;
		});
		
		return doorID;
	}

	private string ProcessDataForPart1(string doorID)
	{
		var password = new StringBuilder();

		using var md5 = System.Security.Cryptography.MD5.Create();

		int number = 1;
		while (password.Length < 8)
		{
			var input = $"{doorID}{number}";

			byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
			byte[] hashBytes = md5.ComputeHash(inputBytes);

			if (hashBytes[0] == 0x00 &&
				hashBytes[1] == 0x00 &&
				hashBytes[2] < 0x10)
			{
				var hash = new StringBuilder();
				foreach (var b in hashBytes)
					hash.Append($"{b:X2}");

				char c = hash.ToString()[5];
				logger.SendDebug(nameof(Day05), $"{c} <== {hash}");
				password.Append(c);
			}

			number++;
		}

		return password.ToString();
	}

	private string ProcessDataForPart2(string doorID)
	{
		var password = "........";
		var passwordArray = password.ToCharArray();

		using var md5 = System.Security.Cryptography.MD5.Create();

		int number = 1;
		while (true)
		{
			var input = $"{doorID}{number}";

			byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
			byte[] hashBytes = md5.ComputeHash(inputBytes);

			if (hashBytes[0] == 0x00 &&
				hashBytes[1] == 0x00 &&
				hashBytes[2] < 0x08)
			{
				var position = hashBytes[2];
				if (passwordArray[position] == '.')
				{
					var hash = new StringBuilder();
					foreach (var b in hashBytes)
						hash.Append($"{b:X2}");

					char c = hash.ToString()[6];
					passwordArray[position] = c;
					password = new string(passwordArray);

					logger.SendDebug(nameof(Day05), $"{position}-{c} ==> {password} <== {hash}");

					if (!password.Contains("."))
						break;
				}
			}

			number++;
		}

		return password;
	}
}
