using System.Diagnostics;

namespace AdventOfCode2023.Day10
{
    public static class Day10
    {
        [Flags]
        enum Direction
        {
            None = 0,
            North = 1 << 1,
            East = 1 << 2,
            South = 1 << 3,
            West = 1 << 4,
        }

        struct PipeDefintion
        {
            public char Symbol { get; }
            public Direction Direction { get; }

            public PipeDefintion(char symbol, Direction direction)
            {
                Symbol = symbol;
                Direction = direction;
            }
        }

        const char LeftSymbol = '0';
        const char RightSymbol = '1';

        static readonly PipeDefintion NS = new PipeDefintion('|', Direction.North | Direction.South);
        static readonly PipeDefintion EW = new PipeDefintion('-', Direction.East | Direction.West);
        static readonly PipeDefintion NE = new PipeDefintion('L', Direction.North | Direction.East);
        static readonly PipeDefintion NW = new PipeDefintion('J', Direction.North | Direction.West);
        static readonly PipeDefintion SW = new PipeDefintion('7', Direction.South | Direction.West);
        static readonly PipeDefintion SE = new PipeDefintion('F', Direction.South | Direction.East);
        static readonly PipeDefintion Ground = new PipeDefintion('.', Direction.None);
        static readonly PipeDefintion Start = new PipeDefintion('S', Direction.None);

        static readonly Dictionary<char, PipeDefintion> pipeDefinitions = new Dictionary<char, PipeDefintion>()
        {
            {NS.Symbol, NS },
            {EW.Symbol, EW },
            {NE.Symbol, NE },
            {NW.Symbol, NW },
            {SW.Symbol, SW },
            {SE.Symbol, SE },
            {Ground.Symbol, Ground },
            {Start.Symbol, Start },
        };

        //The pipes are arranged in a two-dimensional grid of tiles:
        //| is a vertical pipe connecting north and south.
        //- is a horizontal pipe connecting east and west.
        //L is a 90-degree bend connecting north and east.
        //J is a 90-degree bend connecting north and west.
        //7 is a 90-degree bend connecting south and west.
        //F is a 90-degree bend connecting south and east.
        //. is ground; there is no pipe in this tile.
        //S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.

        public static string Part1()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/10/input");

            var sw = Stopwatch.StartNew();

            var startIndex = input.IndexOf('S');
            var lineLength = input.IndexOf('\n') + 1;
            var startX = startIndex % lineLength;
            var startY = startIndex / lineLength;
            var maze = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var loopLength = 0;

            var currentX = startX;
            var currentY = startY;

            var nextDirection = Direction.None;
            //Getting the next direction to the first pipe from the starting pos

            if (pipeDefinitions[maze[startY + 1][startX]].Direction.HasFlag(Direction.South))
            {
                nextDirection = Direction.North;
            }
            else if (pipeDefinitions[maze[startY - 1][startX]].Direction.HasFlag(Direction.North))
            {
                nextDirection = Direction.South;
            }
            else if (pipeDefinitions[maze[startY][startX + 1]].Direction.HasFlag(Direction.West))
            {
                nextDirection = Direction.East;
            }
            else
            {
                nextDirection = Direction.West;
            }

            do
            {
                switch (nextDirection)
                {
                    case Direction.North:
                        currentY--;
                        break;
                    case Direction.East:
                        currentX++;
                        break;
                    case Direction.South:
                        currentY++;
                        break;
                    case Direction.West:
                        currentX--;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(nextDirection));
                }

