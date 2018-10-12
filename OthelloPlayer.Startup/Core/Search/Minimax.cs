using OthelloPlayer.Startup.Core.Sef;
using OthelloPlayer.Startup.Game;
using System;

namespace OthelloPlayer.Startup.Core.Search
{
    public class Minimax
    {
        #region Private Fields

        private readonly ISef _sef;

        #endregion

        #region Constructor

        public Minimax(ISef sef)
        {
            _sef = sef ?? throw new ArgumentNullException($"{nameof(sef)} is null.");
        }

        #endregion

        #region Public Methods

        public decimal Search(GameboardManager manager, Token token, int currentDepth, int maxDepth)
        {
            if (currentDepth == maxDepth)
            {
                return _sef.Evaluate(manager, token);
            }

            decimal value = 0;

            // Maximizing level.
            if (token == Globals.ComputerToken)
            {
                value = decimal.MinValue;

                foreach (var move in manager.ValidComputerMoves)
                {
                    manager[move] = token;
                    var evaluation = Search(manager, Globals.HumanToken, currentDepth + 1, maxDepth);

                    if (evaluation > value)
                    {
                        value = evaluation;
                    }
                }
            }
            // Minimizing level.
            else
            {
                value = decimal.MaxValue;

                foreach(var move in manager.ValidHumanMoves)
                {
                    manager[move] = token;
                    var evaluation = Search(manager, Globals.ComputerToken, currentDepth + 1, maxDepth);

                    if (evaluation < value)
                    {
                        value = evaluation;
                    }
                }
            }

            return value;
        }

        #endregion
    }
}
