using OthelloPlayer.Startup.Game;
using OthelloPlayer.Startup.Game.Display;
using System;
using System.Collections.Generic;

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
                var possibleMoves = new Dictionary<char, OrderedPair>();

                var manager = new GameboardManager(8);

                Console.WriteLine(BoardDisplay.DrawBoard(manager, Token.Black, out possibleMoves));

                var move = GetMove(possibleMoves);

                manager[move] = Token.Black;

                Console.WriteLine(BoardDisplay.DrawBoard(manager, Token.White, out possibleMoves));

                // Game loop.

                    // Black
                        

                    // White

                // End loop.
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
                var input = Console.ReadKey().KeyChar;
                
                if (!pairs.ContainsKey(input))
                {
                    Console.WriteLine($"{input} is not valid. Enter a new move:");
                }
                else
                {
                    return pairs[input];
                }
            }
        }
    }
}
