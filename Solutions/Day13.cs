using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day13: BaseDayWithInput
    {
        Dictionary<char, Dictionary<char, int>> happiness;
        public Day13()
        {
            happiness = [];
            foreach(var line in _input)
            {
                var parts = line.Split(' ');
                var guest1 = parts[0][0];
                var guest2 = parts.Last()[0];
                var h = int.Parse(parts[3]);
                if (parts[2] == "lose")
                    h = -h;
                if (!happiness.ContainsKey(guest1))
                    happiness[guest1] = [];
                happiness[guest1].Add(guest2, h);
            }
        }

        private int GetMaxHappy(string seated)
        {
            if (seated.Length == happiness.Count)
            {
                int total = 0;
                for(int i =0; i< seated.Length; i++)
                {
                    total += happiness[seated[i]].GetValueOrDefault(seated[(i + 1) % seated.Length],0);
                    total += happiness[seated[i]].GetValueOrDefault(seated[(i - 1 + seated.Length) % seated.Length],0);
                }
                return total;
            }
            int maxHappy = int.MinValue;
            foreach (var guest in happiness.Keys)
            {
                if (!seated.Contains(guest))
                {
                    var newSeated = seated + guest;
                    var max = GetMaxHappy(newSeated);
                    if (max > maxHappy)
                        maxHappy = max;
                }
            }
            return maxHappy;
        }
        public override ValueTask<string> Solve_1() => new($"{GetMaxHappy(happiness.First().Key.ToString())}");
        public override ValueTask<string> Solve_2()
        {
            happiness.Add('_', []);
            return new($"{GetMaxHappy(happiness.First().Key.ToString())}");
        }
    }
}
