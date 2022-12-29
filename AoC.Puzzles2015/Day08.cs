using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2015.Properties;

namespace AoC.Puzzles2015;

[Export(typeof(IPuzzle))]
public class Day08 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2015;

	public int Day => 08;

	public string Name => $"Day 08";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day08Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day08(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataForPart1();

		return result;
	}

	private string SolvePart2(string input)
	{
		LoadDataFromInput(input);

		var result = ProcessDataForPart2();

		return result;
	}

	#endregion Solvers

	private readonly List<string> lines = new();

	private void LoadDataFromInput(string input)
	{
		//  First Clear Data
		lines.Clear();

		InputHelper.TraverseInputLines(input, line =>
		{
			lines.Add(line);
		});
	}

	private string ProcessDataForPart1()
	{
		int totalCodeSize = 0;
		int totalTextSize = 0;
		int totalExtraSize = 0;

		foreach (var line in lines)
		{
			int codeSize = line.Length;
			var text = line;
			
			if (text.StartsWith("\"") && text.EndsWith("\""))
			{
				text = text.Substring(1, text.Length - 2);
			}
			text = text.Replace("\\\"", "\"");
			text = text.Replace("\\\\", "\\");

			var i = text.IndexOf("\\x");
			while (i > -1)
			{
				if (i + 4 > text.Length)
					break;
				var hex = text.Substring(i + 2, 2);
				if (!int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int value))
					text = text.Replace($"\\x{hex}", $"\\y{hex}");
				text = text.Replace($"\\x{hex}", ".");
				i = text.IndexOf("\\x");
			}

			int textSize = text.Length;
			int extraSize = codeSize - textSize;

			logger.SendDebug(nameof(Day08), $"{line} => {text} ({codeSize} - {textSize} = {extraSize})");

			totalCodeSize += codeSize;
			totalTextSize += textSize;
			totalExtraSize += extraSize;
		}

		logger.SendDebug(nameof(Day08), $"{totalCodeSize} - {totalTextSize} = {totalExtraSize}");

		return totalExtraSize.ToString();
	}

	private string ProcessDataForPart2()
	{
		int totalCodeSize = 0;
		int totalTextSize = 0;
		int totalExtraSize = 0;

		foreach (var line in lines)
		{
			int textSize = line.Length;
			var code = line;

			code = code.Replace("\\", "\\\\");
			code = code.Replace("\"", "\\\"");
			code = $"\"{code}\"";

			int codeSize = code.Length;
			int extraSize = codeSize - textSize;

			logger.SendDebug(nameof(Day08), $"{line} => {code} ({codeSize} - {textSize} = {extraSize})");

			totalCodeSize += codeSize;
			totalTextSize += textSize;
			totalExtraSize += extraSize;
		}

		logger.SendDebug(nameof(Day08), $"{totalCodeSize} - {totalTextSize} = {totalExtraSize}");

		return totalExtraSize.ToString();
	}
}
