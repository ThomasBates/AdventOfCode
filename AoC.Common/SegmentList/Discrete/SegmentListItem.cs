namespace AoC.Common.SegmentList.Discrete;

public class SegmentListItem : ISegmentListItem
{
	public long MinMeasure { get; set; }

	public long MaxMeasure { get; set; }

	public double Value { get; set; }

	public SegmentListItem(long minMeasure, long maxMeasure, double value = 0)
	{
		MinMeasure = minMeasure;
		MaxMeasure = maxMeasure;
		Value = value;
	}

	public static int Compare(ISegmentListItem a, ISegmentListItem b)
	{
		return a.MinMeasure.CompareTo(b.MinMeasure);
	}
}
