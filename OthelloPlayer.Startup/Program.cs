using OthelloPlayer.Startup.Game;
using OthelloPlayer.Startup.Game.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using OthelloPlayer.Startup.Core.Search;
using OthelloPlayer.Startup.Core.Sef;

namespace OthelloPlayer.Startup
{
    public class Program
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            
            try
            {
                var manager = new GameboardManager(8);
                var minimax = new Minimax(new SimpleSef(), maxDepth: 2);

                var possibleMoves = new Dictionary<char, OrderedPair>();
                var lastMove = new OrderedPair();
                var currentTurn = Token.Black;

                while (!manager.Finish)
                {
                    if (currentTurn == Globals.ComputerToken && manager.ValidComputerMoves.Count != 0)
                    {
                        var movesByWeight = new Dictionary<OrderedPair, decimal>();
                        var maxWeight = decimal.MinValue;

                        foreach (var move in manager.ValidComputerMoves)
                        {
                            // Copy the gameboard.
                            var searchManager = new GameboardManager(manager);

                            // Perform this move.
                            searchManager[move] = Globals.ComputerToken;
                            Logger.Debug(BoardDisplay.DrawBoard(searchManager, currentTurn, out possibleMoves));

                            // Search this tree.
                            var result = minimax.Search(searchManager, Globals.HumanToken, 0);

                            movesByWeight.Add(move, result);
                        }

                        foreach (var move in movesByWeight)
                        {
                            if (move.Value > maxWeight)
                            {
                                maxWeight = move.Value;
                                lastMove = move.Key;
                            }
                        }

                        // Make the move.
                        manager[lastMove] = Globals.ComputerToken;
                    }

                    if (currentTurn == Globals.HumanToken && manager.ValidHumanMoves.Count != 0)
                    {
                        Console.WriteLine($"Human Turn\tLast Move: {lastMove}");
                        Console.WriteLine(BoardDisplay.DrawBoard(manager, currentTurn, out possibleMoves));

                        lastMove = GetMove(possibleMoves);

                        manager[lastMove] = Globals.HumanToken;
                    }

                    currentTurn = SwapTokens(currentTurn);
                    Console.Clear();
                }

                // Score board.
                var blackScore = manager.Score(Token.Black);
                var whiteScore = manager.Score(Token.White);

                Console.WriteLine(BoardDisplay.DrawBoard(manager, currentTurn, out possibleMoves));

                Console.WriteLine($"Black Score: {blackScore}");
                Console.WriteLine($"White Score: {whiteScore}");

                if (blackScore > whiteScore)
                {
                    Console.WriteLine("Black Wins!");
                }
                else if (whiteScore > blackScore)
                {
                    Console.WriteLine("White Wins!");
                }
                else
                {
                    Console.WriteLine("Draw!");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private static OrderedPair GetMove(Dictionary<char, OrderedPair> pairs)
        {
            Console.WriteLine("Enter a move:");
            
            while (true)
            {
                var info = Console.ReadKey();
                if (info.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(1);
                }

                var input = info.KeyChar;
                if (!pairs.ContainsKey(input))
                {
                    Console.WriteLine($"{Environment.NewLine}[ {input} ] is not valid. Enter a new move:");
                }
                else
                {
                    return pairs[input];
                }
            }
        }

        private static Token SwapTokens(Token token)
        {
            if (token == Token.Black)
            {
                return Token.White;
            }
            else
            {
                return Token.Black;
            }
        }
    }
}
