using System;
using System.Collections.Generic;
using System.Text;

namespace OthelloPlayer.Startup.Game.Display
{
    public static class BoardDisplay
    {
        #region Properties

        public static readonly List<char> Moves = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        #endregion

        #region Public Methods

        public static Dictionary<char, OrderedPair> DrawBoard(GameboardManager manager, Token nextToken)
        {
            var piece = ' ';

            var counter = 0;
            var letteredMoves = new Dictionary<char, OrderedPair>();

            var validMoves = (nextToken == Globals.ComputerToken) ? manager.ValidComputerMoves : manager.ValidHumanMoves;

            for (var y = manager.Size - 1; y >= 0; --y)
            {
                var row = new List<char>(manager.Size);

                for (var x = 0; x < manager.Size; ++x)
                {
                    var orderedPair = new OrderedPair(x, y);

                    switch (manager[orderedPair])
                    {
                        case Token.Black:
                            row.Add('B');
                            break;
                        case Token.White:
                            row.Add('W');
                            break;
                        case Token.Open:
                            if (GameboardManager.HasOrderedPair(validMoves, orderedPair))
                            {
                                row.Add(Moves[counter]);
                                letteredMoves.Add(Moves[counter], orderedPair);

                                ++counter;
                            }
                            else
                            {
                                row.Add(' ');
                            }
                            break;
                        default:
                            break;
                    }
                }

                DrawRowDivider(manager.Size);
                DrawRow(row);
            }

            DrawRowDivider(manager.Size);

            return letteredMoves;
        }
        
        #endregion

        #region Private Methods

        private static void DrawRow(List<char> row)
        {
            foreach (var cell in row)
            {
                WriteColor("| ", ConsoleColor.Gray, ConsoleColor.Green);

                switch (cell)
                {
                    case 'B':
                        WriteColor("O ", ConsoleColor.Black, ConsoleColor.Green);
                        break;
                    case 'W':
                        WriteColor("O ", ConsoleColor.White, ConsoleColor.Green);
                        break;
                    default:
                        WriteColor($"{cell} ", ConsoleColor.Red, ConsoleColor.Green);
                        break;
                }
            }

            WriteColor($"|{Environment.NewLine}", ConsoleColor.Gray, ConsoleColor.Green);
        }

        private static void DrawRowDivider(int size)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < (size * 4) + 1; ++i)
            {
                builder.Append('-');
            }

            WriteLineColor(builder.ToString(), ConsoleColor.Gray, ConsoleColor.Green);
        }

        private static void WriteColor(string text, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            
            Console.Write(text);

            Console.ResetColor();
        }

        private static void WriteLineColor(string text, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;

            Console.WriteLine(text);

            Console.ResetColor();
        }

        #endregion
    }
}
