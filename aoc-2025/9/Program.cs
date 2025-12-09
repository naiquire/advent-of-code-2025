using lib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// part 2 has a runtime of like 10 minutes but it does work lol

namespace _9
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			string input = await Input.GetInput(9, 2025);
			//string input = "7,1\n11,1\n11,7\n9,7\n9,5\n2,5\n2,3\n7,3\n";

			List<string> lines = Input.ParseList(input, '\n');
			List<(int, int)> coords = new List<(int, int)>();
			foreach (string line in lines)
			{
				var a = Input.ParseIntList(line, ',');
				coords.Add((a[0], a[1]));
			}

			Console.WriteLine(solve1(coords));
			Console.WriteLine("\r" + solve2(coords));
		}
		static ulong solve1(List<(int X, int Y)> coords)
		{
			ulong maxArea = 0;

			for (int i = 0; i < coords.Count; i++)
			{
				for (int j = 0; j < coords.Count; j++)
				{
					if (i == j) continue;
					ulong area = (ulong)(Math.Abs(coords[j].X - coords[i].X) + 1) * (ulong)(Math.Abs(coords[j].Y - coords[i].Y) + 1);
					if (area > maxArea) maxArea = area;
				}
			}

			return maxArea;
		}
		static ulong solve2(List<(int X, int Y)> red)
		{
			ulong maxArea = 0;
			HashSet<(int X, int Y)> green = new HashSet<(int X, int Y)>();

			for (int i = 0; i < red.Count; i++)
			{
				if (red[i].X == red[(i + 1) % red.Count].X)
				{
					int startY = Math.Min(red[i].Y, red[(i + 1) % red.Count].Y) + 1;
					int endY = Math.Max(red[i].Y, red[(i + 1) % red.Count].Y) - 1;

					for (int y = startY; y <= endY; y++)
					{
						green.Add((red[i].X, y));
					}
				}
				if (red[i].Y == red[(i + 1) % red.Count].Y)
				{
					int startX = Math.Min(red[i].X, red[(i + 1) % red.Count].X) + 1;
					int endX = Math.Max(red[i].X, red[(i + 1) % red.Count].X) - 1;

					for (int x = startX; x <= endX; x++)
					{
						green.Add((x, red[i].Y));
					}
				}
			}

			for (int i = 0; i < red.Count; i++)
			{
				Console.Write($"\r{i}/496       ");
				for (int j = 0; j < red.Count; j++)
				{
					if (i == j) continue;

					ulong area = (ulong)(Math.Abs(red[j].X - red[i].X) + 1) * (ulong)(Math.Abs(red[j].Y - red[i].Y) + 1);
					if (area < maxArea)
					{
						continue;
					}

					bool valid = true;

					int minx = Math.Min(red[i].X, red[j].X) + 1;
					int maxx = Math.Max(red[i].X, red[j].X) - 1;
					int miny = Math.Min(red[i].Y, red[j].Y) + 1;
					int maxy = Math.Max(red[i].Y, red[j].Y) - 1;

					for (int x = minx; x <= maxx; x++)
					{
						if (green.Contains((x, miny)))
						{
							valid = false;
							break;
						}
						if (green.Contains((x, maxy)))
						{
							valid = false;
							break;
						}
					}
					for (int y = miny; y <= maxy; y++)
					{
						if (green.Contains((minx, y)))
						{
							valid = false;
							break;
						}
						if (green.Contains((maxx, y)))
						{
							valid = false;
							break;
						}
					}
					if (!valid) continue;

					if (area > maxArea)
					{
						maxArea = area;
					}
				}
			}

			return maxArea;
		}
	}
}
