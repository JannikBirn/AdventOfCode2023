using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day3
{
    public static class Day3
    {
        class SerialNumber
        {
            public int Number;
            public Range IndexRange;
        }

        public static string Part1()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/3/input");

            var sw = Stopwatch.StartNew();

            var symbolIndezies = new HashSet<int>();
            var numbers = new List<SerialNumber>();

            for (int i = 0; i < input.Length; i++)
            {
                var charachter = input[i];

                if (charachter == '.' || charachter == '\n') continue;

                var number = (int)charachter - 48;

                if (number < 0 || number > 9)
                {
                    symbolIndezies.Add(i);
                    continue;
                }

                var lastIndex = i;

                for (int j = i + 1; j < input.Length; j++)
                {
                    var nextChar = input[j];
                    var nextNumber = (int)nextChar - 48;
                    if (nextNumber < 0 || nextNumber > 9) break;

                    number = number * 10 + nextNumber;
                    lastIndex = j;
                }

                numbers.Add(new SerialNumber { Number = number, IndexRange = new Range(i, lastIndex) });
                i = lastIndex;
            }


            var lineLength = input.IndexOf('\n') + 1;
            var sum = 0;

            for (int i = numbers.Count - 1; i >= 0; i--)
            {
                var number = numbers[i];
                var hasSymbol = false;

                for (int x = number.IndexRange.Start.Value - 1; x <= number.IndexRange.End.Value + 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (y == 0 && x >= number.IndexRange.Start.Value && x <= number.IndexRange.End.Value) continue;

                        var index = x + (y * lineLength);
                        if (index < 0 || index >= input.Length) continue;

                        hasSymbol = symbolIndezies.Contains(index);
                        if (hasSymbol) break;
                    }
                    if (hasSymbol) break;
                }

                if (hasSymbol) sum += number.Number;
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return sum.ToString();
        }

        public static string Part2()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/3/input");

            var sw = Stopwatch.StartNew();

            var gearIndezies = new HashSet<int>();
            var numbers = new Dictionary<int, SerialNumber>();

            for (int i = 0; i < input.Length; i++)
            {
                var charachter = input[i];

                if (charachter == '.' || charachter == '\n') continue;

                if (charachter == '*')
                {
                    gearIndezies.Add(i);
                    continue;
                }

                var number = (int)charachter - 48;

                if (number < 0 || number > 9) continue;

                var lastIndex = i;

                for (int j = i + 1; j < input.Length; j++)
                {
                    var nextChar = input[j];
                    var nextNumber = (int)nextChar - 48;
                    if (nextNumber < 0 || nextNumber > 9) break;

                    number = number * 10 + nextNumber;
                    lastIndex = j;
                }

                var serialNumber = new SerialNumber { Number = number, IndexRange = new Range(i, lastIndex) };

                for (int j = i; j <= lastIndex; j++)
                {
                    numbers.Add(j, serialNumber);
                }
                
                i = lastIndex;
            }

            
            var lineLength = input.IndexOf('\n') + 1;
            var sum = 0;

            foreach (var gearIndex in gearIndezies)
            {
                SerialNumber number1 = null;
                SerialNumber number2 = null;

                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;

                        if (!numbers.TryGetValue(gearIndex + x + (y * lineLength), out var serialNumber)) continue;

                        if (number1 == serialNumber) continue;

                        if (number1 == null) 
                        {
                            number1 = serialNumber;
                        }
                        else
                        {
                            number2 = serialNumber;
                            break;
                        }
                    }
                    if (number2 != null) break;
                }

                if (number1 == null || number2 == null) continue;

                sum += number1.Number * number2.Number;
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return sum.ToString();
        }
    }
}