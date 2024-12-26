using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day18: BaseDayWithInput
    {
        readonly bool[,] map;
        readonly int[,] aliveNeighbours;
        readonly int N;//assume NxN grid, if not replace N(...GetLength(0)) for M(...GetLength(1)) for j/y
        public Day18()
        {
            N = _input.Length;
            map = new bool[N, N];
            aliveNeighbours = new int[N, N];
            SetMapFromInput();
        }
        private void SetMapFromInput()
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    map[i, j] = _input[i][j] == '#';
        }
        void KeepCornersAlive()
        {
            var corners = new (int, int)[] { (0, 0), (0, N - 1), (N - 1, 0), (N - 1, N - 1) };
            foreach (var (i, j) in corners)
                map[i, j] = true;
        }
        void CountAliveNBs()
        {
            for(int i=0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (map[i, j])
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                if (x == 0 && y == 0) continue;
                                if(i+x<0 || j+y < 0 || i + x >= N || j + y >= N) continue;
                                aliveNeighbours[i + x, j + y]++;
                            }
                        }
                    }
                }
            }
        }
        void TickAndResetNBs()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    switch (aliveNeighbours[i, j])
                    {
                        case 3:
                            map[i, j] = true;
                            break;
                        case 2:
                            break;
                        default:
                            map[i, j] = false;
                            break;
                    }
                    aliveNeighbours[i, j] = 0;
                }
            }
        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            for(int i = 0; i < 100; i++)
            {
                CountAliveNBs();
                TickAndResetNBs();
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (map[i, j]) ans++;
                }
            }
            return new($"{ans}");
        }
        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            SetMapFromInput();
            for (int i = 0; i < 100; i++)
            {
                KeepCornersAlive();
                CountAliveNBs();
                TickAndResetNBs();
            }
            KeepCornersAlive();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (map[i, j]) ans++;
                }
            }
            return new($"{ans}");
        }
    }
}
