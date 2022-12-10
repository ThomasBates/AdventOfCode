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
	public class Day08 : IPuzzle
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

		public Day08()
		{
			Name = "Day 08";

			Inputs.Add("Sample Inputs", Resources.Day08SampleInputs);
			Inputs.Add("Puzzle Inputs", Resources.Day08PuzzleInputs);

			Solvers.Add("Solve Part 1", SolvePart1);
			Solvers.Add("Solve Part 2", SolvePart2);
		}

		#endregion Constructors

		private class Node
		{
			public string Name;
			public int ChildCount;
			public int MetadataCount;
			public List<Node> ChildNodes = new List<Node>();
			public List<int> Metadata = new List<int>();
		}

		public string SolvePart1(string input)
		{
			List<int> values = new List<int>();

			GetValuesFromInput(input, values);

			Node root = new Node { Name = "0" };

			ReadNode(root, values);

			int metadataSum = SumMetadata(root);

			return $"The sum of the metadata is {metadataSum}.";
		}

		public string SolvePart2(string input)
		{
			List<int> values = new List<int>();

			GetValuesFromInput(input, values);

			Node root = new Node { Name = "0" };

			ReadNode(root, values);

			int nodeValue = NodeValue(root);

			return $"The value of the root node is {nodeValue}.";
		}

		private void GetValuesFromInput(string input, List<int> values)
		{
			Helper.TraverseInputTokens(input, value =>
			{
				values.Add(int.Parse(value));
			});
		}

		int PopValue(List<int> values)
		{
			int value = values[0];
			values.RemoveAt(0);
			return value;
		}

		private void ReadNode(Node node, List<int> values)
		{
			node.ChildCount = PopValue(values);
			node.MetadataCount = PopValue(values);
			for (int childIndex = 0; childIndex < node.ChildCount; childIndex++)
			{
				Node child = new Node { Name = $"{node.Name}.{childIndex}" };
				ReadNode(child, values);
				node.ChildNodes.Add(child);
			}
			for (int metaIndex = 0; metaIndex < node.MetadataCount; metaIndex++)
			{
				node.Metadata.Add(PopValue(values));
			}
		}

		private int SumMetadata(Node node)
		{
			int sum = node.Metadata.Sum(m => m);
			foreach (var child in node.ChildNodes)
			{
				sum += SumMetadata(child);
			}
			return sum;
		}

		private int NodeValue(Node node)
		{
			int nodeValue = 0;
			foreach (int metadata in node.Metadata)
			{
				if (node.ChildCount == 0)
				{
					nodeValue += metadata;
				}
				else
				{
					int childIndex = metadata - 1;
					if (childIndex < node.ChildCount)
					{
						nodeValue += NodeValue(node.ChildNodes[childIndex]);
					}
				}
			}
			return nodeValue;
		}
	}
}
