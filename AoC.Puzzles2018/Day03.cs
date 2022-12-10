﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AoC.IO;
using AoC.IO.SegmentList;
using AoC.Parser;
using AoC.Puzzle;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018
{
	[Export(typeof(IPuzzle))]
	public class Day03 : IPuzzle
	{
		#region IPuzzle Properties

		public string Name
		{
			get;
		}

		public Dictionary<string, string> Inputs
		{
			get;
		} = new Dictionary<string, string>();

		public Dictionary<string, Func<string, string>> Solvers
		{
			get;
		} = new Dictionary<string, Func<string, string>>();

		#endregion IPuzzle Properties

		#region Constructors

		public Day03()
		{
			Name = "Day 03";

			Inputs.Add("Sample Inputs", Resources.Day03SampleInputs);
			Inputs.Add("Puzzle Inputs", Resources.Day03PuzzleInputs);

			Solvers.Add("Part 1", SolvePart1);
			Solvers.Add("Part 2", SolvePart2);
		}

		#endregion Constructors

		private class ClaimInfo
		{
			public int ID;
			public int Left;
			public int Top;
			public int Width;
			public int Height;

			public override string ToString()
			{
				return $"#{ID} @ {Left},{Top}: {Width}x{Height}";
			}
		}

		private ClaimInfo _claimInfo;
		private readonly List<ClaimInfo> _claims = new List<ClaimInfo>();
		private readonly Stack<string> _valueStack = new Stack<string>();

		public string SolvePart1(string input)
		{
			_claims.Clear();
			_valueStack.Clear();

			using (var _grammar = new L2Grammar())
			using (var _parser = new L2Parser(_grammar))
			{
				_grammar.ReadGrammar(Resources.Day03Grammar);
				_parser.ValueEmitted += Parser_ValueEmitted;
				_parser.TokenEmitted += Parser_TokenEmitted;

				Helper.TraverseInputLines(input, line =>
				{
					_claimInfo = new ClaimInfo();
					_parser.Parse(line);
					_claims.Add(_claimInfo);
				});
			}


			const int fabricSize = 1000;
			var fabric = new int[fabricSize, fabricSize];

			for (int i = 0; i < fabricSize; i++)
			{
				for (int j = 0; j < fabricSize; j++)
				{
					fabric[i, j] = 0;
				}
			}

			foreach (var claim in _claims)
			{
				for (int i = 0; i < claim.Width; i++)
				{
					for (int j = 0; j < claim.Height; j++)
					{
						fabric[claim.Left + i, claim.Top + j]++;
					}
				}
			}

			int area = 0;

			for (int i = 0; i < fabricSize; i++)
			{
				for (int j = 0; j < fabricSize; j++)
				{
					if (fabric[i, j] > 1)
					{
						area++;
					}
				}
			}

			return $"The total area is {area} square inches.";
		}

		public string SolvePart2(string input)
		{
			_claims.Clear();
			_valueStack.Clear();

			using (var _grammar = new L2Grammar())
			using (var _parser = new L2Parser(_grammar))
			{
				_grammar.ReadGrammar(Resources.Day03Grammar);
				_parser.ValueEmitted += Parser_ValueEmitted;
				_parser.TokenEmitted += Parser_TokenEmitted;

				Helper.TraverseInputLines(input, line =>
				{
					_claimInfo = new ClaimInfo();
					_parser.Parse(line);
					_claims.Add(_claimInfo);
				});
			}

			var result = new StringBuilder();

			foreach (var claim1 in _claims)
			{
				bool overlaps = false;

				foreach (var claim2 in _claims)
				{
					if (claim1.ID == claim2.ID)
					{
						continue;
					}

					overlaps = (claim1.Left < claim2.Left + claim2.Width) &&
								(claim1.Left + claim1.Width > claim2.Left) &&
								(claim1.Top < claim2.Top + claim2.Height) &&
								(claim1.Top + claim1.Height > claim2.Top);

					if (overlaps)
					{
						break;
					}
				}

				if (!overlaps)
				{
					result.AppendLine($"Claim {claim1.ID} does not overlap.");
				}
			}

			return result.ToString();
		}

		#region Event Handler Methods

		/// <summary>
		/// Handles the ValueEmitted event of the Parser control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="Dynamic.Parser.ParserEventArgs"/> instance containing the event data.</param>
		/// <created>23/02/2009</created>
		/// <author>Thomas_Bates</author>
		void Parser_ValueEmitted(object sender, ParserEventArgs e)
		{
			//Debug.Print("Filter", "\"" + e.Value + "\"");
			_valueStack.Push(e.Value);
			//DebugShowValueStack();
		}

		/// <summary>
		/// Handles the TokenEmitted event of the Parser control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="Dynamic.Parser.ParserEventArgs"/> instance containing the event data.</param>
		/// <created>23/02/2009</created>
		/// <author>Thomas_Bates</author>
		void Parser_TokenEmitted(object sender, ParserEventArgs e)
		{
			//Debug.Print("Filter", e.Token);

			if (string.IsNullOrEmpty(e.Token))
				return;
			switch (e.Token[0])
			{
				case 's':
					ScopeController(e.Token);
					break;
				case 't':
					TypeChecker(e.Token);
					break;
				case 'c':
					CodeGenerator(e.Token);
					break;
			}
		}

		#endregion
		#region Local Support Methods

		/// <summary>
		/// Controls the scope.
		/// </summary>
		/// <param name="token">The scope token.</param>
		/// <created>24/02/2009</created>
		/// <author>Thomas_Bates</author>
		protected virtual void ScopeController(string token)
		{
			//switch (token)
			//{
				//case "s_resetLists":
				//	break;

				//case "s_saveColumnName":
				//    _columnList.Add(_valueStack.Peek());
				//    break;

				//case "s_saveTableName":
				//    _tableList.Add(_valueStack.Peek());
				//    break;

				//case "s_tableAndColumn":
				//    break;

				//case "s_parentheses":
				//	WrapValue("(", ")");
				//	break;
				//case "s_brackets":
				//	WrapValue("[", "]");
				//	break;
				//case "s_braces":
				//	WrapValue("{", "}");
				//	break;

				//case "s_firstListValue":
				//	_valueStack.Push("1");
				//	DebugShowValueStack();
				//	break;

				//case "s_nextListValue":
				//	string lastValue = _valueStack.Pop();
				//	string lastSize = _valueStack.Pop();
				//	int listSize = int.Parse(lastSize) + 1;
				//	_valueStack.Push(lastValue);
				//	_valueStack.Push(listSize.ToString());
				//	DebugShowValueStack();
				//	break;
			//}
		}

		/// <summary>
		/// checks the type.
		/// </summary>
		/// <param name="token">The type token.</param>
		/// <created>24/02/2009</created>
		/// <author>Thomas_Bates</author>
		protected virtual void TypeChecker(string token)
		{
		}

		/// <summary>
		/// Generates the code.
		/// </summary>
		/// <param name="token">The code token.</param>
		/// <created>24/02/2009</created>
		/// <author>Thomas_Bates</author>
		protected virtual void CodeGenerator(string token)
		{
			string value;

			switch (token)
			{
				case "c_claimId":
					value = _valueStack.Pop();
					_claimInfo.ID = int.Parse(value);
					break;
				case "c_left":
					value = _valueStack.Pop();
					_claimInfo.Left = int.Parse(value);
					break;
				case "c_top":
					value = _valueStack.Pop();
					_claimInfo.Top = int.Parse(value);
					break;
				case "c_width":
					value = _valueStack.Pop();
					_claimInfo.Width = int.Parse(value);
					break;
				case "c_height":
					value = _valueStack.Pop();
					_claimInfo.Height = int.Parse(value);
					break;
			}
		}

		/// <summary>
		/// Writes the value stack to the debug file.
		/// </summary>
		/// <created>23/02/2009</created>
		/// <author>Thomas_Bates</author>
		private void DebugShowValueStack()
		{
			//if (!Debug.IsEnabled("Filter"))
			//	return;
			string values = string.Empty;
			foreach (string value in _valueStack.ToArray())
			{
				if (!string.IsNullOrEmpty(values))
					values = ", " + values;
				values = "\"" + value + "\"" + values;
			}
			//Debug.Print("Filter", "stack:  " + values);
		}

		#endregion
	}
}
