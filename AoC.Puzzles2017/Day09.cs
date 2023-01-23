using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day09 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 09;

	public string Name => $"Day 09";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day09Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day09(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message) => logger.SendDebug(nameof(Day09), message);
	void SendVerbose(string message) => logger.SendVerbose(nameof(Day09), message);

	#endregion Helpers

	private Stream LoadData(string input)
	{
		return InputHelper.GenerateStreamFromString(input);
	}

	private int SolvePart1(Stream inputStream)
	{
		var score = ReadStream(inputStream, part1: true);

		inputStream.Dispose();

		return score;
	}

	private int SolvePart2(Stream inputStream)
	{
		var count = ReadStream(inputStream, part1: false);

		inputStream.Dispose();

		return count;
	}

	private int ReadStream(Stream inputStream, bool part1)
	{
		var score = 0;
		var ignoreNext = false;
		var groupLevel = 0;
		var inGarbage = false;
		var count = 0;
		var originalStream = new MemoryStream();

		while (true)
		{
			var b = inputStream.ReadByte();
			if (b == -1)
				break;
			if (b == 10 || b == 13)
				continue;
			originalStream.WriteByte((byte)b);
			var c = (char)b;

			if (ignoreNext)
			{
				ignoreNext = false;
				continue;
			}

			switch (c)
			{
				case '{':
					if (inGarbage)
					{
						count++;
						break;
					}
					if (groupLevel == 0)
					{
						//  beginning of stream.
						score = 0;
						count = 0;
					}
					groupLevel++;
					break;
				case '}':
					if (inGarbage)
					{
						count++;
						break;
					}
					score += groupLevel;
					groupLevel--;
					if (groupLevel == 0)
					{
						//  end of stream
						originalStream.Position = 0;
						using var reader = new StreamReader(originalStream, Encoding.UTF8);
						var original = reader.ReadToEnd();
						if (part1)
							SendDebug($"score = {score}, stream = {original}");
						else
							SendDebug($"count = {count}, stream = {original}");
						originalStream.Dispose();
						originalStream = new MemoryStream();
					}
					break;
				case '<':
					if (inGarbage)
					{
						count++;
						break;
					}
					inGarbage = true;
					break;
				case '>':
					inGarbage = false;
					break;
				case '!':
					ignoreNext = true;
					break;
				default:
					if (inGarbage)
					{
						count++;
						break;
					}
					break;
			}
		}

		originalStream.Dispose();

		return part1 ? score : count;
	}
}
