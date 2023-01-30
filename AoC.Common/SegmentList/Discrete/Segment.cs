namespace AoC.Common.SegmentList.Discrete;

public class Segment : ISegment
{
	public long MinMeasure { get; set; }

	public long MaxMeasure { get; set; }

	public long Value { get; set; }

	public Segment(long minMeasure, long maxMeasure, long value = 0)
	{
		MinMeasure = minMeasure;
		MaxMeasure = maxMeasure;
		Value = value;
	}

	public static int Compare(ISegment a, ISegment b)
	{
		return a.MinMeasure.CompareTo(b.MinMeasure);
	}

	public override string ToString() => $"[{MinMeasure},{MaxMeasure}] = {Value}";
}
