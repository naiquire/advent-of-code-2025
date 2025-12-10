using lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _10
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			//string input = await Input.GetInput(10, 2025);
			string input = "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}\n[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}\n[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}\n";

			List<string> states = new List<string>();
			List<List<List<int>>> toggles = new List<List<List<int>>>();
			List<int[]> joltages = new List<int[]>();

			List<string> lines = Input.ParseList(input, '\n');
			foreach (var line in lines)
			{
				List<List<int>> ts = new List<List<int>>();

				int bracket = line.IndexOf(']');
				states.Add(line.Substring(1, bracket - 1));

				
				int joltage = line.IndexOf('{');
				joltages.Add(Input.ParseIntList(line.Substring(joltage + 1, line.Length - joltage - 2), ',').ToArray());
				string toggleList = line.Substring(bracket + 2, joltage - bracket - 3);
				List<string> toggle = Input.ParseList(toggleList, ' ');
				foreach (var t in toggle)
				{
					ts.Add(Input.ParseIntList(t.Substring(1, t.Length - 2), ','));
				}

				toggles.Add(ts);
			}

			//Console.WriteLine(solve1(states, toggles));
			Console.WriteLine(solve2(joltages, toggles));
		}

		static int solve1(List<string> states, List<List<List<int>>> togglesList)
		{
			int total = 0;

			for (int i = 0; i < states.Count; i++)
			{
				char[] state = states[i].ToCharArray();
				List<List<int>> toggles = togglesList[i];
				int[] buttons = new int[state.Length];

				// int stateIndex, int value
				Dictionary<char[], int> map = new Dictionary<char[], int>();

				char[] init = new char[state.Length];
				for (int j = 0; j < init.Length; j++)
				{
					init[j] = '.';
				}

				Queue<(char[], int)> queue = new Queue<(char[], int)>();
				Dictionary<string, int> seen = new Dictionary<string, int>();
				queue.Enqueue((init, 0));

				string add = "";
				foreach (char c in init)
				{
					add += c;
				}
				seen[add] = 0;

				string target = string.Empty;
				foreach (char c in state)
				{
					target += c;
				}

				while (queue.Count > 0)
				{
					(char[] current, int num) = queue.Dequeue();

					string temp = string.Empty;
					foreach (char c in current)
					{
						temp += c;
					}


					for (int t = 0; t < toggles.Count; t++)
					{
						char[] nc = new char[current.Length];
						for (int j = 0; j < current.Length; j++)
						{
							nc[j] = current[j];
						}

						foreach (int c in toggles[t])
						{
							nc[c] = nc[c] == '#' ? '.' : '#';
						}

						string add2 = "";
						foreach (char c in nc)
						{
							add2 += c;
						}

						if (seen.ContainsKey(add2))
						{
							if (num + 1 < seen[add2])
							{
								seen[add2] = num + 1;
								queue.Enqueue((nc, num + 1));
								break;
							}
						}
						else
						{
							seen[add2] = num + 1;
							queue.Enqueue((nc, num + 1));
						}
					}
				}

				Console.WriteLine(seen[target]);
				total += seen[target];

				//total += processToggle(init, ref state, buttons, ref map, ref toggles, -1);

			}

			int processToggle(char[] state, ref char[] target, int[] buttons, ref Dictionary<char[], int> map, ref List<List<int>> toggles, int lastIndex)
			{
				bool match = true;
				for (int i = 0; i < state.Length; i++)
				{
					if (state[i] != target[i])
					{
						match = false;
						break;
					}
				}

				if (match) return 0;

				if (map.ContainsKey(state))
				{
					return map[state];
				}

				int minvalue = int.MaxValue;
				for (int i = 0; i < toggles.Count; i++)
				{
					if (i == lastIndex) continue;
					foreach (int c in toggles[i])
					{
						buttons[c]++;
					}

					for (int j = 0; j < buttons.Length; j++)
					{
						if (buttons[j] % 2 == 0)
						{
							state[j] = '.';
						}
						else
						{
							state[j] = '#';
						}
					}

					int value = processToggle(state, ref target, buttons, ref map, ref toggles, i);

					if (map.TryGetValue(state, out int temp))
					{
						if (value < temp)
						{
							map[state] = value;
						}
					}

					if (value < minvalue) minvalue = value;
					
				}
				return minvalue;
			}


			return total;
		}


		static int solv2(List<int[]> joltages, List<List<List<int>>> togglesList)
		{
			int total = 0;

			for (int i = 0; i < joltages.Count; i++)
			{
				int[] joltage = joltages[i];
				List<List<int>> toggles = togglesList[i];

				// int stateIndex, int value
				Dictionary<char[], int> map = new Dictionary<char[], int>();

				int[] init = new int[joltage.Length];
				for (int j = 0; j < init.Length; j++)
				{
					init[j] = 0;
				}

				Queue<(int[], int)> queue = new Queue<(int[], int)>();
				Dictionary<int[], int> seen = new Dictionary<int[], int>();
				queue.Enqueue((init, 0));
				seen[init] = 0;

				while (queue.Count > 0)
				{
					(int[] current, int num) = queue.Dequeue();

					bool match = true;
					Console.Write("Found ~ ");
					for (int c = 0; c < current.Length; c++)
					{
						Console.Write($"{current[c]} ");
						if (current[c] != joltage[c])
						{
							match = false;
							//break;
						}
					}
					Console.WriteLine($" // {num} iterations");
					if (match)
					{
						break;
					}


					for (int t = 0; t < toggles.Count; t++)
					{
						int[] nc = new int[current.Length];
						for (int j = 0; j < current.Length; j++)
						{
							nc[j] = current[j];
						}

						foreach (int c in toggles[t])
						{
							nc[c]++;
						}

						if (seen.ContainsKey(nc))
						{
							if (num + 1 < seen[nc])
							{
								seen[nc] = num + 1;
								queue.Enqueue((nc, num + 1));
								break;
							}
						}
						else
						{
							seen[nc] = num + 1;
							queue.Enqueue((nc, num + 1));
						}
					}
				}

				Console.WriteLine(seen[joltage]);
				total += seen[joltage];

				//total += processToggle(init, ref state, buttons, ref map, ref toggles, -1);

			}

			return total;
		}

		static int solve2(List<int[]> joltages, List<List<List<int>>> togglesList)
		{
			int total = 0;

			for (int jol = 0; jol < joltages.Count; jol++)
			{
				int[] joltage = joltages[jol];
				List<List<int>> toggles = togglesList[jol];

				double[,] simplex = createTableau(joltage, toggles);

				minQ(ref simplex);

				Print(simplex);

				simplex = removeQandArtificials(simplex, (1, 1), (-1, -1));

				Print(simplex);


			}


			return total;
		}

		static void minQ(ref double[,] simplex)
		{
			while (true)
			{
				// find pivot column
				int pc = 0;
				bool done = true;
				for (int i = 2; i < simplex.GetLength(1) - 1; i++)
				{
					if (simplex[0, i] > 0)
					{
						done = false;
						if (simplex[0, i] > simplex[0, pc])
						{
							pc = i;
						}
					}
				}
				if (done) break;

				// find pivot row
				int pr = -1;
				double minRatio = int.MaxValue;
				for (int i = 2; i < simplex.GetLength(0); i++)
				{
					if (simplex[i, pc] > 0)
					{
						double ratio = simplex[i, simplex.GetLength(1) - 1] / simplex[i, pc];
						if (ratio < minRatio)
						{
							minRatio = ratio;
							pr = i;
						}
					}
				}

				// calc pivot row
				double pivot = simplex[pr, pc];
				for (int j = 0; j < simplex.GetLength(1); j++)
				{
					simplex[pr, j] /= pivot;
				}

				// update other rows
				for (int i = 0; i < simplex.GetLength(0); i++)
				{
					if (i == pr) continue;
					double factor = simplex[i, pc];
					for (int j = 0; j < simplex.GetLength(1); j++)
					{
						simplex[i, j] -= factor * simplex[pr, j];
					}
				}
			}
		}

		static double[,] removeQandArtificials(double[,] simplex, (int x, int y) pos, (int x, int y) dim)
		{
			double[,] newSimplex = new double[simplex.GetLength(0) - 1, simplex.GetLength(1) - 1 - (simplex.GetLength(0) - 2)];

			// copy P,constraints,slack/surplus,RHS
			for (int i = pos.x; i < dim.x; i++)
			{
				for (int j = pos.y; j < dim.y; j++)
				{
					newSimplex[i - pos.x, j - pos.y] = simplex[i, j];
				}
				newSimplex[i - pos.x, newSimplex.GetLength(1) - 1]
			}

			return newSimplex;
		}


		static void maxP(ref double[,] simplex)
		{
			while (true)
			{
				// find pivot column
				int pc = 0;
				bool done = true;
				for (int i = 2; i < simplex.GetLength(1) - 1; i++)
				{
					if (simplex[1, i] < 0)
					{
						done = false;
						if (simplex[1, i] < simplex[1, pc])
						{
							pc = i;
						}
					}
				}
				if (done) break;

				// find pivot row
				int pr = -1;
				double minRatio = int.MaxValue;
				for (int i = 2; i < simplex.GetLength(0); i++)
				{
					if (simplex[i, pc] > 0)
					{
						double ratio = simplex[i, simplex.GetLength(1) - 1] / simplex[i, pc];
						if (ratio < minRatio)
						{
							minRatio = ratio;
							pr = i;
						}
					}
				}
			}
		}


		static double[,] createTableau(int[] joltage, List<List<int>> toggles)
		{
			// Q, P, variables, slack/surplus, artificial, RHS
			double[,] simplex = new double[2 + 2 * joltage.Length, 2 + toggles.Count + 2 * joltage.Length + joltage.Length + 1];


			// 0 - objective function
			simplex[1, 1] = 1;
			for (int t = 0; t < toggles.Count; t++)
			{
				simplex[1, 2 + t] = 1;
			}

			// constraints
			for (int i = 2, j = 0; i < 2 + 2 * joltage.Length; i += 2, j++)
			{
				// toggles
				for (int t = 0; t < toggles.Count; t++)
				{
					if (toggles[t].Contains(j))
					{
						simplex[i, 2 + t] = 1;
						simplex[i + 1, 2 + t] = 1;
					}
				}
			}

			// rhs
			for (int i = 2, j = 0; i < 2 + 2 * joltage.Length; i += 2, j++)
			{
				simplex[i, simplex.GetLength(1) - 1] = joltage[j];
				simplex[i + 1, simplex.GetLength(1) - 1] = joltage[j];
			}

			// slack/surplus
			for (int i = 2, j = 0; i < 2 + 2 * joltage.Length; i += 2, j += 2)
			{
				int column = 2 + toggles.Count + j;
				simplex[i, column] = 1;
				simplex[i + 1, column + 1] = -1;
				simplex[0, column + 1] = -1;
			}

			// artificial
			for (int i = 2, j = 0; i < 2 + 2 * joltage.Length; i += 2, j++)
			{
				int column = 2 + toggles.Count + 2 * joltage.Length + j;
				simplex[i + 1, column] = 1;
			}

			// Q
			for (int j = 2; j < 2 + toggles.Count; j++)
			{
				simplex[0, j] = joltage.Length;
			}
			simplex[0, simplex.GetLength(1) - 1] = joltage.Sum();


			Print(simplex);
			return simplex;
		}

		static void Print<T>(T[,] array)
		{
			int x = array.GetLength(0);
			int y = array.GetLength(1);

			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y; j++)
				{
					Console.Write($"{array[i, j],4} ");
				}

				Console.WriteLine();
			}
			Console.WriteLine();
		}
	}
}
