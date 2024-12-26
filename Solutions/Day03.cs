using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day03: BaseDayWithInput
    {
        public Day03()
        {

        }
        private static (int x, int y) Dir(char c) => c switch
        {
            '^' => (0, 1),
            'v' => (0, -1),
            '<' => (-1, 0),
            '>' => (1, 0),
            _ => (0, 0)
        };
        public override ValueTask<string> Solve_1()
        {
            (int x, int y) position = (0, 0);
            HashSet<(int x, int y)> visited = new() {{position}};
            foreach(char c in _input[0])
            {
                position = (position.x + Dir(c).x, position.y + Dir(c).y);
                visited.Add(position);
            }
            return new($"{visited.Count}");
        }
        public override ValueTask<string> Solve_2()
        {
            (int x, int y) position = (0, 0);
            (int x, int y) position2 = (0, 0);
            HashSet<(int x, int y)> visited = new() { { position } };
            bool realSanta = true;
            foreach (char c in _input[0])
            {
                if (realSanta)
                {
                    position = (position.x + Dir(c).x, position.y + Dir(c).y);
                    visited.Add(position);
                }
                else
                {
                    position2 = (position2.x + Dir(c).x, position2.y + Dir(c).y);
                    visited.Add(position2);
                }
                realSanta = !realSanta;
            }
            return new($"{visited.Count}");
        }
    }
}
