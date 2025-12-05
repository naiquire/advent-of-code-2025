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
		static ulong solve2(List<string> ranges)
		{
			ulong total = 0;
			List<(ulong l, ulong h)> rv = new List<(ulong h, ulong l)>();

			// perform the first pass
			foreach (var range in ranges)
			{
				string[] s = range.Split('-');
				ulong[] bounds = Array.ConvertAll(s, ulong.Parse);
				if (rv.Count == 0)
				{
					rv.Add((bounds[0], bounds[1]));
					continue;
				}

				bool foundReplace = merge(bounds);
				if (!foundReplace)
				{
					// no bounds contained
					rv.Add((bounds[0], bounds[1]));
				}
			}

			// perform subsequent passes until no more merges can be made
			while (true)
			{
				bool changed = false;
				foreach (var range in rv)
				{
					ulong[] bounds = new ulong[] { range.l, range.h };
					if (merge(bounds))
					{
						changed = true;
						break;
					}
				}
				if (!changed)
				{
					break;
				}
			}

			foreach (var range in rv.ToHashSet())
			{
				total += (range.h - range.l) + 1;
			}

			return total;

			bool merge(ulong[] bounds)
			{
				bool foundReplace = false;
				for (int i = 0; i < rv.Count; i++)
				{
					// lower bound contained but upper bound exceeds
					if (bounds[0] >= rv[i].l && bounds[0] <= rv[i].h && bounds[1] > rv[i].h)
					{
						ulong temp = rv[i].l;
						rv.RemoveAt(i);
						rv.Add((temp, bounds[1]));
						foundReplace = true;
						break;
					}
					// upper bound contained but lower bound exceeds
					if (bounds[1] <= rv[i].h && bounds[1] >= rv[i].l && bounds[0] < rv[i].l)
					{
						ulong temp = rv[i].h;
						rv.RemoveAt(i);
						rv.Add((bounds[0], temp));
						foundReplace = true;
						break;
					}
					// bounds are completely contained
					if (bounds[0] >= rv[i].l && bounds[1] >= rv[i].l && bounds[0] <= rv[i].h && bounds[1] <= rv[i].h)
					{
						if ((bounds[0], bounds[1]) == rv[i]) continue;

						rv.Remove((bounds[0], bounds[1]));
						foundReplace = true;
						break;
					}
				}
				return foundReplace;
			}
		}
	}
}
