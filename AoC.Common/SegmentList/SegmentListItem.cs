using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Common.SegmentList
{
	public class SegmentListItem : ISegmentListItem
	{
		public double MinMeasure
		{
			get;
			set;
		}

		public double MaxMeasure
		{
			get;
			set;
		}

		public double Value
		{
			get;
			set;
		}

		public SegmentListItem(double minMeasure, double maxMeasure, double value = 0)
		{
			MinMeasure = minMeasure;
			MaxMeasure = maxMeasure;
			Value = value;
		}

		public static int Compare(ISegmentListItem a, ISegmentListItem b)
		{
			if (a.MinMeasure < b.MinMeasure)
			{
				return -1;
			}
			if (a.MinMeasure > b.MinMeasure)
			{
				return 1;
			}
			return 0;
		}
	}
}
