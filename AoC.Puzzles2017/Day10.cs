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
				var hash = GetSparseHash(listSize, lengths, 1).ToArray();
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
			hash = GetKnotHash(line);
			SendDebug($"Hash    = {hash}");
		}
		return hash;
	}

	private string GetKnotHash(string line)
	{
		var lengths = new List<int>();
		foreach (var c in line)
			lengths.Add(c);
		lengths.AddRange(new[] { 17, 31, 73, 47, 23 });
		SendVerbose($"Lengths = {string.Join(",", lengths)}");

		var sparse = GetSparseHash(256, lengths, 64).ToArray();
		SendVerbose($"Sparse  = {string.Join(",", sparse)}");

		var dense = new StringBuilder();
		for (var i = 0; i < 16; i++)
		{
			var n = 0;
			for (var j = 0; j < 16; j++)
				n ^= sparse[i * 16 + j];
			dense.Append($"{n:x2}");
		}
		return dense.ToString();
	}

	private IEnumerable<int> GetSparseHash(int listSize, IEnumerable<int> lengths, int rounds)
	{
		var list = new int[listSize];
		for (var i = 0; i < listSize; i++)
			list[i] = i;
		var position = 0;
		var skipSize = 0;

		for (var round = 0; round < rounds; round++)
		{
			foreach (var length in lengths)
			{
				var newList = list.ToArray();
				for (var i = 0; i < length; i++)
				{
					var oldPos = (position + i) % listSize;
					var newPos = (position + length - i - 1) % listSize;
					newList[newPos] = list[oldPos];
				}
				list = newList;
				position = (position + length + skipSize) % listSize;
				skipSize++;
			}
		}

		return list;
	}
}
