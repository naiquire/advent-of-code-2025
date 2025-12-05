using lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _5
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			string input = await Input.GetInput(5, 2025);
			//string input = "3-5\n10-14\n16-20\n12-18\n\n1\n5\n8\n11\n17\n32";

			string[] a = input.Split(new string[] { "\n\n" }, StringSplitOptions.None);
			List<string> ranges = Input.ParseList(a[0], '\n');
			List<string> ids = Input.ParseList(a[1], '\n');


			Console.WriteLine(solve1(ranges, ids));
			Console.WriteLine(solve2(ranges));
		}
		static int solve1(List<string> ranges, List<string> ids)
		{
			int total = 0;

			foreach (var id in ids)
			{
				checkRange(ulong.Parse(id));
			}

			return total;


			void checkRange(ulong id)
			{
				foreach (var range in ranges)
				{
					ulong[] r = Array.ConvertAll(range.Split('-'), ulong.Parse);
					if (r[0] <= id && r[1] >= id)
					{
						total++;
						return;
					}
				}
			}
		}

		static long solve2(List<string> ranges)
		{
			long total = 0;
			(long st, long ed)[] R = new (long st, long ed)[ranges.Count];
			long count = 0;
			foreach (var range in ranges)
			{
				string[] s = range.Split('-');
				R[count] = ((long.Parse(s[0]), long.Parse(s[1])));
				count++;
			}

			Array.Sort(R);
			long current = -1;
			
			for (int i = 0; i < R.Length; i++)
			{
				if (current >= R[i].st)
				{
					// if current is within the range, adjust the start
					long e = R[i].ed;
					R[i] = ((current + 1, e));
				}
				if (R[i].st <= R[i].ed)
				{
					// valid range, including check for adjusted start exceeding end as range is completely contained
					total += (R[i].ed - R[i].st + 1);
				}
				// update current
				current = Math.Max(current, R[i].ed);
			}
			
			return total;
		}
		
	}
}
