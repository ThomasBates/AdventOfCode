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
	public class Day07Take2 : IPuzzle
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

		public Day07Take2()
		{
			Name = "Day 07(2)";

			Inputs.Add("Sample Inputs", Resources.Day07SampleInputs);
			Inputs.Add("Puzzle Inputs", Resources.Day07PuzzleInputs);

			Solvers.Add("Solve Part 1", SolvePart1);
		}

		#endregion Constructors

		private enum Mark
		{
			None,
			Temporary,
			Permanent
		}
		private class Node
		{
			public string Name;
			public Mark Mark;
		}
		private class Edge
		{
			public Node From;
			public Node To;
		}

		public string SolvePart1(string input)
		{
			var result = new StringBuilder();

			result.AppendLine(SolvePuzzle_BottomUp(input));
			result.AppendLine(SolvePuzzle_TopologicalSort_KahnsAlgorithm(input));
			result.AppendLine(SolvePuzzle_TopologicalSort_DepthFirstSearch(input));

			return result.ToString();
		}

		private void GetTopologyFromInput(string input, List<Node> nodes, List<Edge> edges)
		{
			Helper.TraverseInputLines(input, line =>
			{
				Match match = Regex.Match(line, "Step ([A-Z]) must be finished before step ([A-Z]) can begin\\.");
				string step1 = match.Groups[1].Value;
				string step2 = match.Groups[2].Value;

				Node node1 = nodes.Where(n => String.Equals(n.Name, step1)).FirstOrDefault();
				Node node2 = nodes.Where(n => String.Equals(n.Name, step2)).FirstOrDefault();
				if (node1 == null)
				{
					node1 = new Node { Name = step1 };
					nodes.Add(node1);
				}
				if (node2 == null)
				{
					node2 = new Node { Name = step2 };
					nodes.Add(node2);
				}
				edges.Add(new Edge { From = node2, To = node1 });
			});
		}

		public string SolvePuzzle_BottomUp(string input)
		{
			var nodes = new List<Node>();
			var edges = new List<Edge>();

			GetTopologyFromInput(input, nodes, edges);

			var L = new List<Node>();

			while (true)
			{
				var node = nodes
					.Where(n => !edges.Any(e => e.From == n))
					.OrderBy(n => n.Name)
					.FirstOrDefault();
				if (node == null)
				{
					break;
				}

				L.Add(node);
				nodes.Remove(node);

				foreach (var edge in edges.Where(e => e.To == node).ToList())
				{
					edges.Remove(edge);
				}
			}

			var order = String.Join("", L.Select(n => n.Name).ToArray());
			return $"The step order is {order} (Bottom Up).";
		}

		public string SolvePuzzle_TopologicalSort_KahnsAlgorithm(string input)
		{
			var nodes = new List<Node>();
			var edges = new List<Edge>();

			GetTopologyFromInput(input, nodes, edges);

			//	L ← Empty list that will contain the sorted elements
			var L = new List<Node>();
			//	S ← Set of all nodes with no incoming edge
			var S = nodes.Where(n => !edges.Any(e => e.To == n)).ToList();

			//	while S is non-empty do
			while (S.Count > 0)
			{
				//	remove a node n from S
				Node n = S[0];
				S.RemoveAt(0);
				//	add n to tail of L
				L.Insert(0, n);
				//	for each node m with an edge e from n to m do
				//foreach (var edge in edges.Where(e => e.From == n).ToList())
				foreach (var edge in edges.Where(e => e.From == n).OrderBy(e => e.To.Name))
				{
					Node m = edge.To;
					//	remove edge e from the graph
					edges.Remove(edge);

					//	if m has no other incoming edges then
					if (!edges.Any(e => e.To == m))
					{
						//	insert m into S
						S.Insert(0, m);
					}
				}
			}

			//	if graph has edges then
			if (edges.Count > 0)
			{
				//	return error(graph has at least one cycle)
				throw new InvalidOperationException($"Circular Dependency.");
			}
			//	else 
			//		return L(a topologically sorted order)
			var order = String.Join("", L.Select(n => n.Name).ToArray());
			return $"The step order is {order} (Kahn's Algorithm).";
		}

		public string SolvePuzzle_TopologicalSort_DepthFirstSearch(string input)
		{
			var nodes = new List<Node>();
			var edges = new List<Edge>();

			GetTopologyFromInput(input, nodes, edges);

			//	L ← Empty list that will contain the sorted nodes
			var L = new List<Node>();
			//	while there are unmarked nodes do
			while (nodes.Any(n => n.Mark == Mark.None))
			{
				//	select an unmarked node n
				//Node node = nodes.Where(n => n.Mark == Mark.None).First();
				Node node = nodes.Where(n => n.Mark == Mark.None).OrderBy(n => n.Name).First();
				//	visit(n)
				Visit(node, nodes, edges, L);
			}

			var order = String.Join("", L.Select(n => n.Name).ToArray());
			return $"The step order is {order} (Depth First Search).";
		}

		private void Visit(Node n, List<Node> nodes, List<Edge> edges, List<Node> L)
		{
			//	if n has a permanent mark then return
			if (n.Mark == Mark.Permanent)
			{
				return;
			}
			//	if n has a temporary mark then stop   (not a DAG)
			if (n.Mark == Mark.Temporary)
			{
				throw new InvalidOperationException($"Circular Dependency.");
			}
			//	mark n temporarily
			n.Mark = Mark.Temporary;
			//	for each node m with an edge from n to m do
			//foreach (var edge in edges.Where(e => e.From == n).ToList())
			foreach (var edge in edges.Where(e => e.From == n).OrderBy(e => e.To.Name))
			{
				Node m = edge.To;
				//	visit(m)
				Visit(m, nodes, edges, L);
			}
			//	mark n permanently
			n.Mark = Mark.Permanent;
			//	add n to head of L
			L.Add(n);
		}
	}
}
