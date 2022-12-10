
using System.Collections.Generic;

namespace AoC.IO.SegmentList
{
	public class AccumulatingSegmentList : ISegmentList
	{
		private List<ISegmentListItem> _segmentList = new List<ISegmentListItem>();

		public ISegmentListItem this[int index]
		{
			get
			{
				return _segmentList[index];
			}

			set
			{
				_segmentList[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return _segmentList.Count;
			}
		}

		public void Clear()
		{
			_segmentList.Clear();
		}

		public void AddSegment(double minMeasure, double maxMeasure, double value = 0)
		{
			if (maxMeasure < minMeasure)
			{
				double tmp = minMeasure;
				minMeasure = maxMeasure;
				maxMeasure = tmp;
			}

			if (Count == 0)
			{
				_segmentList.Add(new SegmentListItem(minMeasure, maxMeasure, value));
				return;
			}

			int startIndex = -1;
			int endIndex = -1;

			for (int itemIndex = 0; itemIndex < Count; itemIndex++)
			{
				ISegmentListItem item = _segmentList[itemIndex];

				if (item.MinMeasure == minMeasure)
				{
					startIndex = itemIndex;
					break;
				}
				if (item.MaxMeasure == maxMeasure)
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
					_segmentList.Insert(startIndex, newItem);
					break;
				}
			}

			if (startIndex >= 0)
			{
				for (int itemIndex = startIndex; itemIndex < Count; itemIndex++)
				{
					ISegmentListItem item = _segmentList[itemIndex];

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
						_segmentList.Insert(endIndex, newItem);
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
				ISegmentListItem item = _segmentList[itemIndex];
				item.Value += value;
			}
		}

		public void RemoveSegment(double minMeasure, double maxMeasure)
		{
			ISegmentListItem segment1 = null;
			ISegmentListItem segment2 = null;

			if (maxMeasure < minMeasure)
			{
				double tmp = minMeasure;
				minMeasure = maxMeasure;
				maxMeasure = tmp;
			}

			int segmentIndex = 0;

			while (segmentIndex < _segmentList.Count)
			{
				ISegmentListItem segment = _segmentList[segmentIndex];
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
					_segmentList.RemoveAt(segmentIndex);
				}
				else
				{
					segmentIndex++;
				}
			}

			if ((segment1 == segment2) && (segment1 != null))   //	minMeasure and maxMeasure both in same segment. Split segment.
			{
				ISegmentListItem segment = new SegmentListItem(maxMeasure, segment1.MaxMeasure);
				_segmentList.Add(segment);
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
			while (segmentIndex < _segmentList.Count)
			{
				ISegmentListItem segment = _segmentList[segmentIndex];
				if (segment.MinMeasure == segment.MaxMeasure)
				{
					_segmentList.RemoveAt(segmentIndex);
				}
				else
				{
					segmentIndex++;
				}
			}

			_segmentList.Sort(SegmentListItem.Compare);
		}

		public ISegmentListItem FindSegment(double minMeasure, double maxMeasure)
		{
			if (maxMeasure < minMeasure)
			{
				double tmp = minMeasure;
				minMeasure = maxMeasure;
				maxMeasure = tmp;
			}

			foreach (var segment in _segmentList)
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
}
