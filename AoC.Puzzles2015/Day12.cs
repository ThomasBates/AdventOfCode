using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
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
		//  First Clear Data
		int result = 0;
		int count = 0;

		var parser = GrammarHelper.CreateParser(logger, Resources.Day12Grammar, 
			null, 
			null,
			(token, valueStack) =>
			{
				switch (token)
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
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			});

		InputHelper.TraverseInputLines(input, line =>
		{
			var key = line.Length < 250 ? line : (line.Substring(0, 250) + "...");
			logger.SendDebug(nameof(Day12), $"line = {key}");

			count = 0;

			GrammarHelper.ParseInput(logger, parser, line);

			logger.SendDebug(nameof(Day12), $"count = {count}");
			result = count;
		});

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var scopeStack = new Stack<string>();
		var isRedStack = new Stack<bool>();
		var countStack = new Stack<int>();

		var parser = GrammarHelper.CreateParser(null, Resources.Day12Grammar,
			(token, valueStack) =>
			{
				switch (token)
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
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			},
			(token, valueStack) =>
			{
				switch (token)
				{
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
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			},
			(token, valueStack) =>
			{
				switch (token)
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
							if (!isRedStack.Peek())
							{
								int count = countStack.Pop();
								count += number;
								countStack.Push(count);
							}
						}
						break;
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			});

		int result = 0;

		InputHelper.TraverseInputLines(input, line =>
		{
			var key = line.Length < 250 ? line : (line.Substring(0, 250) + "...");
			logger.SendDebug(nameof(Day12), $"line = {key}");

			scopeStack.Clear();
			isRedStack.Clear();
			countStack.Clear();

			scopeStack.Push("array");
			isRedStack.Push(false);
			countStack.Push(0);

			GrammarHelper.ParseInput(logger, parser, line);

			int count = countStack.Pop();
			logger.SendDebug(nameof(Day12), $"count = {count}");
			result = count;
		});

		return result.ToString();
	}

	#endregion Solvers
}
