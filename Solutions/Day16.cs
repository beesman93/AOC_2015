using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day16 : BaseDayWithInput
    {
        static readonly string compoundsRawStringInput =
        """
        children: 3
        cats: 7
        samoyeds: 2
        pomeranians: 3
        akitas: 0
        vizslas: 0
        goldfish: 5
        trees: 3
        cars: 2
        perfumes: 1
        """;

        readonly Dictionary<string, int> compounds;

        public Day16()
        {
            compounds = compoundsRawStringInput
                .Split("\n")
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .Select(x => x.Split(": "))
                .ToDictionary(x => x[0], x => int.Parse(x[1]));
        }
        private bool IsMatchCompounds(in string inputLine, bool part2=false)
        {
            var parts = inputLine.Split(" ");
            for (int i = 2; i < parts.Length; i += 2)
            {
                var compoundKey = parts[i].Trim(':');
                var compoundVal = int.Parse(parts[i + 1].Trim(','));
                switch (part2, compoundKey)
                {
                    case (true, "cats"):
                    case (true, "trees"):
                        if (compounds[compoundKey] >= compoundVal)
                            return false;
                        break;
                    case (true, "pomeranians"):
                    case (true, "goldfish"):
                        if (compounds[compoundKey] <= compoundVal)
                            return false;
                        break;
                    default:
                        if(compounds[compoundKey] != compoundVal)
                            return false;
                        break;
                };
            }
            return true;
        }
        public override ValueTask<string> Solve_1() => _input.ToList().FindIndex(x => IsMatchCompounds(x,false)) switch
        {
            -1 => new("No Sue."),
            int i => new($"{_input[i].Split(" ")[1].Trim(':')}")
        };
        public override ValueTask<string> Solve_2() => _input.ToList().FindIndex(x => IsMatchCompounds(x,true)) switch
        {
            -1 => new("No Sue."),
            int i => new($"{_input[i].Split(" ")[1].Trim(':')}")
        };
    }
}
