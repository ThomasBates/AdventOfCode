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
	public class Day02 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2018;

		public int Day => 2;

		public string Name => $"Day {Day:00}";

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

		public Day02()
		{
			Inputs.Add("Example Inputs", Resources.Day02Inputs);
			Inputs.Add("Puzzle Inputs", "");

			Solvers.Add("Part 1", SolvePart1);
			Solvers.Add("Part 2", SolvePart2);
		}

		#endregion Constructors

		public string SolvePart1(string input)
		{
			int doubles = 0;
			int triples = 0;

			Helper.TraverseInputTokens(input, value =>
			{
				var charCount = new Dictionary<char, int>();
				foreach (char c in value)
				{
					if (!charCount.Keys.Contains(c))
					{
						charCount.Add(c, 0);
					}
					charCount[c]++;
				}

				bool hasDouble = false;
				bool hasTriple = false;

				foreach (int count in charCount.Values)
				{
					if (count == 2)
					{
						hasDouble = true;
					}
					if (count == 3)
					{
						hasTriple = true;
					}
				}

				if (hasDouble)
				{
					doubles++;
				}
				if (hasTriple)
				{
					triples++;
				}
			});

			int checksum = doubles * triples;

			return $"The checksum is {checksum}.";
		}

		public string SolvePart2(string input)
		{
			var boxIDs = new List<string>();

			Helper.TraverseInputTokens(input, value => boxIDs.Add(value));

			var result = new StringBuilder();

			foreach (string id1 in boxIDs)
			{
				foreach (string id2 in boxIDs)
				{
					if (String.Equals(id1, id2))
					{
						continue;
					}

					if (id1.Length != id2.Length)
					{
						continue;
					}

					StringBuilder common = new StringBuilder();
					int diffCount = 0;
					for (int i = 0; i < id1.Length; i++)
					{
						if (id1[i] == id2[i])
						{
							common.Append(id1[i]);
						}
						else
						{
							diffCount++;
							if (diffCount > 1)
							{
								break;
							}
						}
					}

					if (diffCount == 1)
					{
						result.AppendLine($"The common letters are {common}.");
					}
				}
			}

			if (result.Length == 0)
			{
				result.AppendLine("Can't find the boxes, boss.");
			}

			return result.ToString();
		}
	}
}
