using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AoC.IO;
using AoC.IO.SegmentList;
using AoC.Parser;
using AoC.Puzzle;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018
{
	[Export(typeof(IPuzzle))]
	public class Day09 : IPuzzle
	{
		#region IPuzzle Properties

		public string Name
		{
			get;
		}

		public Dictionary<string, string> Inputs
		{
			get;
		} = new Dictionary<string, string>();

		public Dictionary<string, Func<string, string>> Solvers
		{
			get;
		} = new Dictionary<string, Func<string, string>>();

		#endregion IPuzzle Properties

		#region Constructors

		public Day09()
		{
			Name = "Day 09";

			Inputs.Add("Sample Inputs", Resources.Day09SampleInputs);
			Inputs.Add("Puzzle Inputs", Resources.Day09PuzzleInputs);

			Solvers.Add("Solve Part 1", SolvePart1);
			Solvers.Add("Solve Part 2", SolvePart2);
		}

		#endregion Constructors

		public string SolvePart1(string input)
		{
			var result = new StringBuilder();

			Helper.TraverseInputLines(input, line =>
			{
				string[] values = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

				int playerCount = int.Parse(values[0]);
				int maxMarble = int.Parse(values[6]);
				var players = new int[playerCount];
				var marbles = new int[maxMarble];

				int player = 0;
				int currentMarble = 0;
				marbles[currentMarble] = 0;
				int ringSize = 1;
				for (int marble = 1; marble <= maxMarble; marble++)
				{
					Play(players, marbles, ref ringSize, ref currentMarble, player, marble);

					player++;
					if (player >= playerCount)
					{
						player = 0;
					}
				}


				int score = players.Max(s => s);

				result.AppendLine($"High Score is {score}");
			});

			return result.ToString();
		}

		public string SolvePart2(string input)
		{
			var result = new StringBuilder();

			Helper.TraverseInputLines(input, line =>
			{
				string[] values = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

				int playerCount = int.Parse(values[0]);
				int maxMarble = int.Parse(values[6]) * 100;
				var players = new int[playerCount];
				var marbles = new int[maxMarble];

				int player = 0;
				int currentMarble = 0;
				marbles[currentMarble] = 0;
				int ringSize = 1;
				for (int marble = 1; marble <= maxMarble; marble++)
				{
					Play(players, marbles, ref ringSize, ref currentMarble, player, marble);

					player++;
					if (player >= playerCount)
					{
						player = 0;
					}
				}


				int score = players.Max(s => s);

				result.AppendLine($"High Score is {score}");
			});

			return result.ToString();
		}

		private void Play(int[] players, int[] marbles, ref int ringSize, ref int currentMarble, int player, int marble)
		{
			if (marble % 23 != 0)
			{
				currentMarble = currentMarble + 2;
				if (currentMarble > ringSize)
				{
					currentMarble = currentMarble - ringSize;
				}

				Array.Copy(marbles, currentMarble, marbles, currentMarble + 1, ringSize - currentMarble);

				//for (int i = ringSize; i > currentMarble; i--)
				//{
				//	marbles[i] = marbles[i - 1];
				//}
				marbles[currentMarble] = marble;
				ringSize++;
			}
			else
			{
				players[player] += marble;

				currentMarble = currentMarble - 7;
				if (currentMarble < 0)
				{
					currentMarble = ringSize + currentMarble;
				}

				players[player] += marbles[currentMarble];

				Array.Copy(marbles, currentMarble + 1, marbles, currentMarble, ringSize - currentMarble);

				//for (int i = currentMarble; i < ringSize; i++)
				//{
				//	marbles[i] = marbles[i + 1];
				//}
				ringSize--;
			}
		}
	}
}
