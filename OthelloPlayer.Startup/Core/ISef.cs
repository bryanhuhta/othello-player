using OthelloPlayer.Startup.Game;

namespace OthelloPlayer.Startup.Core
{
    public interface ISef
    {
        decimal Evaluate(Gameboard gameboard, Token token);
    }
}
