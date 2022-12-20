using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

using AoC.Common;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day15 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2022;

		public int Day => 15;

		public string Name => $"Day {Day:00}";

		public Dictionary<string, string> Inputs { get; } = new()
		{
			{"Example Inputs", Resources.Day15Inputs},
			{"Puzzle Inputs",  ""}
		};

		public Dictionary<string, Func<string, string>> Solvers { get; } = new();

		#endregion IPuzzle Properties

		#region Constructors

		public Day15()
		{
			Solvers.Add("Solve Part 1", SolvePart1);
			Solvers.Add("Solve Part 2", SolvePart2);
		}

		#endregion Constructors

		#region SegmentList

		private class Segment
		{
			public int Min;
			public int Max;

			public Segment(int min, int max)
			{
				Min = min;
				Max = max;
			}

			public static int Compare(Segment a, Segment b)
			{
				if (a.Min < b.Min)
				{
					return -1;
				}
				if (a.Min > b.Min)
				{
					return 1;
				}
				return 0;
			}
		}

		private class SegmentList
		{
			private readonly List<Segment> _segmentList = new();

			public int Count => _segmentList.Count;

			public Segment this[int index]
			{
				get => _segmentList[index];
				set => _segmentList[index] = value;
			}

			public void Clear()
			{
				_segmentList.Clear();
			}

			public void AddSegment(int min, int max)
			{
				Segment segment1 = null;
				Segment segment2 = null;

				if (max < min)
					(max, min) = (min, max);

				int segmentIndex = 0;

				while (segmentIndex < _segmentList.Count)
				{
					var segment = _segmentList[segmentIndex];
					if ((segment.Min <= min) && (min <= segment.Max + 1))
					{
						segment1 = segment;
					}
					if ((segment.Min - 1 <= max) && (max <= segment.Max))
					{
						segment2 = segment;
					}

					if ((min < segment.Min) && (segment.Max < max)) //	segment inside minMeasure and maxMeasure
					{
						_segmentList.RemoveAt(segmentIndex);
					}
					else
					{
						segmentIndex++;
					}
				}

				if ((segment1 == null) && (segment2 == null))   //	no overlap. Create new segment.
				{
					var segment = new Segment(min, max);
					_segmentList.Add(segment);
				}
				else if (segment2 == null)  //	minMeasure in existing segment. Extend Segment.
				{
					segment1.Max = max;
				}
				else if (segment1 == null)  //	maxMeasure in existing segment. Extend segment.
				{
					segment2.Min = min;
				}
				else if (segment1 != segment2)  //	minMeasure and maxMeasure in different segments. Merge segments.
				{
					segment1.Max = segment2.Max;
					_segmentList.Remove(segment2);
				}
				//  else  //  minMeasure and maxMeasure both in same segment.  Ignore.

				_segmentList.Sort(Segment.Compare);

			}
		}

		#endregion SegmentList

		private class Sensor
		{
			public Point Location;
			public Point Beacon;
			public int Radius;
		}

		private readonly List<Sensor> sensors = new();

		private string SolvePart1(string input)
		{
			var output = new StringBuilder();

			LoadDataFromInput(input, output);

			int row = string.Equals(input, Inputs["Example Inputs"]) ? 10 : 2000000;

			ProcessDataForPart1(row, output);

			return output.ToString();
		}

		private string SolvePart2(string input)
		{
			var output = new StringBuilder();

			LoadDataFromInput(input, output);

			int max = string.Equals(input, Inputs["Example Inputs"]) ? 20 : 4000000;

			ProcessDataForPart2(0, max, output);

			return output.ToString();
		}

		private void LoadDataFromInput(string input, StringBuilder output)
		{
			sensors.Clear();

			Helper.TraverseInputLines(input, line =>
			{
				Match match = Regex.Match(line, @"Sensor at x=(\-?\d+), y=(\-?\d+): closest beacon is at x=(\-?\d+), y=(\-?\d+)");

				var sensor = new Sensor
				{
					Location = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
					Beacon = new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))
				};
				sensor.Radius = Math.Abs(sensor.Location.X - sensor.Beacon.X) +
								Math.Abs(sensor.Location.Y - sensor.Beacon.Y);
				sensors.Add(sensor);

				output.AppendLine($"Sensor: {sensor.Location}, Beacon: {sensor.Beacon}, Radius: {sensor.Radius}");
			});
		}

		private void ProcessDataForPart1(int row, StringBuilder output)
		{
			var segmentList = BuildSegmentList(row, output);

			var beacons = new HashSet<Point>();
			foreach (Sensor sensor in sensors)
			{
				if (sensor.Beacon.Y == row)
					beacons.Add(sensor.Beacon);
			}

			var total = 0;
			for (int i=0; i<segmentList.Count; i++)
			{
				var segment = segmentList[i];
				total += segment.Max - segment.Min + 1;
			}
			total -= beacons.Count;

			output?.AppendLine();
			output?.AppendLine($"For row {row}: Total = {total} (beacons = {beacons.Count})");
			output?.AppendLine();
		}

		private void ProcessDataForPart2(int min, int max, StringBuilder output)
		{
			Point? found = null;

			for (int row = 0; row <= max && found == null; row++)
			{
				var segmentList = BuildSegmentList(row, null);

				Segment lastSegment = null;
				for (int i = 0; i < segmentList.Count; i++)
				{
					var segment = segmentList[i];

					if (segment.Max < min) continue;
					if (segment.Min > max) break;

					if (lastSegment == null)
					{
						if (segment.Min > min)
						{
							found = new Point(segment.Min - 1, row);
							break;
						}
					}
					else
					{
						if (segment.Min > lastSegment.Max + 1)
						{
							found = new Point(segment.Min - 1, row);
							break;
						}
					}

					lastSegment = segment;
				}

				if (lastSegment.Max < max)
					found = new Point(lastSegment.Max + 1, row);
			}

			if (found.HasValue)
			{
				output.AppendLine(found.Value.ToString());
				output.AppendLine($"frequency = {found.Value.X * 4000000L + found.Value.Y}");
			}
		}

		private SegmentList BuildSegmentList(int row, StringBuilder output)
		{
			var segmentList = new SegmentList();

			foreach (Sensor sensor in sensors)
			{
				int delta = (sensor.Radius - Math.Abs(sensor.Location.Y - row));
				if (delta < 0)
				{
					output?.AppendLine($"Sensor at {sensor.Location}: skipping");
					continue;
				}

				int minX = sensor.Location.X - delta;
				int maxX = sensor.Location.X + delta;

				segmentList.AddSegment(minX, maxX);
				output?.AppendLine($"Sensor at {sensor.Location}: adding segment {minX} to {maxX}");
			}

			return segmentList;
		}
	}
}
