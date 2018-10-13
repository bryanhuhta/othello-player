﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OthelloPlayer.Startup.Game.Display
{
    public static class BoardDisplay
    {
        #region Properties

        public static readonly List<char> Moves = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        #endregion

        #region Public Methods

        public static string DrawBoard(GameboardManager manager, Token nextToken, out Dictionary<char, OrderedPair> letteredMoves)
        {
            var builder = new StringBuilder();
            var piece = ' ';

            var counter = 0;
            letteredMoves = new Dictionary<char, OrderedPair>();

            var validMoves = nextToken == Globals.ComputerToken ? manager.ValidComputerMoves : manager.ValidHumanMoves;
            
            for (var y = manager.Size - 1; y >= 0; --y)
            {
                builder.Append(DrawRowDivider(manager.Size));

                var row = new List<char>(manager.Size);

                for (var x = 0; x < manager.Size; ++x)
                {
                    var currentPosition = new OrderedPair(x, y);
                    var token = manager[currentPosition];

                    if (token == Token.Open)
                    {
                        if (GameboardManager.HasOrderedPair(validMoves, currentPosition))
                        {
                            piece = Moves[counter];
                            letteredMoves.Add(Moves[counter], currentPosition);
                            ++counter;
                        }
                        else
                        {
                            piece = ' ';
                        }
                    }
                    else if (token == Token.Black)
                    {
                        piece = 'B';
                    }
                    else if (token == Token.White)
                    {
                        piece = 'W';
                    }
                    else
                    {
                        piece = '!';
                    }

                    row.Add(piece);
                }

                builder.Append(DrawRow(row));
            }

            builder.Append(DrawRowDivider(manager.Size));
            
            return builder.ToString();
        }
        
        #endregion

        #region Private Methods

        private static string DrawRow(List<char> row)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < row.Count; ++i)
            {
                builder.Append($"| {row[i]} ");
            }

            builder.Append($"|{Environment.NewLine}");

            return builder.ToString();
        }

        private static string DrawRowDivider(int size)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < (size * 4) + 1; ++i)
            {
                builder.Append('-');
            }

            builder.Append($"{Environment.NewLine}");

            return builder.ToString();
        }

        #endregion
    }
}
