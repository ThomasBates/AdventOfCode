using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Text;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day13 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 13;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs 01", Resources.Day13Inputs},
		{"Example Inputs 02", Resources.Day13Inputs2},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day13()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	private enum Turn
	{
		Left, Straight, Right
	}

	private class Cart
	{
		public Point Location;
		public Point Direction;
		public Turn NextTurn;
	}

	private void LoadDataFromInput(string input, List<string> map)
	{
		map.Clear();

		InputHelper.TraverseInputLines(input, line =>
		{
			map.Add(line);
		});
	}

	public string SolvePart1(string input)
	{
		var result = new StringBuilder();

		var map = new List<string>();

		LoadDataFromInput(input, map);

		var carts = new List<Cart>();

		FindCarts(map, carts);

		int tick = 0;
		while (true)
		{
			Tick(map, carts, result);
			tick++;

			if (carts.Count == 1)
			{
				result.AppendLine($"the location of the last cart is ({carts[0].Location.X},{carts[0].Location.Y}) after {tick} ticks.");
				break;
			}
			else if (carts.Count == 0)
			{
				result.AppendLine($"There are no carts left after {tick} ticks.");
				break;
			}
		}

		return result.ToString();
	}

	public string SolvePart2(string input)
	{
		var result = new StringBuilder();

		var map = new List<string>();

		LoadDataFromInput(input, map);

		var carts = new List<Cart>();

		FindCarts(map, carts);

		int tick = 0;
		while (true)
		{
			Tick(map, carts, result);
			tick++;

			if (carts.Count == 1)
			{
				result.AppendLine($"the location of the last cart is ({carts[0].Location.X},{carts[0].Location.Y}) after {tick} ticks.");
				break;
			}
			else if (carts.Count == 0)
			{
				result.AppendLine($"There are no carts left after {tick} ticks.");
				break;
			}
		}

		return result.ToString();
	}

	private void FindCarts(List<string> map, List<Cart> carts)
	{
		for (int y = 0; y < map.Count; y++)
		{
			var line = map[y];

			FindCarts(ref line, y, carts, ">", 1, 0, "-");
			FindCarts(ref line, y, carts, "<", -1, 0, "-");
			FindCarts(ref line, y, carts, "v", 0, 1, "|");
			FindCarts(ref line, y, carts, "^", 0, -1, "|");

			map[y] = line;
		}
	}

	private void FindCarts(ref string line, int y, List<Cart> carts, string cartSymbol, int dX, int dY, string pathSymbol)
	{
		int x = line.IndexOf(cartSymbol);
		while (x >= 0)
		{
			var cart = new Cart
			{
				Location = new Point(x, y),
				Direction = new Point(dX, dY)
			};
			carts.Add(cart);
			line = line.Substring(0, x) + pathSymbol + line.Substring(x + 1);
			x = line.IndexOf(cartSymbol);
		}
	}

	private void Tick(List<string> map, List<Cart> carts, StringBuilder result)
	{
		foreach (var cart in carts.OrderBy(c => c.Location.Y * 1000 + c.Location.X).ToList())
		{
			cart.Location.Offset(cart.Direction);

			var collisions = carts.Where(c => (c != cart) && (c.Location == cart.Location)).ToList();
			if (collisions.Count > 0)
			{
				result.AppendLine($"Collision occurred at ({cart.Location.X},{cart.Location.Y}).");
				foreach (var collision in collisions)
				{	
					carts.Remove(collision);
				}
				carts.Remove(cart);
				continue;
			}

			char path = map[cart.Location.Y][cart.Location.X];
			switch (path)
			{
				case '|':
					//	do nothing
					break;
				case '-':
					//	do nothing
					break;
				case '/':
					cart.Direction = new Point(-cart.Direction.Y, -cart.Direction.X);
					break;
				case '\\':
					cart.Direction = new Point(cart.Direction.Y, cart.Direction.X);
					break;
				case '+':
					switch (cart.NextTurn)
					{
						case Turn.Left:
							cart.Direction = new Point(cart.Direction.Y, -cart.Direction.X);
							cart.NextTurn = Turn.Straight;
							break;
						case Turn.Straight:
							// do nothing.
							cart.NextTurn = Turn.Right;
							break;
						case Turn.Right:
							cart.Direction = new Point(-cart.Direction.Y, cart.Direction.X);
							cart.NextTurn = Turn.Left;
							break;
					}
					break;
			}
		}
	}
}
