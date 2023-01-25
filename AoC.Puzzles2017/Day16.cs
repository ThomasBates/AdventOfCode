using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day16 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 16;

	public string Name => $"Day 16";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day16Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day16(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day16), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day16), message);

	#endregion Helpers

	private List<string> LoadData(string input)
	{
		var moves = new List<string>();

		InputHelper.TraverseInputLines(input, line =>
		{
			var parts = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var part in parts)
				moves.Add(part);
		});
		
		return moves;
	}

	private string SolvePart1(List<string> moves)
	{
		var dancers = (moves.Count < 10 ? "abcde" : "abcdefghijklmnop");

		dancers = DoDance(dancers, moves);

		return dancers;
	}

	private string SolvePart2(List<string> moves)
	{
		var dancers = (moves.Count < 10 ? "abcde" : "abcdefghijklmnop");
		var dancerCount = dancers.Length;

		var partnerDancers = DoPartnerDance(dancers, moves);
		var shiftDancers = DoShiftDance(dancers, moves);

		SendDebug($"after partner dance: {partnerDancers}");
		SendDebug($"after shift dance: {shiftDancers}");

		SendDebug($"after partner/shift dance: {DoShiftDance(partnerDancers, moves)}");
		SendDebug($"after shift/partner dance: {DoPartnerDance(shiftDancers, moves)}");
		SendDebug();

		var map = new char[dancerCount];
		for (var i = 0; i < dancerCount; i++)
			map[i] = (char)('a' + partnerDancers.IndexOf(dancers[i]) - 1);
		var partnerMap = new string(map);
		partnerMap = partnerDancers;

		var shiftMap = new int[dancerCount];
		for (var i = 0; i < dancerCount; i++)
			shiftMap[i] = shiftDancers.IndexOf(dancers[i]);

		SendDebug($"partner map: {partnerMap}");
		SendDebug($"shift map: {string.Join(",", shiftMap)}");

		var a = dancers.ToArray();
		var b = new char[dancerCount];
		for (var i = 0; i < 1000000000; i++)
		{
			//  140 ns
			if (i < 5)
				SendDebug($"round {i + 1}: start: {new string(a)}");

			for (var j = 0; j < dancerCount; j++)
				b[j] = partnerMap[a[j] - 'a'];

			if (i < 5)
				SendDebug($"round {i + 1}: prtnr: {new string(b)}");

			for (var j = 0; j < dancerCount; j++)
				a[shiftMap[j]] = b[j];

			if (i < 5)
			{
				SendDebug($"round {i + 1}: shift: {new string(a)}");
				SendDebug();
			}
		}

		return new string(a);
	}

	//  6 ms
	private string DoDance(string dancers, IEnumerable<string> moves)
	{
		foreach (var move in moves)
		{
			switch (move[0])
			{
				case 's':
					{
						var x = int.Parse(move.Substring(1));
						var newDancers = new char[dancers.Length];
						for (var i = 0; i < newDancers.Length; i++)
						{
							var j = (i + x) % dancers.Length;
							newDancers[j] = dancers[i];
						}
						dancers = new string(newDancers);
					}
					break;
				case 'x':
					{
						var parts = move.Substring(1).Split('/');
						var a = int.Parse(parts[0]);
						var b = int.Parse(parts[1]);
						var newDancers = dancers.ToArray();
						newDancers[a] = dancers[b];
						newDancers[b] = dancers[a];
						dancers = new string(newDancers);
					}
					break;
				case 'p':
					{
						var parts = move.Substring(1).Split('/');
						var a = parts[0];
						var b = parts[1];
						var i = dancers.IndexOf(a);
						var j = dancers.IndexOf(b);
						var newDancers = dancers.ToArray();
						newDancers[i] = dancers[j];
						newDancers[j] = dancers[i];
						dancers = new string(newDancers);
					}
					break;
			}
		}
		return dancers;
	}

	private string DoPartnerDance(string dancers, List<string> moves) =>
		DoDance(dancers, moves.Where(m => m[0] == 'p'));

	private string DoShiftDance(string dancers, List<string> moves) =>
		DoDance(dancers, moves.Where(m => m[0] != 'p'));
}
