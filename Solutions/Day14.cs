using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day14: BaseDayWithInput
    {
        class Reindeer
        {
            public string Name { get; set; }
            public int Speed { get; set; }
            public int FlyTime { get; set; }
            public int RestTime { get; set; }
            public long GetDistance(int time)
            {
                long oneFullCycleTime = FlyTime + RestTime;
                long cycles = time / oneFullCycleTime;
                long distance = cycles * FlyTime * Speed;
                long remainingTime = long.Min(time % oneFullCycleTime, FlyTime);
                distance += remainingTime * Speed;
                return distance;
            }
            public int distanceSoFar;
            public long score;
            private int timer;
            private bool fly;
            public void GetReady()
            {
                score = 0;
                distanceSoFar = 0;
                timer = FlyTime;
                fly = true;
            }
            public int Tick()
            {
                if (timer==0)
                {
                    timer = fly ? RestTime : FlyTime;
                    fly = !fly;
                }
                if (fly) distanceSoFar+=Speed;
                timer--;
                return distanceSoFar;
            }
        }
        List<Reindeer> reindeers;
        public Day14()
        {
            reindeers = [];
            foreach (var line in _input)
            {
                var parts = line.Split(' ');
                reindeers.Add(new Reindeer
                {
                    Name = parts[0],
                    Speed = int.Parse(parts[3]),
                    FlyTime = int.Parse(parts[6]),
                    RestTime = int.Parse(parts[13])
                });
            }
        }
        public override ValueTask<string> Solve_1() => new($"{reindeers.Max(rd => rd.GetDistance(2503))}");
        public override ValueTask<string> Solve_2()
        {
            reindeers.ForEach(rd => rd.GetReady());
            for(int i=0; i < 2503; i++)
            {
                var maxThisSecond = reindeers.Max(rd => rd.Tick());
                foreach (var rd in reindeers.Where(rd => rd.distanceSoFar == maxThisSecond))
                    rd.score++;
            }
            return new($"{reindeers.Max(rd => rd.score)}");
        }
    }
}
