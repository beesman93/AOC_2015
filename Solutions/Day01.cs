using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day01 : BaseDayWithInput
    {
        public Day01()
        {

        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            foreach(char c in _input[0])
                ans+= c == '(' ? 1 : -1;
            return new($"{ans}");
        }
        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            for (int i = 0; i < _input[0].Length; i++)
            {
                ans += _input[0][i] == '(' ? 1 : -1;
                if (ans < 0)
                    return new($"{i + 1}");
            }
            return new($"nop.");
        }
    }
}
