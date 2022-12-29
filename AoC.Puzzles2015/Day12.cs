using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Parser;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day12 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 12;

	public string Name => $"Day 12";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day12Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day12(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		var result = ProcessInputForPart1(input);

		return result;
	}

	private string SolvePart2(string input)
	{
		var result = ProcessInputForPart2(input);

		return result;
	}

	#endregion Solvers

	private string ProcessInputForPart1(string input)
	{
		//  First Clear Data
		int result = 0;

		IL2Grammar grammar = new L2Grammar();
		grammar.OnLogMessageEmitted += (sender, e) => logger.SendDebug(e.Category, e.Message);

		try
		{
			grammar.ReadGrammarDefinition(Resources.Day12Grammar);
		}
		catch (GrammarException ex)
		{
			logger?.SendError("Grammar", $"{ex.GetType().Name}: {ex.Message}");
			return "";
		}

		int count = 0;
		var valueStack = new Stack<string>();

		IParser parser = new L2Parser(grammar);
		parser.OnLogMessageEmitted += (sender, e) => logger.SendDebug(e.Category, e.Message);
		parser.OnValueEmitted += (sender, e) => valueStack.Push(e.Value);
		parser.OnTokenEmitted += (sender, e) =>
		{
			switch (e.Token)
			{
				case "c_negate":
					{
						var number = int.Parse(valueStack.Pop());
						number = -number;
						valueStack.Push(number.ToString());
					}
					break;
				case "c_number":
					{
						var number = int.Parse(valueStack.Pop());
						count += number;
					}
					break;
				default:
					logger.SendError("Parser", $"Unknown token: {e.Token}");
					break;
			}
		};

		Helper.TraverseInputLines(input, line =>
		{
			count = 0;
			valueStack = new Stack<string>();

			try
			{
				parser.Parse(line);
			}
			catch (ParserException ex)
			{
				logger?.SendError("Parser", $"{ex.GetType().Name}: {ex.Message}");
				return;
			}

			var key = line.Length < 250 ? line : line.Substring(0, 250);
			logger.SendDebug(nameof(Day12), $"{count} - {key}");
			result = count;
		});

		return result.ToString();
	}

	private string ProcessInputForPart2(string input)
	{
		//  First Clear Data
		int result = 0;

		IL2Grammar grammar = new L2Grammar();
		grammar.OnLogMessageEmitted += (sender, e) => logger.SendDebug(e.Category, e.Message);

		try
		{
			grammar.ReadGrammarDefinition(Resources.Day12Grammar);
		}
		catch (GrammarException ex)
		{
			logger?.SendError("Grammar", $"{ex.GetType().Name}: {ex.Message}");
			return "";
		}

		//int count = 0;
		var valueStack = new Stack<string>();
		var scopeStack = new Stack<string>();
		var isRedStack = new Stack<bool>();
		var countStack = new Stack<int>();

		IParser parser = new L2Parser(grammar);
		parser.OnLogMessageEmitted += (sender, e) => logger.SendDebug(e.Category, e.Message);
		parser.OnValueEmitted += (sender, e) => valueStack.Push(e.Value);
		parser.OnTokenEmitted += (sender, e) =>
		{
			switch (e.Token)
			{
				case "s_beginArray":
					scopeStack.Push("array");
					countStack.Push(0);
					break;
				case "s_endArray":
					{
						scopeStack.Pop();
						int arrayCount = countStack.Pop();
						if (arrayCount != 0)
						{
							int parentCount = countStack.Pop();
							countStack.Push(parentCount + arrayCount);
						}
					}
					break;
				case "s_beginClass":
					scopeStack.Push("class");
					isRedStack.Push(false);
					countStack.Push(0);
					break;
				case "s_endClass":
					{
						scopeStack.Pop();
						var isRed = isRedStack.Pop();
						int classCount = countStack.Pop();
						if (classCount != 0 && !isRed)
						{
							int parentCount = countStack.Pop();
							countStack.Push(parentCount + classCount);
						}
					}
					break;
				//case "s_beginProperty":
				//	break;
				//case "s_endProperty":
				//	break;
				case "t_checkForRed":
					if (scopeStack.Peek() != "class")
						break;
					if (isRedStack.Peek())
						break;
					if (!valueStack.Peek().ToLower().Contains("red"))
						break;
					isRedStack.Pop();
					isRedStack.Push(true);
					break;
				case "c_negate":
					{
						var number = int.Parse(valueStack.Pop());
						number = -number;
						valueStack.Push(number.ToString());
					}
					break;
				case "c_number":
					{
						var number = int.Parse(valueStack.Pop());
						if (!isRedStack.Peek())
						{
							int count = countStack.Pop();
							count += number;
							countStack.Push(count);
						}
					}
					break;
				default:
					logger.SendError("Parser", $"Unknown token: {e.Token}");
					break;
			}
		};

		Helper.TraverseInputLines(input, line =>
		{
			var key = line.Length < 250 ? line : (line.Substring(0, 250) + "...");
			logger.SendDebug(nameof(Day12), $"line = {key}");

			valueStack.Clear();
			scopeStack.Clear();
			isRedStack.Clear();
			countStack.Clear();

			scopeStack.Push("array");
			isRedStack.Push(false);
			countStack.Push(0);

			try
			{
				parser.Parse(line);
			}
			catch (ParserException ex)
			{
				logger?.SendError("Parser", $"{ex.GetType().Name}: {ex.Message}");
				return;
			}

			int count = countStack.Pop();
			logger.SendDebug(nameof(Day12), $"count = {count}");
			result = count;
		});

		return result.ToString();
	}
}
