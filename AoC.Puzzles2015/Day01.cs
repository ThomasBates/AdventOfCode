using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AoC.Common;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day01 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 01;

	public string Name => $"Day 01";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day01Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day01(ILogger logger)
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
		string result = "";
		foreach(var line in lines)
		{
			var up = line.Count(c => c == '(');
			var dn = line.Count(c => c == ')');
			result = (up - dn).ToString();
			logger.Send(SeverityLevel.Info, nameof(Day01), line);
			logger.Send(SeverityLevel.Info, nameof(Day01), result);
		}

		return result;
	}

	private string ProcessDataForPart2()
	{
		string result = "";
		foreach (var line in lines)
		{
			int level = 0;
			for (int i = 0; i < line.Count(); i++)
			{
				char c = line[i];
				level += c switch 
				{
					'(' => 1,
					')' => -1 ,
					_ => throw new ArgumentOutOfRangeException(nameof(level))
				};
				if (level < 0)
				{
					result = (i+1).ToString();
					break;
				}
			}

			logger.Send(SeverityLevel.Info, nameof(Day01), line);
			logger.Send(SeverityLevel.Info, nameof(Day01), result);
		}

		return result;
	}
}
