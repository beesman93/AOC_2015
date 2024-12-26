using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day17: BaseDayWithInput
    {
        readonly List<int> containers;
        readonly int N;
        readonly Dictionary<(uint mask,int nog), long> dp;
        readonly HashSet<uint> validMasks;
        public Day17()
        {
            dp = [];
            validMasks = [];
            containers = [];
            foreach (var line in _input)
                containers.Add(int.Parse(line));
            N = containers.Count;
        }
        private long CountWaysDP(uint maskUsed, int remainingEggNog)
        {
            if(!dp.ContainsKey((maskUsed,remainingEggNog))) 
                dp[(maskUsed,remainingEggNog)] = CountWays(maskUsed, remainingEggNog);
            return dp[(maskUsed,remainingEggNog)];
        }
        private long CountWays(uint maskUsed, int remainingEggNog)
        {
            if (remainingEggNog <  0) return 0;
            if (remainingEggNog == 0)
            {
                validMasks.Add(maskUsed);
                return 1;
            }
            //if (maskUsed == (1 << N) - 1) return 0; // all containers used, nog not empty should not happen
            long sumWays = 0;
            for (int i = 0; i < N; i++)
            {
                if(((1 << i) & maskUsed) !=0) continue;
                sumWays += CountWaysDP(maskUsed | ((uint)1 << i), remainingEggNog - containers[i]);
            }
            return sumWays;
        }
        public override ValueTask<string> Solve_1()
        {
            CountWaysDP(0, 150);
            return new($"{validMasks.Count}");
        }
        public override ValueTask<string> Solve_2()
        {
            var minSizeContainers = validMasks.Min(x => System.Runtime.Intrinsics.X86.Popcnt.PopCount(x));
            return new($"{validMasks.Count(x => System.Runtime.Intrinsics.X86.Popcnt.PopCount(x) == minSizeContainers)}");
        }
    }
}
