using System;
using System.Collections.Generic;

namespace AoC.Common.Helpers;

public class PathfindingHelper
{
	//  https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
	//  https://en.wikipedia.org/wiki/A*_search_algorithm
	public static IEnumerable<TNode> FindPath<TNode>(
		TNode origin,
		TNode target,
		Func<TNode, IEnumerable<TNode>> getNeighbors,
		Func<TNode, TNode, double> getCost = null,
		Func<TNode, double> getHeuristic = null,
		Action<TNode, double> setCost = null)
	{
		var unvisited = new HashSet<TNode>();
		var visited = new HashSet<TNode>();
		var cost = new Dictionary<TNode, double>();
		var prev = new Dictionary<TNode, TNode>();
		var heuristic = new Dictionary<TNode, double>();

		unvisited.Add(origin);
		cost[origin] = 0;
		TNode current = origin;

		while (true)
		{
			foreach (var neighbor in getNeighbors(current))
			{
				if (visited.Contains(neighbor))
					continue;

				if (!cost.TryGetValue(neighbor, out var neighborCost))
				{
					neighborCost = double.MaxValue;
					cost[neighbor] = neighborCost;
					unvisited.Add(neighbor);
					heuristic[neighbor] = getHeuristic?.Invoke(neighbor) ?? 0;
				}


				var transitionCost = getCost?.Invoke(current, neighbor) ?? 1;
				double newCost = cost[current] + transitionCost;

				if (newCost < neighborCost)
				{
					cost[neighbor] = newCost;
					prev[neighbor] = current;
				}
			}

			unvisited.Remove(current);
			visited.Add(current);
			setCost?.Invoke(current, cost[current]);

			if (target.Equals(current))
				return GetPath();

			current = GetCurrent(out var found);

			if (!found)
				return null;
		}

		TNode GetCurrent(out bool found)
		{
			found = false;

			double minCost = double.MaxValue;
			TNode result = default;

			foreach (var node in unvisited)
			{
				if (cost[node] + heuristic[node] < minCost)
				{
					minCost = cost[node] + heuristic[node];
					result = node;
					found = true;
				}
			}

			return result;
		}

		List<TNode> GetPath()
		{
			var path = new List<TNode>();
			TNode current = target;

			while (true)
			{
				path.Insert(0, current);

				if (origin.Equals(current))
					return path;

				current = prev[current];
				if (current == null)
					return null;
			}
		}
	}
}
