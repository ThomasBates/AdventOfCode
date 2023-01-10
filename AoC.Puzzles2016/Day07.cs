using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day07 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 07;

	public string Name => $"Day 07";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs (part 1)", Resources.Day07Inputs01},
		{"Example Inputs (part 2)", Resources.Day07Inputs02},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day07(ILogger logger)
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

	private List<string> LoadDataFromInput(string input)
	{
		var data = new List<string>();

		InputHelper.TraverseInputLines(input, line =>
		{
			data.Add(line);
		});
		
		return data;
	}

	private int ProcessDataForPart1(List<string> data)
	{
		int count = 0;

		foreach(var line in data)
		{
			var valid = false;
			var sequences = line.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < sequences.Length; i++)
			{
				var hypernet = i % 2 == 1;
				var abbaFound = FindABBA(sequences[i]);
				if (abbaFound)
				{
					if (!hypernet)
					{
						valid = true;
					}
					else
					{
						valid = false;
						break;
					}
				}
			}
			if (valid)
				count++;

			logger.SendDebug(nameof(Day07), $"{(valid?"TLS":"   ")} - {line}");
		}

		return count;
	}

	private bool FindABBA(string sequence)
	{
		for (int i = 3; i < sequence.Length; i++)
		{
			if (sequence[i] != sequence[i - 1] &&
				sequence[i] == sequence[i - 3] &&
				sequence[i - 1] == sequence[i - 2])
			{
				return true;
			}
		}
		return false;
	}

	private int ProcessDataForPart2(List<string> data)
	{
		int count = 0;

		foreach (var line in data)
		{
			var supernetABAs = new HashSet<string>();
			var hypernetABAs = new HashSet<string>();

			var sequences = line.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < sequences.Length; i++)
			{
				var hypernet = i % 2 == 1;
				var abaList = FindABAs(sequences[i]);
				foreach(var aba in abaList)
				{
					if (hypernet)
						hypernetABAs.Add(aba.Substring(1));
					else
						supernetABAs.Add(aba.Substring(0, 2));
				}
			}
			var join = supernetABAs.Intersect(hypernetABAs);
			var valid = join.Any();
			if (valid)
				count++;

			logger.SendDebug(nameof(Day07), $"{(valid ? "SSL" : "   ")} - S({string.Join(",", supernetABAs)}), H({string.Join(",", hypernetABAs)}) - {line}");
		}

		return count;
	}

	private List<string> FindABAs(string sequence)
	{
		var result = new List<string>();

		for (int i = 0; i < sequence.Length - 2; i++)
		{
			if (sequence[i] != sequence[i + 1] &&
				sequence[i] == sequence[i + 2])
			{
				result.Add(sequence.Substring(i, 3));
			}
		}
		return result;
	}
}
