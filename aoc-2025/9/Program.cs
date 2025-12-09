using lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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

			var sw = new Stopwatch();

			sw.Restart();
			ulong p1 = solve1(coords);
			sw.Stop();
			Console.WriteLine($"part 1: {p1}  ~  {sw.ElapsedMilliseconds}ms");

			sw.Restart();
			ulong p2 = solve2(coords);
			sw.Stop();
			Console.WriteLine($"part 2: {p2}  ~  {sw.ElapsedMilliseconds}ms");
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
		static ulong solve2(List<(int X, int Y)> redTiles)
		{
			ulong maxArea = 0;

			bool[,] linkXY;
			Dictionary<int, int> mapXvalues = new Dictionary<int, int>();
			Dictionary<int, int> mapYvalues = new Dictionary<int, int>();

			List<int> Xcoords = redTiles.Select(x => x.X).Distinct().ToList();
			List<int> Ycoords = redTiles.Select(y => y.Y).Distinct().ToList();

			Xcoords.Sort();
			Ycoords.Sort();

			// co-ordinate compression
			for (int x = 0; x < Xcoords.Count; x++)
			{
				mapXvalues[Xcoords[x]] = x;
			}
			for (int y = 0; y < Ycoords.Count; y++)
			{
				mapYvalues[Ycoords[y]] = y;
			}

			linkXY = new bool[Xcoords.Count, Ycoords.Count];
			foreach (var (X, Y) in redTiles)
			{
				int cx = mapXvalues[X];
				int cy = mapYvalues[Y];
				linkXY[cx, cy] = true;
			}

			bool[,] greenTiles = new bool[Xcoords.Count, Ycoords.Count];


			// find green tiles in outline only
			for (int i = 0; i < redTiles.Count; i++)
			{
				if (redTiles[i].X == redTiles[(i + 1) % redTiles.Count].X)
				{
					int startY = Math.Min(mapYvalues[redTiles[i].Y], mapYvalues[redTiles[(i + 1) % redTiles.Count].Y]) + 1;
					int endY = Math.Max(mapYvalues[redTiles[i].Y], mapYvalues[redTiles[(i + 1) % redTiles.Count].Y]) - 1;

					for (int y = startY; y <= endY; y++)
					{
						greenTiles[mapXvalues[redTiles[i].X], y] = true;
					}
				}
				if (redTiles[i].Y == redTiles[(i + 1) % redTiles.Count].Y)
				{
					int startX = Math.Min(mapXvalues[redTiles[i].X], mapXvalues[redTiles[(i + 1) % redTiles.Count].X]) + 1;
					int endX = Math.Max(mapXvalues[redTiles[i].X], mapXvalues[redTiles[(i + 1) % redTiles.Count].X]) - 1;

					for (int x = startX; x <= endX; x++)
					{
						greenTiles[x, mapYvalues[redTiles[i].Y]] = true;
					}
				}
			}

			for (int i = 0; i < redTiles.Count; i++)
			{
				for (int j = 0; j < redTiles.Count; j++)
				{
					if (i == j) continue;

					bool valid = true;

					int minx = Math.Min(mapXvalues[redTiles[i].X], mapXvalues[redTiles[j].X]) + 1;
					int maxx = Math.Max(mapXvalues[redTiles[i].X], mapXvalues[redTiles[j].X]) - 1;
					int miny = Math.Min(mapYvalues[redTiles[i].Y], mapYvalues[redTiles[j].Y]) + 1;
					int maxy = Math.Max(mapYvalues[redTiles[i].Y], mapYvalues[redTiles[j].Y]) - 1;

					if (minx >= linkXY.GetLength(0) || miny >= linkXY.GetLength(1)) continue;
					if (maxx < 0 || maxy < 0) continue;

					// check if any of the outline tiles are within the current rectangle
					// only checking one tile in on each side is necessary for the input, however is not sufficient for all inputs
					for (int x = minx; x <= maxx; x++)
					{
						if (greenTiles[x, miny] || linkXY[x, miny])
						{
							valid = false;
							break;
						}
						if (greenTiles[x, maxy] || linkXY[x, maxy])
						{
							valid = false;
							break;
						}
					}
					for (int y = miny; y <= maxy; y++)
					{
						if (greenTiles[minx, y] || linkXY[minx, y])
						{
							valid = false;
							break;
						}
						if (greenTiles[maxx, y] || linkXY[maxx, y])
						{
							valid = false;
							break;
						}
					}
					if (!valid) continue;

					ulong area = (ulong)(Math.Abs(redTiles[j].X - redTiles[i].X) + 1) * (ulong)(Math.Abs(redTiles[j].Y - redTiles[i].Y) + 1);
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
