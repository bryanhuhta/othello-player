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

        #region Properties

        public int MaxDepth { get; }

        #endregion

        #region Constructor

        public Minimax(ISef sef, int maxDepth)
        {
            _sef = sef ?? throw new ArgumentNullException($"{nameof(sef)} is null.");
            MaxDepth = maxDepth > 1 ? maxDepth : throw new ArgumentException($"{nameof(maxDepth)} is {maxDepth}, but must be greater than 1.");
        }

        #endregion

        #region Public Methods

        public decimal Search(GameboardManager manager, Token token, int currentDepth)
        {
            if (currentDepth == MaxDepth)
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
                    var tempManager = new GameboardManager(manager);
                    tempManager[move] = token;

                    var evaluation = Search(tempManager, Globals.HumanToken, currentDepth + 1);
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
                    var tempManager = new GameboardManager(manager);
                    tempManager[move] = token;

                    var evaluation = Search(tempManager, Globals.ComputerToken, currentDepth + 1);
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
