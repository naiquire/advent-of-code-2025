using lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// part 2 was unsuccessful in C#, solved in Python instead

// some interesting notes about using 2-stage simplex for part 2

	// for the test data, only minimising the artificial function was sufficient to get the correct integer solution
	// however for the full data, this both returned non-integer solutions and simplex arrays that had not been optimised fully (~18500.xxx was the rough value for the total, which was surprisingly close to the actual integer solution of 18369)
	// i had started coding a function to find the integer solutions after maximising the objective function, without realising that the simplex could and would return negative values

	// after implementing a method to maximise the objective function, some of the buttons had to be pressed a negative number of times
	// this also showed that the tableau for the test data wasn't actually optimised, and the first machine is completable in 7 presses, part of which includes pressing the first button -2 times
	// 2-stage simplex cannot handle this directly, so i used a Python library instead that was able to handle this


namespace _10_2
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			string input = await Input.GetInput(10, 2025);
			//string input = "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}\n[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}\n[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}\n";

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

			Console.WriteLine(solve1(states, toggles));
			//Console.WriteLine(solve2simplex(joltages, toggles));
		}

		static int solve1(List<string> states, List<List<List<int>>> togglesList)
		{
			int total = 0;

			for (int i = 0; i < states.Count; i++)
			{
				char[] state = states[i].ToCharArray();
				List<List<int>> toggles = togglesList[i];

				char[] init = new char[state.Length];
				string initStr = "";
				for (int j = 0; j < init.Length; j++)
				{
					init[j] = '.';
					initStr += '.';
				}

				Queue<(char[], int)> queue = new Queue<(char[], int)>();
				Dictionary<string, int> seen = new Dictionary<string, int>();
				queue.Enqueue((init, 0));
				seen[initStr] = 0;

				string target = string.Empty;
				foreach (char c in state)
				{
					target += c;
				}

				while (queue.Count > 0)
				{
					(char[] current, int num) = queue.Dequeue();

					for (int t = 0; t < toggles.Count; t++)
					{
						char[] nc = new char[current.Length];
						Array.Copy(current, nc, current.Length);

						foreach (int c in toggles[t])
						{
							nc[c] = nc[c] == '#' ? '.' : '#';
						}

						string combination = "";
						foreach (char c in nc)
						{
							combination += c;
						}

						if (seen.ContainsKey(combination))
						{
							if (num + 1 < seen[combination])
							{
								seen[combination] = num + 1;
								queue.Enqueue((nc, num + 1));
								break;
							}
						}
						else
						{
							seen[combination] = num + 1;
							queue.Enqueue((nc, num + 1));
						}
					}
				}

				total += seen[target];
			}

			return total;
		}

		#region attempts at part 2 using breadth first search (too slow) and linear programming (simplex returns non-integer and negative solutions), neither of which worked
		static int solve2bfs(List<int[]> joltages, List<List<List<int>>> togglesList)
		{
			int total = 0;

			for (int i = 0; i < joltages.Count; i++)
			{
				int[] joltage = joltages[i];
				List<List<int>> toggles = togglesList[i];

				// int stateIndex, int value
				Dictionary<char[], int> map = new Dictionary<char[], int>();

				int[] init = new int[joltage.Length];

				Queue<(int[], int)> queue = new Queue<(int[], int)>();
				Dictionary<int[], int> seen = new Dictionary<int[], int>();
				queue.Enqueue((init, 0));
				seen[init] = 0;

				while (queue.Count > 0)
				{
					(int[] current, int num) = queue.Dequeue();
					if (seen.ContainsKey(current))
					{
						if (seen[current] >= num)
						{
							queue.Enqueue((current, num));
							continue;
						}
					}

					bool match = true;
					bool pass = false;
					Console.Write("Found ~ ");
					for (int c = 0; c < current.Length; c++)
					{
						Console.Write($"{current[c]} ");
						if (current[c] != joltage[c])
						{
							match = false;
							//break;
						}
						if (current[c] > joltage[c])
						{
							pass = true;
						}
					}
					Console.WriteLine($" // {num} iterations");
					if (match)
					{
						break;
					}
					if (pass)
					{
						continue;
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

		static double solve2simplex(List<int[]> joltages, List<List<List<int>>> togglesList)
		{
			double total = 0;

			for (int jol = 0; jol < joltages.Count; jol++)
			{
				int[] joltage = joltages[jol];
				List<List<int>> toggles = togglesList[jol];

				double[,] simplex = createTableau(joltage, toggles);

				//List<int[]> equations = new List<int[]>();
				//for (int i = 2; i < simplex.GetLength(0); i += 2)
				//{
				//	int[] eq = new int[toggles.Count + 1];
				//	for (int j = 0; j < toggles.Count; j++)
				//	{
				//		eq[j] = (int)simplex[i, 2 + j];
				//	}
				//	eq[eq.Length - 1] = (int)simplex[i, simplex.GetLength(1) - 1];
				//	equations.Add(eq);
				//}

				minQ(ref simplex);
				simplex = removeQandArtificials(simplex, (1, 1), (simplex.GetLength(0) - 1, simplex.GetLength(1) - 2 - joltage.Length));

				//maxP(ref simplex);
				//searchInts(simplex, equations, toggles);

				total += simplex[0, simplex.GetLength(1) - 1];
				Console.WriteLine(total);
			}

			return total * -1;
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
			double[,] newSimplex = new double[dim.x, dim.y];

			for (int i = pos.x; i < dim.x; i++)
			{
				for (int j = pos.y; j < dim.y; j++)
				{
					newSimplex[i - pos.x, j - pos.y] = simplex[i, j];
				}
				newSimplex[i - pos.x, newSimplex.GetLength(1) - 1] = simplex[i, simplex.GetLength(1) - 1];
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
				for (int i = 1; i < simplex.GetLength(1) - 1; i++)
				{
					if (simplex[0, i] < 0)
					{
						done = false;
						if (simplex[0, i] < simplex[0, pc])
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
			int[] countVariables = new int[toggles.Count];

			for (int t = 0; t < toggles.Count; t++)
			{
				foreach (int i in toggles[t])
				{
					countVariables[t]++;
				}
			}

			for (int j = 2; j < 2 + toggles.Count; j++)
			{
				simplex[0, j] = countVariables[j - 2];
			}
			simplex[0, simplex.GetLength(1) - 1] = joltage.Sum();

			return simplex;
		}
		static void searchInts(double[,] simplex, List<int[]> equations, List<List<int>> toggles)
		{
			// solutions ~ index i is button i, pressed value times
			double[] solutions = new double[toggles.Count];
			int lastCol = simplex.GetLength(1) - 1;

			for (int i = 1; i < simplex.GetLength(0); i++)
			{
				double value = simplex[i, lastCol];

				if (value != 0)
				{
					int solutionIndex = -1;
					for (int j = 0; j < 1 + toggles.Count; j++)
					{
						if (simplex[i, j] != 0)
						{
							solutionIndex = j;
							break;
						}
					}

					solutions[solutionIndex] = value;
				}
			}

			byte[] counter = new byte[solutions.Length];

			while (counter[0] < 2)
			{
				foreach (int[] eq in equations)
				{
					double result = eq[eq.Length - 1];
					double total = 0;
					for (int i = 0; i < eq.Length - 1; i++)
					{
						//if (0) floor and add

						//if (1) ceiling and add

					}
				}

				counter[counter.Length - 1]++;
				for (int i = counter.Length - 1; i >= 0; i--)
				{
					if (counter[i] == 2)
					{
						counter[i] = 0;
						counter[i - 1]++;
					}
				}
			}

		}

		#endregion
	}
}
