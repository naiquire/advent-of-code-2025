using lib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _2
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			string input = await Input.GetInput(2, 2025);
			//string input = @"11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";
			List<string> ids = Input.ParseList(input, ',');

			Console.WriteLine(solve1(ids));
			Console.WriteLine(solve2(ids));
		}
		static ulong solve1(List<string> ids)
		{
			ulong invalidTotal = 0;
			foreach (string id in ids)
			{
				var split = id.Split('-');
				(ulong f, ulong l) range = (ulong.Parse(split[0]), ulong.Parse(split[1]));

				for (ulong i = range.f; i <= range.l; i++)
				{
					string s = i.ToString();
					int length = s.Length;
					if (length % 2 != 0) continue;

					if (s.Substring(0, length / 2) == s.Substring(length / 2))
					{
						invalidTotal += i;
					}
				}
			}
			return invalidTotal;
		}
		static ulong solve2(List<string> ids)
		{
			ulong invalidTotal = 0;
			foreach (string id in ids)
			{
				HashSet<ulong> invalids = new HashSet<ulong>();
				var split = id.Split('-');
				(ulong f, ulong l) range = (ulong.Parse(split[0]), ulong.Parse(split[1]));

				// for each value in the given range of ids
				for (ulong i = range.f; i <= range.l; i++)
				{
					// for each set of possible fully repeating sequences
					for (int j = 1; j <= i.ToString().Length / 2; j++)
					{
						if (i.ToString().Length % j != 0) continue;
						string sequence = i.ToString().Substring(0, j);

						// check if the entire number is made up of this sequence
						bool isRepeat = true;
						for (int k = j; k < i.ToString().Length; k += j)
						{
							if (i.ToString().Substring(k, j) != sequence)
							{
								isRepeat = false;
							}
						}
						if (isRepeat && !invalids.Contains(i))
						{
							invalids.Add(i);
							invalidTotal += i;
						}
					}
				}
			}
			return invalidTotal;
		}
	}
}
