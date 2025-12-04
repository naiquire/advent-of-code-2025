using lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			string input = await Input.GetInput(4, 2025);
			//string input = "..@@.@@@@.\n@@@.@.@.@@\n@@@@@.@.@@\n@.@@@@..@.\n@@.@@@@.@@\n.@@@@@@@.@\n.@.@.@.@@@\n@.@@@.@@@@\n.@@@@@@@@.\n@.@.@@@.@.";
			char[,] grid = Input.ParseCharGrid(input);

			Console.WriteLine(solve1(grid));
			Console.WriteLine(solve2(grid));
		}
		static int solve1(char[,] grid)
		{
			int total = 0;
			HashSet<(int, int)> pos = A.FindAllChar('@', grid);
			foreach ((int x, int y) p in pos)
			{
				int adj = 0;
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						if (i == 0 && j == 0) continue;
						(int x, int y) neighbor = (p.x + i, p.y + j);
						if (pos.Contains(neighbor))
						{
							adj++;
						}
					}
				}
				if (adj < 4)
				{
					total++;
				}
			}

			return total;
		}
		static int solve2(char[,] grid)
		{
			int total = 0;

			while (true)
			{
				int removed = remove();
				if (removed == 0) break;
				total += removed;
			}

			int remove()
			{
				int removed = 0;
				HashSet<(int, int)> pos = A.FindAllChar('@', grid);
				foreach ((int x, int y) p in pos)
				{
					int adj = 0;
					for (int i = -1; i <= 1; i++)
					{
						for (int j = -1; j <= 1; j++)
						{
							if (i == 0 && j == 0) continue;
							(int x, int y) neighbor = (p.x + i, p.y + j);
							if (pos.Contains(neighbor))
							{
								adj++;
							}
						}
					}
					if (adj < 4)
					{
						removed++;
						grid[p.x, p.y] = 'x';
					}
				}
				return removed;
			}

			return total;
		}
	}
}
