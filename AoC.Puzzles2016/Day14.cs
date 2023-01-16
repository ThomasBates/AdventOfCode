using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day14 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 14;

	public string Name => $"Day 14";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day14Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day14(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => FindKeys(LoadData(input),    0).ToString());
		Solvers.Add("Solve Part 2", input => FindKeys(LoadData(input), 2016).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message) => logger.SendDebug(nameof(Day14), message);
	void LoggerSendVerbose(string message) => logger.SendVerbose(nameof(Day14), message);

	#endregion Helpers

	private string LoadData(string input)
	{
		string salt = "";

		InputHelper.TraverseInputTokens(input, value =>
		{
			salt = value;
		});

		return salt;
	}

	private int FindKeys(string salt, int keyStretch)
	{
		int index = 0;
		int maxIndex = int.MaxValue;

		var keys = new List<(int index, char triplet, string hash)>();
		var candidates = new List<(int index, char triplet, string hash)>();

		using var md5 = MD5.Create();

		while (index < maxIndex)
		{
			var hash = GetHash(md5, salt, index, keyStretch);

			var five = FindFive(hash);
			if (five != (char)0)
			{
				LoggerSendDebug($"Found    five:      {index,6} => {five} {hash}");
				while (candidates.Count > 0 && candidates[0].index < index - 1000)
				{
					LoggerSendVerbose($"Dropping candidate: {candidates[0].index,6} => {candidates[0].triplet} {candidates[0].hash}");
					candidates.RemoveAt(0);
				}

				for (int i = 0; i < candidates.Count;)
				{
					var (candidateIndex, candidateTriplet, candidateHash) = candidates[i];
					LoggerSendVerbose($"Checking candidate: {candidateIndex,6} => {candidateTriplet} {candidateHash}");
					if (candidateTriplet == five)
					{
						LoggerSendDebug($"Adding   key:       {candidateIndex,6} => {candidateTriplet} {candidateHash}");
						keys.Add((candidateIndex, candidateTriplet, candidateHash));
						if (keys.Count == 64)
							maxIndex = index + 1001;
						candidates.RemoveAt(i);
						continue;
					}
					i++;
				}
			}

			var triplet = FindTriplet(hash);
			if (triplet != (char)0)
			{
				LoggerSendVerbose($"Adding   candidate: {index,6} => {triplet} {hash}");
				candidates.Add((index, triplet, hash));
			}

			index++;
		}

		var ordered = keys.OrderBy(k => k.index).ToList();

		for (int i=0; i< ordered.Count; i++)
			LoggerSendDebug($"key {i + 1,2}: {ordered[i].index,6} => {ordered[i].triplet} {ordered[i].hash}");

		return ordered[63].index;
	}

	private string GetHash(HashAlgorithm md5, string salt, int index, int keyStretch)
	{
		byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes($"{salt}{index}");

		byte[] hashBytes = md5.ComputeHash(inputBytes);

		for (int i = 0; i < keyStretch; i++)
		{
			//  https://www.reddit.com/r/adventofcode/comments/5i8pzz/comment/db6q79m/?utm_source=share&utm_medium=web2x&context=3
			inputBytes = new byte[hashBytes.Length * 2];
			for (int j = 0; j < hashBytes.Length; j++)
			{
				var digit0 = hashBytes[j] >> 4;
				var digit1 = hashBytes[j] & 0x0f;
				inputBytes[j * 2 + 0] = (byte)(digit0 < 10 ? digit0 + '0' : digit0 - 10 + 'a');
				inputBytes[j * 2 + 1] = (byte)(digit1 < 10 ? digit1 + '0' : digit1 - 10 + 'a');
			}

			hashBytes = md5.ComputeHash(inputBytes);
		}

		string hash = string.Join("", hashBytes.Select(b => b.ToString("x2")));
		return hash;
	}

	private char FindTriplet(string hash)
	{
		for (int i = 0; i < hash.Length - 2; i++)
		{
			if (hash[i] == hash[i + 1] &&
				hash[i] == hash[i + 2])
			{
				return hash[i];
			}
		}
		return (char)0;
	}

	private char FindFive(string hash)
	{
		for (int i = 0; i < hash.Length - 4; i++)
		{
			if (hash[i] == hash[i + 1] &&
				hash[i] == hash[i + 2] &&
				hash[i] == hash[i + 3] &&
				hash[i] == hash[i + 4])
			{
				return hash[i];
			}
		}
		return (char)0;
	}
}
