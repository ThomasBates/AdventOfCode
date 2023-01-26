using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Text.RegularExpressions;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day21 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 21;

	public string Name => $"Day 21";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day21Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day21(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day21), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day21), message);

	#endregion Helpers

	private class Data
	{
		public Dictionary<string, string> Rules = new();
	}

	private Data LoadData(string input)
	{
		var data = new Data();

		InputHelper.TraverseInputLines(input, line =>
		{
			var match = Regex.Match(line, @"([.#/]+) => ([.#/]+)");
			if (!match.Success)
			{
				logger.SendError(nameof(Day21), $"Cannot match line: {line}");
				return;
			}

			var keys = GetKeyPermutations(match.Groups[1].Value);
			var value = match.Groups[2].Value;

			foreach (var key in keys)
				data.Rules[key] = value;
		});

		foreach (var rule in data.Rules)
			SendVerbose($"{rule}");

		return data;

		static IEnumerable<string> GetKeyPermutations(string key)
		{
			var keys = new HashSet<string> { key };
			key = RotateKey(key);
			keys.Add(key);
			key = RotateKey(key);
			keys.Add(key);
			key = RotateKey(key);
			keys.Add(key);
			key = FlipKey(key);
			keys.Add(key);
			key = RotateKey(key);
			keys.Add(key);
			key = RotateKey(key);
			keys.Add(key);
			key = RotateKey(key);
			keys.Add(key);
			return keys;
		}
		static string RotateKey(string key)
		{
			if (key.Length == 5)
				return $"{key[3]}{key[0]}/{key[4]}{key[1]}";
			if (key.Length == 11)
				return $"{key[8]}{key[4]}{key[0]}/{key[9]}{key[5]}{key[1]}/{key[10]}{key[6]}{key[2]}";
			return key;
		}
		static string FlipKey(string key)
		{
			if (key.Length == 5)
				return $"{key.Substring(3, 2)}/{key.Substring(0, 2)}";
			if (key.Length == 11)
				return $"{key.Substring(8, 3)}/{key.Substring(4, 3)}/{key.Substring(0, 3)}";
			return key;
		}
	}

	private int SolvePart1(Data data)
	{
		return GenerateArt(data, 5);
	}

	private object SolvePart2(Data data)
	{
		return GenerateArt(data, 18);
	}

	private int GenerateArt(Data data, int iterationCount)
	{
		char[,] grid = new char[3, 3]
		{
			{'.','#','.'},
			{'.','.','#'},
			{'#','#','#'},
		};
		var gridSize = 3;

		var count = CountPixels();

		VisualizeGrid(0);

		for (var i = 0; i < iterationCount; i++)
		{
			var divSize = (gridSize % 2 == 0) ? 2 : 3;
			var newSize = divSize + 1;
			var div = gridSize / divSize;

			SendVerbose($"{i + 1}: {gridSize} ({div} x {divSize}) => {div * newSize} ({div} x {newSize})");

			var newGrid = new char[div * newSize, div * newSize];
			for (var divX = 0; divX < div; divX++)
			{
				for (var divY = 0; divY < div; divY++)
				{
					var keyBuilder = new StringBuilder();
					for (var x = 0; x < divSize; x++)
					{
						if (keyBuilder.Length > 0)
							keyBuilder.Append("/");
						for (var y = 0; y < divSize; y++)
							keyBuilder.Append(grid[divX * divSize + x, divY * divSize + y]);
					}
					var key = keyBuilder.ToString();

					if (!data.Rules.TryGetValue(key, out var value))
						value = "-------------------";

					SendVerbose($"div ({divX,2},{divY,2}): {key} => {value}");

					for (var x = 0; x < newSize; x++)
						for (var y = 0; y < newSize; y++)
							newGrid[divX * newSize + x, divY * newSize + y] = value[x * (newSize + 1) + y];
				}
			}
			grid = newGrid;
			gridSize = div * newSize;

			count = CountPixels();

			VisualizeGrid(i + 1);
		}

		return count;

		int CountPixels()
		{
			var count = 0;
			for (var x = 0; x < gridSize; x++)
				for (var y = 0; y < gridSize; y++)
					if (grid[x, y] == '#')
						count++;
			return count;
		}
		void VisualizeGrid(int iteration)
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{iteration}: {count} pixels are on");
			for (var x = 0; x < gridSize; x++)
			{
				for (var y = 0; y < gridSize; y++)
					builder.Append(grid[x, y]);
				builder.AppendLine();
			}
			SendDebug(builder.ToString());
		}
	}
}
