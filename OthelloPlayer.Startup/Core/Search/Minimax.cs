using OthelloPlayer.Startup.Core.Sef;
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



        #endregion
    }
}
