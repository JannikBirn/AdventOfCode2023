using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day6
{
    public static class Day6
    {
        struct Race
        {
            public long Time;
            public long Distance;
        }

        public static string Part1()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/6/input");

            var races = ParseInput(input);

            var marginAcc = races.Select(GetMargin).Aggregate((acc, v) => acc * v);

            return marginAcc.ToString();

            Race[] ParseInput(string input)
            {
                var lines = input.Split('\n');

                var times = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(v => int.Parse(v)).ToArray();
                var distances = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(v => int.Parse(v)).ToArray();

                return times.Select((v, i) => new Race { Time = v, Distance = distances[i] }).ToArray();
            }
        }

        public static string Part2()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/6/input");

            var race = ParseInput(input);

            var marginAcc = GetMargin(race);

            return marginAcc.ToString();

            static Race ParseInput(string input)
            {
                var lines = input.Replace(" ", string.Empty).Split('\n');

                var time = long.Parse(lines[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1]);
                var distance = long.Parse(lines[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1]);

                return new Race { Time = time, Distance = distance };
            }
        }

        static long GetMargin(Race race)
        {
            var minTime = GetMinTime(race);
            var maxTime = GetMaxTime(race);
            var margin = maxTime - minTime + 1;
            return margin;
        }

        static int GetMinTime(Race race)
        {
            var time = 1;
            while (GetDistanceByHoldingTime(time, race.Time) < race.Distance)
            {
                time++;
            }

            return time;
        }

        static long GetMaxTime(Race race)
        {
            var time = race.Time - 1;
            while (GetDistanceByHoldingTime(time, race.Time) < race.Distance)
            {
                time--;
            }

            return time;
        }

        static long GetDistanceByHoldingTime(long holdingTime, long raceDuration)
        {
            var travelTime = raceDuration - holdingTime;
            if (travelTime < 0) return 0;

            return travelTime * holdingTime;
        }        
    }
}
