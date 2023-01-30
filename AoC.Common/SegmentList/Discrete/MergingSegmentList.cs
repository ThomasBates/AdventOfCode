using System.Collections;
using System.Collections.Generic;

namespace AoC.Common.SegmentList.Discrete;

public class MergingSegmentList : ISegmentList
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

		if (Count == 0)
		{
			segments.Add(new Segment(minMeasure, maxMeasure));
			return;
		}

		ISegment segment1 = null;
		ISegment segment2 = null;

		int segmentIndex = 0;

		while (segmentIndex < segments.Count)
		{
			ISegment segment = segments[segmentIndex];
			if ((segment.MinMeasure <= minMeasure) && (minMeasure <= segment.MaxMeasure + 1))
			{
				segment1 = segment;
			}
			if ((segment.MinMeasure - 1 <= maxMeasure) && (maxMeasure <= segment.MaxMeasure))
			{
				segment2 = segment;
			}

			if ((minMeasure < segment.MinMeasure) && (segment.MaxMeasure < maxMeasure)) //	segment inside minMeasure and maxMeasure
			{
				segments.RemoveAt(segmentIndex);
			}
			else
			{
				segmentIndex++;
			}
		}

		if ((segment1 == null) && (segment2 == null))	//	no overlap. Create new segment.
		{
			ISegment segment = new Segment(minMeasure, maxMeasure);
			segments.Add(segment);
		}
		else if (segment2 == null)	//	minMeasure in existing segment. Extend Segment.
		{
			segment1.MaxMeasure = maxMeasure;
		}
		else if (segment1 == null)	//	maxMeasure in existing segment. Extend segment.
		{
			segment2.MinMeasure = minMeasure;
		}
		else if (segment1 != segment2)	//	minMeasure and maxMeasure in different segments. Merge segments.
		{
			segment1.MaxMeasure = segment2.MaxMeasure;
			segments.Remove(segment2);
		}
		//  else  //  minMeasure and maxMeasure both in same segment.  Ignore.

		segments.Sort(Segment.Compare);
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
			AddSegment(list[i].MinMeasure, list[i].MaxMeasure);
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
