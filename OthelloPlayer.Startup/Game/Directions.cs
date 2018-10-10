using System;
using System.Collections.Generic;

namespace OthelloPlayer.Startup.Game
{
    public static class Directions
    {
        #region Properties

        public static readonly Dictionary<string, Tuple<int, int>> CardinalDirections = new Dictionary<string, Tuple<int, int>>
        {
            { "North", new Tuple<int, int>(0, 1) },
            { "Northeast", new Tuple<int, int>(1, 1) },
            { "East", new Tuple<int, int>(1, 0) },
            { "Southeast", new Tuple<int, int>(1, -1) },
            { "South", new Tuple<int, int>(0, -1) },
            { "Southwest", new Tuple<int, int>(-1, -1) },
            { "West", new Tuple<int, int>(-1, 0) },
            { "Northwest", new Tuple<int, int>(-1, 1) }

        };

        #endregion
    }
}
