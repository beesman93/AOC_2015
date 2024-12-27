using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day25: BaseDayWithInput
    {
        struct InfiniteCoord(int col, int row)
        {
            public int col = col;
            public int row = row;
            public override readonly string ToString()
                => $"col:{col}, row:{row}";
            public override readonly bool Equals(object? obj)
                => obj is InfiniteCoord o && o.col == col && o.row == row;
            public override readonly int GetHashCode()
                => HashCode.Combine(col, row);
            static public InfiniteCoord GetNext(InfiniteCoord coord)
            {
                InfiniteCoord newCoord = new(coord.col, coord.row);
                if (coord.row == 1)
                {
                    newCoord.col = 1;
                    newCoord.row = coord.col + 1;
                }
                else
                {
                    newCoord.col++;
                    newCoord.row--;
                }
                return newCoord;
            }
        }
        static long GetNextCode(long code) => (code * 252533) % 33554393;
        readonly long startCode;
        readonly InfiniteCoord targetCoord;
        public Day25()
        {
            startCode = 20151125;
            var parts = _input[0].Split(' ');
            int row=0, col=0;
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] == "row" && i + 1 < parts.Length) row = int.Parse(parts[i + 1].Trim(','));
                if (parts[i] == "column" && i + 1 < parts.Length) col = int.Parse(parts[i + 1].Trim('.'));
            }
            targetCoord = new(col, row);
        }
        public override ValueTask<string> Solve_1()
        {
            InfiniteCoord coord = new(1, 1);
            long code = startCode;
            while (coord.col!=targetCoord.col||coord.row!=targetCoord.row)
            {
                code = GetNextCode(code);
                coord = InfiniteCoord.GetNext(coord);
            }
            return new($"{code}");
        }
        public override ValueTask<string> Solve_2() => new("Merry Christmas!");
    }
}
