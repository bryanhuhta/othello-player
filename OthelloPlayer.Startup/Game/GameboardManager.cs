using System;
using System.Collections.Generic;
using static OthelloPlayer.Startup.Game.OrderedPair;

namespace OthelloPlayer.Startup.Game
{
    public class GameboardManager
    {
        #region Private Fields

        private static Gameboard _gameboard;

        #endregion

        #region Properties

        public int Size => _gameboard.Board.GetLength(0);
        public List<OrderedPair> ValidWhiteMoves { get; private set; }
        public List<OrderedPair> ValidBlackMoves { get; private set; }
        
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

            ValidBlackMoves = BuildPossibleMovesList(Token.Black);
            ValidWhiteMoves = BuildPossibleMovesList(Token.White);
        }

        #endregion

        #region Public Methods
        
        public Token this[OrderedPair orderedPair]
        {
            get
            {
                // Validate position bounds.
                if (!_gameboard.IsValidPosition(orderedPair))
                {
                    throw new ArgumentOutOfRangeException(
                        $"{nameof(orderedPair)} is {orderedPair}, but must be between {Gameboard.MinimumSize - 1} and {Size - 1}.");
                }

                return _gameboard[orderedPair];
            }

            // TODO: Implement rules on where valid positions are.
            set
            {
                // Validate position bounds.
                if (!_gameboard.IsValidPosition(orderedPair))
                {
                    throw new ArgumentOutOfRangeException(
                        $"{nameof(orderedPair)} is {orderedPair}, but must be between {Gameboard.MinimumSize - 1} and {Size - 1}.");
                }

                // Make sure either Black or White is trying to be placed.
                if (value == Token.Open)
                {
                    throw new ArgumentException($"Cannot place {value}, must use {Token.White} or {Token.Black}.");
                }

                // Make sure this position is open. !!Is this needed?
                if (_gameboard[orderedPair] != Token.Open)
                {
                    throw new ArgumentException(
                        $"Position {orderedPair} already has value {_gameboard[orderedPair]}. Value of {Token.Open} is required.");
                }

                // Check if this is a valid move.
                if (value == Token.Black && !ValidBlackMoves.Contains(orderedPair))
                {
                    // throw exception
                }

                // 1. Place token.

                // 2. Capture pieces.

                // Re-build 'moves' lists.
                ValidBlackMoves = BuildPossibleMovesList(Token.Black);
                ValidWhiteMoves = BuildPossibleMovesList(Token.White);
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
        
        private static bool IsValid(OrderedPair orderedPair, Token currentToken)
        {
            if (currentToken == Token.Open)
            {
                throw new ArgumentException($"Cannot place {currentToken}, must use {Token.White} or {Token.Black}.");
            }
            
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var newOrderedPair = orderedPair + direction;

                if (_gameboard[newOrderedPair] == Token.Open)
                {
                    // Go to next direction. (Open token)
                    continue;
                }

                if (_gameboard[newOrderedPair] == currentToken)
                {
                    // Go to next direction. (Same token)
                    continue;
                }

                if (Search(newOrderedPair, direction, currentToken))
                {
                    // There exists a move that can capture opponent pieces.
                    return true;
                }
            }

            // No valid moves from this position.
            return false;
        }

        private static bool Search(OrderedPair orderedPair, Direction direction, Token currentToken)
        {
            if (!_gameboard.IsValidPosition(orderedPair))
            {
                return false;
            }

            if (_gameboard[orderedPair] == Token.Open)
            {
                return false;
            }

            if (_gameboard[orderedPair] == currentToken)
            {
                return true;
            }
            
            return Search(orderedPair + direction, direction, currentToken);
        }

        private static List<OrderedPair> BuildPossibleMovesList(Token token)
        {
            if (token == Token.Open)
            {
                throw new ArgumentException($"Cannot place {token}, must use {Token.White} or {Token.Black}.");
            }

            var movesList = new List<OrderedPair>();
            
            for (var x = 0; x < _gameboard.Board.GetLength(0); ++x)
            {
                for (var y = 0; y < _gameboard.Board.GetLength(1); ++y)
                {
                    var orderPair = new OrderedPair(x , y);

                    if (IsValid(orderPair, token))
                    {
                        movesList.Add(orderPair);
                    }
                }
            }

            return movesList;
        }

        #endregion
    }
}