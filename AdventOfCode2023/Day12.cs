using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Numerics;

namespace AdventOfCode2023
{
    public class Day12
    {
        class ConditionRecord
        {           
            public string Row { get; }
            public int[] Groups { get; }

            public ConditionRecord(string row, int[] groups)
            {
                Row = row;
                Groups = groups;
            }

            public int GetPossibleArrangementsBruteForce()
            {
                var totalPossabilities = (ulong)Math.Pow(2, Row.Count(v => v == Unknown));

                var damageInOriginalRow = Row.Count(v => v == Damaged);
                var totalDamages = Groups.Sum();
                var validPossabilities = 0;
                var neededDamaged = totalDamages - damageInOriginalRow;

                for (ulong current = 0; current < totalPossabilities; current++)
                {
                    var currentDamages = BitOperations.PopCount(current);

                    if (currentDamages != neededDamaged) continue;
                    
                    //Check if the current layout is valid
                    var isValid = true;
                    var currentGroup = 0;
                    var unknownCount = 0;
                    var damageLength = 0;

                    for (int i = 0; i < Row.Length; i++)
                    {
                        var currentChar = Row[i];

                        if (currentChar == Unknown)
                        {
                            currentChar = IsBitSet((ulong)current, unknownCount) ? Damaged : Operational;
                            unknownCount++;
                        }

                        if (currentChar == Operational)
                        {
                            if (damageLength > 0)
                            {
                                if (damageLength != Groups[currentGroup])
                                {
                                    isValid = false;
                                    break;
                                }

                                damageLength = 0;
                                currentGroup++;
                            }

                            continue;
                        }

                        if (currentGroup >= Groups.Length)
                        {
                            isValid = false;
                            break;
                        }

                        //CurrentChar is Damaged
                        damageLength++;

                        if (damageLength > Groups[currentGroup])
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (currentGroup < Groups.Length && Groups[currentGroup] != damageLength)
                    {
                        isValid = false;
                    }

                    if (isValid)
                    {
                        var validPosses = Interlocked.Increment(ref validPossabilities);
                        Console.WriteLine(validPosses);
                    }
                }

                return validPossabilities;
            }

            public long GetPossibleArrangementsRecursive()
            {
                var cache = new Dictionary<(int rowLength, int numberLength), long>();
                return GetPossibles(Row.AsSpan(), new ReadOnlySpan<int>(Groups));

                long GetPossibles(ReadOnlySpan<char> row, ReadOnlySpan<int> numbers)
                {
                    if (row.Length == 0) return numbers.Length == 0 ? 1 : 0;
                    if (numbers.Length == 0) return row.Contains(Damaged) ? 0 : 1;

                    var key = (row.Length, numbers.Length);
                    if (cache.TryGetValue(key, out var cachedResult))
                    {
                        return cachedResult;
                    }

                    long result = 0;

                    if (row[0] == Operational || row[0] == Unknown)
                    {
                        result += GetPossibles(row[1..], numbers);
                    }
                    if (row[0] == Damaged || row[0] == Unknown)
                    {
                        if (numbers[0] <= row.Length && !row[..numbers[0]].Contains(Operational) 
                            && (numbers[0] == row.Length || row[numbers[0]] != Damaged))
                        {
                            result += GetPossibles(row[Math.Min(numbers[0] + 1, row.Length)..], numbers[1..]);
                        }
                    }

                    cache.Add(key, result);

                    return result;
                }
            }

            static bool IsBitSet(ulong number, int bitPosition)
            {
                // Shift 1 to the left by the desired bit position and perform bitwise AND with the number
                // If the result is not zero, the bit is set; otherwise, it's not set
                return (number & (1u << bitPosition)) != 0;
            }
        }

        const char Operational = '.';
        const char Damaged = '#';
        const char Unknown = '?';

        public static string Part1()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/12/input");

            var sw = Stopwatch.StartNew();

            var records = ParseInput_Part1(input);

            var sum = 0;
            foreach (var record in records)
            {
                var possabilites = record.GetPossibleArrangementsBruteForce();
                sum += possabilites;
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return sum.ToString();
        }

        public static string Part2()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/12/input");

            var sw = Stopwatch.StartNew();

            var records = ParseInput_Part2(input);

            long sum = 0;

            foreach (var record in records)
            {
                var possabilites = record.GetPossibleArrangementsRecursive();
                sum += possabilites;
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return sum.ToString();
        }

        static ConditionRecord[] ParseInput_Part1(string input)
        {
            var rows = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var records = new ConditionRecord[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                var data = rows[i].Split(new char[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);
                records[i] = new ConditionRecord(data[0], data.Skip(1).Select(int.Parse).ToArray());
            }

            return records;
        }

        static ConditionRecord[] ParseInput_Part2(string input)
        {
            var rows = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var records = new ConditionRecord[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                var data = rows[i].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                records[i] = new ConditionRecord(
                    string.Join(Unknown, Enumerable.Repeat(data[0], 5)),
                    Enumerable.Repeat(data.Skip(1).Select(int.Parse), 5).SelectMany(v => v).ToArray());
            }

            return records;
        }
    }
}
