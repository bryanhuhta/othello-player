using OthelloPlayer.Startup.Game;

namespace OthelloPlayer.Startup.Core.Sef
{
    public interface ISef
    {
        decimal Evaluate(GameboardManager manager, Token token);
    }
}
