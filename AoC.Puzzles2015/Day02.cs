using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day02 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 02;

	public string Name => $"Day 02";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day02Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day02(ILogger logger)
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

		Helper.TraverseInputLines(input, line =>
		{
			lines.Add(line);
		});
	}

	private string ProcessDataForPart1()
	{
		int result = 0;
		foreach (var line in lines)
		{
			var parts = line.Split(new[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);
			var l = int.Parse(parts[0]);
			var w = int.Parse(parts[1]);
			var h = int.Parse(parts[2]);
			var a =
				2 * l * w +
				2 * w * h +
				2 * h * l +
				l * w * h / Math.Max(Math.Max(l, w), h);

			result += a;
			logger.Send(SeverityLevel.Info, nameof(Day01), $"{line} => {a}");
		}

		return result.ToString();
	}

	private string ProcessDataForPart2()
	{
		int result = 0;
		foreach (var line in lines)
		{
			var parts = line.Split(new[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);
			var l = int.Parse(parts[0]);
			var w = int.Parse(parts[1]);
			var h = int.Parse(parts[2]);

			var r = 2 * (l + w + h) 
				- 2 * Math.Max(Math.Max(l, w), h) 
				+ l * w * h;

			result += r;
			logger.Send(SeverityLevel.Info, nameof(Day01), $"{line} => {r}");
		}

		return result.ToString();
	}
}
