using System.Diagnostics;

namespace AdventOfCode2023.Day7
{
    public static class Day7
    {
        enum KindType
        {
            Uninitilized = 0,
            FiveOfAKind = 1,
            FourOfAKind = 2,
            FullHouse = 3,
            ThreOfAKind = 4,
            TwoPair = 5,
            OnePair = 6,
            HighCard = 7,
        }

        class Game : IComparable<Game>
        {
            public string Cards { get; }
            public int Bet { get; }
            bool JIsJoker { get; }
            KindType Kind { get; }

            readonly static Dictionary<char, int> helperDictonary = new Dictionary<char, int>();

            public Game(string cards, int bet, bool hasJokers)
            {
                if (cards.Length != 5) throw new ArgumentException(nameof(cards));

                Cards = cards;
                Bet = bet;
                JIsJoker = hasJokers;


                helperDictonary.Clear();
                foreach (var card in cards)
                {
                    if (helperDictonary.ContainsKey(card)) helperDictonary[card]++;
                    else helperDictonary.Add(card, 1);
                }

                if (hasJokers && helperDictonary.ContainsKey('J') && helperDictonary.Count > 1)
                {
                    var highestAmountOfCards = helperDictonary.Where(v => v.Key != 'J').OrderByDescending(v => v.Value).First();
                    helperDictonary[highestAmountOfCards.Key] += helperDictonary['J'];
                    helperDictonary.Remove('J');
                }

                if (helperDictonary.Count == 1)
                {
                    Kind = KindType.FiveOfAKind;
                }
                else if (helperDictonary.Count == 2)
                {
                    if (helperDictonary.Any(v => v.Value == 4))
                    {
                        Kind = KindType.FourOfAKind;
                    }
                    else
                    {
                        Kind = KindType.FullHouse;
                    }
                }
                else if (helperDictonary.Count == 3)
                {
                    if (helperDictonary.Any(v => v.Value == 3))
                    {
                        Kind = KindType.ThreOfAKind;
                    }
                    else
                    {
                        Kind = KindType.TwoPair;
                    }
                }
                else if(helperDictonary.Count == 4)
                {
                    Kind = KindType.OnePair;
                }
                else
                {
                    Kind = KindType.HighCard;
                }
            }

            public int CompareTo(Game? other)
            {
                if (Kind != other.Kind)
                {
                    return (int)other.Kind - (int)Kind;
                }

                for (int i = 0; i < Cards.Length; i++)
                {
                    if (Cards[i] == other.Cards[i]) continue;

                    return GetCardPower(Cards[i]) - GetCardPower(other.Cards[i]);
                }

                return 0;
            }    
            
            int GetCardPower(char card)
            {
                return card switch
                {
                    'A' => 13,
                    'K' => 12,
                    'Q' => 11,
                    'J' when !JIsJoker => 10,
                    'J' when JIsJoker => 0,
                    'T' => 9,
                    '9' => 8,
                    '8' => 7,
                    '7' => 6,
                    '6' => 5,
                    '5' => 4,
                    '4' => 3,
                    '3' => 2,
                    '2' => 1,
                    _ => throw new ArgumentOutOfRangeException(nameof(card)),
                };
            }
        }

        public static string Part1()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/7/input");

            var sw = Stopwatch.StartNew();

            var games = ParseInput(input, false);
            var gamesOrderedByRank = games.OrderBy(v => v).ToArray();
            Console.WriteLine(string.Join(",\n", gamesOrderedByRank.Select((v, i) => i + " " + v.Cards + " " + v.Bet)));
            var totalWinnings = gamesOrderedByRank.Select((v, i) => v.Bet * (i + 1)).Sum();

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return totalWinnings.ToString();            
        }

        public static string Part2()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/7/input");

            var sw = Stopwatch.StartNew();

            var games = ParseInput(input, true);
            var gamesOrderedByRank = games.OrderBy(v => v).ToArray();
            Console.WriteLine(string.Join(",\n", gamesOrderedByRank.Select((v, i) => i + " " + v.Cards + " " + v.Bet)));
            var totalWinnings = gamesOrderedByRank.Select((v, i) => v.Bet * (i + 1)).Sum();

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return totalWinnings.ToString();
        }

        static Game[] ParseInput(string input, bool hasJokers)
        {
            var gameStrings = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            return gameStrings.Select(v =>
            {
                var parts = v.Split(' ');
                var cards = parts[0];
                var bet = int.Parse(parts[1]);

                return new Game(cards, bet, hasJokers);
            }).ToArray();
        }
    }
}
