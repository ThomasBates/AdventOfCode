using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using AoC.Parser;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day10 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2022;

		public int Day => 10;

		public string Name => $"Day {Day:00}";

		public Dictionary<string, string> Inputs { get; } = new Dictionary<string, string>()
		{
			{"Example Inputs", Resources.Day10Inputs},
			{"Puzzle Inputs",  ""}
		};

		public Dictionary<string, Func<string, string>> Solvers { get; } = new Dictionary<string, Func<string, string>>()
		{
			{ "Part 1", SolvePart1 },
			{ "Part 2", SolvePart2 }
		};

		#endregion IPuzzle Properties

		private static string SolvePart1(string input)
		{
			StringBuilder output = new StringBuilder();

			//SolvePart1WithFileParser(input, output);
			SolvePart1WithParserHelper(input, output);

			return output.ToString();
		}

		private static string SolvePart2(string input)
		{
			StringBuilder output = new StringBuilder();

			//SolvePart2WithFileParser(input, output);
			SolvePart2WithParserHelper(input, output);

			return output.ToString();
		}

		private static void SolvePart1WithFileParser(string input, StringBuilder output)
		{
			const int InitialSample = 20;
			const int SampleInterval = 40;

			int cycle = 0;
			int nextSample = InitialSample;
			int x = 1;
			int signalStrength = 0;

			var valueStack = new Stack<string>();

			using (var _grammar = new L2Grammar())
			using (var _parser = new L2Parser(_grammar))
			{
				try
				{
					_grammar.ReadGrammar(Resources.Day10Grammar2);
				}
				catch (GrammarException ex)
				{
					output.AppendLine($"{ex.Message}");
				}
				_parser.ValueEmitted += Parser_ValueEmitted;
				_parser.TokenEmitted += Parser_TokenEmitted;

				try
				{
					_parser.Parse(input);
				}
				catch (ParserException ex)
				{
					output.AppendLine($"{ex.Message}");
				}
			}

			output.AppendLine($"The signal strength is {signalStrength}.");

			void Parser_ValueEmitted(object sender, ParserEventArgs e)
			{
				output.AppendLine($"value emitted: {e.Value}");
				valueStack.Push(e.Value);
			}
			void Parser_TokenEmitted(object sender, ParserEventArgs e)
			{
				output.AppendLine($"token emitted: {e.Token}");

				if (string.IsNullOrEmpty(e.Token)) return;

				switch (e.Token[0])
				{
					case 's': ScopeController(e.Token); break;
					case 't': TypeChecker(e.Token); break;
					case 'c': CodeGenerator(e.Token); break;
				}
			}
			void ScopeController(string token)
			{
			}
			void TypeChecker(string token)
			{
			}
			void CodeGenerator(string token)
			{
				switch (token)
				{
					case "c_noop":
						{
							cycle++;
							if (cycle >= nextSample)
							{
								int sample = nextSample * x;
								signalStrength += sample;
								output.AppendLine($"cycle = {nextSample}, x = {x}, sample = {sample}, signal = {signalStrength}");
								nextSample += SampleInterval;
							}

							output.AppendLine($"cycle = {cycle}");
						}
						break;
					case "c_negate":
						{
							if (valueStack.Count < 1)
							{
								output.AppendLine("valueStack is empty");
								break;
							}

							string arg = valueStack.Pop();
							valueStack.Push("-" + arg);
						}
						break;
					case "c_addx":
						{
							if (valueStack.Count == 0)
							{
								output.AppendLine("valueStack is empty");
								break;
							}

							int dx = int.Parse(valueStack.Pop());
							cycle += 2;

							if (cycle >= nextSample)
							{
								int sample = nextSample * x;
								signalStrength += sample;
								output.AppendLine($"cycle = {nextSample}, x = {x}, sample = {sample}, signal = {signalStrength}");
								nextSample += SampleInterval;
							}

							x += dx;
							output.AppendLine($"cycle = {cycle}, x = {x}");
						}
						break;
				}
			}
		}

		private static void SolvePart1WithParserHelper(string input, StringBuilder output)
		{
			const int InitialSample = 20;
			const int SampleInterval = 40;

			int cycle = 0;
			int nextSample = InitialSample;
			int x = 1;
			int signalStrength = 0;

			ParserHelper.RunParser(input, output, Resources.Day10Grammar2, null, null,
				(token, valueStack) =>
				{
					switch (token)
					{
						case "c_noop":
							{
								cycle++;
								if (cycle >= nextSample)
								{
									int sample = nextSample * x;
									signalStrength += sample;
									output.AppendLine($"cycle = {nextSample}, x = {x}, sample = {sample}, signal = {signalStrength}");
									nextSample += SampleInterval;
								}

								output.AppendLine($"cycle = {cycle}");
							}
							break;
						case "c_negate":
							{
								if (valueStack.Count < 1)
								{
									output.AppendLine("valueStack is empty");
									break;
								}

								string arg = valueStack.Pop();
								valueStack.Push("-" + arg);
							}
							break;
						case "c_addx":
							{
								if (valueStack.Count == 0)
								{
									output.AppendLine("valueStack is empty");
									break;
								}

								int dx = int.Parse(valueStack.Pop());
								cycle += 2;

								if (cycle >= nextSample)
								{
									int sample = nextSample * x;
									signalStrength += sample;
									output.AppendLine($"cycle = {nextSample}, x = {x}, sample = {sample}, signal = {signalStrength}");
									nextSample += SampleInterval;
								}

								x += dx;
								output.AppendLine($"cycle = {cycle}, x = {x}");
							}
							break;
					}
				});

			output.AppendLine($"The signal strength is {signalStrength}.");
		}

		private static void SolvePart2WithFileParser(string input, StringBuilder output)
		{
			const int ScreenWidth = 40;

			int scanPosition = 0;
			int x = 1;

			var screen = new StringBuilder();

			var valueStack = new Stack<string>();

			using (var _grammar = new L2Grammar())
			using (var _parser = new L2Parser(_grammar))
			{
				try
				{
					_grammar.ReadGrammar(Resources.Day10Grammar2);
				}
				catch (GrammarException ex)
				{
					output.AppendLine($"{ex.Message}");
				}
				_parser.ValueEmitted += Parser_ValueEmitted;
				_parser.TokenEmitted += Parser_TokenEmitted;

				try
				{
					_parser.Parse(input);
				}
				catch (ParserException ex)
				{
					output.AppendLine($"{ex.Message}");
				}
			}

			output.Append(screen);

			void Parser_ValueEmitted(object sender, ParserEventArgs e)
			{
				output.AppendLine($"value emitted: {e.Value}");
				valueStack.Push(e.Value);
			}
			void Parser_TokenEmitted(object sender, ParserEventArgs e)
			{
				output.AppendLine($"token emitted: {e.Token}");

				if (string.IsNullOrEmpty(e.Token)) return;

				switch (e.Token[0])
				{
					case 's': ScopeController(e.Token); break;
					case 't': TypeChecker(e.Token); break;
					case 'c': CodeGenerator(e.Token); break;
				}
			}
			void ScopeController(string token)
			{
			}
			void TypeChecker(string token)
			{
				switch (token)
				{
					case "t_integer":
						{
							if (valueStack.Count < 1)
							{
								output.AppendLine("valueStack is empty");
								break;
							}

							string arg = valueStack.Peek();
							if (!int.TryParse(arg, out var _))
							{
								output.AppendLine("'{arg}' is not an integer");
							}
						}
						break;
				}
			}
			void CodeGenerator(string token)
			{
				switch (token)
				{
					case "c_noop":
						{
							if (Math.Abs(scanPosition - x) < 2)
								screen.Append("@@");
							else
								screen.Append("  ");
							scanPosition++;

							if (scanPosition >= ScreenWidth)
							{
								screen.AppendLine();
								scanPosition = 0;
							}
						}
						break;
					case "c_negate":
						{
							if (valueStack.Count < 1)
							{
								output.AppendLine("valueStack is empty");
								break;
							}

							string arg = valueStack.Pop();
							valueStack.Push("-" + arg);
						}
						break;
					case "c_addx":
						{
							if (Math.Abs(scanPosition - x) < 2)
								screen.Append("@@");
							else
								screen.Append("  ");
							scanPosition++;

							if (scanPosition >= ScreenWidth)
							{
								screen.AppendLine();
								scanPosition = 0;
							}

							if (Math.Abs(scanPosition - x) < 2)
								screen.Append("@@");
							else
								screen.Append("  ");
							scanPosition++;

							if (scanPosition >= ScreenWidth)
							{
								screen.AppendLine();
								scanPosition = 0;
							}

							x += int.Parse(valueStack.Pop());
						}
						break;
				}
			}
		}


		private static void SolvePart2WithParserHelper(string input, StringBuilder output)
		{
			const int ScreenWidth = 40;

			int scanPosition = 0;
			int x = 1;

			var screen = new StringBuilder();

			ParserHelper.RunParser(input, output, Resources.Day10Grammar2, 
				null,
				(token, valueStack) =>
				{
					switch (token)
					{
						case "t_integer":
							{
								if (valueStack.Count < 1)
								{
									output.AppendLine("valueStack is empty");
									break;
								}

								string arg = valueStack.Peek();
								if (!int.TryParse(arg, out var _))
								{
									output.AppendLine("'{arg}' is not an integer");
								}
							}
							break;
					}
				},
				(token, valueStack) =>
				{
					switch (token)
					{
						case "c_noop":
							{
								if (Math.Abs(scanPosition - x) < 2)
									screen.Append("@@");
								else
									screen.Append("  ");
								scanPosition++;

								if (scanPosition >= ScreenWidth)
								{
									screen.AppendLine();
									scanPosition = 0;
								}
							}
							break;
						case "c_negate":
							{
								if (valueStack.Count < 1)
								{
									output.AppendLine("valueStack is empty");
									break;
								}

								string arg = valueStack.Pop();
								valueStack.Push("-" + arg);
							}
							break;
						case "c_addx":
							{
								if (Math.Abs(scanPosition - x) < 2)
									screen.Append("@@");
								else
									screen.Append("  ");
								scanPosition++;

								if (scanPosition >= ScreenWidth)
								{
									screen.AppendLine();
									scanPosition = 0;
								}

								if (Math.Abs(scanPosition - x) < 2)
									screen.Append("@@");
								else
									screen.Append("  ");
								scanPosition++;

								if (scanPosition >= ScreenWidth)
								{
									screen.AppendLine();
									scanPosition = 0;
								}

								x += int.Parse(valueStack.Pop());
							}
							break;
					}
				});


			output.Append(screen);
		}
	}
}
