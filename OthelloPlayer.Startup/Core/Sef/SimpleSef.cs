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

        public decimal Evaluate(GameboardManager manager, Token token)
        {
            return token != Globals.HumanToken
            ? EdgeWeight(manager) + OpenSpaceWeight(manager) + CornerWeight(manager) + ScoreWeight(manager)
                : (-1) * EdgeWeight(manager) + OpenSpaceWeight(manager) + CornerWeight(manager) + ScoreWeight(manager);
        }

        #endregion

        #region Private Methods

        private int EdgeWeight(GameboardManager manager)
        {
            int weight = 0;

            for (var x = 1; x < manager.Size - 2; x++)
            {
                if (manager[new OrderedPair(x, 0)] == Globals.ComputerToken)
                {
                    weight++;
                }
                else if (manager[new OrderedPair(x, 0)] == Globals.HumanToken)
                {
                    weight--;
                }
            }
            for (var x = 1; x < manager.Size - 2; x++)
            {
                if (manager[new OrderedPair(x, 1)] == Globals.ComputerToken)
                {
                    weight--;
                }
                else if (manager[new OrderedPair(x, 1)] == Globals.HumanToken)
                {
                    weight++;
                }
            }
            for (var x = 1; x < manager.Size - 2; x++)
            {
                if (manager[new OrderedPair(x, manager.Size - 2)] == Globals.ComputerToken)
                {
                    weight--;
                }
                else if (manager[new OrderedPair(x, manager.Size - 2)] == Globals.HumanToken)
                {
                    weight++;
                }
            }
            for (var x = 1; x < manager.Size - 2; x++)
            {
                if (manager[new OrderedPair(x, manager.Size - 1)] == Globals.ComputerToken)
                {
                    weight++;
                }
                else if (manager[new OrderedPair(x, manager.Size - 1)] == Globals.HumanToken)
                {
                    weight--;
                }
            }
            for (var y = 0; y < manager.Size; y++)
            {
                if (manager[new OrderedPair(0, y)] == Globals.ComputerToken)
                {
                    weight++;
                }
                else if (manager[new OrderedPair(0, y)] == Globals.HumanToken)
                {
                    weight--;
                }
            }
            for (var y = 0; y < manager.Size; y++)
            {
                if (manager[new OrderedPair(manager.Size - 1, y)] == Globals.ComputerToken)
                {
                    weight++;
                }
                else if (manager[new OrderedPair(manager.Size - 1, y)] == Globals.HumanToken)
                {
                    weight--;
                }
            }
            return weight;
        }

        private int OpenSpaceWeight(GameboardManager manager)
        {
            List<OrderedPair> counter = new List<OrderedPair>();
            int weight = 0;
            for (var x = 0; x < manager.Size; x++)
            {
                for (var y = 0; y < manager.Size; y++)
                {
                    if (manager[new OrderedPair(x, y)] == Globals.ComputerToken)
                    {
                        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                        {
                            var orderedPair = new OrderedPair(x, y) + direction;
                            if (manager[orderedPair] == Token.Open)
                            {
                                if (!GameboardManager.HasOrderedPair(counter, orderedPair))
                                {
                                    counter.Add(orderedPair);
                                    weight -= 1;
                                }
                            }
                        }
                    }
                    if (manager[new OrderedPair(x, y)] == Globals.HumanToken)
                    {
                        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                        {
                            var orderedPair = new OrderedPair(x, y) + direction;
                            if (manager[orderedPair] == Token.Open)
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

        private int CornerWeight(GameboardManager manager)
        {
            if (manager[new OrderedPair(0, 0)] == Globals.ComputerToken)
            {
                BlackWeight += 20;
            }
            else if (manager[new OrderedPair(0, 0)] == Globals.HumanToken)
            {
                BlackWeight -= 20;
            }
            else if (manager[new OrderedPair(0, manager.Size)] == Globals.ComputerToken)
            {
                BlackWeight += 20;
            }
            else if (manager[new OrderedPair(0, manager.Size)] == Globals.HumanToken)
            {
                BlackWeight -= 20;
            }
            else if (manager[new OrderedPair(manager.Size, 0)] == Globals.ComputerToken)
            {
                BlackWeight += 20;
            }
            else if (manager[new OrderedPair(manager.Size, 0)] == Globals.HumanToken)
            {
                BlackWeight -= 20;
            }
            else if (manager[new OrderedPair(manager.Size, manager.Size)] == Globals.ComputerToken)
            {
                BlackWeight += 20;
            }
            else if (manager[new OrderedPair(manager.Size, manager.Size)] == Globals.HumanToken)
            {
                BlackWeight -= 20;
            }
            return BlackWeight;
        }

        private int ScoreWeight(GameboardManager manager)
        {
            return manager.Score(Globals.ComputerToken) - manager.Score(Globals.HumanToken);
        }

        #endregion
    }
}
