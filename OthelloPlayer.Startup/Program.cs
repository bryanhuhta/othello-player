using OthelloPlayer.Startup.Game;
using OthelloPlayer.Startup.Game.Display;
using System;

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
                var count = 0;

                var manager = new GameboardManager(8);

                Console.WriteLine(BoardDisplay.DrawBoard(manager, Token.Black, out count));
                Console.WriteLine($"count: {count}");
                
                // Game loop.
                //while (!manager.Finish)
                {
                    // Draw board

                    // Player 1 move

                    // Draw board

                    // Player 2 move
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
