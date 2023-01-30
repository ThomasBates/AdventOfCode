using System;
using System.Collections;
using System.Collections.Generic;

namespace AoC.Common.SegmentList.Discrete;

public class AccumulatingSegmentList : ISegmentList
{
	private readonly List<ISegment> segments = new();

	public ISegment this[int index]
	{
		get => segments[index];
		set => segments[index] = value;
	}

	public int Count => segments.Count;

	public void Clear()
	{
		segments.Clear();
	}

	public void AddSegment(long minMeasure, long maxMeasure, long value = 0)
	{
		if (maxMeasure < minMeasure)
			(maxMeasure, minMeasure) = (minMeasure, maxMeasure);

		if (segments.Count == 0)
		{
			segments.Add(new Segment(minMeasure, maxMeasure, value));
			return;
		}
		//  Assert: at least one segment exists.
		var firstSegment = segments[0];
		var lastSegment = segments[segments.Count - 1];

		if (maxMeasure < firstSegment.MinMeasure)
		{
			segments.Insert(0, new Segment(minMeasure, maxMeasure, value));
			return;
		}
		if (minMeasure > lastSegment.MaxMeasure)
		{
			segments.Add(new Segment(minMeasure, maxMeasure, value));
			return;
		}
		//  Assert: new segment interacts with at least one existing segment.

		if (minMeasure < firstSegment.MinMeasure)
		{
			segments.Insert(0, new Segment(minMeasure, firstSegment.MinMeasure - 1, 0));
		}
		if (maxMeasure > lastSegment.MaxMeasure)
		{
			segments.Add(new Segment(lastSegment.MaxMeasure + 1, maxMeasure, 0));
		}
		//	Assert: existing segments span breadth of new segment. Could still be gaps.


		for (int itemIndex = 0; itemIndex < segments.Count; itemIndex++)
		{
			var thisSegment = segments[itemIndex];

			if (thisSegment.MaxMeasure < minMeasure)
				continue;
			if (thisSegment.MinMeasure > maxMeasure)
				break;
			//	Assert: thisSegment interacts with new segment.

			if (thisSegment.MinMeasure < minMeasure)
			{
				var newSegment = new Segment(minMeasure, thisSegment.MaxMeasure, thisSegment.Value);
				segments.Insert(itemIndex + 1, newSegment);
				thisSegment.MaxMeasure = minMeasure - 1;
				continue;
			}
			if (thisSegment.MaxMeasure > maxMeasure)
			{
				var newSegment = new Segment(thisSegment.MinMeasure, maxMeasure, thisSegment.Value + value);
				segments.Insert(itemIndex, newSegment);
				thisSegment.MinMeasure = maxMeasure + 1;
				break;
			}

			//	Assert: thisSegment.MinMeasure >= minMeasure.
			//	Assert: thisSegment.MinMeasure <= maxMeasure.
			//	Assert: thisSegment.MaxMeasure >= minMeasure.
			//  Assert: thisSegment.MaxMeasure <= maxMeasure.
			//	i.e.: thisSegment is completely contained by new segment.

			if (itemIndex > 0)
			{
				var prevSegment = segments[itemIndex - 1];
				var gapMinMeasure = Math.Max(prevSegment.MaxMeasure, minMeasure);
				if (gapMinMeasure < thisSegment.MinMeasure - 1)
				{
					segments.Insert(itemIndex, new Segment(gapMinMeasure, thisSegment.MinMeasure - 1, value));
					continue;
				}
			}

			thisSegment.Value += value;

			if (itemIndex < segments.Count - 1)
			{
				var nextSegment = segments[itemIndex + 1];
				var gapMaxMeasure = Math.Min(nextSegment.MinMeasure, maxMeasure);
				if (gapMaxMeasure > thisSegment.MaxMeasure + 1)
				{
					segments.Insert(itemIndex + 1, new Segment(thisSegment.MaxMeasure + 1, gapMaxMeasure, 0));
				}
			}
		}
	}

	public void RemoveSegment(long minMeasure, long maxMeasure)
	{
		ISegment segment1 = null;
		ISegment segment2 = null;

		if (maxMeasure < minMeasure)
			(maxMeasure, minMeasure) = (minMeasure, maxMeasure);

		int segmentIndex = 0;

		while (segmentIndex < segments.Count)
		{
			ISegment segment = segments[segmentIndex];
			if ((segment.MinMeasure <= minMeasure) && (minMeasure <= segment.MaxMeasure))
			{
				segment1 = segment;
			}
			if ((segment.MinMeasure <= maxMeasure) && (maxMeasure <= segment.MaxMeasure))
			{
				segment2 = segment;
			}

			if ((minMeasure < segment.MinMeasure) && (segment.MaxMeasure < maxMeasure)) //	segment inside minMeasure and maxMeasure - remove it.
			{
				segments.RemoveAt(segmentIndex);
			}
			else
			{
				segmentIndex++;
			}
		}

		if ((segment1 == segment2) && (segment1 != null))   //	minMeasure and maxMeasure both in same segment. Split segment.
		{
			ISegment segment = new Segment(maxMeasure + 1, segment1.MaxMeasure);
			segments.Add(segment);
			segment1.MaxMeasure = minMeasure - 1;
		}
		else	//	minMeasure and maxMeasure in separate segments or no segments
		{
			if (segment1 != null)	//	minMeasure in existing segment. Clip segment.
			{
				segment1.MaxMeasure = minMeasure - 1;
			}
			if (segment2 != null)	//	maxMeasure in existing segment. Clip segment.
			{
				segment2.MinMeasure = maxMeasure + 1;
			}
		}

		segmentIndex = 0;
		while (segmentIndex < segments.Count)
		{
			ISegment segment = segments[segmentIndex];
			if (segment.MinMeasure > segment.MaxMeasure)
			{
				segments.RemoveAt(segmentIndex);
			}
			else
			{
				segmentIndex++;
			}
		}

		segments.Sort(Segment.Compare);
	}

	public ISegment FindSegment(long minMeasure, long maxMeasure)
	{
		if (maxMeasure < minMeasure)
			(maxMeasure, minMeasure) = (minMeasure, maxMeasure);

		foreach (var segment in segments)
		{
			//	Are minMeasure and maxMeasure wholly within the segment?
			if ((segment.MinMeasure <= minMeasure) && (maxMeasure <= segment.MaxMeasure))
			{
				return segment;
			}
		}
		return null;
	}

	public void Union(ISegmentList list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			AddSegment(list[i].MinMeasure, list[i].MaxMeasure, list[i].Value);
		}
	}

	public void Intersect(ISegmentList list)
	{
		if ((Count == 0) || (list.Count == 0))
		{
			return;
		}

		for (int i = 1; i < list.Count; i++)
		{
			long minMeasure = list[i - 1].MaxMeasure;
			long maxMeasure = list[i].MinMeasure;

			if (minMeasure < maxMeasure)
			{
				RemoveSegment(minMeasure, maxMeasure);
			}
		}
	}

	public void Difference(ISegmentList list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			RemoveSegment(list[i].MinMeasure, list[i].MaxMeasure);
		}
	}

	public IEnumerator<ISegment> GetEnumerator()
	{
		return segments.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return segments.GetEnumerator();
	}
}
