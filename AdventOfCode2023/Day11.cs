using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day11
    {
        const char Galaxy = '#';
        const char EmptySpace = '.';

        public static string Part1()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/11/input");

            var sw = Stopwatch.StartNew();

            var sum = GetSumOfPairs(input, 2);

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return sum.ToString();
        }

        public static string Part2()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/11/input");

            var sw = Stopwatch.StartNew();

            var sum = GetSumOfPairs(input, 1_000_000);

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return sum.ToString();
        }

        static long GetSumOfPairs(string input, int spaceExpansion)
        {
            var spaceImage = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(v => v.ToCharArray()).ToArray();

            //Chaning the empty space to its distance value
            //In fact, the result is that any rows or columns that contain no galaxies should all actually be twice as big.
            var doubleDistanceRows = new HashSet<int>();
            var doubleDistanceColumns = new HashSet<int>();
            var galaxies = new HashSet<(int y, int x)>();

            for (int y = 0; y < spaceImage.Length; y++)
            {
                if (spaceImage[y].Any(v => v != EmptySpace)) continue;

                doubleDistanceRows.Add(y);
            }

            for (int x = 0; x < spaceImage[0].Length; x++)
            {
                var isEmptyColumn = true;
                for (int y = 0; y < spaceImage.Length; y++)
                {
                    if (spaceImage[y][x] == Galaxy)
                    {
                        galaxies.Add((y, x));
                        isEmptyColumn = false;
                    }
                }

                if (isEmptyColumn)
                {
                    doubleDistanceColumns.Add(x);
                }
            }

            long sum = 0;

            var checkedGalaxies = new HashSet<((int y, int x), (int y, int x))>();

            foreach (var galaxyA in galaxies)
            {
                foreach (var galaxyB in galaxies)
                {
                    if (galaxyA == galaxyB) continue;
                    if (checkedGalaxies.Contains((galaxyA, galaxyB)) || checkedGalaxies.Contains((galaxyB, galaxyA))) continue;

                    var distanceY = Math.Abs(galaxyB.y - galaxyA.y);
                    var distanceX = Math.Abs(galaxyB.x - galaxyA.x);

                    var minY = Math.Min(galaxyA.y, galaxyB.y);
                    var minX = Math.Min(galaxyA.x, galaxyB.x);

                    var maxY = minY + distanceY;
                    var maxX = minX + distanceX;

                    for (int y = minY + 1; y < maxY; y++)
                    {
                        if (!doubleDistanceRows.Contains(y)) continue;

                        distanceY += spaceExpansion - 1;
                    }

                    for (int x = minX + 1; x < maxX; x++)
                    {
                        if (!doubleDistanceColumns.Contains(x)) continue;

                        distanceX += spaceExpansion - 1;
                    }

                    checkedGalaxies.Add((galaxyA, galaxyB));
                    sum += distanceY + distanceX;
                    if (sum < 0) throw new Exception("sum type is not big enough");
                    Console.WriteLine($"Distance {galaxyA} - {galaxyB} : {distanceX + distanceY}");
                }
            }

            return sum;
        }
    }
}
