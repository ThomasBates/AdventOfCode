using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day19 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 19;

	public string Name => $"Day 19";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day19Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day19(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2a (BFS)", SolvePart2BFS);
		Solvers.Add("Solve Part 2b (DFS)", SolvePart2DFS);
		Solvers.Add("Solve Part 2c (Clever)", SolvePart2Clever);
		Solvers.Add("Solve Part 2d", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart1(data);

		return result.ToString();
	}

	private string SolvePart2BFS(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart2BreadthFirst(data);

		return result.ToString();
	}

	private string SolvePart2DFS(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart2DepthFirst(data);

		return result.ToString();
	}

	private string SolvePart2Clever(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart2Clever(data);

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart2(data);

		return result.ToString();
	}

	#endregion Solvers

	private (List<(string, string)>, string) LoadDataFromInput(string input)
	{
		//  First Clear Data
		var replacements = new List<(string, string)>();
		var molecule = "";

		bool replacementsDone = false;
		InputHelper.TraverseInputLines(input, line =>
		{
			if (string.IsNullOrEmpty(line))
			{
				replacementsDone = true;
				return;
			}

			if (!replacementsDone)
			{
				var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				replacements.Add((parts[0], parts[2]));
			}
			else
			{
				molecule = line;
			}
		}, false);

		foreach (var (from, to) in replacements.OrderBy(r => r.Item2))
			logger.SendVerbose(nameof(Day19), $"{from} => {to}");

		return (replacements, molecule);
	}

	private int ProcessDataForPart1((List<(string, string)>, string) data)
	{
		var (replacements, molecule) = data;

		var distinct = new HashSet<string>();

		foreach (var (from, to) in replacements)
		{
			int startIndex = 0;
			var index = molecule.IndexOf(from, startIndex);

			while (index >= 0 && startIndex < molecule.Length - 1)
			{
				var modified = $"{molecule.Substring(0, index)}{to}{molecule.Substring(index + from.Length)}";
				distinct.Add(modified);
				logger.SendDebug(nameof(Day19), modified);

				startIndex = index + from.Length;
				index = molecule.IndexOf(from, startIndex);
			}
		}
		return distinct.Count;
	}

	//  Too much memory
	private int ProcessDataForPart2BreadthFirst((List<(string, string)>, string) data)
	{
		var (replacements, targetMolecule) = data;

		var (bestPath, _) = BreadthFirst(replacements, targetMolecule);

		return bestPath.Count;
	}

	//  Too much time
	private int ProcessDataForPart2DepthFirst((List<(string, string)>, string) data)
	{
		var (replacements, targetMolecule) = data;

		var (bestPath, _) = DepthFirst(replacements, targetMolecule);

		return bestPath.Count;
	}

	//  Too clever
	private int ProcessDataForPart2Clever((List<(string, string)>, string) data)
	{
		var (replacements, targetMolecule) = data;

		var molecule = targetMolecule;

		var fullPath = new List<(string, string)>();

		while (true)
		{
			var arIndex = molecule.IndexOf("Ar");
			if (arIndex < 0)
			{
				var (finalPath, finalSegment) = DepthFirst(replacements, molecule);
				fullPath.AddRange(finalPath);

				logger.SendDebug(nameof(Day19), $"Final: {molecule} => {finalSegment}");

				return fullPath.Count;
			}

			var rnIndex = molecule.LastIndexOf("Rn", arIndex);
			var preIndex = molecule.LastIndexOf("Rn", rnIndex - 1);

			var segment = preIndex < 0
				? molecule.Substring(0, arIndex + 2)
				: molecule.Substring(preIndex + 2, arIndex - preIndex);

			var (path, newSegment) = DepthFirst(replacements, segment);
			fullPath.AddRange(path);

			logger.SendDebug(nameof(Day19), $"{segment} => {newSegment}");

			var newMolecule = preIndex < 0
				? $"{newSegment}{molecule.Substring(arIndex + 2)}"
				: $"{molecule.Substring(0, preIndex + 2)}{newSegment}{molecule.Substring(arIndex + 2)}";

			logger.SendDebug(nameof(Day19), $"{molecule} =>");
			logger.SendDebug(nameof(Day19), $"{newMolecule}");

			molecule = newMolecule;
		}
	}

	//  https://www.reddit.com/r/adventofcode/comments/3xflz8/comment/cy4h7ji/?utm_source=share&utm_medium=web2x&context=3
	private int ProcessDataForPart2((List<(string, string)>, string) data)
	{
		var (_, targetMolecule) = data;

		var totalCount = targetMolecule.Count(char.IsUpper);
		var rnCount = countStr("Rn");
		var arCount = countStr("Ar");
		var yCount = countStr("Y");
		var totalSteps = totalCount - rnCount - arCount - 2*yCount - 1;

		logger.SendDebug(nameof(Day19), $"{totalCount} - {rnCount} - {arCount} - 2*{yCount} - 1 = {totalSteps}");

		return totalSteps;

		int countStr(string x)
		{
			var count = 0;
			var index = targetMolecule.IndexOf(x);
			while (index >= 0)
			{
				count++;
				index = targetMolecule.IndexOf(x, index + 1);
			}
			return count;
		}
	}

	private (List<(string, string)>, string) BreadthFirst(List<(string, string)> replacements, string targetMolecule)
	{
		var holding = new List<(List<(string, string)>, string)>
		{
			(new List<(string, string)>(), targetMolecule)
		};

		while (holding.Count > 0)
		{
			var (path, molecule) = holding[0];
			holding.RemoveAt(0);

			var found = false;
			foreach (var (from, to) in replacements.OrderBy(r => r.Item2))
			{
				if (from == "e")
				{
					if (to == molecule)
					{
						var modified = from;
						var newPath = new List<(string, string)>(path) { (from, to) };
						logger.SendVerbose(nameof(Day19), $"{from} => {to}: e => {molecule}");
						return (newPath, modified);
					}
					continue;
				}

				int startIndex = 0;
				var index = molecule.IndexOf(to, startIndex);

				while (index >= 0 && startIndex < molecule.Length - 1)
				{
					var modified = $"{molecule.Substring(0, index)}{from}{molecule.Substring(index + to.Length)}";
					var newPath = new List<(string, string)>(path) { (from, to) };

					logger.SendVerbose(nameof(Day19), $"{from} => {to}: {modified} => {molecule}");

					holding.Add((newPath, modified));
					found = true;

					startIndex = index + to.Length;
					index = molecule.IndexOf(to, startIndex);
				}
			}

			if (!found)
				return (path, molecule);
		}

		return (new List<(string, string)>(), targetMolecule);
	}

	private (List<(string, string)>, string) DepthFirst(List<(string, string)> replacements, string targetMolecule)
	{
		List<(string, string)> bestPath = null;
		string bestMolecule = "";

		DoReplacements(new List<(string, string)>(), targetMolecule);

		return (bestPath, bestMolecule);

		void DoReplacements(List<(string, string)> path, string molecule)
		{
			if (bestPath != null && path.Count >= bestPath.Count - 1)
				return;

			var found = false;
			foreach (var (from, to) in replacements.OrderBy(r => r.Item2))
			{
				if (from == "e")
				{
					if (to == molecule)
					{
						var modified = from;
						var newPath = new List<(string, string)>(path) { (from, to) };
						logger.SendVerbose(nameof(Day19), $"{from} => {to}: e => {molecule}");
						if (bestPath == null || bestPath.Count > newPath.Count)
						{
							bestPath = newPath;
							bestMolecule = modified;
						}
					}
					continue;
				}

				int startIndex = 0;
				var index = molecule.IndexOf(to, startIndex);

				while (index >= 0 && startIndex < molecule.Length - 1)
				{
					var modified = $"{molecule.Substring(0, index)}{from}{molecule.Substring(index + to.Length)}";
					var newPath = new List<(string, string)>(path) { (from, to) };

					logger.SendVerbose(nameof(Day19), $"{from} => {to}: {modified} => {molecule}");

					DoReplacements(newPath, modified);
					found = true;

					startIndex = index + to.Length;
					index = molecule.IndexOf(to, startIndex);
				}
			}
			if (!found)
			{
				bestPath = path;
				bestMolecule = molecule;
			}
		}
	}
}
