using System.Diagnostics;

namespace AdventOfCode2023
{
    public class Day4
    {
        class Scratchcard
        {
            public HashSet<int> WinningNumbers;
            public int[] OwnNumbers;

            public int copies;
        }

        public static string Part1()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/4/input");

            var sw = Stopwatch.StartNew();

            var cardsInput = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var scratchcards = new List<Scratchcard>();

            foreach (var cardInput in cardsInput)
            {
                var parts = cardInput.Split(':', '|');

                var scratchcard = new Scratchcard
                {
                    WinningNumbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet(),
                    OwnNumbers = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()
                };

                scratchcards.Add(scratchcard);
            }

            var totalPoints = 0;

            foreach (var scratchcard in scratchcards)
            {
                var points = 0;

                foreach (var ownNumber in scratchcard.OwnNumbers)
                {
                    if (!scratchcard.WinningNumbers.Contains(ownNumber)) continue;

                    points++;
                }

                if (points > 0) totalPoints += (int)MathF.Pow(2, points - 1);
            }


            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return totalPoints.ToString();
        }

        public static string Part2()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/4/input");

            var sw = Stopwatch.StartNew();

            var cardsInput = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var scratchcards = new List<Scratchcard>();

            foreach (var cardInput in cardsInput)
            {
                var parts = cardInput.Split(':', '|');

                var scratchcard = new Scratchcard
                {
                    WinningNumbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet(),
                    OwnNumbers = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()
                };

                scratchcards.Add(scratchcard);
            }


            for (int i = 0; i < scratchcards.Count; i++)
            {
                Scratchcard scratchcard = scratchcards[i];
                var wins = 0;

                foreach (var ownNumber in scratchcard.OwnNumbers)
                {
                    if (!scratchcard.WinningNumbers.Contains(ownNumber)) continue;

                    wins++;
                }

                for (int j = 0; j < wins; j++)
                {
                    scratchcards[i + j + 1].copies += scratchcard.copies + 1;
                }
            }


            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return scratchcards.Select(v => v.copies + 1).Sum().ToString();
        }
    }
}