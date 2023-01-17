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
public class Day16 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 16;

	public string Name => $"Day 16";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day16Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day16(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1 (method 1)", input => SolvePart1(LoadData(input), Method1).ToString());
		Solvers.Add("Solve Part 1 (method 2)", input => SolvePart1(LoadData(input), Method2).ToString());
		Solvers.Add("Solve Part 2 (method 1)", input => SolvePart2(LoadData(input), Method1).ToString());
		Solvers.Add("Solve Part 2 (method 2)", input => SolvePart2(LoadData(input), Method2).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day16), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day16), message);

	#endregion Helpers

	private (string, int) LoadData(string input)
	{
		string start = "";
		int disk = -1;

		InputHelper.TraverseInputTokens(input, value =>
		{
			if (string.IsNullOrEmpty(start))
				start = value;
			else
				disk = int.Parse(value);
		});

		return (start, disk);
	}

	private string SolvePart1((string, int) data, Func<string, int, string> method)
	{
		var (start, diskSize) = data;

		if (diskSize < 0)
			diskSize = 272;

		return method(start, diskSize);
	}

	private string SolvePart2((string, int) data, Func<string, int, string> method)
	{
		var (start, diskSize) = data;

		if (diskSize < 0)
			diskSize = 35651584;

		return method(start, diskSize);
	}

	private string Method1(string start, int diskSize)
	{
		var a = start.ToCharArray();
		while (a.Length <= diskSize)
		{
			var b = new char[a.Length * 2 + 1];
			for (int i = 0; i < a.Length; i++)
			{
				b[i] = a[i];
				b[b.Length - 1 - i] = a[i] == '0' ? '1' : '0';
			}
			b[a.Length] = '0';
			a = b;
		}
		a = a.Take(diskSize).ToArray();

		a = CalcChecksum(a);

		return new string(a);
	}

	private string Method2(string start, int diskSize)
	{
		var disk = new char[diskSize];

		for (int i = 0; i < start.Length; i++)
			disk[i] = start[i];

		var pos = 0;
		for (int i = start.Length; i < diskSize; i++)
		{
			if (pos == 0)
			{
				pos = i;
				disk[i] = '0';
			}
			else
			{
				disk[i] = disk[--pos] == '0' ? '1' : '0';
			}
		}

		var a = CalcChecksum(disk);

		return new string(a);
	}

	private char[] CalcChecksum(char[] a)
	{
		while (a.Length % 2 == 0)
		{
			var b = new char[a.Length / 2];
			for (int i = 0; i < a.Length; i += 2)
				b[i / 2] = a[i] == a[i + 1] ? '1' : '0';
			a = b;
		}
		return a;
	}
}
