using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day06: BaseDayWithInput
    {
        enum Action
        {
            TurnOn,
            TurnOff,
            Toggle
        }
        struct Instruction
        {
            public Action action;
            public int x1, y1, x2, y2;
        }
        readonly bool[,] grid;
        readonly int[,] grid2;
        readonly List<Instruction> instructions;
        public Day06()
        {
            grid = new bool[1000, 1000];
            grid2 = new int[1000, 1000];
            instructions = [];
            foreach (var line in _input)
            {
                Action a = line switch
                {
                    var s when s.StartsWith("turn on") => Action.TurnOn,
                    var s when s.StartsWith("turn off") => Action.TurnOff,
                    var s when s.StartsWith("toggle") => Action.Toggle,
                    _ => throw new Exception("Invalid input")
                };
                var lr = line.Split(" through ");
                var xy1 = lr[0].Split(" ").Last().Split(",");
                var xy2 = lr[1].Split(",");
                instructions.Add(new Instruction
                {
                    action = a,
                    x1 = int.Parse(xy1[0]),
                    y1 = int.Parse(xy1[1]),
                    x2 = int.Parse(xy2[0]),
                    y2 = int.Parse(xy2[1])
                });
            }
        }
        public override ValueTask<string> Solve_1()
        {
            foreach(var instr in instructions)
            {
                for (int i = instr.x1; i <= instr.x2; i++)
                {
                    for (int j = instr.y1; j <= instr.y2; j++)
                    {
                        grid[i,j] = instr.action switch
                        {
                            Action.TurnOn => true,
                            Action.TurnOff => false,
                            Action.Toggle => !grid[i, j],
                            _ => throw new Exception("Invalid action")
                        };
                    }
                }
            }
            long ans = 0;
            foreach (var b in grid) if (b) ans++;
            return new($"{ans}");
        }
        public override ValueTask<string> Solve_2()
        {
            foreach (var instr in instructions)
            {
                for (int i = instr.x1; i <= instr.x2; i++)
                {
                    for (int j = instr.y1; j <= instr.y2; j++)
                    {
                        grid2[i, j] = instr.action switch
                        {
                            Action.TurnOn => grid2[i,j]+1,
                            Action.TurnOff => (grid2[i, j] - 1) >= 0 ? grid2[i,j]-1:0,
                            Action.Toggle => grid2[i, j] + 2,
                            _ => throw new Exception("Invalid action")
                        };
                    }
                }
            }
            long ans = 0;
            foreach (var b in grid2) ans+=b;
            return new($"{ans}");
        }
    }
}
