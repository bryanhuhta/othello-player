using System;

namespace OthelloPlayer.Startup.Game
{
    public class Gameboard
    {
        #region Constants

        public const int MinimumSize = 1;

        #endregion

        #region Properties

        public Token[,] Board { get; }

        #endregion

        #region Constructor

        public Gameboard(int size)
        {
            if (size < MinimumSize)
            {
                throw new ArgumentException($"{nameof(size)} cannot be less than {MinimumSize}.");
            }

            Board = InitBoard(size);
        }

        public Gameboard(Gameboard gameboard)
        {
            if (gameboard == null)
            {
                throw new ArgumentNullException($"{nameof(gameboard)} is null.");
            }

            Board = new Token[gameboard.Board.GetLength(0), gameboard.Board.GetLength(1)];

            Array.Copy(gameboard.Board, Board, gameboard.Board.GetLength(0) * gameboard.Board.GetLength(1));
        }

        #endregion
        
        #region Private Methods

        private static Token[,] InitBoard(int size)
        {
            var board = new Token[size, size];

            // Blank entire board.
            for (var x = 0; x < size; ++x)
            {
                for (var y = 0; y < size; ++y)
                {
                    board[x, y] = Token.Open;
                }
            }

            return board;
        }

        #endregion
    }
}