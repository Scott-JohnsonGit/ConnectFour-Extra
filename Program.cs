namespace Connect4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            C4Board b = new C4Board(SpecialRules.None); // Create new game with no special rules
            while (true)
            {
                PrintB(b.DebugBoard);
                // check for and print a winner
                if (b.CheckWin(out int player))
                {
                    Console.WriteLine($"Player {player} Won");
                }
                // Find and place the next piece
                int c = AskColumn() - 1;
                b.TryPlacePiece(b.LowestEmptySpace(c), c);
            }
        }
        /// <summary>
        /// Prints visual representation of the current board
        /// </summary>
        /// <param name="board">current board state</param>
        private static void PrintB(string board)
        {
            Console.Clear();
            Console.WriteLine(board);
        }
        /// <summary>
        /// Asks the player for a column to drop their piece
        /// </summary>
        /// <returns>Column player specifies</returns>
        private static int AskColumn()
        {
            // Add idiot proofing later!!!!
            string column = Console.ReadLine();
            return int.Parse(column);
        }
    }
}