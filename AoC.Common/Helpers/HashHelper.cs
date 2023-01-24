using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Common.Helpers;

public class HashHelper
{
	public static string GetKnotHash(string line)
	{
		var lengths = new List<int>();
		foreach (var c in line)
			lengths.Add(c);
		lengths.AddRange(new[] { 17, 31, 73, 47, 23 });

		var sparse = GetSparseHash(256, lengths, 64).ToArray();

		var dense = new StringBuilder();
		for (var i = 0; i < 16; i++)
		{
			var n = 0;
			for (var j = 0; j < 16; j++)
				n ^= sparse[i * 16 + j];
			dense.Append($"{n:x2}");
		}
		return dense.ToString();
	}

	public static IEnumerable<int> GetSparseHash(int listSize, IEnumerable<int> lengths, int rounds)
	{
		var list = new int[listSize];
		for (var i = 0; i < listSize; i++)
			list[i] = i;
		var position = 0;
		var skipSize = 0;

		for (var round = 0; round < rounds; round++)
		{
			foreach (var length in lengths)
			{
				var newList = list.ToArray();
				for (var i = 0; i < length; i++)
				{
					var oldPos = (position + i) % listSize;
					var newPos = (position + length - i - 1) % listSize;
					newList[newPos] = list[oldPos];
				}
				list = newList;
				position = (position + length + skipSize) % listSize;
				skipSize++;
			}
		}

		return list;
	}
}
