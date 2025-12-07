using lib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _7
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			string input = await Input.GetInput(7, 2025);
			//string input = ".......S.......\n...............\n.......^.......\n...............\n......^.^......\n...............\n.....^.^.^.....\n...............\n....^.^...^....\n...............\n...^.^...^.^...\n...............\n..^...^.....^..\n...............\n.^.^.^.^.^...^.\n...............";
			char[,] grid = Input.ParseCharGrid(input);

			Console.WriteLine(solve1(grid));
			Console.WriteLine(solve2(grid));
		}
		static int solve1(char[,] grid)
		{
			(int x, int y) g = (grid.GetLength(0) - 1, grid.GetLength(1) - 1);

			(int x, int y) S = A.FindChar('S', grid);
			HashSet<(int x, int y)> splitters = A.FindAllChar('^', grid);
			HashSet<(int x, int y)> beams = new HashSet<(int, int)>() { S };

			Queue<(int x, int y)> toProcess = new Queue<(int x, int y)>(beams);
			while (toProcess.Count > 0)
			{
				var beam = toProcess.Dequeue();
				if (beam.x == g.x) continue;

				if (grid[beam.x + 1, beam.y] == '^')
				{
					// split beam
					if (beams.Add((beam.x + 1, beam.y - 1))) toProcess.Enqueue((beam.x + 1, beam.y - 1));
					if (beams.Add((beam.x + 1, beam.y + 1))) toProcess.Enqueue((beam.x + 1, beam.y + 1));
				}
				else
				{
					// continue straight
					if (beams.Add((beam.x + 1, beam.y))) toProcess.Enqueue((beam.x + 1, beam.y));
				}
			}

			int total = 0;

			foreach (var splitter in splitters)
			{
				// number of splits is equal to the number of splitters which have a beam directly above them
				if (beams.Contains((splitter.x - 1, splitter.y))) total++;
			}

			return total;
		}
		static ulong solve2(char[,] grid)
		{
			(int x, int y) g = (grid.GetLength(0) - 1, grid.GetLength(1) - 1);

			(int x, int y) S = A.FindChar('S', grid);
			HashSet<(int x, int y)> splitters = A.FindAllChar('^', grid);

			// memoisation dictionary
			Dictionary<(int x, int y), ulong> values = new Dictionary<(int x, int y), ulong>();
			return processBeam(S);

			ulong processBeam((int x, int y) beam)
			{
				if (beam.x == g.x)
				{
					// if beam reached the bottom, then found a valid path
					return 1;
				}
				if (values.ContainsKey(beam))
				{
					// return cached value instead of recalculating
					return values[beam];
				}

				if (grid[beam.x + 1, beam.y] == '^')
				{
					// split beam and recursively calculate both paths
					ulong value = processBeam((beam.x + 1, beam.y - 1)) + processBeam((beam.x + 1, beam.y + 1));
					// cache value
					values[beam] = value;
					return value;
				}
				else
				{
					// linear complexity, no caching needed
					return processBeam((beam.x + 1, beam.y));
				}
			}
		}
	}
}
