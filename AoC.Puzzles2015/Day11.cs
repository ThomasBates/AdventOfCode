using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day11 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

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

	private readonly List<string> lines = new();

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		lines.Clear();

		InputHelper.TraverseInputLines(input, line =>
		{
			lines.Add(line);
		});
	}

	private string ProcessDataForPart1()
	{
		string result = "";
		foreach (var line in lines)
		{
			var password = GetNextPassword(line);
			logger.SendDebug(nameof(Day11), $"{line} => {password}");
			result = password;
		}
		return result;
	}

	private string ProcessDataForPart2()
	{
		string result = "";
		foreach (var password0 in lines)
		{
			var password1 = GetNextPassword(password0);
			logger.SendDebug(nameof(Day11), $"{password0} => {password1}");
			var password2 = GetNextPassword(password1);
			logger.SendDebug(nameof(Day11), $"{password1} => {password2}");
			result = password2;
		}
		return result;
	}

	private string GetNextPassword(string password)
	{
		bool ok = false;
		while (!ok)
		{
			password = IncrementPassword(password);
			ok = AssessPassword(password);
		}
		return password;
	}

	private bool AssessPassword(string password)
	{

		//  Rule: Passwords must include one increasing straight of at least three letters
		bool rule1 = false;
		for (int i = 0; i < 6 & !rule1; i++)
			rule1 = (((password[i] + 1) == password[i + 1]) && ((password[i + 1] + 1) == password[i + 2]));

		//  Rule: Passwords may not contain the letters i, o, or l
		bool rule2 = 
			!password.Contains("i") && 
			!password.Contains("l") && 
			!password.Contains("o");

		//  Rule: Passwords must contain at least two different, non-overlapping pairs of letters
		bool rule3 = false;
		bool found1 = false;
		for (int i = 0; i < 7 & !rule3; i++)
		{
			if (password[i] == password[i + 1])
			{
				if (found1)
				{
					rule3 = true;
				}
				else
				{
					found1 = true;
					i++;
				}
			}
		}
		
		return rule1 && rule2 && rule3;
	}

	private string IncrementPassword(string password)
	{
		var chars = password.ToCharArray();
		for (int pos = 7; pos >= 0; pos--)
		{
			char c = chars[pos];
			if (c < 'z')
			{
				chars[pos] = ++c;
				break;
			}
			else
			{
				chars[pos] = 'a';
			}
		}
		return new string(chars);
	}
}
