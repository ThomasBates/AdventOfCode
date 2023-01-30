using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Common.SegmentList.Discrete;
using AoC.Common.Types;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day23 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 23;

	public string Name => "Day 23";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs (1)", Resources.Day23Inputs01},
		{"Example Inputs (2)", Resources.Day23Inputs02},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day23(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day23), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day23), message);

	#endregion Helpers

	private class Nanobot
	{
		public Nanobot(int x, int y, int z, int r)
		{
			Position = new Point3D(x, y, z);
			Radius = r;
		}
		public Point3D Position;
		public int Radius;
		public override string ToString() => $"pos={Position}, r={Radius}";
	}

	private class Data
	{
		public List<Nanobot> Nanobots = new();
	}

	private Data LoadData(string input)
	{
		var data = new Data();

		InputHelper.TraverseInputLines(input, line =>
		{
			var match = Regex.Match(line, @"pos=<(\-?\d+),(\-?\d+),(\-?\d+)>, r=(\d+)");
			if (!match.Success)
			{
				logger.SendError(nameof(Day23), $"Cannot match line: {line}");
				return;
			}

			var x = int.Parse(match.Groups[1].Value);
			var y = int.Parse(match.Groups[2].Value);
			var z = int.Parse(match.Groups[3].Value);
			var r = int.Parse(match.Groups[4].Value);
			data.Nanobots.Add(new Nanobot(x, y, z, r));
		});

		return data;
	}

	private object SolvePart1(Data data)
	{
		var maxRadius = data.Nanobots.Max(n => n.Radius);
		var maxNanobot = data.Nanobots.FirstOrDefault(n => n.Radius == maxRadius);

		var inRange = data.Nanobots.Where(n => maxNanobot.Position.ManhattanDistance(n.Position) <= maxRadius).Count();

		return inRange;
	}

	//	From: https://www.reddit.com/r/adventofcode/comments/aa9uvg/comment/ecw0d91
	//  Note: This solution does not guarantee the correct answer,
	//  but it produced the correct answer with the provided puzzle data:
	//	See: https://www.reddit.com/r/adventofcode/comments/a8s17l/comment/ech7h4h
	private object SolvePart2(Data data)
	{
		ISegmentList segmentList = new AccumulatingSegmentList();
		segmentList.AddSegment(long.MinValue, long.MaxValue);

		foreach (var nanobot in data.Nanobots)
		{
			var size = nanobot.Position.ManhattanSize();
			segmentList.AddSegment(size - nanobot.Radius, size + nanobot.Radius, 1);
		}

		double maxValue = 0;
		ISegmentListItem maxSegment = null;

		for (var i=0;i<segmentList.Count;i++)
		{
			var segment = segmentList[i];
			if (segment.Value > maxValue)
			{
				maxValue = segment.Value;
				maxSegment = segment;
			}
		}

		return maxSegment.MinMeasure;
	}
}
