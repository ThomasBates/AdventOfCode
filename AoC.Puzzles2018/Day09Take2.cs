﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day09Take2 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 9;

	public string Name => $"Day {Day:00} (2)";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day09Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day09Take2()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
	}

	#endregion Constructors

	private class Marble
	{
		public int Value;
		public Marble Left;
		public Marble Right;
		public override string ToString()
		{
			return $"[{Left.Value}]-[{Value}]-[{Right.Value}]";
		}
	}

	public string SolvePart1(string input)
	{
		var result = new StringBuilder();

		InputHelper.TraverseInputLines(input, (Action<string>)(line =>
		{
			string[] values = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

			int playerCount = int.Parse(values[0]);
			int maxMarble = int.Parse(values[6]);
			var players = new long[playerCount];

			var currentMarble = new Marble { Value = 0 };
			currentMarble.Right = currentMarble;
			currentMarble.Left = currentMarble;

			int player = 1;
			for (int marble = 1; marble <= maxMarble; marble++)
			{
				Play(players, ref currentMarble, player, marble);

				player++;
				if (player >= playerCount)
				{
					player = 0;
				}
			}

			long score = players.Max(s => s);

			if (score == 32)
			{
				while (currentMarble.Value != 0)
				{
					currentMarble = currentMarble.Right;
				}

				result.Append($"{currentMarble.Value}  ");
				currentMarble = currentMarble.Right;

				while (currentMarble.Value != 0)
				{
					result.Append($"{currentMarble.Value}  ");
					currentMarble = currentMarble.Right;
				}
				result.AppendLine();
			}
			result.AppendLine($"High Score is {score}");

			//	Clean up.
			while (currentMarble.Right != currentMarble)
			{
				Marble left = currentMarble.Left;
				Marble right = currentMarble.Right;

				left.Right = right;
				right.Left = left;

				//	allow gc to clean it up.
				currentMarble.Left = null;
				currentMarble.Right = null;

				currentMarble = left;
			}
		}));

		return result.ToString();
	}

	private void Play(long[] players, ref Marble currentMarble, int player, int marble)
	{
		if (marble % 23 != 0)
		{
			Marble left = currentMarble.Right;
			Marble right = left.Right;

			currentMarble = new Marble
			{
				Value = marble,
				Left = left,
				Right = right
			};
			left.Right = currentMarble;
			right.Left = currentMarble;
		}
		else
		{
			players[player] += marble;

			currentMarble = currentMarble.Left.Left.Left.Left.Left.Left.Left;
			players[player] += currentMarble.Value;

			Marble left = currentMarble.Left;
			Marble right = currentMarble.Right;

			left.Right = right;
			right.Left = left;

			//	allow gc to clean it up.
			currentMarble.Left = null;
			currentMarble.Right = null;

			currentMarble = right;
		}
	}
}
