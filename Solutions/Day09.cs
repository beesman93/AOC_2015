using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day09: BaseDayWithInput
    {
        readonly Dictionary<string, int> cities_index;
        readonly List<(string, string, int)> init_distances;
        readonly int[,] distances;
        List<List<int?>> cache;
        readonly int N;
        public Day09()
        {
            cities_index = [];
            init_distances = [];
            cache = [];
            foreach (var line in _input)
            {
                var parts = line.Split(" = ");
                var lr = parts[0].Split(" to ");
                cities_index.TryAdd(lr[0], cities_index.Count);
                cities_index.TryAdd(lr[1], cities_index.Count);
                init_distances.Add((lr[0], lr[1], int.Parse(parts[1])));
            }
            N = cities_index.Count;
            distances = new int[N, N];
            foreach (var (a, b, d) in init_distances)
            {
                distances[cities_index[a], cities_index[b]] = d;
                distances[cities_index[b], cities_index[a]] = d;
            }
        }

        int TotalCost(int mask, int currCityIndex, bool weWantLongestRoute)
        {
            if (mask == (1 << cities_index.Count) - 1)
                return 0;
            if (cache[currCityIndex][mask].HasValue)
                #pragma warning disable CS8629 // It's checked above
                return cache[currCityIndex][mask].Value;
                #pragma warning restore CS8629
            int ans = weWantLongestRoute?0 : int.MaxValue;
            for (int i = 0; i < N; i++)
                if ((mask & (1 << i)) == 0)
                    ans = weWantLongestRoute
                        ? Math.Max( ans, distances[currCityIndex,i] + TotalCost(mask | (1 << i), i, weWantLongestRoute))
                        : Math.Min( ans, distances[currCityIndex, i] + TotalCost(mask | (1 << i), i, weWantLongestRoute));
            cache[currCityIndex][mask] = ans;
            return ans;
        }


        int SolveTspNoReturn(int startingCity, bool weWantLongestRoute = false)
        {
            cache = [];
            for (int i = 0; i < N; i++)
            {
                List<int?> row = [];
                for (int j = 0; j < (1 << N); j++)
                    row.Add(null);
                cache.Add(row);
            }
            return TotalCost(1 << startingCity, startingCity,weWantLongestRoute);
        }
        public override ValueTask<string> Solve_1()
        {
            long minCost = long.MaxValue;
            for (int i = 0; i < N; i++)
                minCost = long.Min(SolveTspNoReturn(i), minCost);
            return new($"{minCost}");
        }
        public override ValueTask<string> Solve_2()
        {
            long maxCost = 0;
            for (int i = 0; i < N; i++)
                maxCost = long.Max(SolveTspNoReturn(i,true), maxCost);
            return new($"{maxCost}");
        }
    }
}
