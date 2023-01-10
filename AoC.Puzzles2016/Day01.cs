using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day01 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 01;

	public string Name => $"Day 01";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day01Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day01(ILogger logger)
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

	private List<(string, int)> LoadDataFromInput(string input)
	{
		//  First Clear Data
		var instructions = new List<(string, int)>();

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var part in parts)
			{
				var value = part.Trim();
				var turn = value.Substring(0, 1);
				var distance = int.Parse(value.Substring(1));

				instructions.Add((turn, distance));
			}
		});

		return instructions;
	}

	private readonly List<Point> directions = new()
	{
		new Point(0,1),
		new Point(1,0),
		new Point(0,-1),
		new Point(-1,0),
	};

	private int ProcessDataForPart1(List<(string, int)> instructions)
	{
		var location = new Point(0, 0);
		var direction = 0;

		foreach(var (turn, distance) in instructions)
		{
			direction = turn switch
			{
				"L" => (direction + 3) % 4,
				"R" => (direction + 1) % 4,
				_ => direction
			};

			location.Offset(directions[direction].X * distance, directions[direction].Y * distance);

			logger.SendDebug(nameof(Day01), $"{turn}-{distance} => {location}");
		}

		return Math.Abs(location.X) + Math.Abs(location.Y);
	}

	private int ProcessDataForPart2(List<(string, int)> instructions)
	{
		var location = new Point(0, 0);
		var direction = 0;

		var path = new List<Point>();

		foreach (var (turn, distance) in instructions)
		{
			direction = turn switch
			{
				"L" => (direction + 3) % 4,
				"R" => (direction + 1) % 4,
				_ => direction
			};

			for (int i = 0; i < distance; i++)
			{
				location.Offset(directions[direction]);

				logger.SendDebug(nameof(Day01), $"{turn}-{distance} => {location}");

				if (path.Contains(location))
					return Math.Abs(location.X) + Math.Abs(location.Y);

				path.Add(location);
			}
		}

		return Math.Abs(location.X) + Math.Abs(location.Y);
	}
}
