using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using AoC.IO;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day13 : IPuzzle
	{
		#region IPuzzle Properties

		public int Year => 2022;

		public int Day => 13;

		public string Name => $"Day {Day:00}";

		public Dictionary<string, string> Inputs { get; } = new()
		{
			{"Example Inputs", Resources.Day13Inputs},
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

		private class PacketElement : IComparable<PacketElement>
		{
			public bool IsList => List != null;
			public int Number;
			public List<PacketElement> List;

			public PacketElement()
			{
				List = new List<PacketElement>();
			}

			public PacketElement(PacketElement element) : this()
			{
				List.Add(element);
			}

			public PacketElement(int number, bool isList = false)
			{
				if (isList)
				{
					List = new List<PacketElement>
					{
						new PacketElement(number)
					};
				}
				else
					Number = number;
			}

			public void AddNumber(int number)
			{
				List.Add(new PacketElement(number));
			}

			public void AddElement(PacketElement element)
			{
				List.Add(element);
			}

			public List<PacketElement> AsList()
			{
				if (IsList)
					return List;
				var result = new List<PacketElement> { this };
				return result;
			}

			public int CompareTo(PacketElement other)
			{
				if (!this.IsList && !other.IsList)
					return this.Number.CompareTo(other.Number);

				var thisList = this.AsList();
				var otherList = other.AsList();

				for (int i = 0; i < Math.Min(thisList.Count, otherList.Count); i++)
				{
					int compare = thisList[i].CompareTo(otherList[i]);
					if (compare != 0)
						return compare;
				}

				if (thisList.Count < otherList.Count)
					return -1;
				if (thisList.Count > otherList.Count)
					return 1;

				return 0;
			}

			public override string ToString()
			{
				if (!IsList)
					return Number.ToString();
				return $"[{string.Join(",", List)}]";
			}
		}

		private string SolvePart1(string input)
		{
			var output = new StringBuilder();

			var allPackets = new List<PacketElement>();

			LoadDataFromInput(input, allPackets, output);

			int result = 0;

			for (int i=0, p = 1; i < allPackets.Count; i += 2, p++)
			{
				var p1 = allPackets[i];
				var p2 = allPackets[i + 1];

				int compare = p1.CompareTo(p2);

				if (compare < 0)
				{
					output.AppendLine($"Pair {p} is in the right order.");
					result += p;
				}
				else
				{
					output.AppendLine($"Pair {p} is NOT in the right order.");
				}
			}

			output.AppendLine($"The answer is {result}");

			return output.ToString();
		}

		private string SolvePart2(string input)
		{
			var output = new StringBuilder();

			var allPackets = new List<PacketElement>();

			LoadDataFromInput(input, allPackets, output);

			var dividerPacket1 = new PacketElement(new PacketElement(2, true));
			allPackets.Add(dividerPacket1);

			var dividerPacket2 = new PacketElement(new PacketElement(6, true));
			allPackets.Add(dividerPacket2);

			var sorted = allPackets.OrderBy(p => p).ToList();

			foreach (var packet in sorted)
			{
				output.AppendLine(packet.ToString());
			}

			var index1 = sorted.IndexOf(dividerPacket1) + 1;
			var index2 = sorted.IndexOf(dividerPacket2) + 1;

			output.AppendLine($"{index1} * {index2} = {index1 * index2}");

			return output.ToString();
		}

		private void LoadDataFromInput(string input, List<PacketElement> allPackets, StringBuilder output)
		{
			var packetStack = new Stack<PacketElement>();
			PacketElement openList = null;

			Helper.ParseInput(input, Resources.Day13Grammar,
				null,
				null,
				(token, valueStack) =>
				{
					switch (token)
					{
						case "c_openList":
							var newList = new PacketElement();
							if (openList != null)
							{
								packetStack.Push(openList);
								openList.AddElement(newList);
							}
							openList = newList;
							break;

						case "c_closeList":
							if (packetStack.Count > 0)
							{
								openList = packetStack.Pop();
							}
							else
							{
								output.AppendLine(openList.ToString());
								allPackets.Add(openList);
								openList = null;
							}
							break;

						case "c_number":
							var number = int.Parse(valueStack.Pop());
							openList.AddNumber(number);
							break;

						default:
							output.AppendLine($"Unknown token: {token}");
							break;
					}
				},
				(severity, category, message) =>
				{
					output.AppendLine($"[{severity,-7}] - [{category,-15}] - {message}");
				});

		}
	}
}
