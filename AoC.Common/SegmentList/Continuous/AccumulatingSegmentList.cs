﻿using System.Collections.Generic;

namespace AoC.Common.SegmentList.Continuous;

public class AccumulatingSegmentList : ISegmentList
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
		if (maxMeasure < minMeasure)
			(maxMeasure, minMeasure) = (minMeasure, maxMeasure);

		if (Count == 0)
		{
			segmentList.Add(new SegmentListItem(minMeasure, maxMeasure, value));
			return;
		}

		int startIndex = -1;
		int endIndex = -1;

		for (int itemIndex = 0; itemIndex < Count; itemIndex++)
		{
			ISegmentListItem item = segmentList[itemIndex];

			if (item.MinMeasure == minMeasure)
			{
				startIndex = itemIndex;
				break;
			}
			if (item.MaxMeasure == minMeasure)
			{
				startIndex = itemIndex + 1;
				break;
			}
			if (item.MaxMeasure > minMeasure)
			{
				ISegmentListItem newItem = new SegmentListItem(item.MinMeasure, item.MaxMeasure, item.Value);
				item.MaxMeasure = minMeasure;
				newItem.MinMeasure = minMeasure;

				startIndex = itemIndex + 1;
				segmentList.Insert(startIndex, newItem);
				break;
			}
		}

		if (startIndex >= 0)
		{
			for (int itemIndex = startIndex; itemIndex < Count; itemIndex++)
			{
				ISegmentListItem item = segmentList[itemIndex];

				if (item.MaxMeasure == maxMeasure)
				{
					endIndex = itemIndex;
					break;
				}
				if (item.MaxMeasure > maxMeasure)
				{
					ISegmentListItem newItem = new SegmentListItem(item.MinMeasure, item.MaxMeasure, item.Value);
					item.MinMeasure = maxMeasure;
					newItem.MaxMeasure = maxMeasure;

					endIndex = itemIndex;
					segmentList.Insert(endIndex, newItem);
					break;
				}
			}
		}

		//  if maxMeasure is greater than the last item.maxMeasure, just go to the end of the last item.
		if ((startIndex != -1) && (endIndex == -1))
		{
			endIndex = Count - 1;
		}

		for (int itemIndex = startIndex; itemIndex <= endIndex; itemIndex++)
		{
			ISegmentListItem item = segmentList[itemIndex];
			item.Value += value;
		}
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
			AddSegment(list[i].MinMeasure, list[i].MaxMeasure, list[i].Value);
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
