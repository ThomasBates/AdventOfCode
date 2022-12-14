using System;
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
	public class Day10 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2018;

		public int Day => 10;

		public string Name => $"Day {Day:00}";

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

		public Day10()
		{
			Inputs.Add("Example Inputs", Resources.Day10Inputs);
			Inputs.Add("Puzzle Inputs", "");

			Solvers.Add("Solve Part 1", SolvePart1);
			Solvers.Add("Solve Part 2", SolvePart2);
		}

		#endregion Constructors

		private class Point
		{
			public int X0, Y0;
			public int dX, dY;
		}

		private Point _point;
		private readonly List<Point> _points = new List<Point>();
		private readonly Stack<string> _valueStack = new Stack<string>();
		private int _sign = 1;

		private void LoadPointsFromInput_Grammar(string input)
		{
			_points.Clear();
			_valueStack.Clear();

			var _grammar = new L2Grammar();
			var _parser = new L2Parser(_grammar);

			_grammar.ReadGrammarDefinition(Resources.Day10Grammar);
			_parser.OnValueEmitted += Parser_ValueEmitted;
			_parser.OnTokenEmitted += Parser_TokenEmitted;

			Helper.TraverseInputLines(input, (Action<string>)(line =>
			{
				_point = new Point();
				_parser.Parse(line);
				_points.Add(_point);
			}));
		}

		private void LoadPointsFromInput_Regex(string input)
		{
			_points.Clear();
			Helper.TraverseInputLines(input, (Action<string>)(line =>
			{
				Match match = Regex.Match(line, @"position=<\s?(-?\d+), \s?(-?\d+)> velocity=<\s?(-?\d+), \s?(-?\d+)>");

				_points.Add(new Point()
				{
					X0 = int.Parse(match.Groups[1].Value),
					Y0 = int.Parse(match.Groups[2].Value),
					dX = int.Parse(match.Groups[3].Value),
					dY = int.Parse(match.Groups[4].Value)
				});
			}));
		}

		private void LoadPointsFromInput_Split(string input)
		{
			_points.Clear();
			Helper.TraverseInputLines(input, (Action<string>)(line =>
			{
				string[] parts = line.Split(new char[] { '<', '>', ',' }, StringSplitOptions.RemoveEmptyEntries);

				_points.Add(new Point()
				{
					X0 = int.Parse(parts[1]),
					Y0 = int.Parse(parts[2]),
					dX = int.Parse(parts[4]),
					dY = int.Parse(parts[5])
				});
			}));
		}

		public string SolvePart1(string input)
		{
			var result = new StringBuilder();

			//LoadPointsFromInput_Grammar(input);
			LoadPointsFromInput_Regex(input);
			//LoadPointsFromInput_Split(input);

			int lastMinX = 0;
			int lastMaxX = 0;
			int lastMinY = 0;
			int lastMaxY = 0;

			int lastWidth = int.MaxValue;
			int lastHeight = int.MaxValue;
			int s = 0;
			while (true)
			{
				int minX = _points.Min(p => p.X0 + p.dX * s);
				int maxX = _points.Max(p => p.X0 + p.dX * s);
				int minY = _points.Min(p => p.Y0 + p.dY * s);
				int maxY = _points.Max(p => p.Y0 + p.dY * s);

				int width = maxX - minX;
				int height = maxY - minY;

				if ((width > lastWidth) || (height > lastHeight))
				{
					break;
				}

				lastMinX = minX;
				lastMaxX = maxX;
				lastMinY = minY;
				lastMaxY = maxY;

				lastWidth = width;
				lastHeight = height;
				s++;
			}
			s--;
			result.AppendLine($"local minimum size at {s} seconds.");

			var grid = new int[lastWidth + 1, lastHeight + 1];

			for (int gy = 0; gy <= lastHeight; gy++)
			{
				for (int gx = 0; gx <= lastWidth; gx++)
				{
					grid[gx, gy] = 0;
				}
			}

			foreach (var point in _points)
			{
				int x = point.X0 + point.dX * s;
				int y = point.Y0 + point.dY * s;

				int gx = (x - lastMinX);
				int gy = (y - lastMinY);

				grid[gx, gy]++;
			}

			for (int gy = 0; gy <= lastHeight; gy++)
			{
				for (int gx = 0; gx <= lastWidth; gx++)
				{
					int count = grid[gx, gy];
					if (count == 0)
						result.Append(" ");
					else
						result.Append(grid[gx, gy].ToString());
				}
				result.AppendLine();
			}

			return result.ToString();
		}

		public string SolvePart2(string input)
		{
			return "";
		}

		#region Event Handler Methods

		/// <summary>
		/// Handles the ValueEmitted event of the Parser control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="Dynamic.Parser.ParserEventArgs"/> instance containing the event data.</param>
		void Parser_ValueEmitted(object sender, ParserEventArgs e)
		{
			_valueStack.Push(e.Value);
		}

		/// <summary>
		/// Handles the TokenEmitted event of the Parser control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="Dynamic.Parser.ParserEventArgs"/> instance containing the event data.</param>
		void Parser_TokenEmitted(object sender, ParserEventArgs e)
		{
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
		protected virtual void ScopeController(string token)
		{
		}

		/// <summary>
		/// checks the type.
		/// </summary>
		/// <param name="token">The type token.</param>
		protected virtual void TypeChecker(string token)
		{
		}

		/// <summary>
		/// Generates the code.
		/// </summary>
		/// <param name="token">The code token.</param>
		protected virtual void CodeGenerator(string token)
		{
			string value;

			switch (token)
			{
				case "c_positive":
					_sign = 1;
					break;
				case "c_negative":
					_sign = -1;
					break;
				case "c_x":
					value = _valueStack.Pop();
					int x = int.Parse(value);
					_valueStack.Push((_sign * x).ToString());
					break;
				case "c_y":
					value = _valueStack.Pop();
					int y = int.Parse(value);
					_valueStack.Push((_sign * y).ToString());
					break;
				case "c_position":
					value = _valueStack.Pop();
					_point.Y0 = int.Parse(value);
					value = _valueStack.Pop();
					_point.X0 = int.Parse(value);
					break;
				case "c_velocity":
					value = _valueStack.Pop();
					_point.dY = int.Parse(value);
					value = _valueStack.Pop();
					_point.dX = int.Parse(value);
					break;
			}
		}

		#endregion
	}
}
