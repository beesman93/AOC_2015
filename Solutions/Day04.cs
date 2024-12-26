using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day04: BaseDayWithInput
    {
        public Day04()
        {

        }
        public override ValueTask<string> Solve_1()
        {
            MD5 md5 = MD5.Create();
            for(int i =0;i < int.MaxValue; i++)
            {
                md5.ComputeHash(Encoding.ASCII.GetBytes(_input[0]+i));
                if (md5.Hash is not null && md5.Hash[0] == 0 && md5.Hash[1]==0 && md5.Hash[2]<16)
                    return new($"{i}");
            }
            return new($"nada");
        }
        public override ValueTask<string> Solve_2()
        {
            MD5 md5 = MD5.Create();
            for (int i = 0; i < int.MaxValue; i++)
            {
                md5.ComputeHash(Encoding.ASCII.GetBytes(_input[0] + i));
                if (md5.Hash is not null && md5.Hash[0] == 0 && md5.Hash[1] == 0 && md5.Hash[2] == 0)
                    return new($"{i}");
            }
            return new($"nada");
        }
    }
}
