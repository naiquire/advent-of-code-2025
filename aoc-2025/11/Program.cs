using lib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _11
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			string input = await Input.GetInput(11, 2025);
			//string input = "svr: aaa bbb\r\naaa: fft\r\nfft: ccc\r\nbbb: tty\r\ntty: ccc\r\nccc: ddd eee\r\nddd: hub\r\nhub: fff\r\neee: dac\r\ndac: fff\r\nfff: ggg hhh\r\nggg: out\r\nhhh: out\r\n";

			Dictionary<string, List<string>> connections = new Dictionary<string, List<string>>();
			List<string> con = Input.ParseList(input, '\n');
			foreach (string line in con)
			{
				int colonIndex = line.IndexOf(':');
				string label = line.Substring(0, colonIndex);
				List<string> data = Input.ParseList(line.Substring(colonIndex + 1).Trim(), ' ');
				connections[label] = data;
			}

			Console.WriteLine(solve1(connections, "you", "out"));
			Console.WriteLine(solve2(connections));
		}
		static ulong solve1(Dictionary<string, List<string>> connections, string start, string end)
		{
			Dictionary<string, ulong> map = new Dictionary<string, ulong>();
			return dfs(start);

			ulong dfs(string currentLabel)
			{
				if (currentLabel == end) return 1;
				else if (currentLabel == "out") return 0;

				if (map.ContainsKey(currentLabel))
				{
					return map[currentLabel];
				}

				ulong value = 0;
				foreach (string c in connections[currentLabel])
				{
					value += dfs(c);
				}
				map[currentLabel] = value;
				return value;
			}
		}
		static ulong solve2(Dictionary<string, List<string>> connections)
		{
			return (
				solve1(connections, "svr", "dac") *
				solve1(connections, "dac", "fft") *
				solve1(connections, "fft", "out"))
				+ (
				solve1(connections, "svr", "fft") *
				solve1(connections, "fft", "dac") *
				solve1(connections, "dac", "out")
			);
		}
	}
}
