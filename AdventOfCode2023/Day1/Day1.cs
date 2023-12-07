using System.Diagnostics;

public static class Day1
{
    struct SpelledNumber
    {
        public string Letters;
        public int Number;
    }

    static readonly SpelledNumber One = new() { Letters = "one", Number = 1 };
    static readonly SpelledNumber Two = new() { Letters = "two", Number = 2 };
    static readonly SpelledNumber Three = new() { Letters = "three", Number = 3 };
    static readonly SpelledNumber Four = new() { Letters = "four", Number = 4 };
    static readonly SpelledNumber Five = new() { Letters = "five", Number = 5 };
    static readonly SpelledNumber Six = new() { Letters = "six", Number = 6 };
    static readonly SpelledNumber Seven = new() { Letters = "seven", Number = 7 };
    static readonly SpelledNumber Eight = new() { Letters = "eight", Number = 8 };
    static readonly SpelledNumber Nine = new() { Letters = "nine", Number = 9 };

    static readonly SpelledNumber[] SpelledNumbers = new[]
    {
        One, Two, Three, Four, Five, Six, Seven, Eight, Nine
    };

    public static string Part1()
    {
        var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/1/input");

        var sum = 0;
        var firstNumber = -1;
        var secondNumber = -1;

        var sw = Stopwatch.StartNew();

        foreach (var item in input)
        {
            if (item == '\n')
            {
                if (secondNumber == -1)
                {
                    sum += firstNumber * 10 + firstNumber;
                } 
                else
                {
                    sum += firstNumber * 10 + secondNumber;
                }
                
                firstNumber = -1;
                secondNumber = -1;
                continue;
            }

            var number = (int)item - 48;
            if (number < 0 || number > 9) continue;
            if (firstNumber == -1)
            {
                firstNumber = number;
            }
            else
            {
                secondNumber = number;
            }            
        }

        sw.Stop();
        Console.WriteLine(sw.Elapsed.ToString());

        return sum.ToString();
    }

    public static string Part2()
    {
        return Part2_Reverse();
        var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/1/input");

        var sw = Stopwatch.StartNew();   

        var sum = 0;
        var firstNumber = -1;
        var secondNumber = -1;

        for (int i = 0; i < input.Length; i++)
        {
            char item = input[i];
            if (item == '\n')
            {
                if (secondNumber == -1)
                {
                    sum += firstNumber * 10 + firstNumber;
                }
                else
                {
                    sum += firstNumber * 10 + secondNumber;
                }

                firstNumber = -1;
                secondNumber = -1;
                continue;
            }
            

            var number = (int)item - 48;
            if (number < 0 || number > 9)
            {
                foreach (var spelledNumber in SpelledNumbers)
                {
                    if (item != spelledNumber.Letters[0]) continue;
                    if (input.Length < i + spelledNumber.Letters.Length || !input[i..(i + spelledNumber.Letters.Length)].Equals(spelledNumber.Letters)) continue;
                    number = spelledNumber.Number;
                    i += spelledNumber.Letters.Length - 2;
                    break;
                }
            }

            if (number < 0 || number > 9) continue;

            if (firstNumber == -1)
            {
                firstNumber = number;
            }
            else
            {
                secondNumber = number;
            }
        }

        sw.Stop();
        Console.WriteLine(sw.Elapsed.ToString());

        //54087

        return sum.ToString();
    }

    public static string Part2_Reverse()
    {
        var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/1/input");

        var sw = Stopwatch.StartNew();

        var sum = 0;
        var firstNumber = -1;
        var fistNumberIndex = -1;
        var secondNumber = -1;

        for (int i = 0; i < input.Length; i++)
        {
            char item = input[i];
            if (item == '\n')
            {
                for (int j = i - (1); j > fistNumberIndex; j--)
                {
                    var secondItem = input[j];
                    if (secondItem == '\n') break;

                    var number2 = (int)secondItem - 48;
                    if (number2 >= 0 && number2 <= 9)
                    {
                        secondNumber = number2;
                    }
                    else
                    {
                        foreach (var spelledNumber in SpelledNumbers)
                        {
                            if (secondItem != spelledNumber.Letters[^1]) continue;
                            if (!input[(j - spelledNumber.Letters.Length + 1)..(j + 1)].Equals(spelledNumber.Letters)) continue;
                            secondNumber = spelledNumber.Number;
                            break;
                        }
                    }

                    if (secondNumber != -1) break;
                }

                if (secondNumber == -1)
                {
                    sum += firstNumber * 10 + firstNumber;
                }
                else
                {
                    sum += firstNumber * 10 + secondNumber;
                }


                firstNumber = -1;
                secondNumber = -1;
                continue;
            }

            if (firstNumber != -1) continue;

            var number = (int)item - 48;
            if (number >= 0 && number <= 9)
            {
                firstNumber = number;
                fistNumberIndex = i;
            }
            else
            {
                foreach (var spelledNumber in SpelledNumbers)
                {
                    if (item != spelledNumber.Letters[0]) continue;
                    if (input.Length < i + spelledNumber.Letters.Length || !input[i..(i + spelledNumber.Letters.Length)].Equals(spelledNumber.Letters)) continue;
                    firstNumber = spelledNumber.Number;                    
                    i += spelledNumber.Letters.Length;
                    fistNumberIndex = i;
                    break;
                }
            }
        }

        sw.Stop();
        Console.WriteLine(sw.Elapsed.ToString());

        return sum.ToString();
    }
}