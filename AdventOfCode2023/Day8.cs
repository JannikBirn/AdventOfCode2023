using System.Diagnostics;
using System.Numerics;

namespace AdventOfCode2023
{
    public static class Day8
    {
        public static string Part1()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/8/input");

            var sw = Stopwatch.StartNew();

            var instructions = ParseInstructions(input);
            var network = ParseNetwork(input);

            var currentNode = "AAA";
            var instructionIndex = 0;

            while (currentNode != "ZZZ")
            {
                var instruction = instructions[instructionIndex % instructions.Length];

                currentNode = instruction switch
                {
                    'L' => network[currentNode].Item1,
                    'R' => network[currentNode].Item2,
                    _ => throw new ArgumentException()
                };

                instructionIndex++;
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return instructionIndex.ToString();
        }

        public static string Part2()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/8/input");

            var sw = Stopwatch.StartNew();

            var instructions = ParseInstructions(input);
            var network = ParseNetwork(input);

            var startNodes = network.Keys.Where(v => v[^1] == 'A').ToArray();

            var node2LoopCount = new Dictionary<string, HashSet<int>>();

            Parallel.ForEach(startNodes, v =>
            {
                var currentNode = v;
                var instructionIndex = 0;

                var index2EndNodeCount = new Dictionary<string, HashSet<int>>();

                while (true)
                {
                    var instruction = instructions[instructionIndex % instructions.Length];

                    currentNode = instruction switch
                    {
                        'L' => network[currentNode].Item1,
                        'R' => network[currentNode].Item2,
                        _ => throw new ArgumentException()
                    };

                    instructionIndex++;

                    if (currentNode[^1] == 'Z')
                    {
                        if (index2EndNodeCount.ContainsKey(currentNode))
                        {
                            //Doing the loop twice to make sure we dont go a different root after finding a "Z" Node once.
                            var clampedInstructionIndex = instructionIndex % instructions.Length;
                            if (index2EndNodeCount[currentNode].Any(v => v % instructions.Length == clampedInstructionIndex))
                            {
                                //Found loops
                                lock (node2LoopCount)
                                {
                                    //Getting all the possible index loops and combine them for the starting node.
                                    node2LoopCount.Add(v, index2EndNodeCount.Values.SelectMany(v => v).ToHashSet());
                                }
                                break;
                            }
                            else
                            {
                                throw new Exception("Presently, the algorithm does not encompass the scenario where the same node is visited multiple times with distinct instruction indices.");
                            }
                        }
                        else
                        {
                            index2EndNodeCount.Add(currentNode, new HashSet<int>() { instructionIndex });
                        }
                    }
                }
            });

            var smallestLoops = node2LoopCount.Values.Select(v => v.Min()).ToArray();
            var lcm = GetLcm(smallestLoops);

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return lcm.ToString();
        }

        static string ParseInstructions(string input)
        {
            return input[..input.IndexOf('\n')];
        }

        static Dictionary<string, (string, string)> ParseNetwork(string input)
        {
            var network = new Dictionary<string, (string, string)>();
            foreach (var item in input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Skip(1))
            {
                var values = item.Split(new[] { '=', ' ', '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                network.Add(values[0], (values[1], values[2]));
            }

            return network;
        }

        static BigInteger GetLcm(int[] numbers)
        {
            var primeDivisors = new List<int>();

            while (numbers.Any(v => v != 1))
            {
                var lowestPrimeDivisor = 2;

                while (numbers.All(v => v % lowestPrimeDivisor != 0))
                {
                    lowestPrimeDivisor = GenerateNextPrime(lowestPrimeDivisor);
                }

                for (int i = 0; i < numbers.Length; i++)
                {
                    if (numbers[i] % lowestPrimeDivisor != 0) continue;

                    numbers[i] /= lowestPrimeDivisor;
                }

                primeDivisors.Add(lowestPrimeDivisor);
            }

            var lcm = new BigInteger(1);

            foreach (var primeDivisor in primeDivisors) lcm *= primeDivisor;

            return lcm;
        }

        static int GenerateNextPrime(int prime)
        {
            if (prime <= 1)
            {
                return 2; // 2 is the first prime number
            }

            int number = prime;
            if (number % 2 == 0) number++;
            else number += 2;

            while (true)
            {
                if (IsPrime(number))
                {
                    return number;
                }

                number += 2;
            }

            return number;

            static bool IsPrime(int num)
            {
                if (num < 2)
                {
                    return false;
                }

                int sqrt = (int)Math.Sqrt(num);

                for (int i = 2; i <= sqrt; i++)
                {
                    if (num % i == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
