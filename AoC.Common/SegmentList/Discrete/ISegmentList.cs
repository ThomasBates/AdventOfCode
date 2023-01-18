namespace AoC.Common.SegmentList.Discrete;

public interface ISegmentList
{
	int Count { get; }

	ISegmentListItem this[int index] { get; set; }

	void Clear();

	void AddSegment(long minMeasure, long maxMeasure, double value = 0);

	void RemoveSegment(long minMeasure, long maxMeasure);

	ISegmentListItem FindSegment(long minMeasure, long maxMeasure);

	void Union(ISegmentList list);

	void Intersect(ISegmentList list);

	void Difference(ISegmentList list);
}
