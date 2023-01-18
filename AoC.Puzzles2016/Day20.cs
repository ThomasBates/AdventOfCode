using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Common.SegmentList;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day20 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 20;

	public string Name => $"Day 20";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day20Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day20(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day20), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day20), message);

	#endregion Helpers

	private List<(UInt32, UInt32)> LoadData(string input)
	{
		var data = new List<(UInt32, UInt32)>();

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split('-');
			data.Add((UInt32.Parse(parts[0]), UInt32.Parse(parts[1])));
		});
		
		return data;
	}

	private UInt32 SolvePart1(List<(UInt32, UInt32)> data)
	{
		var segmentList = SegmentData(data);

		return (UInt32)(segmentList[0].MaxMeasure + 0.5);
	}

	private int SolvePart2(List<(UInt32, UInt32)> data)
	{
		var segmentList = SegmentData(data);

		return segmentList.Count - 1;
	}

	private ISegmentList SegmentData(List<(UInt32, UInt32)> data)
	{
		ISegmentList segmentList = new MergingSegmentList();

		foreach (var (lo, hi) in data)
			segmentList.AddSegment(lo - 0.5, hi + 0.5);

		for (int i = 0; i < segmentList.Count; i++)
		{
			var segment = segmentList[i];
			LoggerSendDebug($"{segment.MinMeasure + 0.5} - {segment.MaxMeasure - 0.5}");
		}

		return segmentList;
	}
}
