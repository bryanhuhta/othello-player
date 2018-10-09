using System;

namespace OthelloPlayer.Startup.Game
{
    public class GameboardManager
    {
        #region Private Fields

        private Gameboard _gameboard;

        #endregion

        #region Properties

        public int Size => _gameboard.Board.GetLength(0);

        #endregion

        #region Constructor

        public GameboardManager(int size = 10)
        {
            if (size < 4)
            {
                throw new ArgumentException($"{nameof(size)} is {size}; expected to be at least 4.");
            }

            if (size % 2 != 0)
            {
                throw new ArgumentException($"{nameof(size)} is {size}; expected to be an even value.");
            }

            _gameboard = InitGameboard(size);
        }

        #endregion

        #region Public Methods
        
        public Token this[int x, int y]
        {
            get
            {
                // Validate position bounds.
                if (x < Gameboard.MinimumSize - 1 || x > Size - 1)
                {
                    throw new ArgumentOutOfRangeException(
                        $"{nameof(x)} is {x}, but must be between {Gameboard.MinimumSize - 1} and {Size - 1}.");
                }

                // Validate position bounds.
                if (y < Gameboard.MinimumSize - 1 || y > Size - 1)
                {
                    throw new ArgumentOutOfRangeException(
                        $"{nameof(x)} is {x}, but must be between {Gameboard.MinimumSize - 1} and {Size - 1}.");
                }

                return _gameboard.Board[x, y];
            }

            // TODO: Implement rules on where valid positions are.
            set
            {
                // Validate position bounds.
                if (x < Gameboard.MinimumSize - 1 || x > Size - 1)
                {
                    throw new ArgumentOutOfRangeException(
                        $"{nameof(x)} is {x}, but must be between {Gameboard.MinimumSize - 1} and {Size - 1}.");
                }

                // Validate position bounds.
                if (y < Gameboard.MinimumSize - 1 || y > Size - 1)
                {
                    throw new ArgumentOutOfRangeException(
                        $"{nameof(x)} is {x}, but must be between {Gameboard.MinimumSize - 1} and {Size - 1}.");
                }

                // Make sure either Black or White is trying to be placed.
                if (value == Token.Open)
                {
                    throw new ArgumentException($"Cannot place {value}, must use {Token.White} or {Token.Black}.");
                }

                // Make sure this position is open.
                if (_gameboard.Board[x, y] != Token.Open)
                {
                    throw new ArgumentException(
                        $"Position ({x}, {y}) already has value {_gameboard.Board[x, y]}. Value of {Token.Open} is required.");
                }

                // Place token.
                _gameboard.Board[x, y] = value;
            }
        }

        #endregion

        #region Private Methods
        
        private static Gameboard InitGameboard(int size)
        {
            var gameboard = new Gameboard(size);

            // Set opening pieces.
            gameboard.Board[size / 2, size / 2] = Token.Black;
            gameboard.Board[(size / 2) - 1, (size / 2) - 1] = Token.Black;

            gameboard.Board[(size / 2) - 1, size / 2] = Token.White;
            gameboard.Board[size / 2, (size / 2) - 1] = Token.White;

            return gameboard;
        }
        
        #endregion
    }
}