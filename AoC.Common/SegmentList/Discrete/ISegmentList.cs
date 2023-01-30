using System.Collections.Generic;

namespace AoC.Common.SegmentList.Discrete;

public interface ISegmentList : IEnumerable<ISegment>
{
	int Count { get; }

	ISegment this[int index] { get; set; }

	void Clear();

	void AddSegment(long minMeasure, long maxMeasure, long value = 0);

	void RemoveSegment(long minMeasure, long maxMeasure);

	ISegment FindSegment(long minMeasure, long maxMeasure);

	void Union(ISegmentList list);

	void Intersect(ISegmentList list);

	void Difference(ISegmentList list);
}
