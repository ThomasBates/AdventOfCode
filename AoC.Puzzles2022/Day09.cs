using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day09 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 9;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs (1)", Resources.Day09Inputs},
		{"Example Inputs (2)", Resources.Day09Inputs2},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new()
	{
		{ "Part 1 (a)", SolvePart1a },
		{ "Part 2 (a)", SolvePart2a },
		{ "Part 1 (b)", SolvePart1b },
		{ "Part 2 (b)", SolvePart2b }
	};

	#endregion IPuzzle Properties

	private static string SolvePart1a(string input)
	{
		var output = new StringBuilder();

		var forest = new List<string>();
		var tailSpots = new HashSet<string>();
		int headX = 0;
		int headY = 0;
		int tailX = 0;
		int tailY = 0;

		tailSpots.Add($"{tailX},{tailY}");

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			int dx = 0;
			int dy = 0;
			switch (parts[0])
			{
				case "U": dx =  0; dy =  1; break;
				case "D": dx =  0; dy = -1; break;
				case "L": dx = -1; dy =  0; break;
				case "R": dx =  1; dy =  0; break;
			}

			int count = int.Parse(parts[1]);

			for (int i = 0; i < count; i++)
			{
				headX += dx;
				headY += dy;

				if (headX >= tailX + 2)
				{
					tailX = headX - 1;
					tailY = headY;
				}

				if (headX <= tailX - 2)
				{
					tailX = headX + 1;
					tailY = headY;
				}

				if (headY >= tailY + 2)
				{
					tailY = headY - 1;
					tailX = headX;
				}

				if (headY <= tailY - 2)
				{
					tailY = headY + 1;
					tailX = headX;
				}


				tailSpots.Add($"{tailX},{tailY}");
			}
		});

		foreach (var spot in tailSpots.OrderBy(s => s))
		{
			output.AppendLine(spot);
		}

		output.AppendLine($"The answer is {tailSpots.Count}");

		return output.ToString();
	}

	private static string SolvePart2a(string input)
	{
		var output = new StringBuilder();

		var forest = new List<string>();
		var tailSpots = new HashSet<string>();
		var rope = new Point[10];

		for (int i = 0; i < 10; i++)
			rope[i] = new Point(0, 0);

		tailSpots.Add($"0,0");

		InputHelper.TraverseInputLines(input, line =>
		{
			output.AppendLine(line);
			var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			var dir = new Point(0, 0);
			switch (parts[0])
			{
				case "U": dir.Y = 1; break;
				case "D": dir.Y = -1; break;
				case "L": dir.X = -1; break;
				case "R": dir.X = 1; break;
			}

			int count = int.Parse(parts[1]);

			for (int i = 0; i < count; i++)
			{
				rope[0].Offset(dir);

				for (int knot = 1; knot < rope.Length; knot++)
				{
					var head = knot - 1;
					var tail = knot;


					if (rope[head].X >= rope[tail].X + 2)
					{
						rope[tail].X = rope[head].X - 1;
						if (rope[tail].Y < rope[head].Y)
							rope[tail].Y++;
						if (rope[tail].Y > rope[head].Y)
							rope[tail].Y--;
					}

					if (rope[head].X <= rope[tail].X - 2)
					{
						rope[tail].X = rope[head].X + 1;
						if (rope[tail].Y < rope[head].Y)
							rope[tail].Y++;
						if (rope[tail].Y > rope[head].Y)
							rope[tail].Y--;
					}

					if (rope[head].Y >= rope[tail].Y + 2)
					{
						rope[tail].Y = rope[head].Y - 1;
						if (rope[tail].X < rope[head].X)
							rope[tail].X++;
						if (rope[tail].X > rope[head].X)
							rope[tail].X--;
					}

					if (rope[head].Y <= rope[tail].Y - 2)
					{
						rope[tail].Y = rope[head].Y + 1;
						if (rope[tail].X < rope[head].X)
							rope[tail].X++;
						if (rope[tail].X > rope[head].X)
							rope[tail].X--;
					}
				}

				var ropeTail = rope[rope.Length - 1];

				tailSpots.Add($"{ropeTail.X},{ropeTail.Y}");
			}
			output.AppendLine(string.Join(", ", rope));
		});

		foreach (var spot in tailSpots.ToArray().OrderBy(s => s))
		{
			output.AppendLine(spot);
		}

		output.AppendLine($"The answer is {tailSpots.Count}");

		return output.ToString();
	}

	private static string SolvePart1b(string input)
	{
		return SolveRope(input, 2);
	}

	private static string SolvePart2b(string input)
	{
		return SolveRope(input, 10);
	}


	private static string SolveRope(string input, int length)
	{
		var output = new StringBuilder();

		var forest = new List<string>();
		var tailSpots = new HashSet<string>();
		var rope = new Point[length];

		for (int i = 0; i < length; i++)
			rope[i] = new Point(0, 0);

		tailSpots.Add($"0,0");

		InputHelper.TraverseInputLines(input, line =>
		{
			output.AppendLine(line);
			var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			var dir = new Point(0, 0);
			switch (parts[0])
			{
				case "U": dir.Y = 1; break;
				case "D": dir.Y = -1; break;
				case "L": dir.X = -1; break;
				case "R": dir.X = 1; break;
			}

			int count = int.Parse(parts[1]);

			for (int i = 0; i < count; i++)
			{
				rope[0].Offset(dir);

				for (int knot = 1; knot < rope.Length; knot++)
				{
					var head = knot - 1;
					var tail = knot;


					if (rope[head].X >= rope[tail].X + 2)
					{
						rope[tail].X = rope[head].X - 1;
						if (rope[tail].Y < rope[head].Y)
							rope[tail].Y++;
						if (rope[tail].Y > rope[head].Y)
							rope[tail].Y--;
					}

					if (rope[head].X <= rope[tail].X - 2)
					{
						rope[tail].X = rope[head].X + 1;
						if (rope[tail].Y < rope[head].Y)
							rope[tail].Y++;
						if (rope[tail].Y > rope[head].Y)
							rope[tail].Y--;
					}

					if (rope[head].Y >= rope[tail].Y + 2)
					{
						rope[tail].Y = rope[head].Y - 1;
						if (rope[tail].X < rope[head].X)
							rope[tail].X++;
						if (rope[tail].X > rope[head].X)
							rope[tail].X--;
					}

					if (rope[head].Y <= rope[tail].Y - 2)
					{
						rope[tail].Y = rope[head].Y + 1;
						if (rope[tail].X < rope[head].X)
							rope[tail].X++;
						if (rope[tail].X > rope[head].X)
							rope[tail].X--;
					}
				}

				var ropeTail = rope[rope.Length - 1];

				tailSpots.Add($"{ropeTail.X},{ropeTail.Y}");
			}
			output.AppendLine(string.Join(", ", rope));
		});

		foreach (var spot in tailSpots.ToArray().OrderBy(s => s))
		{
			output.AppendLine(spot);
		}

		output.AppendLine($"The answer is {tailSpots.Count}");

		return output.ToString();
	}
}
