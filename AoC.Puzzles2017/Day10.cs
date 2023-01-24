using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day10 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 10;

	public string Name => $"Day 10";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day10Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day10(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day10), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day10), message);

	#endregion Helpers

	private List<string> LoadData(string input)
	{
		var lines = new List<string>();

		InputHelper.TraverseInputLines(input, line =>
		{
			lines.Add(line);
		}, ignoreEmptyLines: false);

		return lines;
	}

	private int SolvePart1(List<string> lines)
	{
		var result = 0;
		foreach (var line in lines)
		{
			try
			{
				var lengths = new List<int>();
				var parts = line.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (var part in parts)
					lengths.Add(int.Parse(part));
				SendDebug($"Lengths = {string.Join(",", lengths)}");

				var listSize = lengths.Count < 5 ? 5 : 256;
				var hash = HashHelper.GetSparseHash(listSize, lengths, 1).ToArray();
				SendDebug($"Hash    = {string.Join(",", hash)}");

				result = hash[0] * hash[1];
				SendDebug($"Result  = {result}");
			}
			catch (Exception)
			{
				continue;
			}
		}
		return result;
	}

	private string SolvePart2(List<string> lines)
	{
		var hash = "";
		foreach (var line in lines)
		{
			SendDebug($"Line    = {line}");
			hash = HashHelper.GetKnotHash(line);
			SendDebug($"Hash    = {hash}");
		}
		return hash;
	}

}
