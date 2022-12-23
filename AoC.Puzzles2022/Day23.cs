using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Text;
using AoC.Common;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day23 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 23;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day23Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day23(ILogger logger)
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

		var result = ProcessDataForPart1(10);

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataForPart2();

		return result;
	}

	#endregion Solvers

	private HashSet<Point> elves;

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		elves = new HashSet<Point>();

		int x = 0;
		Helper.TraverseInputLines(input, line =>
		{
			for (int y = 0; y < line.Length; y++)
			{
				if (line[y] == '#')
					elves.Add(new Point(x, y));
			}
			x++;
		});
	}

	private readonly char[] moveTypes = new[] { 'N', 'S', 'W', 'E' };
	private int moveIndex;

	private string ProcessDataForPart1(int count)
	{
		moveIndex = 0;
		for (int i = 0; i < count; i++)
		{
			ScatterElves();

			VisualizeElves(i + 1);
		}

		var min = new Point(int.MaxValue, int.MaxValue);
		var max = new Point(int.MinValue, int.MinValue);

		foreach (var elf in elves)
		{
			min.X = Math.Min(min.X, elf.X);
			min.Y = Math.Min(min.Y, elf.Y);
			max.X = Math.Max(max.X, elf.X);
			max.Y = Math.Max(max.Y, elf.Y);
		}

		var dx = (max.X - min.X + 1);
		var dy = (max.Y - min.Y + 1);
		var empties = dx * dy - elves.Count;

		return $"{dx} x {dy} - {elves.Count} = {empties}";
	}

	private string ProcessDataForPart2()
	{
		moveIndex = 0;
		int round = 1;

		while (true)
		{
			var moves = ScatterElves();
			if (moves == 0)
				break;
			round++;
		}

		VisualizeElves(round);

		return $"Round {round}";
	}

	private int ScatterElves()
	{
		var proposed = new Dictionary<Point, Point>();
		var targets = new Dictionary<Point, int>();

		//  Step 1. Considerations
		foreach (var elf in elves)
		{
			bool N = elves.Contains(new Point(elf.X - 1, elf.Y));
			bool S = elves.Contains(new Point(elf.X + 1, elf.Y));
			bool W = elves.Contains(new Point(elf.X, elf.Y - 1));
			bool E = elves.Contains(new Point(elf.X, elf.Y + 1));

			bool NW = elves.Contains(new Point(elf.X - 1, elf.Y - 1));
			bool NE = elves.Contains(new Point(elf.X - 1, elf.Y + 1));
			bool SW = elves.Contains(new Point(elf.X + 1, elf.Y - 1));
			bool SE = elves.Contains(new Point(elf.X + 1, elf.Y + 1));

			if (!N && !S && !W && !E && !NW && !NE && !SW && !SE)
				continue;

			Point? target = null;
			for (int i = 0; i < 4 && !target.HasValue; i++)
			{
				var tempIndex = (moveIndex + i) % 4;
				var moveType = moveTypes[tempIndex];
				switch (moveType)
				{
					case 'N':
						if (!NW && !N && !NE)
							target = new Point(elf.X - 1, elf.Y);
						break;
					case 'S':
						if (!SW && !S && !SE)
							target = new Point(elf.X + 1, elf.Y);
						break;
					case 'W':
						if (!NW && !W && !SW)
							target = new Point(elf.X, elf.Y - 1);
						break;
					case 'E':
						if (!NE && !E && !SE)
							target = new Point(elf.X, elf.Y + 1);
						break;
				}
			}
			if (target.HasValue)
			{
				proposed[elf] = target.Value;
				if (targets.TryGetValue(target.Value, out var count))
					targets[target.Value] = count + 1;
				else
					targets[target.Value] = 1;
			}
		}
		moveIndex = (moveIndex + 1) % moveTypes.Length;

		var newElves = new HashSet<Point>();

		//  Step 2. Movement
		foreach (var elf in elves)
		{
			if (!proposed.TryGetValue(elf, out var target))
			{
				newElves.Add(elf);
				continue;
			}
			if (targets[target] > 1)
			{
				newElves.Add(elf);
				continue;
			}
			newElves.Add(target);
		}
		elves = newElves;

		return proposed.Count;
	}

	private void VisualizeElves(int round)
	{
		var output = new StringBuilder();
		output.AppendLine($"== End of Round {round} ==");

		var min = new Point(int.MaxValue, int.MaxValue);
		var max = new Point(int.MinValue, int.MinValue);

		foreach (var elf in elves)
		{
			min.X = Math.Min(min.X, elf.X);
			min.Y = Math.Min(min.Y, elf.Y);
			max.X = Math.Max(max.X, elf.X);
			max.Y = Math.Max(max.Y, elf.Y);
		}

		var dx = (max.X - min.X + 1);
		var dy = (max.Y - min.Y + 1);

		output.AppendLine($"{dx} x {dy}");

		for (int x = min.X - 1; x <= max.X + 1; x++)
		{
			for (int y = min.Y - 1; y <= max.Y + 1; y++)
			{
				if (elves.Contains(new Point(x, y)))
					output.Append("#");
				else
					output.Append(".");
			}
			output.AppendLine();
		}

		logger.Send(SeverityLevel.Debug, nameof(Day23), output.ToString());
	}
}
