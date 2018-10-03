using System;

namespace OthelloPlayer.Startup.Game
{
    public class Gameboard
    {
        #region Properties

        public int Size { get; }
        public string WhiteToken { get; } = "O";
        public string BlackToken { get; } = "X";

        #endregion

        #region Constructor

        public Gameboard(int size)
        {
            if (size < 4)
            {
                throw new ArgumentException($"{nameof(size)} is {size}; expected to be at least 4.");
            }

            if (size % 2 != 0)
            {
                throw new ArgumentException($"{nameof(size)} is {size}; expected to be an even value.");
            }

            Size = size;
        }

        #endregion

        #region Public Methods

        

        #endregion
    }
}