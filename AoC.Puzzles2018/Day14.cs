using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day14 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 14;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs 01", Resources.Day14Inputs},
		{"Example Inputs 02", Resources.Day14Inputs2},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day14()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	public string SolvePart1(string input)
	{
		var result = new StringBuilder();

		InputHelper.TraverseInputLines(input, line =>
		{
			int criticalRecipe = int.Parse(line);
			int maxRecipes = criticalRecipe + 10;

			byte[] recipes = new byte[maxRecipes + 10];

			int elf1 = 0;
			int elf2 = 1;

			recipes[elf1] = 3;
			recipes[elf2] = 7;

			int recipeCount = 2;

			//DrawRecipes(recipes, recipeCount, elf1, elf2, result);

			while (recipeCount < maxRecipes)
			{
				//	Make New Recipes.
				int sum = recipes[elf1] + recipes[elf2];
				if (sum > 9)
				{
					recipes[recipeCount] = 1;
					recipeCount++;
				}
				recipes[recipeCount] = (byte)(sum % 10);
				recipeCount++;

				//	Move the elves.
				elf1 = (elf2 + recipes[elf1] + 1) % recipeCount;
				elf2 = (elf2 + recipes[elf2] + 1) % recipeCount;

				//DrawRecipes(recipes, recipeCount, elf1, elf2, result);
			}

			for (int r = criticalRecipe; r < maxRecipes; r++)
			{
				result.Append(recipes[r].ToString());
			}
			result.AppendLine();
		});

		return result.ToString();
	}

	private void DrawRecipes(byte[] recipes, int recipeCount, int elf1, int elf2, StringBuilder result)
	{
		for (int r = 0; r < recipeCount; r++)
		{
			if (r == elf1)
				result.Append($"({recipes[r]})");
			else if (r == elf2)
				result.Append($"[{recipes[r]}]");
			else
				result.Append($" {recipes[r]} ");
		}
		result.AppendLine();
	}

	public string SolvePart2(string input)
	{
		var result = new StringBuilder();

		InputHelper.TraverseInputLines(input, line =>
		{
			int Tail = int.Parse(line);

			var tail = new List<byte>();
			while (Tail > 0)
			{
				tail.Insert(0, (byte)(Tail % 10));
				Tail /= 10;
			}

			var recipes = new List<byte> { 3, 7 };
			int elf1 = 0;
			int elf2 = 1;

			//DrawRecipes(recipes, elf1, elf2, result);

			bool done;
			while (true)
			{
				//	Make New Recipes.
				int sum = recipes[elf1] + recipes[elf2];
				if (sum > 9)
				{
					recipes.Add(1);
					done = CheckTail(recipes, tail);
					if (done)
						break;
				}
				recipes.Add((byte)(sum % 10));
				done = CheckTail(recipes, tail);
				if (done)
					break;

				//	Move the elves.
				elf1 = (elf1 + recipes[elf1] + 1) % recipes.Count;
				elf2 = (elf2 + recipes[elf2] + 1) % recipes.Count;

				//DrawRecipes(recipes, elf1, elf2, result);
			}

			result.AppendLine($"{line} first appears after {recipes.Count - tail.Count} recipes.");
		});

		return result.ToString();
	}

	private void DrawRecipes(List<byte> recipes, int elf1, int elf2, StringBuilder result)
	{
		for (int r = 0; r < recipes.Count; r++)
		{
			if (r == elf1)
				result.Append($"({recipes[r]})");
			else if (r == elf2)
				result.Append($"[{recipes[r]}]");
			else
				result.Append($" {recipes[r]} ");
		}
		result.AppendLine();
	}

	private bool CheckTail(List<byte> recipes, List<byte> tail)
	{
		if (recipes.Count < tail.Count)
		{
			return false;
		}

		for (int i = 0; i < tail.Count; i++)
		{
			if (recipes[recipes.Count-1 - i] != tail[tail.Count-1 - i])
			{
				return false;
			}
		}
		return true;
	}
}
