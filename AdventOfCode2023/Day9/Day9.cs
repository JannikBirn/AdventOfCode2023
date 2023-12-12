using System.Diagnostics;

namespace AdventOfCode2023.Day9
{
    public static class Day9
    {
        public static string Part1()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/9/input");

            var sw = Stopwatch.StartNew();

            var data = ParseInput(input);
            var predictionSum = 0;

            for (int i = 0; i < data.Length; i++)
            {
                var current = data[i];                
                var prediction = current[^1];

                while (current.Any(v => v != 0))
                {
                    var difference = new int[current.Length - 1];
                    for (int j = 0; j < difference.Length; j++)
                    {
                        difference[j] = current[j + 1] - current[j]; 
                    }
                    current = difference;
                    prediction += current[^1];
                }

                predictionSum += prediction;
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return predictionSum.ToString();
        }

        public static string Part2()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/9/input");

            var sw = Stopwatch.StartNew();

            var data = ParseInput(input);
            var predictionSum = 0;

            for (int i = 0; i < data.Length; i++)
            {
                var current = data[i];
                var depth = 0;
                var prediction = current[0];

                while (current.Any(v => v != 0))
                {
                    var difference = new int[current.Length - 1];
                    for (int j = 0; j < difference.Length; j++)
                    {
                        difference[j] = current[j + 1] - current[j];
                    }

                    current = difference;
                    depth++;

                    if (depth % 2 == 0)
                    {
                        prediction += current[0];
                    }
                    else
                    {
                        prediction -= current[0];
                    }                    
                }

                predictionSum += prediction;
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return predictionSum.ToString();
        }

        static int[][] ParseInput(string input)
        {
            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(n => int.Parse(n))
                    .ToArray())
                .ToArray();
        }
    }
}
