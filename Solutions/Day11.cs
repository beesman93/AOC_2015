using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day11: BaseDayWithInput
    {
        public Day11()
        {

        }

        class SantaPassword
        {
            public int[] PWD { get; set; }

            public SantaPassword(string pwd)
            {
                PWD = pwd.Select(x => x - 'a').ToArray();
            }

            public bool ContainsIlleaglChar() => PWD.Any(x => x == 'i' - 'a' || x == 'o' - 'a' || x == 'l' - 'a');
            public bool ContainsStraight() => PWD.Where((x, i) => i < PWD.Length - 2 && PWD[i + 1] == x + 1 && PWD[i + 2] == x + 2).Any();
            public bool ContainsTwoPairs() => PWD.Where((x, i) => i < PWD.Length - 1 && PWD[i + 1] == x).Distinct().Count() >= 2;
            public bool IsValid() => !ContainsIlleaglChar() && ContainsStraight() && ContainsTwoPairs();

            public static SantaPassword operator ++(SantaPassword sp)
            {
                for (int i = sp.PWD.Length - 1; i >= 0; i--)
                {
                    sp.PWD[i]++;
                    if (sp.PWD[i] == 26)
                        sp.PWD[i] = 0;
                    else
                        break;
                }
                return sp;
            }

            public override string ToString()
            {
                return PWD.Aggregate("", (acc, x) => acc + (char)(x+'a'));
            }
        }

        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            SantaPassword santaPassword = new (_input[0]);
            while(!santaPassword.IsValid())
                santaPassword++;
            return new($"{santaPassword.ToString()}");
        }
        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            SantaPassword santaPassword = new(_input[0]);
            while (!santaPassword.IsValid())
                santaPassword++;
            santaPassword++;
            while (!santaPassword.IsValid())
                santaPassword++;
            return new($"{santaPassword}");
        }
    }
}
