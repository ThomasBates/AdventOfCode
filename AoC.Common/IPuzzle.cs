using System;
using System.Collections.Generic;

namespace AoC.Common
{
	public interface IPuzzle
	{
		int Year { get; }

		int Day { get; }

		string Name { get; }

		Dictionary<string, string> Inputs { get; }

		Dictionary<string, Func<string, string>> Solvers { get; }
	}
}
