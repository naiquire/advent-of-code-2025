using lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			string input = await Input.GetInput(6, 2025);
			//string input = "123 328  51 64 \n 45 64  387 23 \n  6 98  215 314\n*   +   *   +  \n";

			List<string> lines = Input.ParseList(input, '\n');
			List<string[]> array = new List<string[]>();

			// parse input for part 1
			foreach (var line in lines)
			{
				List<string> list = new List<string>();
				string s = line.Trim();
				List<string> values = s.Split(' ').ToList();
				foreach (var v in values)
				{
					if (!string.IsNullOrWhiteSpace(v))
					{
						list.Add(v);
					}
				}
				array.Add(list.ToArray());
			}

			Console.WriteLine(solve1(array));
			Console.WriteLine(solve2(Input.ParseCharGrid(input)));
		}
		static ulong solve1(List<string[]> array)
		{
			ulong total = 0;
			for (int i = 0; i < array[0].Length; i++)
			{
				char op = array[array.Count - 1][i][0];
				ulong subtotal = op == '*' ? (ulong)1 : (ulong)0;

				for (int j = 0; j < array.Count - 1; j++)
				{
					if (op == '*')
					{
						subtotal *= ulong.Parse(array[j][i]);
					}
					if (op == '+')
					{
						subtotal += ulong.Parse(array[j][i]);
					}
				}
				total += subtotal;
			}

			return total;
		}
		static ulong solve2(char[,] array)
		{
			ulong total = 0;
			int lengthY = array.GetLength(0) - 1;
			int lengthX = array.GetLength(1) - 1;

			int opX = lengthX + 1;
			int endX = lengthX;

			// search from right for operator on bottom row
			for (int x = lengthX; x >= 0; x--)
			{
				if (array[lengthY, x] == '+' || array[lengthY, x] == '*')
				{
					// found operator, evaluate columns from endX to opX inclusive
					opX = x;
					char op = array[lengthY, opX];
					
					ulong subtotal = op == '*' ? (ulong)1 : (ulong)0;
					for (int i = endX; i >= opX; i--)
					{
						// read column i up to not including operator row
						string numStr = "";
						for (int y = 0; y < lengthY; y++)
						{
							// pass over spaces
							if (array[y, i] != ' ') numStr += array[y, i];
						}

						// perform operation on read column
						if (op == '*')
						{
							subtotal *= ulong.Parse(numStr);
						}
						if (op == '+')
						{
							subtotal += ulong.Parse(numStr);
						}
					}

					// pass over empty column
					endX = opX - 2;
					x = opX - 1;

					total += subtotal;
				}
			}

			return total;
		}
	}
}
