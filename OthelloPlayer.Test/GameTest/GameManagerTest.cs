using System;
using NUnit.Framework;
using OthelloPlayer.Startup.Game;

namespace OthelloPlayer.Test.GameTest
{
    [TestFixture]
    public class GameManagerTest
    {
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(-1)]
        public void GameManager_CreateBoard_InvalidSize(int size)
        {
            Assert.Throws<ArgumentException>(() => new GameboardManager(size));
        }

        [TestCase(4)]
        [TestCase(6)]
        [TestCase(10)]
        public void GameManager_CreateBoardInit_Valid(int size)
        {
            var manager = new GameboardManager(size);
            
            Assert.AreEqual(manager[(size / 2) - 1, (size / 2) - 1], Token.Black);
            Assert.AreEqual(manager[size / 2, size / 2], Token.Black);

            Assert.AreEqual(manager[(size / 2) - 1, size / 2], Token.White);
            Assert.AreEqual(manager[size / 2, (size / 2) - 1], Token.White);
        }

        [TestCase(10, 0, 0, Token.Black)]
        [TestCase(10, 9, 9, Token.White)]
        [TestCase(10, 0, 9, Token.Black)]
        [TestCase(10, 9, 0, Token.White)]
        public void GameManager_SetToken_Valid(int size, int x, int y, Token token)
        {
            var manager = new GameboardManager(size);

            manager[x, y] = token;

            Assert.AreEqual(manager[x, y], token);
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(10, 0)]
        [TestCase(0, 10)]
        public void GameManager_SetToken_InvalidIndex(int x, int y)
        {
            var manager = new GameboardManager();

            Assert.Throws<ArgumentOutOfRangeException>(() => manager[x, y] = Token.Black);
        }

        [Test]
        public void GameManager_SetTokenPlaceOpen_InvalidToken()
        {
            var manager = new GameboardManager();

            Assert.Throws<ArgumentException>(() => manager[0, 0] = Token.Open);
        }

        [Test]
        public void GameManager_SetTokenPlaceRepeated_InvalidToken()
        {
            var manager = new GameboardManager();

            Assert.Throws<ArgumentException>(() => manager[5, 5] = Token.Black);
        }
    }
}