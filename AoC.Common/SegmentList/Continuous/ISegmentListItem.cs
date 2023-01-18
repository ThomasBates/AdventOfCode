namespace AoC.Common.SegmentList.Continuous;

public interface ISegmentListItem
{
	double MinMeasure { get; set; }

	double MaxMeasure { get; set; }

	double Value { get; set; }
}
