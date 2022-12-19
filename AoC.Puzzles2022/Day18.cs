using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using AoC.IO;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022;

[Export(typeof(IPuzzle))]
public class Day18 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2022;

	public int Day => 18;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day18Inputs},
		{"Example Inputs (2)", Resources.Day18Inputs2},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day18()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
		Solvers.Add("Solve Part 2 (b)", SolvePart2b);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		var output = new StringBuilder();

		var voxels = new List<string>();

		LoadDataFromInput(input, voxels, output);

		ProcessDataForPart1(voxels, output);

		return output.ToString();
	}

	private string SolvePart2(string input)
	{
		var output = new StringBuilder();

		var voxels = new List<string>();

		LoadDataFromInput(input, voxels, output);

		ProcessDataForPart2(voxels, output);

		return output.ToString();
	}

	private string SolvePart2b(string input)
	{
		var output = new StringBuilder();

		var voxels = new List<string>();

		LoadDataFromInput(input, voxels, output);

		ProcessDataForPart2b(voxels, output);

		return output.ToString();
	}

	#endregion Solvers

	private void LoadDataFromInput(string input, List<string> voxels, StringBuilder output = null)
	{
		voxels.Clear();
		Helper.TraverseInputLines(input, line =>
		{
			voxels.Add(line);
		});
	}

	private void ProcessDataForPart1(List<string> voxels, StringBuilder output = null)
	{
		int surfaceArea = 0;

		foreach(var voxel in voxels)
		{
			output.AppendLine($"Testing {voxel}");

			int voxelArea = 0;

			var parts = voxel.Split(',');
			int x = int.Parse(parts[0]);
			int y = int.Parse(parts[1]);
			int z = int.Parse(parts[2]);

			voxelArea += CheckVoxel(x - 1, y, z) ? 1 : 0;
			voxelArea += CheckVoxel(x + 1, y, z) ? 1 : 0;
			voxelArea += CheckVoxel(x, y - 1, z) ? 1 : 0;
			voxelArea += CheckVoxel(x, y + 1, z) ? 1 : 0;
			voxelArea += CheckVoxel(x, y, z - 1) ? 1 : 0;
			voxelArea += CheckVoxel(x, y, z + 1) ? 1 : 0;

			output.AppendLine($"{voxel} => {voxelArea}");

			surfaceArea += voxelArea;
		}

		output.AppendLine($"Surface Area = {surfaceArea}");

		bool CheckVoxel(int x,int y,int z)
		{
			var voxel = $"{x},{y},{z}";
			var result = !voxels.Contains(voxel);
			output.AppendLine($"  {voxel} => {result}");
			return result;
		}
	}

	private class Node
	{
		public string Voxel;
		public int X; 
		public int Y;
		public int Z;
		public List<Node> Neighbors = new();
		public bool Touched;
		public bool Visited;
	}

	//  This solution is incorrect. I haven't taken the time to figure out why.
	//	The solution below, adapted from Jonathan Paulson, works.
	private void ProcessDataForPart2(List<string> voxels, StringBuilder output = null)
	{
		int minX = int.MaxValue;
		int minY=int.MaxValue;
		int minZ = int.MaxValue;
		int maxX = int.MinValue;
		int maxY = int.MinValue;
		int maxZ = int.MinValue;

		foreach (var voxel in voxels)
		{
			var parts = voxel.Split(',');
			int x = int.Parse(parts[0]);
			int y = int.Parse(parts[1]);
			int z = int.Parse(parts[2]);

			minX = Math.Min(minX, x);
			minY = Math.Min(minY, y);
			minZ = Math.Min(minZ, z);
			maxX = Math.Max(maxX,x);
			maxY = Math.Max(maxY,y);
			maxZ = Math.Max(maxZ, z);
		}

		var nodes = new List<Node>();

		for(int x=minX; x<=maxX; x++)
		{
			for (int y=minY;y<=maxY; y++)
			{
				for (int z=minZ; z<=maxZ; z++)
				{
					var voxel = $"{x},{y},{z}";
					if( !voxels.Contains(voxel))
					{
						nodes.Add(new Node() { Voxel = voxel, X = x, Y = y, Z = z });
					}
				}
			}
		}

		foreach (var node in nodes)
		{
			CheckForNeighbor(node, node.X - 1, node.Y, node.Z);
			CheckForNeighbor(node, node.X + 1, node.Y, node.Z);
			CheckForNeighbor(node, node.X, node.Y - 1, node.Z);
			CheckForNeighbor(node, node.X, node.Y + 1, node.Z);
			CheckForNeighbor(node, node.X, node.Y, node.Z - 1);
			CheckForNeighbor(node, node.X, node.Y, node.Z + 1);
		}

		nodes[0].Touched = true;

		//TouchNeighbors(nodes[0]);

		void TouchNeighbors(Node node)
		{
			node.Visited = true;
			foreach (var neighbor in node.Neighbors)
			{
				if (!neighbor.Visited)
					TouchNeighbors(neighbor);
			}
		};
		
		while (true)
		{
			var current = nodes.FirstOrDefault(n => n.Touched && !n.Visited);
			if (current == null)
				break;

			foreach (var neighbor in current.Neighbors)
			{
				if (!neighbor.Visited)
					neighbor.Touched = true;
			}

			current.Visited = true;
		}

		var bubbles = nodes.Where(n => !n.Visited);

		output.AppendLine($"{bubbles.Count()} cells in bubbles.");

		foreach (var node in bubbles)
		{
			voxels.Add(node.Voxel);
		}

		ProcessDataForPart1(voxels, output);

		void CheckForNeighbor(Node node, int x, int y, int z)
		{
			var neighbor = nodes.FirstOrDefault(n => n.X == x && n.Y == y && n.Z == z);
			if (neighbor != null) node.Neighbors.Add(neighbor);
		}
	}

	//  https://github.com/jonathanpaulson/AdventOfCode/blob/master/2022/18.py
	private void ProcessDataForPart2b(List<string> voxels, StringBuilder output = null)
	{
		var outside = new HashSet<string>();
		var inside = new HashSet<string>();

		int result = 0;

		foreach (var voxel in voxels)
		{
			var parts = voxel.Split(',');
			int x = int.Parse(parts[0]);
			int y = int.Parse(parts[1]);
			int z = int.Parse(parts[2]);

			if (ReachesOutside(x - 1, y, z)) result++;
			if (ReachesOutside(x + 1, y, z)) result++;
			if (ReachesOutside(x, y - 1, z)) result++;
			if (ReachesOutside(x, y + 1, z)) result++;
			if (ReachesOutside(x, y, z - 1)) result++;
			if (ReachesOutside(x, y, z + 1)) result++;
		}

		bool ReachesOutside(int x, int y, int z)
		{
			var voxel = $"{x},{y},{z}";

			if (outside.Contains(voxel))
				return true;
			if (inside.Contains(voxel) ) 
				return false;

			var seen = new HashSet<string>();

			var deque = new List<string>() { voxel };

			while (deque.Count > 0)
			{
				voxel = deque[0];
				deque.RemoveAt(0);

				if (voxels.Contains(voxel))
					continue;

				if (seen.Contains(voxel))
					continue;

				seen.Add(voxel);

				if (seen.Count > 5000)
				{
					foreach(var v in seen)
						outside.Add(v);
					return true;
				}
				var parts = voxel.Split(',');
				x = int.Parse(parts[0]);
				y = int.Parse(parts[1]);
				z = int.Parse(parts[2]);

				deque.Add($"{x - 1},{y},{z}");
				deque.Add($"{x + 1},{y},{z}");
				deque.Add($"{x},{y - 1},{z}");
				deque.Add($"{x},{y + 1},{z}");
				deque.Add($"{x},{y},{z - 1}");
				deque.Add($"{x},{y},{z + 1}");
			}

			foreach (var v in seen)
				inside.Add(v);

			return false;
		}

		output.AppendLine($"The answer is {result}");
	}
}
