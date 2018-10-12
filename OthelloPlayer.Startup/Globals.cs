using OthelloPlayer.Startup.Game;
using System.Configuration;

public static class Globals
{
    public static Token HumanToken = ConfigurationManager.AppSettings["ComputerMove"].Equals("First") ? Token.White : Token.Black;
    public static Token ComputerToken = ConfigurationManager.AppSettings["ComputerMove"].Equals("First") ? Token.Black : Token.White;
}
