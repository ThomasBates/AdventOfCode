using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day02 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 02;

	public string Name => $"Day 02";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs (1)", Resources.Day02Inputs01},
		{"Example Inputs (2)", Resources.Day02Inputs02},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day02(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day02), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day02), message);

	#endregion Helpers

	private List<List<int>> LoadData(string input)
	{
		var data = new List<List<int>>();


		InputHelper.TraverseInputLines(input, line =>
		{
			var row = new List<int>();
			data.Add(row);

			var cells = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var cell in cells)
				row.Add(int.Parse(cell));
		});
		
		return data;
	}

	private object SolvePart1(List<List<int>> data)
	{
		var sum = 0;

		foreach (var row in data)
		{
			var min = row.Min(v => v);
			var max = row.Max(v => v);
			sum += max - min;
		}

		return sum;
	}

	private object SolvePart2(List<List<int>> data)
	{
		var sum = 0;

		foreach (var row in data)
		{
			foreach (var m in row)
			{
				foreach (var n in row)
				{
					if (m != n && m % n == 0)
						sum += m / n;
				}
			}
		}

		return sum;
	}
}
