namespace AoC.Common.SegmentList.Continuous;

public interface ISegmentList
{
	int Count { get; }

	ISegmentListItem this[int index] { get; set; }

	void Clear();

	void AddSegment(double minMeasure, double maxMeasure, double value = 0);

	void RemoveSegment(double minMeasure, double maxMeasure);

	ISegmentListItem FindSegment(double minMeasure, double maxMeasure);

	void Union(ISegmentList list);

	void Intersect(ISegmentList list);

	void Difference(ISegmentList list);
}
