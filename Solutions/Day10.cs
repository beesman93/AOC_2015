using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day10: BaseDayWithInput
    {
        const double CONWAY_APROX =
            1.3035772690342963912570991121525518907307025046594048757548613906285508878524615571268157668644252255534713930470949026839628498935515543474;
        public Day10()
        {

        }
        string LookAndSay(string s)
        {
            StringBuilder sb = new();
            for (int i = 0; i < s.Length; i++)
            {
                int count = 1;
                while (i + 1 < s.Length && s[i] == s[i + 1])
                {
                    i++;
                    count++;
                }
                sb.Append(count);
                sb.Append(s[i]);
            }
            return sb.ToString();
        }

        long LookAndSayDigitsAfterN(string s, int n)
        {
            for (int i = 0; i < n; i++)
                s = LookAndSay(s);
            return s.Length;
        }

        long LookAndSayDigitsAfterNConwayConstAprox(string s, int n)
        {
            long ans = s.Length;
            for(int i = 0; i < n; i++)
                ans = (long)Math.Ceiling(ans * CONWAY_APROX);
            return ans;
        }

        public override ValueTask<string> Solve_1() 
            => new($"ans: {LookAndSayDigitsAfterN(_input[0], 40)}\n(~= {LookAndSayDigitsAfterNConwayConstAprox(_input[0], 40)})");
        public override ValueTask<string> Solve_2() 
            => new($"ans: {LookAndSayDigitsAfterN(_input[0], 50)}\n(~= {LookAndSayDigitsAfterNConwayConstAprox(_input[0], 50)})");
    }
}
