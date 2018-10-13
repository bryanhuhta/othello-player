using System.Collections.Generic;

namespace OthelloPlayer.Startup.Game
{
    public class OrderedPair
    {
        #region Properties

        public int X { get; set; }
        public int Y { get; set; }

        public static readonly Dictionary<Direction, OrderedPair> Directions = new Dictionary<Direction, OrderedPair>
        {
            { Direction.North, new OrderedPair(0, 1) },
            { Direction.NorthEast, new OrderedPair(1, 1) },
            { Direction.East, new OrderedPair(1, 0) },
            { Direction.SouthEast, new OrderedPair(1, -1) },
            { Direction.South, new OrderedPair(0, -1) },
            { Direction.SouthWest, new OrderedPair (-1, -1) },
            { Direction.West, new OrderedPair(-1, 0) },
            { Direction.NorthWest, new OrderedPair(-1, 1) }

        };

        public enum Direction
        {
            North,
            NorthEast,
            East,
            SouthEast,
            South,
            SouthWest,
            West,
            NorthWest
        };

        #endregion

        #region Constructor

        public OrderedPair()
        {
            X = 0;
            Y = 0;
        }

        public OrderedPair(int x, int y)
        {
            X = x;
            Y = y;
        }

        public OrderedPair(OrderedPair orderedPair)
        {
            X = orderedPair.X;
            Y = orderedPair.Y;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"( {X}, {Y} )";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var temp = (OrderedPair)obj;

            return X == temp.X && Y == temp.Y;
        }

        public static OrderedPair operator +(OrderedPair lhs, OrderedPair rhs)
        {
            return new OrderedPair(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static OrderedPair operator +(OrderedPair lhs, Direction rhs)
        {
            return new OrderedPair(lhs.X + Directions[rhs].X, lhs.Y + Directions[rhs].Y);
        }

        #endregion
    }
}
