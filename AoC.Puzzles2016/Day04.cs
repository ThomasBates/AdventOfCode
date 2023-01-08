using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day04 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 04;

	public string Name => $"Day 04";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day04Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day04(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	#region Solvers

	private string SolvePart1(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart1(data);

		return result.ToString();
	}

	private string SolvePart2(string input)
	{
		var data = LoadDataFromInput(input);

		var result = ProcessDataForPart2(data);

		return result.ToString();
	}

	#endregion Solvers

	private class Room
	{
		public string RawCode;
		public string Name;
		public int Sector;
		public string Checksum;
		public bool isDecoy;
		public string RealName;
	}

	private List<Room> LoadDataFromInput(string input)
	{
		var data = new List<Room>();

		InputHelper.TraverseInputLines(input, line =>
		{
			var checksumPos = line.IndexOf('[');
			var sectorPos = line.LastIndexOf('-', checksumPos);

			var room = new Room
			{
				RawCode = line,
				Name = line.Substring(0, sectorPos),
				Sector = int.Parse(line.Substring(sectorPos + 1, checksumPos - sectorPos - 1)),
				Checksum = line.Substring(checksumPos + 1, 5)
			};

			data.Add(room);
		});
		
		return data;
	}

	private int ProcessDataForPart1(List<Room> data, bool doLogging = true)
	{
		int total = 0;

		foreach(var room in data)
		{
			var counts = new Dictionary<char, int>();

			foreach (char c in room.Name)
			{
				if (c == '-')
					continue;

				if (!counts.TryGetValue(c, out var count))
					count = 0;
				counts[c] = count + 1;
			}

			var ordered = counts.OrderByDescending(c => c.Value).ThenBy(c => c.Key).Select(c => c.Key);
			var checksum = string.Join("", ordered).Substring(0,5);
			if (checksum == room.Checksum)
			{
				total+= room.Sector;
				if (doLogging)
					logger.SendDebug(nameof(Day04), $"{room.Name}-{room.Sector}[{room.Checksum}] <==> {checksum} - MATCH");
			}
			else
			{
				room.isDecoy = true;
				if (doLogging)
					logger.SendDebug(nameof(Day04), $"{room.Name}-{room.Sector}[{room.Checksum}] <==> {checksum} - NO MATCH");
			}
		}

		return total;
	}

	private int ProcessDataForPart2(List<Room> data)
	{
		ProcessDataForPart1(data, false);

		foreach (var room in data)
		{
			int n = room.Sector % 26;
			var realName = new StringBuilder();

			foreach (char c in room.Name.ToLower())
			{
				if (c == '-')
					realName.Append(' ');
				else
				{
					if (c + n <= 'z')
						realName.Append((char)(c + n));
					else
						realName.Append((char)(c + n - 26));
				}
			}
			room.RealName = realName.ToString();


			if (!room.isDecoy && (room.RealName.Contains("north") || room.RealName.Contains("pole")))
			{
				logger.SendDebug(nameof(Day04), $"{realName} ({(room.isDecoy ? "DECOY" : "REAL")}) ({room.Name}-{room.Sector}[{room.Checksum}])");
				return room.Sector;
			}
		}

		return 0;
	}
}
