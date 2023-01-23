using System;
using System.Collections.Generic;

namespace AoC.Common.Helpers;

public class PathfindingHelper
{
	public static IEnumerable<TNode> FindPath<TNode>(
		TNode origin,
		TNode target,
		Func<TNode, IEnumerable<TNode>> getNeighbors,
		Func<TNode, TNode, double> getDistance,
		Action<TNode, double> setDistance = null)
	{
		var unvisited = new HashSet<TNode>();
		var visited = new HashSet<TNode>();
		var distance = new Dictionary<TNode, double>();
		var prev = new Dictionary<TNode, TNode>();

		unvisited.Add(origin);
		distance[origin] = 0;
		TNode current = origin;

		while (true)
		{
			foreach (var neighbor in getNeighbors(current))
			{
				if (visited.Contains(neighbor))
					continue;

				if (!distance.TryGetValue(neighbor, out var neighborDistance))
				{
					neighborDistance = double.MaxValue;
					distance[neighbor] = neighborDistance;
					unvisited.Add(neighbor);
				}

				double newDistance = distance[current] + getDistance(current, neighbor);

				if (newDistance < neighborDistance)
				{
					distance[neighbor] = newDistance;
					prev[neighbor] = current;
				}
			}

			unvisited.Remove(current);
			visited.Add(current);
			setDistance?.Invoke(current, distance[current]);

			if (target.Equals(current))
				return GetPath(prev, origin, target);

			current = GetCurrent(unvisited, distance, out var found);

			if (!found)
				return null;
		}

		TNode GetCurrent(HashSet<TNode> unvisited, Dictionary<TNode, double> distance, out bool found)
		{
			found = false;

			double minDistance = double.MaxValue;
			TNode result = default;

			foreach (var node in unvisited)
			{
				if (distance[node] < minDistance)
				{
					minDistance = distance[node];
					result = node;
					found = true;
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
				if (source.Equals(current))
					return path;

				path.Insert(0, current);

				current = prev[current];
				if (current == null)
					return null;
			}
		}
	}
}
