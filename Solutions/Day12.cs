using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day12: BaseDayWithInput
    {
        public Day12()
        {

        }
        public override ValueTask<string> Solve_1() => new($"{Regex.Matches(_input[0], @"-?\d+").Sum(m => int.Parse(m.Value))}");
        public override ValueTask<string> Solve_2()
        {
            Stack<(int idxFrom, int idxTo)> redObjects = [];
            var json = _input[0];
            Stack<(int startIndex, bool isObject, bool isRed)> stack = [];
            for (int i = 0; i < json.Length; i++)
            {
                switch (json[i])
                {
                    case '{':
                    case '[':
                        stack.Push((i, json[i]=='{', false));
                        break;
                    case '}':
                    case ']':
                        if (stack.TryPop(out var obj) && obj.isObject && obj.isRed)
                        {
                            while (redObjects.TryPeek(out var underObj) && underObj.idxFrom > obj.startIndex)
                                redObjects.Pop();
                            redObjects.Push((obj.startIndex, i));
                        }
                        break;
                    case 'r':
                        if (i < json.Length - 3
                            && json[i + 1] == 'e'
                            && json[i + 2] == 'd'
                            && stack.TryPop(out var tmpPop))
                            stack.Push((tmpPop.startIndex, tmpPop.isObject, true));
                        break;
                };
            }
            while(redObjects.TryPop(out (int idxFrom, int idxTo) range))
                json = json.Remove(range.idxFrom, range.idxTo - range.idxFrom + 1);
            return new($"{Regex.Matches(json, @"-?\d+").Sum(m => int.Parse(m.Value))}");

        }
    }
}
