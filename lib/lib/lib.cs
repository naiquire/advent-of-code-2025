using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace lib
{
	public static class Input
	{
		private static readonly string cookie = Environment.GetEnvironmentVariable("aoc-cookie");
		public static async Task<string> GetInput()
		{
			var uri = new Uri("https://adventofcode.com");
			var cookies = new CookieContainer();
			cookies.Add(uri, new System.Net.Cookie("session", cookie));
			var handler = new HttpClientHandler() { CookieContainer = cookies };
			var client = new HttpClient(handler) { BaseAddress = uri };
			var response = await client.GetAsync($"/{DateTime.Today.Year}/day/{DateTime.Today.Day}/input");
			return await response.Content.ReadAsStringAsync();
		}
		public static async Task<string> GetInput(int day, int year)
		{
			var uri = new Uri("https://adventofcode.com");
			var cookies = new CookieContainer();
			cookies.Add(uri, new System.Net.Cookie("session", cookie));
			var handler = new HttpClientHandler() { CookieContainer = cookies };
			var client = new HttpClient(handler) { BaseAddress = uri };
			var response = await client.GetAsync($"/{year}/day/{day}/input");
			return await response.Content.ReadAsStringAsync();
		}

		public static List<string> ParseList(string s, char csv)
		{
			return s.Trim('\n').Split(csv).ToList();
		}

		public static List<int> ParseIntList(string s, char csv)
		{
			List<int> list = new List<int>();
			foreach (string n in s.Trim('\n').Split(csv).ToList())
			{
				list.Add(int.Parse(n));
			}
			return list;
		}

		public static char[,] ParseCharGrid(string s)
		{
			string[] lines = s.Trim('\n').Split('\n');
			int r = lines.Length;
			int c = lines[0].Length;
			char[,] grid = new char[r, c];
			for (int i = 0; i < r; i++)
			{
				for (int j = 0; j < c; j++)
				{
					grid[i, j] = lines[i][j];
				}
			}
			return grid;
		}
	}
	public static class Log
	{
		public static void Logger(string code, ConsoleColor consoleColor, string text)
		{
			Console.Write("[ ");
			Console.ForegroundColor = consoleColor;
			Console.Write(code.ToUpper());
			Console.ResetColor();
			Console.WriteLine($" ] {text}");
		}
		public static void Color(string text, ConsoleColor consoleColor)
		{
			Console.ForegroundColor = consoleColor;
			Console.Write(text);
			Console.ResetColor();
		}
		public static void ColorLine(string text, ConsoleColor consoleColor)
		{
			Console.ForegroundColor = consoleColor;
			Console.WriteLine(text);
			Console.ResetColor();
		}
	}

	public abstract class A
	{
		public static void Print<T>(T[] array)
		{
			int l = array.Length;
			for (int i = 0; i < l; i++)
			{
				Console.Write($"{array[i]} ");
			}
			Console.WriteLine();
		}
		public static void Print<T>(T[,] array)
		{
			(int x, int y) = (array.GetLength(0), array.GetLength(1));
			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y; y++)
				{
					Console.Write($"{array[i, j]} ");
				}
				Console.WriteLine();
			}
		}

		public static (int, int) FindChar(char element, char[,] array)
		{
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					if (array[i, j] == element)
					{
						return (i, j);
					}
				}
			}
			return (-1, -1);
		}
		public static HashSet<(int, int)> FindAllChar(char pathChar, char[,] map)
		{
			HashSet<(int, int)> path = new HashSet<(int, int)>();
			for (int i = 0; i < map.GetLength(0); i++)
			{
				for (int j = 0; j < map.GetLength(1); j++)
				{
					if (map[i, j] == pathChar)
					{
						path.Add((i, j));
					}
				}
			}
			return path;
		}

		public static double Average(int[] array)
		{
			return array.Sum() / array.Length;
		}
	}
	public abstract class L
	{
		public static void Print<T>(List<T> array)
		{
			int l = array.Count;
			for (int i = 0; i < l; i++)
			{
				Console.Write($"{array[i]} ");
			}
			Console.WriteLine();
		}

		public static Dictionary<T, int> ElementFrequency<T>(List<T> list)
		{
			Dictionary<T, int> map = new Dictionary<T, int>();
			foreach (T item in list)
			{
				if (map.ContainsKey(item))
				{
					map[item]++;
				}
				else
				{
					map.Add(item, 1);
				}
			}
			return map;
		}

		public static double Average(int[] array)
		{
			return array.Sum() / array.Length;
		}
	}

	public abstract class AdvFunc
	{
		public static readonly HashSet<(int x, int y)> dirs = new HashSet<(int, int)>() { (0, -1), (1, 0), (0, 1), (-1, 0) };
		public static double[] GaussianElimination(double[,] matrix)
		{
			int numOfUnknowns = matrix.GetLength(1);
			double multiplier, temp;
			temp = matrix[0, 0];
			for (int i = 0; i < numOfUnknowns + 1; i++) { matrix[0, i] /= temp; }
			for (int row = 1; row < numOfUnknowns; row++)
			{
				for (int sub = 1; sub <= row; sub++)
				{
					multiplier = matrix[row, sub - 1] / matrix[sub - 1, sub - 1];
					for (int j = 0; j < numOfUnknowns + 1; j++)
					{
						matrix[row, j] = matrix[row, j] - (matrix[sub - 1, j] * multiplier);
					}
				}
				temp = matrix[row, row];
				for (int i = 0; i < numOfUnknowns + 1; i++) { matrix[row, i] /= temp; }
			}
			double[] solutions = new double[numOfUnknowns];
			for (int numOfSubs = 0; numOfSubs < numOfUnknowns; numOfSubs++)
			{
				double total = 0;
				total = matrix[numOfUnknowns - numOfSubs - 1, numOfUnknowns];
				for (int i = 1; i <= numOfSubs; i++)
				{
					total -= matrix[numOfUnknowns - numOfSubs - 1, numOfUnknowns - i] * solutions[numOfUnknowns - i];
				}
				solutions[numOfUnknowns - numOfSubs - 1] = total;
			}
			return solutions;
		}

		public static HashSet<(int, int)> dfs(char[,] map, char start, char end, char pathChar)
		{
			(int x, int y) startPos = A.FindChar(start, map);
			(int x, int y) endPos = A.FindChar(end, map);

			HashSet<(int x, int y)> visited = new HashSet<(int, int)>();
			HashSet<(int x, int y)> currentPath = new HashSet<(int x, int y)>();
			HashSet<(int x, int y)> path = A.FindAllChar(pathChar, map);

			path.Add(startPos);
			path.Add(endPos);

			if (searchAround(startPos))
			{
				return currentPath;
			}
			return null;

			bool searchAround((int x, int y) coord)
			{
				visited.Add(coord);
				currentPath.Add(coord);

				if (coord == endPos)
				{
					return true;
				}
				foreach (var (x, y) in dirs)
				{
					var nextCoord = (coord.x + x, coord.y + y);
					if (path.Contains(nextCoord))
					{
						if (searchAround(nextCoord))
						{
							return true;
						}
					}
				}
				currentPath.Remove(coord);
				return false;
			}
		}

		public static void PrintPath(char[,] map, HashSet<(int, int)> path)
		{
			int r = map.GetLength(0);
			int c = map.GetLength(1);

			for (int i = 0; i < r; i++)
			{
				for (int j = 0; j < c; j++)
				{
					if (path.Contains((i, j)))
					{
						Log.Color(map[i, j].ToString(), ConsoleColor.Green);
					}
					else
					{
						Console.Write(map[i, j].ToString());
					}
				}
			}
		}

	}
}