                nextDirection = pipeDefinitions[maze[currentY][currentX]].Direction ^ GetOppositeDirection(nextDirection);
                loopLength++;
            }
            while (currentX != startX || currentY != startY);

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            return (loopLength / 2).ToString();
        }

        public static string Part2()
        {
            var input = Utils.GetAdventInput("https://adventofcode.com/2023/day/10/input");

            var sw = Stopwatch.StartNew();

            var startIndex = input.IndexOf('S');
            var lineLength = input.IndexOf('\n') + 1;
            var startX = startIndex % lineLength;
            var startY = startIndex / lineLength;
            var mazeLines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(v => v.ToCharArray()).ToArray();

            var loopSet = new HashSet<(int, int)>();

            var currentX = startX;
            var currentY = startY;

            var nextDirection = Direction.None;
            //Getting the next direction to the first pipe from the starting pos

            if (pipeDefinitions[mazeLines[startY + 1][startX]].Direction.HasFlag(Direction.South))
            {
                nextDirection = Direction.North;
            }
            else if (pipeDefinitions[mazeLines[startY - 1][startX]].Direction.HasFlag(Direction.North))
            {
                nextDirection = Direction.South;
            }
            else if (pipeDefinitions[mazeLines[startY][startX + 1]].Direction.HasFlag(Direction.West))
            {
                nextDirection = Direction.East;
            }
            else
            {
                nextDirection = Direction.West;
            }

            var startDirection = nextDirection;

            //Mapping the loop
            do
            {
                switch (nextDirection)
                {
                    case Direction.North:
                        currentY--;
                        break;
                    case Direction.East:
                        currentX++;
                        break;
                    case Direction.South:
                        currentY++;
                        break;
                    case Direction.West:
                        currentX--;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(nextDirection));
                }

                nextDirection = pipeDefinitions[mazeLines[currentY][currentX]].Direction ^ GetOppositeDirection(nextDirection);
                loopSet.Add((currentY, currentX));
            }
            while (currentX != startX || currentY != startY);

            //Mapping the right and left side of the loop
            nextDirection = startDirection;
            currentX = startX;
            currentY = startY;
            do
            {
                switch (nextDirection)
                {
                    case Direction.North:
                        currentY--;
                        break;
                    case Direction.East:
                        currentX++;
                        break;
                    case Direction.South:
                        currentY++;
                        break;
                    case Direction.West:
                        currentX--;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(nextDirection));
                }

                var prevDirection = nextDirection;
                nextDirection = pipeDefinitions[mazeLines[currentY][currentX]].Direction ^ GetOppositeDirection(nextDirection);

                switch (nextDirection)
                {
                    case Direction.North:
                        SpreadSymbol(currentY, currentX - 1, LeftSymbol);
                        SpreadSymbol(currentY, currentX + 1, RightSymbol);                      
                        break;
                    case Direction.East:
                        SpreadSymbol(currentY - 1, currentX, LeftSymbol);
                        SpreadSymbol(currentY + 1, currentX, RightSymbol);
                        if (prevDirection == Direction.South) SpreadSymbol(currentY, currentX - 1, RightSymbol);
                        break;
                    case Direction.South:
                        SpreadSymbol(currentY, currentX - 1, RightSymbol);
                        SpreadSymbol(currentY, currentX + 1, LeftSymbol);
                        break;
                    case Direction.West:
                        SpreadSymbol(currentY - 1, currentX, RightSymbol);
                        SpreadSymbol(currentY + 1, currentX, LeftSymbol);
                        if (prevDirection == Direction.North) SpreadSymbol(currentY, currentX + 1, RightSymbol);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(nextDirection));
                }
            }
            while (currentX != startX || currentY != startY);

            var outsideSymbol = mazeLines[0][0];
            var insideSymbol = outsideSymbol == RightSymbol ? LeftSymbol : RightSymbol;

            var insideSymbolCount = mazeLines.Sum(line => line.Count(symbol => symbol == insideSymbol));

            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            Console.WriteLine(string.Join('\n', mazeLines.Select(v => string.Join("", v))));

            return insideSymbolCount.ToString();

            void SpreadSymbol(int y, int x, char symbolToSpread)
            {
                if (y < 0 || y >= mazeLines.Length || x < 0 || x >= mazeLines[y].Length) return;
                if (loopSet.Contains((y, x))) return;

                if (mazeLines[y][x] == symbolToSpread) return;

                mazeLines[y][x] = symbolToSpread;

                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    for (int xOffset = -1; xOffset <= 1; xOffset++)
                    {
                        if (yOffset == 0 && xOffset == 0) continue;

                        SpreadSymbol(y + yOffset, x + xOffset, symbolToSpread);
                    }
                }
            }
        }

        static Direction GetOppositeDirection(Direction nextDirection)
        {
            return nextDirection switch
            {
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.West => Direction.East,
                Direction.East => Direction.West,
                _ => throw new NotImplementedException(),                
            };
        }
    }
}
