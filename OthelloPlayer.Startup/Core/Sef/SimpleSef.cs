using OthelloPlayer.Startup.Game;
using System;
using System.Collections.Generic;
using static OthelloPlayer.Startup.Game.OrderedPair;

namespace OthelloPlayer.Startup.Core.Sef
{
    public class SimpleSef : ISef
    {
        #region Properties
        int weight = 0;

        public int BlackWeight { get; private set; }
        public int WhiteWeight { get; private set; }

        #endregion

        #region Public Methods

        public decimal Evaluate(GameboardManager gameboard, Token token)
        {
            return token != Globals.HumanToken
            ? EdgeWeight(gameboard) + OpenSpaceWeight(gameboard) + CornerWeight(gameboard) + ScoreWeight(gameboard)
                : (-1) * EdgeWeight(gameboard) + OpenSpaceWeight(gameboard) + CornerWeight(gameboard) + ScoreWeight(gameboard);
        }

        #endregion

        #region Private Methods
        private int EdgeWeight(GameboardManager gameboard)
        {
            int weight = 0;

            for (var x = 1; x < gameboard.Size - 2; x++)
            {
                if (gameboard[new OrderedPair(x, 0)] == Globals.ComputerToken)
                {
                    weight++;
                }
                else if (gameboard[new OrderedPair(x, 0)] == Globals.HumanToken)
                {
                    weight--;
                }
            }
            for (var x = 1; x < gameboard.Size - 2; x++)
            {
                if (gameboard[new OrderedPair(x, 1)] == Globals.ComputerToken)
                {
                    weight--;
                }
                else if (gameboard[new OrderedPair(x, 1)] == Globals.HumanToken)
                {
                    weight++;
                }
            }
            for (var x = 1; x < gameboard.Size - 2; x++)
            {
                if (gameboard[new OrderedPair(x, gameboard.Size - 2)] == Globals.ComputerToken)
                {
                    weight--;
                }
                else if (gameboard[new OrderedPair(x, gameboard.Size - 2)] == Globals.HumanToken)
                {
                    weight++;
                }
            }
            for (var x = 1; x < gameboard.Size - 2; x++)
            {
                if (gameboard[new OrderedPair(x, gameboard.Size - 1)] == Globals.ComputerToken)
                {
                    weight++;
                }
                else if (gameboard[new OrderedPair(x, gameboard.Size - 1)] == Globals.HumanToken)
                {
                    weight--;
                }
            }
            for (var y = 0; y < gameboard.Size; y++)
            {
                if (gameboard[new OrderedPair(0, y)] == Globals.ComputerToken)
                {
                    weight++;
                }
                else if (gameboard[new OrderedPair(0, y)] == Globals.HumanToken)
                {
                    weight--;
                }
            }
            for (var y = 0; y < gameboard.Size; y++)
            {
                if (gameboard[new OrderedPair(gameboard.Size - 1, y)] == Globals.ComputerToken)
                {
                    weight++;
                }
                else if (gameboard[new OrderedPair(gameboard.Size - 1, y)] == Globals.HumanToken)
                {
                    weight--;
                }
            }
            return weight;
        }

        private int OpenSpaceWeight(GameboardManager gameboard)
        {
            List<OrderedPair> counter = new List<OrderedPair>();
            int weight = 0;
            for (var x = 0; x < gameboard.Size; x++)
            {
                for (var y = 0; y < gameboard.Size; y++)
                {
                    if (gameboard[new OrderedPair(x, y)] == Globals.ComputerToken)
                    {
                        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                        {
                            var orderedPair = new OrderedPair(x, y) + direction;
                            if (gameboard[orderedPair] == Token.Open)
                            {
                                if (!GameboardManager.HasOrderedPair(counter, orderedPair))
                                {
                                    counter.Add(orderedPair);
                                    weight -= 1;
                                }
                            }
                        }
                    }
                    if (gameboard[new OrderedPair(x, y)] == Globals.HumanToken)
                    {
                        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                        {
                            var orderedPair = new OrderedPair(x, y) + direction;
                            if (gameboard[orderedPair] == Token.Open)
                            {
                                if (!GameboardManager.HasOrderedPair(counter, orderedPair))
                                {
                                    counter.Add(orderedPair);
                                    weight += 1;
                                }
                            }
                        }
                    }
                }
            }
            return weight;
        }

        private int CornerWeight(GameboardManager gameboard)
        {
            if (gameboard[new OrderedPair(0, 0)] == Globals.ComputerToken)
            {
                BlackWeight += 20;
            }
            else if (gameboard[new OrderedPair(0, 0)] == Globals.HumanToken)
            {
                BlackWeight -= 20;
            }
            else if (gameboard[new OrderedPair(0, gameboard.Size)] == Globals.ComputerToken)
            {
                BlackWeight += 20;
            }
            else if (gameboard[new OrderedPair(0, gameboard.Size)] == Globals.HumanToken)
            {
                BlackWeight -= 20;
            }
            else if (gameboard[new OrderedPair(gameboard.Size, 0)] == Globals.ComputerToken)
            {
                BlackWeight += 20;
            }
            else if (gameboard[new OrderedPair(gameboard.Size, 0)] == Globals.HumanToken)
            {
                BlackWeight -= 20;
            }
            else if (gameboard[new OrderedPair(gameboard.Size, gameboard.Size)] == Globals.ComputerToken)
            {
                BlackWeight += 20;
            }
            else if (gameboard[new OrderedPair(gameboard.Size, gameboard.Size)] == Globals.HumanToken)
            {
                BlackWeight -= 20;
            }
            return BlackWeight;
        }

        private int ScoreWeight(GameboardManager gameboard)
        {
            return gameboard.Score(Globals.ComputerToken) - gameboard.Score(Globals.HumanToken);
        }
        #endregion
    }
}
