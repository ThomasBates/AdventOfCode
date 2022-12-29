namespace AoC.Common.SegmentList;

public interface ISegmentListItem
{
	double MinMeasure { get; set; }

	double MaxMeasure { get; set; }

	double Value { get; set; }
}
