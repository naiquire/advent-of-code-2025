using lib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _1
{
	internal class Program
	{
		static async Task Main()
		{
			string input = await Input.GetInput(1, 2025);

			List<string> s = Input.ParseList(input, '\n');
			Console.WriteLine(solve1(s));
			Console.WriteLine(solve2(s));
		}
		static int solve1(List<string> s)
		{
			int output = 0;
			int pos = 50;

			foreach (string line in s)
			{
				bool isRight = line[0] == 'R';
				int num = int.Parse(line.Substring(1));

				if (isRight)
				{
					pos += num;
					while (pos >= 100) pos -= 100;
				}
				else
				{
					pos -= num;
					while (pos < 0) pos += 100;
				}

				if (pos == 0) output++;
			}

			return output;
		}

		static int solve2(List<string> s)
		{
			int output = 0;
			int pos = 50;

			foreach (string line in s)
			{
				if (line == "") continue;

				bool isRight = line[0] == 'R';
				int num = int.Parse(line.Substring(1));

				if (isRight)
				{
					pos += num;
					while (pos >= 100)
					{
						output++;
						pos -= 100;
					}
				}
				else
				{
					if (pos == 0) output--;
					pos -= num;

					while (pos < 0)
					{
						output++;
						pos += 100;
					}

					if (pos == 0) output++;
				}
			}

			return output;
		}
	}
}
