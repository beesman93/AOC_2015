using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day05: BaseDayWithInput
    {
        public Day05()
        {
        }
        private static readonly HashSet<char> vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u' };
        private static readonly HashSet<string> badPairs = new HashSet<string> { "ab", "cd", "pq", "xy" };
        private static bool IsNice(string s)
        {
            bool hasDouble = false;
            int vowelCount = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (vowels.Contains(s[i])) vowelCount++;
                if(i>0)
                {
                    var test = s[(i - 1)..(i+1)];
                    if (badPairs.Contains(test)) return false;
                    if (s[i] == s[i - 1]) hasDouble = true;
                }
            }
            return hasDouble && (vowelCount >= 3);
        }
        private static bool IsNiceP2(string s)
        {
            bool hasSpacedDouble = false;
            bool hasRepeatedPair = false;
            Dictionary<string, int> pairs = [];
            for (int i = 0; i < s.Length; i++)
            {
                if (i > 0)
                {
                    var pair = s[(i - 1)..(i + 1)];
                    if (pairs.ContainsKey(pair))
                    {
                        if (pairs[pair] < i - 1)
                            hasRepeatedPair = true;
                    }
                    else
                    {
                        pairs.Add(pair, i);
                    }
                }
                if (i>1 && s[i] == s[i - 2]) hasSpacedDouble = true;
            }
            return hasSpacedDouble && hasRepeatedPair;
        }
        public override ValueTask<string> Solve_1() => new($"{_input.Count(IsNice)}");
        public override ValueTask<string> Solve_2() => new($"{_input.Count(IsNiceP2)}");
    }
}
