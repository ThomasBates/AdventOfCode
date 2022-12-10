using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.IO.SegmentList
{
	public interface ISegmentListItem
	{
		double MinMeasure
		{
			get;
			set;
		}

		double MaxMeasure
		{
			get;
			set;
		}

		double Value
		{
			get;
			set;
		}
	}
}
