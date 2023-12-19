using System.Diagnostics;

namespace AdventOfCode2023
{
    public class Day5
    {
        struct Map
        {
            public uint DestinationRangeStart;
            public uint SourceRangeStart;
            public uint RangeLength;
        }

        struct SeedRange
        {
            public uint Start; //include
            public uint Length; //exclude
        }

        public static string Part1()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/5/input");

            var sw = Stopwatch.StartNew();

            var seeds = ParseSeedds_Part1(input);
            var seed2soil = ParseMap(input, "seed-to-soil");
            var soil2fertilizer = ParseMap(input, "soil-to-fertilizer");
            var fertilizer2water = ParseMap(input, "fertilizer-to-water");
            var water2light = ParseMap(input, "water-to-light");
            var light2temperature = ParseMap(input, "light-to-temperature");
            var temperature2humidity = ParseMap(input, "temperature-to-humidity");
            var humidity2location = ParseMap(input, "humidity-to-location");

            var lowestLocation = seeds
                .Select(v => Seed2Location(v))
                .Min();

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return lowestLocation.ToString();

            long Seed2Location(long seed)
            {
                long v = Source2Destination(seed2soil, seed);
                v = Source2Destination(soil2fertilizer, v);
                v = Source2Destination(fertilizer2water, v);
                v = Source2Destination(water2light, v);
                v = Source2Destination(light2temperature, v);
                v = Source2Destination(temperature2humidity, v);
                v = Source2Destination(humidity2location, v);
                return v;
            }
        }

        public static string Part2()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/5/input");

            var sw = Stopwatch.StartNew();

            var seedRanges = ParseSeedds_Part2(input);
            var seed2soil = ParseMap(input, "seed-to-soil");
            var soil2fertilizer = ParseMap(input, "soil-to-fertilizer");
            var fertilizer2water = ParseMap(input, "fertilizer-to-water");
            var water2light = ParseMap(input, "water-to-light");
            var light2temperature = ParseMap(input, "light-to-temperature");
            var temperature2humidity = ParseMap(input, "temperature-to-humidity");
            var humidity2location = ParseMap(input, "humidity-to-location");

            var objectLock = new object();
            long lowestLocation = long.MaxValue;

            var a = Parallel.ForEach(seedRanges, seedRange =>
            {
                Console.WriteLine($"Started Seed Range: {seedRange.Start} - {(long)(seedRange.Start + seedRange.Length)}");
                var b = Parallel.For(seedRange.Start, seedRange.Start + seedRange.Length, seed =>
                {
                    var location = Seed2Location(seed);

                    if (location < lowestLocation)
                    {
                        lock (objectLock)
                        {
                            if (location < lowestLocation)
                            {
                                Console.WriteLine($"New Low Location Found: {location} ({Thread.CurrentThread.ManagedThreadId})");
                                lowestLocation = location;
                            }
                        }
                    }
                });

                Console.WriteLine($"Finished Seed Range: {seedRange.Start} Status: {b.IsCompleted}");
            });

            Console.WriteLine($"Finished ALL Status: {a.IsCompleted}");

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return lowestLocation.ToString();

            long Seed2Location(long seed)
            {
                long v = Source2Destination(seed2soil, seed);
                v = Source2Destination(soil2fertilizer, v);
                v = Source2Destination(fertilizer2water, v);
                v = Source2Destination(water2light, v);
                v = Source2Destination(light2temperature, v);
                v = Source2Destination(temperature2humidity, v);
                v = Source2Destination(humidity2location, v);
                return v;
            }
        }

        static uint[] ParseSeedds_Part1(string input)
        {
            var startIndex = input.IndexOf("seeds:");
            var endIndex = input.IndexOf("\n", startIndex);
            var seedInput = input[startIndex..endIndex];

            return seedInput.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(v => uint.Parse(v))
                .ToArray();
        }

        static SeedRange[] ParseSeedds_Part2(string input)
        {
            var seeds = ParseSeedds_Part1(input);
            var seedRanges = new SeedRange[seeds.Length / 2];

            for (int i = 0; i < seedRanges.Length; i++)
            {
                seedRanges[i] = new SeedRange
                {
                    Start = seeds[i * 2],
                    Length = seeds[i * 2 + 1]
                };
            }

            return seedRanges;
        }

        static Map[] ParseMap(string input, string mapName)
        {
            var startIndex = input.IndexOf(mapName);
            var endIndex = input.IndexOf("\n\n", startIndex);
            if (endIndex == -1) endIndex = input.Length;
            var mapInput = input[startIndex..endIndex];

            return mapInput.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(v => v.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(v => new Map
                {
                    DestinationRangeStart = uint.Parse(v[0]),
                    SourceRangeStart = uint.Parse(v[1]),
                    RangeLength = uint.Parse(v[2])
                })
                .ToArray();
        }

        static long Source2Destination(Map[] mapping, long sourceIndex)
        {
            foreach (var map in mapping)
            {
                var offset = sourceIndex - map.SourceRangeStart;
                if (offset < 0 || offset >= map.RangeLength) continue;

                return map.DestinationRangeStart + offset;
            }

            return sourceIndex;
        }
    }
}
