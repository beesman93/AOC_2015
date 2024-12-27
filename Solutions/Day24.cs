using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day24: BaseDayWithInput
    {
        int N;
        int maxMask;
        int[] boxes;
        int maxWeight;
        public Day24()
        {
            N = _input.Length;
            maxMask = (1 << N) - 1;
            boxes = new int[N];
            for (int i = 0; i < N; i++)
                boxes[N-i-1] = int.Parse(_input[i]);
            maxWeight = boxes.Sum() / 3;

        }

        Dictionary<int, int> dpMaskWeight = [];
        int GetWeight(int mask)
        {
            if (dpMaskWeight.TryGetValue(mask, out int value)) return value;
            int weight = 0;
            for (int i = 0; i < N; i++)
                if ((mask & (1 << i)) != 0)
                    weight += boxes[i];
            dpMaskWeight[mask] = weight;
            return weight;
        }
        Dictionary<(int usedMask, int weight), HashSet<int>> DP = [];
        uint bestMaskLen = uint.MaxValue;
        HashSet<int> GetMasksToCompleteNumber(int usedMask, int weight)
        {
            if(System.Runtime.Intrinsics.X86.Popcnt.PopCount((uint)usedMask) > bestMaskLen) return [];
            if (weight < 0) return [];
            if (DP.TryGetValue((usedMask, weight), out var value)) return value;
            if (weight == 0)
            {
                bestMaskLen = System.Runtime.Intrinsics.X86.Popcnt.PopCount((uint)usedMask);
                DP[(usedMask, weight)] = [usedMask];
                return [usedMask];
            }
            HashSet<int> masks = [];
            for (int i = 0; i < N; i++)
            {
                if ((usedMask & (1 << i)) != 0) continue;
                var newMask = usedMask | (1 << i);
                var newWeight = weight - boxes[i];
                var newMasks = GetMasksToCompleteNumber(newMask, newWeight);
                masks.UnionWith(newMasks);
            }
            DP[(usedMask, weight)] = masks;
            return masks;
        }
        public override ValueTask<string> Solve_1()
        {
            var tests = GetMasksToCompleteNumber(0, maxWeight);
            long minQE = long.MaxValue;
            foreach (var test in tests)
            {
                long QE = 1;
                for (int i = 0; i < N; i++)
                {
                    if((test & (1 << i)) == 0) continue;
                        QE*=GetWeight(1 << i);
                }
                if (QE < minQE)
                    minQE = QE;
            }
            return new($"{minQE}");
        }
        public override ValueTask<string> Solve_2()
        {
            var tests = GetMasksToCompleteNumber(0, boxes.Sum() / 4);
            bestMaskLen = uint.MaxValue;
            long minQE = long.MaxValue;
            foreach (var test in tests)
            {
                long QE = 1;
                for (int i = 0; i < N; i++)
                {
                    if ((test & (1 << i)) == 0) continue;
                    QE *= GetWeight(1 << i);
                }
                if (QE < minQE)
                    minQE = QE;
            }
            return new($"{minQE}");
        }
    }
}
