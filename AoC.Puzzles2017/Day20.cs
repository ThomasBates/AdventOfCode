using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media.Media3D;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day20 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 20;

	public string Name => $"Day 20";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs (1)", Resources.Day20Inputs01},
		{"Example Inputs (2)", Resources.Day20Inputs02},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day20(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day20), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day20), message);

	#endregion Helpers

	private List<(Point3D, Vector3D, Vector3D)> LoadData(string input)
	{
		var particles = new List<(Point3D, Vector3D, Vector3D)>();

		InputHelper.TraverseInputLines(input, line =>
		{
			var match = Regex.Match(line, @"p=<\s*(-?\d+),\s*(-?\d+),\s*(-?\d+)>, v=<\s*(-?\d+),\s*(-?\d+),\s*(-?\d+)>, a=<\s*(-?\d+),\s*(-?\d+),\s*(-?\d+)>");
			if (!match.Success)
			{
				logger.SendError(nameof(Day20), $"Cannot match line: {line}");
				return;
			}

			particles.Add((
				new Point3D(
					double.Parse(match.Groups[1].Value),
					double.Parse(match.Groups[2].Value),
					double.Parse(match.Groups[3].Value)),
				new Vector3D(
					double.Parse(match.Groups[4].Value),
					double.Parse(match.Groups[5].Value),
					double.Parse(match.Groups[6].Value)),
				new Vector3D(
					double.Parse(match.Groups[7].Value),
					double.Parse(match.Groups[8].Value),
					double.Parse(match.Groups[9].Value))));
		});
		
		return particles;
	}

	private int SolvePart1(List<(Point3D p, Vector3D v, Vector3D a)> particles)
	{
		var minAcceleration = particles.Min(p => Magnitude(p.a));
		var minParticle = particles.FirstOrDefault(p => Magnitude(p.a) == minAcceleration);
		var minIndex = particles.IndexOf(minParticle);

		return minIndex;
	}

	private int SolvePart2(List<(Point3D p, Vector3D v, Vector3D a)> particles)
	{
		var time = 0;

		while (true)
		{
			SendDebug($"time {time,4}: {particles.Count,4} particles");

			//	check current collisions
			var collisions = particles.GroupBy(p => p.p)
				.Where(g => g.Count() > 1)
				.Select(g => g.Key)
				.ToList();
			particles = particles.Where(p => !collisions.Contains(p.p)).ToList();

			//	check potential collisions
			var found = false;
			for (var i = 0; i < particles.Count && !found; i++)
			{
				var (p1, v1, _) = particles[i];
				for (var j = i + 1; j < particles.Count && !found; j++)
				{
					var (p2, v2, _) = particles[j];

					var d1 = Magnitude(p1 - p2);
					var d2 = Magnitude((p1 + v1) - (p2 + v2));
					if (d1 > d2)
						found = true; 
				}
			}
			if (!found)
				return particles.Count;

			//	move particles
			for (var i = 0; i < particles.Count; i++)
			{
				var (p, v, a) = particles[i];
				v += a;
				p.Offset(v.X, v.Y, v.Z);
				particles[i] = (p, v, a);
			}
			time++;
		}
	}

	private int Magnitude(Point3D p)
	{
		var magnitude = Math.Abs(p.X) + Math.Abs(p.Y) + Math.Abs(p.Z);
		return (int)magnitude;
	}

	private int Magnitude(Vector3D p)
	{
		var magnitude = Math.Abs(p.X) + Math.Abs(p.Y) + Math.Abs(p.Z);
		return (int)magnitude;
	}
}
