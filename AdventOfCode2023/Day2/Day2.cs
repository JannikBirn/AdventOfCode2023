using System.Diagnostics;

namespace AdventOfCode2023.Day2
{
    public static class Day2
    {
        const string GamePrefix = "Game ";

        const string Red = "red";
        const string Green = "green";
        const string Blue = "blue";

        const int MaxRedCubes = 12;
        const int MaxGreenCubes = 13;
        const int MaxBlueCubes = 14;

        public static string Part1()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/2/input");

            var sw = Stopwatch.StartNew();

            var summedGameIds = 0;
            var currentGameId = 0;
            var currentSetBlue = 0;
            var currentSetRed = 0;
            var currentSetGreen = 0;
            var isParsingGameId = true;
            var isCurrentGameValid = true;

            for (int i = GamePrefix.Length; i < input.Length; i++)
            {
                var character = input[i];

                if (character == '\n')
                {
                    isCurrentGameValid = isCurrentGameValid
                        && currentSetRed <= MaxRedCubes
                        && currentSetGreen <= MaxGreenCubes
                        && currentSetBlue <= MaxBlueCubes;

                    if (isCurrentGameValid)
                    {
                        summedGameIds += currentGameId;
                    }
                    
                    i += GamePrefix.Length;
                    isCurrentGameValid = true;
                    isParsingGameId = true;
                    currentGameId = 0;
                    currentSetBlue = 0;
                    currentSetRed = 0;
                    currentSetGreen = 0;
                    continue;
                }

                if (!isCurrentGameValid) continue;

                if (isParsingGameId)
                {
                    var gameIdNumber = (int)character - 48;

                    if (gameIdNumber < 0 || gameIdNumber > 9)
                    {
                        isParsingGameId = false;
                        continue;
                    }

                    currentGameId = currentGameId * 10 + gameIdNumber;
                    continue;
                }

                if (character == ';')
                {
                    isCurrentGameValid = isCurrentGameValid
                        && currentSetRed <= MaxRedCubes
                        && currentSetGreen <= MaxGreenCubes
                        && currentSetBlue <= MaxBlueCubes;

                    currentSetRed = 0;
                    currentSetGreen = 0;
                    currentSetBlue = 0;                  

                    //todo check if we have more nen max cubes and set game invalid
                    continue;
                }

                if (character == ' ' || character == ',') continue;

                var currentCubeNumber = 0;
                var numberLength = 0;

                for (int j = i; j < input.Length; j++)
                {
                    var cubeNumberChar = input[j];
                    var cubeNumber = (int)cubeNumberChar - 48;
                    if (cubeNumber < 0 || cubeNumber > 9)
                    {
                        numberLength = j - i;
                        break;
                    }

                    currentCubeNumber = currentCubeNumber * 10 + cubeNumber;
                }

                var colorChar = input[i + numberLength + 1]; //+1 because there is always a space infront of the color

                switch (colorChar)
                {
                    case 'r':
                        currentSetRed = currentCubeNumber;
                        i += numberLength + Red.Length;
                        break;
                    case 'g':
                        currentSetGreen = currentCubeNumber;
                        i += numberLength + Green.Length;
                        break;
                    case 'b':
                        currentSetBlue = currentCubeNumber;
                        i += numberLength + Blue.Length;
                        break;
                    default: 
                        throw new ArgumentOutOfRangeException(nameof(colorChar), null, colorChar.ToString());
                }
            }   
            
            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return summedGameIds.ToString();
        }

        public static string Part2()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/2/input");

            var sw = Stopwatch.StartNew();

            var summedPowers = 0;
            var maxSetBlue = 0;
            var maxSetRed = 0;
            var maxSetGreen = 0;

            for (int i = input.IndexOf(':') + 1; i < input.Length; i++)
            {
                var character = input[i];

                if (character == '\n')
                {
                    summedPowers += maxSetBlue * maxSetRed * maxSetGreen;

                    var nextGameIndex = input.IndexOf(':', i);
                    if (nextGameIndex > 0) i = nextGameIndex;
                    maxSetBlue = 0;
                    maxSetRed = 0;
                    maxSetGreen = 0;
                    continue;
                }

                if (character == ' ' || character == ',' || character == ';') continue;

                var currentCubeNumber = 0;
                var numberLength = 0;

                for (int j = i; j < input.Length; j++)
                {
                    var cubeNumberChar = input[j];
                    var cubeNumber = (int)cubeNumberChar - 48;
                    if (cubeNumber < 0 || cubeNumber > 9)
                    {
                        numberLength = j - i;
                        break;
                    }

                    currentCubeNumber = currentCubeNumber * 10 + cubeNumber;
                }

                var colorChar = input[i + numberLength + 1]; //+1 because there is always a space infront of the color

                switch (colorChar)
                {
                    case 'r':
                        if (maxSetRed < currentCubeNumber) maxSetRed = currentCubeNumber;
                        i += numberLength + Red.Length;
                        break;
                    case 'g':
                        if (maxSetGreen < currentCubeNumber) maxSetGreen = currentCubeNumber;
                        i += numberLength + Green.Length;
                        break;
                    case 'b':
                        if (maxSetBlue < currentCubeNumber) maxSetBlue = currentCubeNumber;
                        i += numberLength + Blue.Length;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(colorChar), null, colorChar.ToString());
                }
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());


            return summedPowers.ToString();
        }
    }
}
