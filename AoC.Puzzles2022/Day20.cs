using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day20 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 20;

	public string Name => $"Day {Day:00}";

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

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		LoadDataFromInput(input, 1);

		var result = DecryptFile(1);

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInput(input, 811589153);

		var result = DecryptFile(10);

		return result;
	}

	#endregion Solvers

	private readonly LinkedList<long> file = new();
	private readonly List<LinkedListNode<long>> nodes = new();

	private void LoadDataFromInput(string input, long key)
	{
		file.Clear();
		nodes.Clear();

		Helper.TraverseInputTokens(input, value =>
		{
			nodes.Add(file.AddLast(long.Parse(value) * key));
		});
	}

	private string DecryptFile(int mixCount)
	{
		logger.Send(SeverityLevel.Debug, nameof(Day20), $"File size = {file.Count}");

		if (file.Count < 100)
			logger.Send(SeverityLevel.Debug, nameof(Day20), string.Join(", ", file));

		for (int i = 0; i < mixCount; i++)
			MixFile();

		var zero = file.Find(0);
		var x = FindValueAt(zero, 1000);
		var y = FindValueAt(zero, 2000);
		var z = FindValueAt(zero, 3000);

		if (file.Count < 100)
			logger.Send(SeverityLevel.Debug, nameof(Day20), string.Join(", ", file));

		logger.Send(SeverityLevel.Debug, nameof(Day20), $"({x}, {y}, {z}) => {x + y + z}");
		return (x + y + z).ToString();
	}

	private void MixFile()
	{
		foreach (var node in nodes)
		{
			if (node.Value == 0) continue;
			if (node.Value < 0)
			{
				var count = (-node.Value) % (file.Count - 1);
				if (count > 0)
				{
					var right = node;
					for (int i = 0; i < count; i++)
						right = right.Previous ?? right.List.Last;
					file.Remove(node);
					file.AddBefore(right, node);
				}
			}
			if (node.Value > 0)
			{
				var count = node.Value % (file.Count - 1);
				if (count > 0)
				{
					var left = node;
					for (int i = 0; i < count; i++)
						left = left.Next ?? left.List.First;
					file.Remove(node);
					file.AddAfter(left, node);
				}
			}

			if (file.Count < 100)
				logger.Send(SeverityLevel.Debug, nameof(Day20), string.Join(", ", file));
		}
	}

	private long FindValueAt(LinkedListNode<long> zero, int count)
	{
		count %= file.Count;
		var node = zero;
		for (int i = 0; i < count; i++)
			node = node.Next ?? node.List.First;
		return node.Value;
	}
}
