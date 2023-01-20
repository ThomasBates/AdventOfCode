using System;
using System.Collections.Generic;
using System.Drawing;

namespace AoC.Common.Helpers;

public class PathfindingHelper
{
	public static IEnumerable<TNode> FindPath<TNode>(
		TNode origin,
		TNode target,
		Func<TNode, IEnumerable<TNode>> getNeighbors,
		Func<TNode, TNode, double> getDistance,
		Action<TNode, double> setDistance = null)
		where TNode : class
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

			if (current == target)
				return GetPath(prev, origin, target);

			current = GetCurrent(unvisited, distance);

			if (current == null)
				return null;
		}

		TNode GetCurrent(HashSet<TNode> unvisited, Dictionary<TNode, double> distance)
		{
			double minDistance = double.MaxValue;
			TNode result = null;

			foreach (var node in unvisited)
			{
				if (distance[node] < minDistance)
				{
					minDistance = distance[node];
					result = node;
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
					return path;

				path.Insert(0, current);

				current = prev[current];
				if (current == null)
					return null;
			}
		}
	}

	public static IEnumerable<Point> FindPath(
		Point origin,
		Point target,
		Func<Point, IEnumerable<Point>> getNeighbors,
		Func<Point, Point, double> getDistance,
		Action<Point, double> setDistance = null)
	{
		var unvisited = new HashSet<Point>();
		var visited = new HashSet<Point>();
		var distance = new Dictionary<Point, double>();
		var prev = new Dictionary<Point, Point>();

		unvisited.Add(origin);
		distance[origin] = 0;
		Point? current = origin;

		while (true)
		{
			foreach (var neighbor in getNeighbors(current.Value))
			{
				if (visited.Contains(neighbor))
					continue;

				if (!distance.TryGetValue(neighbor, out var neighborDistance))
				{
					neighborDistance = double.MaxValue;
					distance[neighbor] = neighborDistance;
					unvisited.Add(neighbor);
				}

				double newDistance = distance[current.Value] + getDistance(current.Value, neighbor);

				if (newDistance < neighborDistance)
				{
					distance[neighbor] = newDistance;
					prev[neighbor] = current.Value;
				}
			}

			unvisited.Remove(current.Value);
			visited.Add(current.Value);
			setDistance?.Invoke(current.Value, distance[current.Value]);

			if (current == target)
				return GetPath(prev, origin, target);

			current = GetCurrent(unvisited, distance);

			if (current == null)
				return null;
		}

		Point? GetCurrent(HashSet<Point> unvisited, Dictionary<Point, double> distance)
		{
			double minDistance = double.MaxValue;
			Point? result = null;

			foreach (var node in unvisited)
			{
				if (distance[node] < minDistance)
				{
					minDistance = distance[node];
					result = node;
				}
			}

			return result;
		}

		List<Point> GetPath(Dictionary<Point, Point> prev, Point source, Point target)
		{
			var path = new List<Point>();
			Point current = target;

			while (true)
			{
				if (current == source)
					return path;

				path.Insert(0, current);

				current = prev[current];
				if (current == null)
					return null;
			}
		}
	}
}
