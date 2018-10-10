using System.Collections.Generic;

namespace OthelloPlayer.Startup.Game
{
    public static class Directions
    {
        #region Properties

        public static readonly Dictionary<string, OrderedPair> CardinalDirections = new Dictionary<string, OrderedPair>
        {
            { "North", new OrderedPair { X = 0, Y = 1 } },
            { "Northeast", new OrderedPair { X = 1, Y = 1 } },
            { "East", new OrderedPair { X = 1, Y = 0 } },
            { "Southeast", new OrderedPair { X = 1, Y = -1 } },
            { "South", new OrderedPair { X = 0, Y = -1 } },
            { "Southwest", new OrderedPair { X = -1, Y = -1 } },
            { "West", new OrderedPair { X = -1, Y = 0 } },
            { "Northwest", new OrderedPair { X = -1, Y = 1 } }

        };

        #endregion
    }
}
