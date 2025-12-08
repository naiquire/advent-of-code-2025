using lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _8
{
	internal class Program
	{
		const int part1Iterations = 1000;
		static async Task Main(string[] args)
		{
			string input = await Input.GetInput(8, 2025);
			//string input = "162,817,812\n57,618,57\n906,360,560\n592,479,940\n352,342,300\n466,668,158\n542,29,236\n431,825,988\n739,650,466\n52,470,668\n216,146,977\n819,987,18\n117,168,530\n805,96,715\n346,949,466\n970,615,88\n941,993,340\n862,61,35\n984,92,344\n425,690,689";

			List<string> lines = Input.ParseList(input, '\n');
			List<(int, int, int)> data = new List<(int, int, int)>();
			foreach (string line in lines)
			{
				var a = Input.ParseIntList(line, ',');
				data.Add((a[0], a[1], a[2]));
			}

			Console.WriteLine(solve1(data));
			Console.WriteLine(solve2(data));
		}
		static int solve1(List<(int X, int Y, int Z)> coords)
		{
			double[,] adjMatrix = buildMatrix(coords);
			List<HashSet<(int X, int Y, int Z)>> circuits = new List<HashSet<(int X, int Y, int Z)>>();

			List<(int, int)> orderedArcs = sortArcs(adjMatrix);
			int arcIndex = 0;

			for (int i = 0; i < part1Iterations; i++)
			{
				(int n1, int n2) minArcIndex = orderedArcs[arcIndex];
				mergeCircuits(ref circuits, coords, minArcIndex);

				arcIndex++;
			}

			circuits = circuits.OrderByDescending(c => c.Count).ToList();
			return circuits[0].Count * circuits[1].Count * circuits[2].Count;
		}
		static ulong solve2(List<(int X, int Y, int Z)> coords)
		{
			double[,] adjMatrix = buildMatrix(coords);
			List<HashSet<(int X, int Y, int Z)>> circuits = new List<HashSet<(int X, int Y, int Z)>>();

			List<(int, int)> orderedArcs = sortArcs(adjMatrix);
			int arcIndex = 0;

			while (true)
			{
				(int n1, int n2) minArcIndex = orderedArcs[arcIndex];
				mergeCircuits(ref circuits, coords, minArcIndex);

				if (circuits.Count == 1 && circuits[0].Count == coords.Count)
				{
					return (ulong)coords[minArcIndex.n1].X * (ulong)coords[minArcIndex.n2].X;
				}

				arcIndex++;
			}
		}

		static double[,] buildMatrix(List<(int X, int Y, int Z)> coords)
		{
			double[,] adjMatrix = new double[coords.Count, coords.Count];
			for (int i = 0; i < coords.Count; i++)
			{
				for (int j = 0; j < coords.Count; j++)
				{
					double dist;
					if (i == j) dist = int.MaxValue;
					else
					{
						dist = Math.Sqrt(Math.Pow(coords[i].X - coords[j].X, 2) +
										 Math.Pow(coords[i].Y - coords[j].Y, 2) +
										 Math.Pow(coords[i].Z - coords[j].Z, 2));
					}

					adjMatrix[i, j] = dist;
				}
			}
			return adjMatrix;
		}
		static List<(int, int)> sortArcs(double[,] adjMatrix)
		{
			List<(int n1, int n2)> arcs = new List<(int, int)>();
			for (int j = 0; j < adjMatrix.GetLength(0); j++)
			{
				for (int k = j + 1; k < adjMatrix.GetLength(1); k++)
				{
					arcs.Add((j, k));
				}
			}

			return arcs.OrderBy(a => adjMatrix[a.n1, a.n2]).ToList();
		}
		static void mergeCircuits(ref List<HashSet<(int X, int Y, int Z)>> circuits, List<(int X, int Y, int Z)> coords, (int n1, int n2) minArcIndex)
		{
			int index1 = -1;
			int index2 = -1;

			// find any circuits that contain either coord
			for (int c = 0; c < circuits.Count; c++)
			{
				if (circuits[c].Contains(coords[minArcIndex.n1]))
				{
					index1 = c;
				}
				if (circuits[c].Contains(coords[minArcIndex.n2]))
				{
					index2 = c;
				}

				if (index1 != -1 && index2 != -1)
				{
					if (index1 == index2) return;
					break;
				}
			}

			if (index1 == -1 && index2 == -1) // no connection found, create new circuit
			{
				circuits.Add(new HashSet<(int X, int Y, int Z)>() { coords[minArcIndex.n1], coords[minArcIndex.n2] });
			}
			else if (index1 == -1) // index2 is in a circuit, add arcindex1
			{
				circuits[index2].Add(coords[minArcIndex.n1]);
			}
			else if (index2 == -1) // index1 is in a circuit, add arcindex2
			{
				circuits[index1].Add(coords[minArcIndex.n2]);
			}
			else if (index1 != index2) // both coords found in different circuits, merge circuits
			{
				foreach (var coord in circuits[index2])
				{
					circuits[index1].Add(coord);
				}
				circuits.RemoveAt(index2);
			}
		}
	}
}
