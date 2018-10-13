using System;
using System.Collections.Generic;
using static OthelloPlayer.Startup.Game.OrderedPair;

namespace OthelloPlayer.Startup.Game
{
    public class GameboardManager
    {
        #region Private Fields

        private Gameboard _gameboard;

        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Properties

        public int Size => _gameboard.Board.GetLength(0);
        public bool Finish => ValidHumanMoves.Count == 0 && ValidComputerMoves.Count == 0;

        public List<OrderedPair> ValidHumanMoves { get; private set; }
        public List<OrderedPair> ValidComputerMoves { get; private set; }

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

            ValidComputerMoves = BuildPossibleMovesList(_gameboard, Globals.ComputerToken);
            ValidHumanMoves = BuildPossibleMovesList(_gameboard, Globals.HumanToken);
        }

        public GameboardManager(GameboardManager manager)
        {
            _gameboard = manager.GetGameboardCopy();
            
            ValidComputerMoves = new List<OrderedPair>(manager.ValidComputerMoves.Count);
            manager.ValidComputerMoves.ForEach((move) =>
            {
                ValidComputerMoves.Add(new OrderedPair(move));
            });

            ValidHumanMoves = new List<OrderedPair>(manager.ValidHumanMoves.Count);
            manager.ValidHumanMoves.ForEach((move) =>
            {
                ValidHumanMoves.Add(new OrderedPair(move));
            });
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

                // Check if this is a valid move (Computer).
                if (value == Globals.ComputerToken && !HasOrderedPair(ValidComputerMoves, orderedPair))
                {
                    throw new ArgumentException($"{orderedPair} is not a valid move for {value}.");
                }

                // Check if this is a valid move (Human).
                if (value == Globals.HumanToken && !HasOrderedPair(ValidHumanMoves, orderedPair))
                {
                    throw new ArgumentException($"{orderedPair} is not a valid move for {value}.");
                }

                // 1. Place token.
                _gameboard[orderedPair] = value;

                // 2. Capture pieces.
                FlipTokens(_gameboard, orderedPair, value);

                // Re-build 'moves' lists.
                ValidComputerMoves = BuildPossibleMovesList(_gameboard, Globals.ComputerToken);
                ValidHumanMoves = BuildPossibleMovesList(_gameboard, Globals.HumanToken);
            }
        }
        
        public int Score(Token token)
        {
            var counter = 0;
            for (var x = 0; x < _gameboard.Board.GetLength(0); ++x)
            {
                for (var y = 0; y < _gameboard.Board.GetLength(1); ++y)
                {
                    if (_gameboard[new OrderedPair(x, y)] == token)
                    {
                        ++counter;
                    }
                }
            }
            return counter;
        }

        public Gameboard GetGameboardCopy()
        {
            return new Gameboard(_gameboard);
        }

        public static bool HasOrderedPair(List<OrderedPair> list, OrderedPair orderedPair)
        {
            foreach (var item in list)
            {
                if (item.Equals(orderedPair))
                {
                    return true;
                }
            }

            return false;
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

        private static bool IsValid(Gameboard gameboard, OrderedPair orderedPair, Token currentToken)
        {
            if (currentToken == Token.Open)
            {
                throw new ArgumentException($"Cannot place {currentToken}, must use {Token.White} or {Token.Black}.");
            }

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var newOrderedPair = orderedPair + direction;

                if (!gameboard.IsValidPosition(newOrderedPair))
                {
                    continue;
                }

                if (gameboard[newOrderedPair] == Token.Open)
                {
                    // Go to next direction. (Open token)
                    continue;
                }

                if (gameboard[newOrderedPair] == currentToken)
                {
                    // Go to next direction. (Same token)
                    continue;
                }

                if (Search(gameboard, newOrderedPair, direction, currentToken))
                {
                    // There exists a move that can capture opponent pieces.
                    return true;
                }
            }

            // No valid moves from this position.
            return false;
        }

        private static bool Search(Gameboard gameboard, OrderedPair orderedPair, Direction direction, Token currentToken)
        {
            while (true)
            {
                if (!gameboard.IsValidPosition(orderedPair))
                {
                    return false;
                }

                if (gameboard[orderedPair] == Token.Open)
                {
                    return false;
                }

                if (gameboard[orderedPair] == currentToken)
                {
                    return true;
                }

                orderedPair = orderedPair + direction;
            }
        }

        private static void FlipTokens(Gameboard gameboard, OrderedPair orderedPair, Token currentToken)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (!Search(gameboard, orderedPair + direction, direction, currentToken))
                {
                    continue;
                }

                var temp = new OrderedPair(orderedPair + direction);
                while (gameboard[temp] != currentToken)
                {
                    gameboard[temp] = currentToken;
                    temp = temp + direction;
                }
            }
        }

        private static List<OrderedPair> BuildPossibleMovesList(Gameboard gameboard, Token token)
        {
            if (token == Token.Open)
            {
                throw new ArgumentException($"Cannot place {token}, must use {Token.White} or {Token.Black}.");
            }

            var movesList = new List<OrderedPair>();

            for (var x = 0; x < gameboard.Board.GetLength(0); ++x)
            {
                for (var y = 0; y < gameboard.Board.GetLength(1); ++y)
                {
                    var orderPair = new OrderedPair(x, y);

                    if (gameboard[orderPair] != Token.Open)
                    {
                        continue;
                    }

                    if (IsValid(gameboard, orderPair, token))
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