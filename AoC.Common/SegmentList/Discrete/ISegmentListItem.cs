namespace AoC.Common.SegmentList.Discrete;

public interface ISegmentListItem
{
	long MinMeasure { get; set; }

	long MaxMeasure { get; set; }

	double Value { get; set; }
}
