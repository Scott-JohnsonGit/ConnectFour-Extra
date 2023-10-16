using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    /// <summary>
    /// A game of connect four
    /// </summary>
    internal class C4Board
    {
        #region Properties
        /// <summary>
        /// The current board state
        /// </summary>
        public BoardSpace[,] Board { get { return _board; } }
        /// <summary>
        /// Start template for the XL board special rule
        /// </summary>
        public readonly BoardSpace[,] XLboardTemplate = new BoardSpace[9, 10]
        {
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty }
        };
        /// <summary>
        /// Board state displayed as values 0 - empty, 1 - player one, 2 - player two, 3 - player three, 4 - special type
        /// </summary>
        public string DebugBoard { get { return _debugBoard; } }
        #endregion
        /// <summary>
        /// Editable state of the game board
        /// </summary>
        private BoardSpace[,] _board = new BoardSpace[6, 7]
        {
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty },
            { BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty, BoardSpace.Empty }
        };
        /// <summary>
        /// Editable _debug board, updates per piece placed
        /// </summary>
        private string _debugBoard;
        /// <summary>
        /// Players in the game
        /// </summary>
        private ushort MaxPlayers = 2;
        /// <summary>
        /// The current players turn
        /// </summary>
        private ushort PTurn = 0;
        /// <summary>
        /// Create a game of Connect four 
        /// </summary>
        /// <param name="rule">Special rule to be played in</param>
        public C4Board(SpecialRules rule)
        {
            SetDebug();
        }
        /// <summary>
        /// Updates and sets the DebugBoard string
        /// </summary>
        private void SetDebug()
        {
            _debugBoard = "";
            for (int y = 0; y < _board.GetLength(0); y++)
            {
                for (int x = 0; x < _board.GetLength(1); x++)
                {
                    _debugBoard += $"{(int)_board[y, x]}, "; // add each value to string indiviually
                }
                _debugBoard += "\n"; // add a new line after each row
            }
        }
        /// <summary>
        /// Finds the lowest unoccupied space in a game of connect four
        /// </summary>
        /// <param name="column">Column to search down</param>
        /// <returns>Lowest available space</returns>
        /// <exception cref="Exception">Inability to find suitable location</exception>
        public int LowestEmptySpace(int column)
        {
            // Ignore garbage column inputs
            if (column >= _board.GetLength(0) || column < 0 || _board[0, column] != BoardSpace.Empty)
            {
                return -1;
            }
            // If no pieces have been placed in column
            else if (_board.GetLength(0) - 1 == (int)BoardSpace.Empty)
            {
                return _board.GetLength(0) - 1;
            }
            // Search down the specified column
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                if (_board[i, column] != BoardSpace.Empty)
                {
                    return i - 1;
                }
            }
            throw new Exception("No valid space was found");
        }
        /// <summary>
        /// Attempt to change value of a board space
        /// </summary>
        /// <param name="row">X coordinate</param>
        /// <param name="column">Y coordinate</param>
        /// <returns>Success of attempt</returns>
        public bool TryPlacePiece(int row, int column)
        {
            if (row > -1)
            {
                _board[row, column] = (BoardSpace)PTurn + 1;
                // adjust debug string
                SetDebug();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Change the current player turn
        /// </summary>
        public void ChangePlayer()
        {
            ChangePlayer(69);
        }
        /// <summary>
        /// Switches the current player by requested
        /// </summary>
        /// <param name="player">Requested player</param>
        /// <exception cref="Exception">Invalid player</exception>
        public void ChangePlayer(ushort player)
        {
            if (player != 69)
            {
                if (player > 3)
                {
                    throw new Exception($"No player number {player}");
                }
                else
                {
                    PTurn = (ushort)(player - 1);
                    return;
                }
            }
            // no requested player
            if (PTurn < MaxPlayers)
            {
                PTurn++;
            }
            else
            {
                PTurn = 0;
            }
        }
        /// <summary>
        /// Look for consecutive pieces belonging to the same player
        /// </summary>
        /// <param name="player">Winning player number</param>
        /// <returns>True if a player has won the game, else false</returns>
        public bool CheckWin(out int player)
        {
            // checks each player seperately
            for (ushort i = 0; i < MaxPlayers; i++)
            {
                player = i + 1;
                if (HorizontalW((ushort)player) || VerticalW((ushort)player))
                {
                    return true;
                }
            }
            // no player has won
            player = -1;
            return false;
        }
        /// <summary>
        /// Searches all horizontal rows
        /// </summary>
        /// <param name="player">current player being checked</param>
        /// <returns>True if player has won</returns>
        private bool HorizontalW(ushort player)
        {
            for (int y = 0; y < _board.GetLength(0); y++)
            {
                int ConsecutivePieces = 0;
                for (int x = 0; x < _board.GetLength(1) - 3; x++)
                {
                    if (_board[y, x] == (BoardSpace)player)
                    {
                        ConsecutivePieces++;
                    }
                    // Non corrent player piece interupts counting
                    else
                    {
                        ConsecutivePieces = 0;
                    }
                    // player has won
                    if (ConsecutivePieces > 3)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Searches all columns
        /// </summary>
        /// <param name="player">Current player being checked</param>
        /// <returns>True if player has won</returns>
        private bool VerticalW(ushort player)
        {
            for (int x = 0; x < _board.GetLength(1); x++)
            {
                int consecutivePieces = 0;
                for (int y = 0; y < _board.GetLength(0); y++)
                {
                    if (_board[y, x] == (BoardSpace)player)
                    {
                        consecutivePieces++;
                    }
                    // Non corrent player piece interupts counting
                    else
                    {
                        consecutivePieces = 0;
                    }
                    // player has won
                    if (consecutivePieces > 3)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    /// <summary>
    /// Each type of acceptable space on each board position
    /// </summary>
    enum BoardSpace
    {
        Empty = 0,
        PlayerOne = 1,
        PlayerTwo = 2,
        PlayerThree = 3,
        SpecialCase = 4,
    }
    /// <summary>
    /// Types of acceptable special rules for the game
    /// </summary>
    enum SpecialRules
    {
        None = 0,
        ExtraPlayer = 1,
        LargeBoard = 2,
        DoubleTurn = 3
    }
}
