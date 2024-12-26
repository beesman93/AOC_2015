using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day02: BaseDayWithInput
    {
        readonly List<int[]> boxes;
        public Day02()
        {
            boxes = [];
            foreach (var line in _input)
            {
                var parts = line.Split('x').Select(int.Parse).ToArray();
                boxes.Add(parts);
            }
        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            foreach (var box in boxes)
            {
                long minSide = long.MaxValue;
                for (int i = 0; i < box.Length; i++)
                {
                    for (int j = i + 1; j < box.Length; j++)
                    {
                        ans += 2 * box[i] * box[j];
                        minSide = Math.Min(minSide, box[i] * box[j]);
                    }
                }
                ans += minSide;
            }
            return new($"{ans}");
        }
        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            foreach (var box in boxes)
            {
                long minPerimeter = long.MaxValue;
                for (int i = 0; i < box.Length; i++)
                {
                    for (int j = i + 1; j < box.Length; j++)
                    {
                        minPerimeter = Math.Min(minPerimeter, 2 * (box[i] + box[j]));
                    }
                }
                ans += minPerimeter + box[0]*box[1]*box[2];
            }
            return new($"{ans}");
        }
    }
}
