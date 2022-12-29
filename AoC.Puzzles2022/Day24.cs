using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day24 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 24;

	public string Name => "Day 24";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day24Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day24(ILogger logger)
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

	private class Blizzard
	{
		public Point Location;
		public Point Direction;
	}

	private class Valley
	{
		public Point Min = new(1, 1);
		public Point Max;
		public Point Entrance;
		public Point Exit;
		public List<Blizzard> Blizzards = new();
	}

	private Valley valley;

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		valley = new Valley();

		int x = 0;
		InputHelper.TraverseInputLines(input, line =>
		{
			var i = line.IndexOf("#.#");
			if (i >= 0)
			{
				if (x == 0)
				{
					valley.Max.Y = line.Length - 2;
					valley.Entrance = new Point(x, i + 1);
				}
				else
				{
					valley.Max.X = x - 1;
					valley.Exit = new Point(x, i + 1);
				}
			}

			for(int y = 0; y < line.Length;y++)
			{
				char c = line[y];
				switch (c)
				{
					case '>':
						valley.Blizzards.Add(new Blizzard { Location = new Point(x, y), Direction = new Point(0, 1) });
						break;
					case '<':
						valley.Blizzards.Add(new Blizzard { Location = new Point(x, y), Direction = new Point(0, -1) });
						break;
					case '^':
						valley.Blizzards.Add(new Blizzard { Location = new Point(x, y), Direction = new Point(-1, 0) });
						break;
					case 'v':
						valley.Blizzards.Add(new Blizzard { Location = new Point(x, y), Direction = new Point(1, 0) });
						break;
				}
			}

			x++;
		});
	}

	private string ProcessDataForPart1()
	{
		var time = TraverseValley(valley.Entrance, valley.Exit);

		return $"{time}";
	}

	private string ProcessDataForPart2()
	{
		var time = TraverseValley(valley.Entrance, valley.Exit);
		time += TraverseValley(valley.Exit, valley.Entrance);
		time += TraverseValley(valley.Entrance, valley.Exit);

		return $"{time}";
	}

	private readonly List<Point> directions = new() { new Point(1, 0), new Point(0, 1), new Point(-1, 0), new Point(0, -1), new Point(0, 0) };

	private int TraverseValley(Point start, Point goal)
	{
		var clones = new List<Point> { start };

		int time = 0;
		bool found = false;

		while (!found)
		{
			time++;

			//  shift blizzards
			foreach (var blizzard in valley.Blizzards)
			{
				blizzard.Location.Offset(blizzard.Direction);
				if (blizzard.Location.X < valley.Min.X)
					blizzard.Location.X = valley.Max.X;
				if (blizzard.Location.X > valley.Max.X)
					blizzard.Location.X = valley.Min.X;
				if (blizzard.Location.Y < valley.Min.Y)
					blizzard.Location.Y = valley.Max.Y;
				if (blizzard.Location.Y > valley.Max.Y)
					blizzard.Location.Y = valley.Min.Y;
			}

			//  choose possible directions
			var choices = new HashSet<Point>();
			foreach (var clone in clones)
			{
				var cloneChoices = new List<Point>();
				foreach (var direction in directions)
				{
					var choice = clone;
					choice.Offset(direction);

					if (choice == goal)
					{
						found = true;
						choices = new HashSet<Point> { choice };
						break;
					}

					if (choice.X < valley.Min.X ||
						choice.X > valley.Max.X ||
						choice.Y < valley.Min.Y ||
						choice.Y > valley.Max.Y)
					{
						if (choice != start)
							continue;
					}

					var blizzard = valley.Blizzards.FirstOrDefault(b => b.Location == choice);
					if (blizzard == null)
						cloneChoices.Add(choice);
				}

				if (found)
					break;

				foreach (var choice in cloneChoices)
					choices.Add(choice);
			}

			clones = choices.ToList();

			if (valley.Max.Y < 10)
				logger.SendDebug(nameof(Day24), $"{time}: {string.Join(", ", clones)}");
			else
				logger.SendDebug(nameof(Day24), $"{time}: {clones.Count}");
		}

		return time;
	}
}
