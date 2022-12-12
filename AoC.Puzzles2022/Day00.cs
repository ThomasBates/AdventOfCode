using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AoC.IO;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day00 : IPuzzle
	{
		#region IPuzzle Properties

		public string Name => "Day 00";

		public Dictionary<string, string> Inputs { get; } = new Dictionary<string, string>()
		{
			{"Example Inputs", Resources.Day00ExampleInputs},
			{"Puzzle Inputs",  Resources.Day00PuzzleInputs}
		};

		public Dictionary<string, Func<string, string>> Solvers { get; } = new Dictionary<string, Func<string, string>>();

		#endregion IPuzzle Properties

		#region Constructors

		public Day00()
		{
			Solvers.Add("Solve Part 1", SolvePart1);
			Solvers.Add("Solve Part 2", SolvePart2);
		}

		#endregion Constructors

		private string SolvePart1(string input)
		{
			StringBuilder output = new StringBuilder();

			LoadDataFromInput(input, output);

			//

			return output.ToString();
		}

		private string SolvePart2(string input)
		{
			StringBuilder output = new StringBuilder();

			LoadDataFromInput(input, output);

			//

			return output.ToString();
		}

		private void LoadDataFromInput(string input, StringBuilder output)
		{
			Helper.TraverseInputTokens(input, value =>
			{
			});

			Helper.TraverseInputLines(input, line =>
			{
			});


			ParserHelper.RunParser(input, output, Resources.Day00Grammar,
				(token, valueStack) =>
				{
					switch (token)
					{
						case "s_scope":
							break;
					}
				},
				(token, valueStack) =>
				{
					switch (token)
					{
						case "t_type":
							break;
					}
				},
				(token, valueStack) =>
				{
					switch (token)
					{
						case "c_code":
							break;
					}
				});

		}
	}
}
