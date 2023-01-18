using System.Collections.Generic;

namespace AoC.Common.SegmentList.Continuous;

public class MergingSegmentList : ISegmentList
{
	private readonly List<ISegmentListItem> segmentList = new();

	public ISegmentListItem this[int index]
	{
		get => segmentList[index];
		set => segmentList[index] = value;
	}

	public int Count => segmentList.Count;

	public void Clear()
	{
		segmentList.Clear();
	}

	public void AddSegment(double minMeasure, double maxMeasure, double value = 0)
	{
		ISegmentListItem segment1 = null;
		ISegmentListItem segment2 = null;

		if (maxMeasure < minMeasure)
			(maxMeasure, minMeasure) = (minMeasure, maxMeasure);

		int segmentIndex = 0;

		while (segmentIndex < segmentList.Count)
		{
			ISegmentListItem segment = segmentList[segmentIndex];
			if ((segment.MinMeasure <= minMeasure) && (minMeasure <= segment.MaxMeasure))
			{
				segment1 = segment;
			}
			if ((segment.MinMeasure <= maxMeasure) && (maxMeasure <= segment.MaxMeasure))
			{
				segment2 = segment;
			}

			if ((minMeasure < segment.MinMeasure) && (segment.MaxMeasure < maxMeasure)) //	segment inside minMeasure and maxMeasure
			{
				segmentList.RemoveAt(segmentIndex);
			}
			else
			{
				segmentIndex++;
			}
		}

		if ((segment1 == null) && (segment2 == null))	//	no overlap. Create new segment.
		{
			ISegmentListItem segment = new SegmentListItem(minMeasure, maxMeasure);
			segmentList.Add(segment);
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
			segmentList.Remove(segment2);
		}
		//  else  //  minMeasure and maxMeasure both in same segment.  Ignore.

		segmentList.Sort(SegmentListItem.Compare);
	}

	public void RemoveSegment(double minMeasure, double maxMeasure)
	{
		ISegmentListItem segment1 = null;
		ISegmentListItem segment2 = null;

		if (maxMeasure < minMeasure)
			(maxMeasure, minMeasure) = (minMeasure, maxMeasure);

		int segmentIndex = 0;

		while (segmentIndex < segmentList.Count)
		{
			ISegmentListItem segment = segmentList[segmentIndex];
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
				segmentList.RemoveAt(segmentIndex);
			}
			else
			{
				segmentIndex++;
			}
		}

		if ((segment1 == segment2) && (segment1 != null))   //	minMeasure and maxMeasure both in same segment. Split segment.
		{
			ISegmentListItem segment = new SegmentListItem(maxMeasure, segment1.MaxMeasure);
			segmentList.Add(segment);
			segment1.MaxMeasure = minMeasure;
		}
		else	//	minMeasure and maxMeasure in separate segments or no segments
		{
			if (segment1 != null)	//	minMeasure in existing segment. Clip segment.
			{
				segment1.MaxMeasure = minMeasure;
			}
			if (segment2 != null)	//	maxMeasure in existing segment. Clip segment.
			{
				segment2.MinMeasure = maxMeasure;
			}
		}

		segmentIndex = 0;
		while (segmentIndex < segmentList.Count)
		{
			ISegmentListItem segment = segmentList[segmentIndex];
			if (segment.MinMeasure == segment.MaxMeasure)
			{
				segmentList.RemoveAt(segmentIndex);
			}
			else
			{
				segmentIndex++;
			}
		}

		segmentList.Sort(SegmentListItem.Compare);
	}

	public ISegmentListItem FindSegment(double minMeasure, double maxMeasure)
	{
		if (maxMeasure < minMeasure)
			(maxMeasure, minMeasure) = (minMeasure, maxMeasure);

		foreach (var segment in segmentList)
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

		for (int i = 0; i <= list.Count; i++)
		{
			double minMeasure, maxMeasure;

			if (i == 0)
			{
				minMeasure = this[0].MinMeasure;
			}
			else
			{
				minMeasure = list[i - 1].MaxMeasure;
			}

			if (i == list.Count)
			{
				maxMeasure = this[Count - 1].MaxMeasure;
			}
			else
			{
				maxMeasure = list[i].MinMeasure;
			}

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
}
