using lib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _3
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			string input = await Input.GetInput(3, 2025);
			//string input = "987654321111111\n811111111111119\n234234234234278\n818181911112111";
			List<string> lines = Input.ParseList(input, '\n');

			Console.WriteLine(solve(lines, 2));
			Console.WriteLine(solve(lines, 12));
		}
		static ulong solve(List<string> lines, int count)
		{
			ulong total = 0;
			foreach (string l in lines)
			{
				List<int> a = new List<int>();
				foreach (var i in l)
				{
					a.Add(int.Parse(i.ToString()));
				}

				string result = string.Empty;
				int index = 0;

				for (int i = 0; i < count; i++)
				{
					(int num, int index) high = (-1, -1);
					for (int j = index; j < a.Count - count + 1 + i; j++)
					{
						if (a[j] > high.num)
						{
							high = (a[j], j);
						}
					}

					result += high.num.ToString();
					index = high.index + 1;
				}

				total += ulong.Parse(result);
			}

			return total;
		}
	}
}
