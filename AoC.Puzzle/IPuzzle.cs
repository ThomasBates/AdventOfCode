using System;
using System.Collections.Generic;

namespace AoC.Puzzle
{
	public interface IPuzzle
	{
		string Name
		{
			get;
		}

		Dictionary<string, string> Inputs
		{
			get;
		}

		Dictionary<string, Func<string, string>> Solvers
		{
			get;
		}
	}
}
