using lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _12
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			string input = await Input.GetInput(12, 2025);
			//string input = "0:\n###\n##.\n##.\n\n1:\n###\n##.\n.##\n\n2:\n.##\n###\n##.\n\n3:\n##.\n###\n##.\n\n4:\n###\n#..\n###\n\n5:\n###\n.#.\n###\n\n4x4: 0 0 0 0 2 0\n12x5: 1 0 1 0 2 2\n12x5: 1 0 1 0 3 2\n";

			List<bool[,]> presents = new List<bool[,]>();
			List<((int X, int Y) dimensions, int[] num)> boxes = new List<((int X, int Y), int[])>();

			List<string> sections = input.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
			foreach (var section in sections)
			{
				List<string> lines = Input.ParseList(section, '\n');
				if (!lines[0].Contains('x'))
				{
					bool[,] present = new bool[3, 3];
					for (int l = 1; l < lines.Count; l++)
					{
						for (int c = 0; c < lines[l].Length; c++)
						{
							present[l - 1, c] = lines[l][c] == '#';
						}
					}
					presents.Add(present);
				}
				else
				{
					foreach (var line in lines)
					{
						int colonIndex = line.IndexOf(':');
						List<int> dimensions = Input.ParseIntList(line.Substring(0, colonIndex), 'x');
						int[] nums = Input.ParseIntList(line.Substring(colonIndex + 2), ' ').ToArray();

						boxes.Add(((dimensions[0], dimensions[1]), nums));
					}
				}
			}

			Console.WriteLine(solve1(presents, boxes));
		}
		static int solve1(List<bool[,]> presents, List<((int X, int Y) dimensions, int[] num)> boxes)
		{
			int total = 0;

			for (int i = 0; i < boxes.Count; i++)
			{
				var (dimensions, num) = boxes[i];

				int usedArea = 9 * num.Sum();
				if (usedArea <= dimensions.X * dimensions.Y)
				{
					total++;
				}
			}

			return total;
		}
	}
}
