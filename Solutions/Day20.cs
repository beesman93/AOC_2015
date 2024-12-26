using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day20: BaseDayWithInput
    {
        int minPresents;
        public Day20()
        {
            minPresents = int.Parse(_input[0]); 
        }

        static HashSet<int> getDivisors(int n)
        {
            HashSet<int> divisors = [];
            for (int i = 1; i <= Math.Sqrt(n); i++)
                if (n % i == 0)
                {
                    divisors.Add(i);
                    divisors.Add(n / i);
                }
            return divisors;
        }

        public override ValueTask<string> Solve_1()
        {
            for(int i = 1; i < int.MaxValue; i++)
            {
                int presents = 0;
                foreach (int divisor in getDivisors(i))
                    presents += divisor * 10;
                if (presents >= minPresents)
                    return new($"{i}");
            }
            return new($"Too big brother.");
        }
        public override ValueTask<string> Solve_2()
        {
            Dictionary<int, int> divisorUsage = [];
            for (int i = 1; i < int.MaxValue; i++)
            {
                int presents = 0;
                foreach (int divisor in getDivisors(i))
                {
                    if (!divisorUsage.ContainsKey(divisor))
                        divisorUsage[divisor] = 0;
                    divisorUsage[divisor]++;
                    if (divisorUsage[divisor] <= 50)
                        presents += divisor * 11;
                }
                if (presents >= minPresents)
                    return new($"{i}");
            }
            return new($"Too big brother.");
        }
    }
}
