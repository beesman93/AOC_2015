using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day19: BaseDayWithInput
    {
        Dictionary<string, List<string>> replacements;
        HashSet<string> molecules;
        string initial;
        int maxShorteningByStep;
        public Day19()
        {
            molecules = [];
            replacements = [];
            for (int i = 0; i < _input.Length - 2; i++)
            {
                string[] parts = _input[i].Split(" => ");
                if (!replacements.ContainsKey(parts[0]))
                    replacements[parts[0]] = [];
                replacements[parts[0]].Add(parts[1]);
                maxShorteningByStep = Math.Max(maxShorteningByStep, parts[1].Length - parts[0].Length);
            }
            initial = _input.Last();
        }
        List<string> getReplacements(string s, string find, string replacement)
        {
            List<string> result = [];
            int index = s.IndexOf(find);
            while (index != -1)
            {
                result.Add(s.Substring(0, index) + replacement + s.Substring(index + find.Length));
                index = s.IndexOf(find, index + 1);
            }
            return result;
        }
        public override ValueTask<string> Solve_1()
        {
            foreach(var key in replacements.Keys)
                foreach (var replacement in replacements[key])
                    getReplacements(initial, key, replacement).ForEach(mol => molecules.Add(mol));
            return new($"{molecules.Count}");
        }
        int best = int.MaxValue;
        Dictionary<(string, int), int> DP = [];
        int shortestSynthesisBackwards(string s, int steps)
        {
            if (DP.ContainsKey((s, steps)))
                return DP[(s, steps)];
            int ret = int.MaxValue;
            if (s.Length > (best - steps * maxShorteningByStep))
                ret = int.MaxValue;
            else if (steps >= best)
                ret = int.MaxValue;  
            else if (s == "e")
            {
                best = Math.Min(best, steps);
                ret = steps;
            }
            else if (s.Length == 1)
                ret = int.MaxValue;
            else
            {
                foreach (var key in replacements.Keys)
                {
                    foreach (var replacement in replacements[key])
                    {
                        getReplacements(s, replacement, key).Where(mol => !DP.ContainsKey((mol, steps + 1))).ToList().ForEach(mol => ret = Math.Min(ret, shortestSynthesisBackwards(mol, steps + 1)));
                    }
                }
            }
            DP[(s, steps)] = ret;
            return ret;
        }
        public override ValueTask<string> Solve_2() => new($"{shortestSynthesisBackwards(initial, 0)}");
    }
}
