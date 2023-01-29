using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day12 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 12;

	public string Name => $"Day 12";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day12Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day12(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day12), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day12), message);

	#endregion Helpers

	private Dictionary<int, List<int>> LoadData(string input)
	{
		var data = new Dictionary<int, List<int>>();

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
			var id = int.Parse(parts[0]);
			data.Add(id, new());
			for (int i=2;i<parts.Length;i++)
				data[id].Add(int.Parse(parts[i]));
		});
		
		return data;
	}

	private int SolvePart1(Dictionary<int, List<int>> data)
	{
		var source = 0;
		var count = 0;
		foreach (var target in data.Keys)
		{
			var path = PathfindingHelper.FindPath(source, target,
				getNeighbors: (current) => data[current]);

			if (path != null)
				count++;
		}
		return count;
	}

	private object SolvePart2(Dictionary<int, List<int>> data)
	{
		var group = 0;
		var available = data.Keys.ToList();
		while (available.Any())
		{
			group++;

			var source = available.First();
			available.Remove(source);
			foreach (var target in available.ToList())
			{
				var path = PathfindingHelper.FindPath(source, target,
					getNeighbors: (current) => data[current]);

				if (path != null)
					available.Remove(target);
			}
		}
		return group;
	}
}
