using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day03 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 03;

	public string Name => $"Day 03";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day03(ILogger logger)
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

	private List<(int,int,int)> LoadDataFromInput(string input)
	{
		var data = new List<(int, int, int)>();

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			data.Add((int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])));
		});
		
		return data;
	}

	private int ProcessDataForPart1(List<(int, int, int)> data)
	{
		int count = 0;

		foreach (var (n1, n2, n3) in data)
		{
			if (n1 + n2 > n3 && n1 + n3 > n2 && n2 + n3 > n1)
				count++;
		}

		return count;
	}

	private int ProcessDataForPart2(List<(int c1, int c2, int c3)> data)
	{
		int count = 0;

		for (int i=0; i<data.Count; i+=3)
		{
			var (n1, n2, n3) = (data[i].c1, data[i + 1].c1, data[i + 2].c1);
			if (n1 + n2 > n3 && n1 + n3 > n2 && n2 + n3 > n1)
				count++;

			(n1, n2, n3) = (data[i].c2, data[i + 1].c2, data[i + 2].c2);
			if (n1 + n2 > n3 && n1 + n3 > n2 && n2 + n3 > n1)
				count++;

			(n1, n2, n3) = (data[i].c3, data[i + 1].c3, data[i + 2].c3);
			if (n1 + n2 > n3 && n1 + n3 > n2 && n2 + n3 > n1)
				count++;

		}

		return count;
	}
}
