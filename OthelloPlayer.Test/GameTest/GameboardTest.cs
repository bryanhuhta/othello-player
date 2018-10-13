using System;
using NUnit.Framework;
using OthelloPlayer.Startup.Game;

namespace OthelloPlayer.Test.GameTest
{
    [TestFixture]
    public class GameboardTest
    {
        [TestCase(1)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(10)]
        public void Gameboard_CreateNewGameboard_Valid(int size)
        {
            var gameboard = new Gameboard(size);

            Assert.NotNull(gameboard);
        }

        [TestCase(1)]
        [TestCase(8)]
        [TestCase(10)]
        public void Gameboard_ValidateNewGameboard_Valid(int size)
        {
            var gameboard = new Gameboard(size);

            for (var x = 0; x < size; ++x)
            {
                for (var y = 0; y < size; ++y)
                {
                    Assert.AreEqual(gameboard.Board[x, y], Token.Open);
                }
            }
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Gameboard_CreateNewGameboard_InvalidSize(int size)
        {
            Assert.Throws<ArgumentException>(() => new Gameboard(size));
        }

        [Test]
        public void Gameboard_CopyNewGameboard_InvalidNullParameter()
        {
            Assert.Throws<ArgumentNullException>(() => new Gameboard(null));
        }
    }
}
