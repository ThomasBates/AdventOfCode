using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day17 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 17;

	public string Name => $"Day 17";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs (1)", "ihgpwlah"},
		{"Example Inputs (2)", "kglvqrro"},
		{"Example Inputs (3)", "ulqzkmiv"},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day17(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day17), message);

	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day17), message);

	#endregion Helpers

	private string LoadData(string input)
	{
		var passcode = "";

		InputHelper.TraverseInputLines(input, line =>
		{
			passcode = line;
		});

		return passcode;
	}

	private string SolvePart1(string passcode)
	{
		var allPaths = FindAllPaths(passcode);

		var ordered = allPaths.OrderBy(s => s.Path.Length);

		return ordered.FirstOrDefault().Path;
	}

	private int SolvePart2(string passcode)
	{
		var allPaths = FindAllPaths(passcode);

		var ordered = allPaths.OrderByDescending(s => s.Path.Length);

		return ordered.FirstOrDefault().Path.Length;
	}

	private class State
	{
		public int X;
		public int Y;
		public string Path = "";

		public override string ToString() => $"({X},{Y})[{Path}]";
	}

	private List<State> FindAllPaths(string passcode)
	{
		var allPaths = new List<State>();
		var initialState = new State();

		LoggerSendVerbose($"Start:  {initialState}");

		HashAlgorithm algorithm = MD5.Create();
		var hash = GetHash(algorithm, passcode, initialState.Path);

		TryPath(algorithm, passcode, allPaths, initialState, 0, -1, hash[0], "U");
		TryPath(algorithm, passcode, allPaths, initialState, 0, +1, hash[1], "D");
		TryPath(algorithm, passcode, allPaths, initialState, -1, 0, hash[2], "L");
		TryPath(algorithm, passcode, allPaths, initialState, +1, 0, hash[3], "R");

		return allPaths;
	}

	private void TryPath(HashAlgorithm algorithm, string passcode, List<State> allPaths, State state, int dx, int dy, char hashDigit, string pathStep)
	{
		if (!"bcdef".Contains(hashDigit))
			return;

		var newX = state.X + dx;
		var newY = state.Y + dy;

		if (newX < 0 || newX > 3 || newY < 0 || newY > 3)
			return;

		var newState = new State
		{
			X = state.X + dx,
			Y = state.Y + dy,
			Path = state.Path + pathStep
		};

		if (newState.X == 3 && newState.Y == 3)
		{
			LoggerSendVerbose($"Final:  {newState}  ##################################");
			allPaths.Add(newState);
			return;
		}

		LoggerSendVerbose($"Trying: {newState}");

		var hash = GetHash(algorithm, passcode, newState.Path);

		TryPath(algorithm, passcode, allPaths, newState, 0, -1, hash[0], "U");
		TryPath(algorithm, passcode, allPaths, newState, 0, +1, hash[1], "D");
		TryPath(algorithm, passcode, allPaths, newState, -1, 0, hash[2], "L");
		TryPath(algorithm, passcode, allPaths, newState, +1, 0, hash[3], "R");
	}

	private string GetHash(HashAlgorithm algorithm, string passcode, string path)
	{
		byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes($"{passcode}{path}");

		byte[] hashBytes = algorithm.ComputeHash(inputBytes);

		string hash = string.Join("", hashBytes.Select(b => b.ToString("x2")));
		return hash;
	}
}
