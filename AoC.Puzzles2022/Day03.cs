﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using AoC.IO;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day03 : IPuzzle
	{
		#region IPuzzle Properties

		public string Name => "Day 03";

		public Dictionary<string, string> Inputs { get; } = new Dictionary<string, string>()
		{
			{"Example Inputs", Resources.Day03ExampleInputs},
			{"Puzzle Inputs",  Resources.Day03PuzzleInputs}
		};

		public Dictionary<string, Func<string, string>> Solvers { get; } = new Dictionary<string, Func<string, string>>()
		{
			{ "Part 1", SolvePart1 },
			{ "Part 2", SolvePart2 }
		};

		#endregion IPuzzle Properties

		public static string SolvePart1(string input)
		{
			StringBuilder output = new StringBuilder();

			int total = 0;

			Helper.TraverseInputTokens(input, value =>
			{
				var a = new HashSet<char>();
				var b = new HashSet<char>();
				for (int i = 0; i < value.Length / 2; i++)
				{
					a.Add(value[i]);
				}
				for (int i = value.Length / 2; i < value.Length; i++)
				{
					b.Add(value[i]);
				}

				var c = a.Intersect(b).FirstOrDefault();

				if (c == 0)
					return;

				int priority = (c <= 'Z') ?
					27 + (int)c - (int)'A' : 
					 1 + (int)c - (int)'a';

				output.AppendLine($"{string.Join("", a)} {string.Join("", b)} - {c} {priority}");

				total += priority;
			});

			output.AppendLine($"The answer is {total}");

			return output.ToString();
		}

		public static string SolvePart2(string input)
		{
			StringBuilder output = new StringBuilder();

			int total = 0;
			var group = new List<HashSet<char>>();

			Helper.TraverseInputTokens(input, value =>
			{
				var a = new HashSet<char>();
				group.Add(a);

				for (int i = 0; i < value.Length; i++)
					a.Add(value[i]);

				output.AppendLine($"{string.Join("", a)}");

				if (group.Count == 3)
				{
					var c = group[0].Intersect(group[1]).Intersect(group[2]).FirstOrDefault();

					int priority = (c <= 'Z') ?
						27 + (int)c - (int)'A' :
						 1 + (int)c - (int)'a';

					output.AppendLine($"{c} {priority}");

					total += priority;

					group.Clear();
				}
			});

			output.AppendLine($"The answer is {total}");

			return output.ToString();
		}
	}
}
