using System;
using System.Collections.Generic;

namespace OthelloPlayer.Startup.Game
{
    public class GameboardManager
    {
        #region Private Fields

        private static Gameboard _gameboard;

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

                // loop everything
                //  -> IsValid(x, y)

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
        
        private static bool IsValid(int x, int y, Token currentToken)
        {
            if (currentToken == Token.Open)
            {
                throw new ArgumentException($"Cannot place {currentToken}, must use {Token.White} or {Token.Black}.");
            }
            
            if (_gameboard.Board[x, y] != Token.Open)
            {
                return false;
            }
            
            foreach (var direction in Directions.CardinalDirections)
            {
                var newX = direction.Value.Item1;
                var newY = direction.Value.Item2;

                if (newX < Gameboard.MinimumSize - 1 || newX > _gameboard.Board.GetLength(0) - 1)
                {
                    // Skip this direction. (Out of bounds)
                    continue;
                }

                if (newY < Gameboard.MinimumSize - 1 || newY > _gameboard.Board.GetLength(1) - 1)
                {
                    // Skip this direction. (Out of bounds)
                    continue;
                }

                if (_gameboard.Board[newX, newY] == Token.Open || _gameboard.Board[newX, newY] == currentToken)
                {
                    // Skip this direction. (Spot is open or has same token)
                    continue;
                }

                // search down direction
            }
        }

        public static bool Search(int x, int y, KeyValuePair<string, Tuple<int, int>> direction, Token currentToken)
        {
            if (x < Gameboard.MinimumSize - 1 || x > _gameboard.Board.GetLength(0) - 1)
            {
                return false;
            }

            if (y < Gameboard.MinimumSize - 1 || y > _gameboard.Board.GetLength(1) - 1)
            {
                return false;
            }

            if (_gameboard.Board[x, y] == Token.Open)
            {
                return false;
            }

            if (_gameboard.Board[x, y] == currentToken)
            {
                return true;
            }

            return Search(x + direction.Value.Item1, y + direction.Value.Item2, direction, currentToken);
        }

        #endregion
    }
}