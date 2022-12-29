using System;
using System.Collections.Generic;

namespace AoC.Common.Helpers;

public class PathfindingHelper
{
	public static IEnumerable<TNode> FindPath<TNode>(
		IEnumerable<TNode> allNodes,
		Func<TNode, IEnumerable<TNode>> getNeighbors,
		Func<TNode, TNode, double> getDistance,
		TNode source,
		TNode target)
		where TNode : class
	{
		Dictionary<TNode, bool> visited = new();
		Dictionary<TNode, double> distance = new();
		Dictionary<TNode, TNode> prev = new();

		foreach (var node in allNodes)
		{
			visited[node] = false;
			distance[node] = double.MaxValue;
			prev[node] = default;
		}

		distance[source] = 0;
		TNode current = source;

		while (true)
		{
			foreach (var neighbor in getNeighbors(current))
			{
				if (visited[neighbor])
					continue;

				double d = distance[current] + getDistance(current, neighbor);
				if (d < distance[neighbor])
				{
					distance[neighbor] = d;
					prev[neighbor] = current;
				}
			}

			visited[current] = true;

			if (current == target)
			{
				return GetPath(prev, source, target);
			}

			current = GetCurrent(visited, distance);

			if (current == null)
			{
				return null;
			}
		}

		TNode GetCurrent(Dictionary<TNode, bool> visited, Dictionary<TNode, double> distance)
		{
			double minDistance = double.MaxValue;
			TNode result = null;

			foreach (var valve in visited.Keys)
			{
				if (visited[valve])
					continue;

				if (distance[valve] < minDistance)
				{
					minDistance = distance[valve];
					result = valve;
				}
			}

			return result;
		}

		List<TNode> GetPath(Dictionary<TNode, TNode> prev, TNode source, TNode target)
		{
			var path = new List<TNode>();
			TNode current = target;

			while (true)
			{
				if (current == source)
				{
					return path;
				}

				path.Insert(0, current);

				current = prev[current];
				if (current == null)
				{
					return null;
				}
			}
		}
	}
}
