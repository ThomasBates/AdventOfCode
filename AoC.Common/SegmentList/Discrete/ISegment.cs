namespace AoC.Common.SegmentList.Discrete;

public interface ISegment
{
	long MinMeasure { get; set; }

	long MaxMeasure { get; set; }

	long Value { get; set; }
}
