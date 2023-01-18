using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Net.Http.Headers;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day21 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 21;

	public string Name => $"Day 21";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day21Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day21(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day21), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day21), message);

	#endregion Helpers

	private List<(string, string, string)> LoadData(string input)
	{
		var operations = new List<(string,string,string)>();

		var arg1 = "abcdefgh";
		var passwordArray = arg1.ToCharArray();
		var passwordLength = arg1.Length;

		GrammarHelper.ParseInput(logger, input, Resources.Day21Grammar,
			scopeControllerAction: null,
			typeCheckerAction: null,
			codeGeneratorAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "c_start":
					case "c_end":
					case "c_rotateLeft":
					case "c_rotateRight":
					case "c_rotateBased":
						var arg = valueStack.Pop();
						operations.Add((token, arg, ""));
						break;
					case "c_swapPosition":
					case "c_swapLetter":
					case "c_reverse":
					case "c_move":
						var arg2 = valueStack.Pop();
						var arg1 = valueStack.Pop();
						operations.Add((token, arg1, arg2));
						break;
				}
			});
		
		return operations;
	}

	private string SolvePart1(List<(string, string, string)> operations)
	{
		var password = "abcdefgh";
		var passwordArray = password.ToCharArray();
		var passwordLength = password.Length;

		LogStep($"starting with");

		foreach (var (op, arg1, arg2) in operations)
		{
			switch (op)
			{
				case "c_start":
					password = arg1;
					passwordArray = password.ToCharArray();
					passwordLength = password.Length;
					LogStep($"starting with");
					break;
				case "c_end":
					LogStep($"ending with {arg1}?");
					break;
				case "c_swapPosition":
					{
						var index1 = int.Parse(arg1);
						var index2 = int.Parse(arg2);
						passwordArray = Swap(passwordArray, index1, index2);
						password = new string(passwordArray);
						LogStep($"swap position {index1} with position {index2}");
					}
					break;
				case "c_swapLetter":
					{
						var letter1 = arg1[0];
						var letter2 = arg2[0];
						passwordArray = Swap(passwordArray, letter1, letter2);
						password = new string(passwordArray);
						LogStep($"swap letter {letter1} with letter {letter2}");
					}
					break;
				case "c_rotateLeft":
					{
						var steps = int.Parse(arg1);
						passwordArray = Rotate(passwordArray, -steps);
						password = new string(passwordArray);
						LogStep($"rotate left {steps} step(s)");
					}
					break;
				case "c_rotateRight":
					{
						var steps = int.Parse(arg1);
						passwordArray = Rotate(passwordArray, steps);
						password = new string(passwordArray);
						LogStep($"rotate right {steps} step(s)");
					}
					break;
				case "c_rotateBased":
					{
						var letter = arg1[0];
						var index = password.IndexOf(letter);
						var steps = 1 + index + (index < 4 ? 0 : 1);
						passwordArray = Rotate(passwordArray, steps);
						password = new string(passwordArray);
						LogStep($"rotate based on position of letter {letter}");
					}
					break;
				case "c_reverse":
					{
						var index1 = int.Parse(arg1);
						var index2 = int.Parse(arg2);
						passwordArray = Reverse(passwordArray, index1, index2);
						password = new string(passwordArray);
						LogStep($"reverse positions {index1} through {index2}");
					}
					break;
				case "c_move":
					{
						var index1 = int.Parse(arg1);
						var index2 = int.Parse(arg2);
						passwordArray = Move(passwordArray, index1, index2);
						password = new string(passwordArray);
						LogStep($"move position {index1} to position {index2}");
					}
					break;
			}
		}

		return password;

		void LogStep(string step) => LoggerSendDebug($"{step,-40} => {password}");
	}

	private string SolvePart2(List<(string, string, string)> operations)
	{
		var password = "fbgdceah";
		var passwordArray = password.ToCharArray();
		var passwordLength = password.Length;

		LogStep($"starting with");

		for (int i = operations.Count - 1; i >= 0; i--)
		{
			var (op, arg1, arg2) = operations[i];
			switch (op)
			{
				case "c_start":
					LogStep($"ending with {arg1}?");
					break;
				case "c_end":
					password = arg1;
					passwordArray = password.ToCharArray();
					passwordLength = password.Length;
					LogStep("starting with");
					break;
				case "c_swapPosition":
					{
						var index1 = int.Parse(arg1);
						var index2 = int.Parse(arg2);
						passwordArray = Swap(passwordArray, index1, index2);
						password = new string(passwordArray);
						LogStep($"swap position {index1} with position {index2}");
					}
					break;
				case "c_swapLetter":
					{
						var letter1 = arg1[0];
						var letter2 = arg2[0];
						passwordArray = Swap(passwordArray, letter1, letter2);
						password = new string(passwordArray);
						LogStep($"swap letter {letter1} with letter {letter2}");
					}
					break;
				case "c_rotateLeft":
					{
						var steps = int.Parse(arg1);
						passwordArray = Rotate(passwordArray, steps);
						password = new string(passwordArray);
						LogStep($"rotate left {steps} step(s)");
					}
					break;
				case "c_rotateRight":
					{
						var steps = int.Parse(arg1);
						passwordArray = Rotate(passwordArray, -steps);
						password = new string(passwordArray);
						LogStep($"rotate right {steps} step(s)");
					}
					break;
				case "c_rotateBased":
					{
						var letter = arg1[0];
						var index = password.IndexOf(letter);
						var steps = 0;
						for (int pos = passwordLength - 1; pos >= 0; pos--) 
						{
							steps = 1 + pos + (pos < 4 ? 0 : 1);
							var newPos = (pos + steps) % passwordLength;
							if (newPos == index)
								break;
						}
						if (steps != 0)
						{
							passwordArray = Rotate(passwordArray, -steps);
							password = new string(passwordArray);
						}
						LogStep($"rotate based on position of letter {letter}");
					}
					break;
				case "c_reverse":
					{
						var index1 = int.Parse(arg1);
						var index2 = int.Parse(arg2);
						passwordArray = Reverse(passwordArray, index1, index2);
						password = new string(passwordArray);
						LogStep($"reverse positions {index1} through {index2}");
					}
					break;
				case "c_move":
					{
						var index1 = int.Parse(arg1);
						var index2 = int.Parse(arg2);
						passwordArray = Move(passwordArray, index2, index1);
						password = new string(passwordArray);
						LogStep($"move position {index1} to position {index2}");
					}
					break;
			}
		}

		return password;

		void LogStep(string step) => LoggerSendDebug($"{step,-40} => {password}");
	}

	private char[] Swap(char[] passwordArray, int index1, int index2)
	{
		var letter1 = passwordArray[index1];
		var letter2 = passwordArray[index2];
		passwordArray[index1] = letter2;
		passwordArray[index2] = letter1;
		return passwordArray;
	}

	private char[] Swap(char[] passwordArray, char letter1, char letter2)
	{
		var password = new string(passwordArray);
		var index1 = password.IndexOf(letter1);
		var index2 = password.IndexOf(letter2);
		passwordArray[index1] = letter2;
		passwordArray[index2] = letter1;
		return passwordArray;
	}

	private char[] Rotate(char[] array, int steps)
	{
		var newArray = new char[array.Length];
		for (int pos = 0; pos < array.Length; pos++)
		{
			var newPos = pos + steps;
			while (newPos < 0)
				newPos += array.Length;
			while (newPos > array.Length - 1)
				newPos -= array.Length;
			newArray[newPos] = array[pos];
		}
		return newArray;
	}

	private char[] Reverse(char[] array, int index1, int index2)
	{
		var newArray = new char[array.Length];
		for (int pos = 0; pos < array.Length; pos++)
		{
			if (pos < index1 || pos > index2)
			{
				newArray[pos] = array[pos];
				continue;
			}

			var newPos = index2 - (pos - index1);
			newArray[newPos] = array[pos];
		}
		return newArray;
	}

	private char[] Move(char[] array, int index1, int index2)
	{
		var newArray = new char[array.Length];
		for (int pos = 0; pos < array.Length; pos++)
		{
			if (pos < Math.Min(index1, index2) ||
				pos > Math.Max(index1, index2))
			{
				newArray[pos] = array[pos];
				continue;
			}

			var newPos = pos == index1
				? index2
				: index1 < index2
					? pos - 1
					: pos + 1;
			newArray[newPos] = array[pos];
		}
		return newArray;
	}
}
