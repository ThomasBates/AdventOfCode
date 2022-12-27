using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using AoC.Common;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day03 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 03;

	public string Name => $"Day 03";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day03Inputs},
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
		int result = -1;
		foreach (var line in lines)
		{
			var houses = new HashSet<Point>();

			var house = new Point(0, 0);
			houses.Add(house);

			for (int i = 0; i < line.Length; i++)
			{
				var c= line[i];
				switch (c) 
				{
					case '^':
						house.Offset(0, 1);
						break;
					case 'v':
						house.Offset(0, -1);
						break;
					case '>':
						house.Offset(1, 0);
						break;
					case '<':
						house.Offset(-1, 0);
						break;
				}
				houses.Add(house);
			}

			result = houses.Count;

			logger.Send(SeverityLevel.Debug, nameof(Day03), $"{line}");
			logger.Send(SeverityLevel.Debug, nameof(Day03), $"{result} houses");
		}

		return result.ToString();
	}

	private string ProcessDataForPart2()
	{
		int result = -1;
		foreach (var line in lines)
		{
			var houses = new HashSet<Point>();

			var house = new Point[2] { new Point(0, 0), new Point(0, 0) };
			houses.Add(house[0]);

			for (int i = 0; i < line.Length; i++)
			{
				var c = line[i];
				switch (c)
				{
					case '^':
						house[i % 2].Offset(0, 1);
						break;
					case 'v':
						house[i % 2].Offset(0, -1);
						break;
					case '>':
						house[i % 2].Offset(1, 0);
						break;
					case '<':
						house[i % 2].Offset(-1, 0);
						break;
				}
				houses.Add(house[i % 2]);
			}

			result = houses.Count;

			logger.Send(SeverityLevel.Debug, nameof(Day03), $"{line}");
			logger.Send(SeverityLevel.Debug, nameof(Day03), $"{result} houses");
		}

		return result.ToString();
	}
}
