using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day23: BaseDayWithInput
    {
        public Day23()
        {
        }

        public uint RunInput(uint A, uint B)
        {
            int i = 0;
            while (i < _input.Length)
            {
                var parts = _input[i].Split(' ');
                switch (parts[0])
                {
                    case "hlf":
                        if (parts[1] == "a")
                            A /= 2;
                        else
                            B /= 2;
                        i++;
                        break;
                    case "tpl":
                        if (parts[1] == "a")
                            A *= 3;
                        else
                            B *= 3;
                        i++;
                        break;
                    case "inc":
                        if (parts[1] == "a")
                            A++;
                        else
                            B++;
                        i++;
                        break;
                    case "jmp":
                        i += int.Parse(parts[1]);
                        break;
                    case "jie":
                        if (parts[1] == "a,")
                        {
                            if (A % 2 == 0)
                                i += int.Parse(parts[2]);
                            else
                                i++;
                        }
                        else
                        {
                            if (B % 2 == 0)
                                i += int.Parse(parts[2]);
                            else
                                i++;
                        }
                        break;
                    case "jio":
                        if (parts[1] == "a,")
                        {
                            if (A == 1)
                                i += int.Parse(parts[2]);
                            else
                                i++;
                        }
                        else
                        {
                            if (B == 1)
                                i += int.Parse(parts[2]);
                            else
                                i++;
                        }
                        break;
                }
            }
            return B;
        }
        public override ValueTask<string> Solve_1() => new($"{RunInput(0,0)}");
        public override ValueTask<string> Solve_2() => new($"{RunInput(1, 0)}");
    }
}
