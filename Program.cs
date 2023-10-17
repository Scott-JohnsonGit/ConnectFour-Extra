using System.Data.Common;

namespace Connect4
{
    internal class Program
    {
        /// <summary>
        /// Sets up game with preset rules and accounts
        /// </summary>
        static void Main(string[] args)
        {
            C4Game game = new C4Game(SpecialRules.None, "Scott(The Creator)", "Luka Aggoune(good guy)");
            // Continue forever (even after winning)
            while (true)
            {
                // Print current board state
                Console.WriteLine(game.GameBoard.DebugBoard);
                // Takes turn checking for a win
                game.TakeTurn(int.Parse(Console.ReadLine()), out Connect4Player? winner, out WinType winCondition);
                Console.Clear();
                // A player won
                if (winner != null)
                {
                    Console.WriteLine($"{winner.CurrentAccount} WON! {winCondition}, this is their total win number {winner.GameWins}");
                }
                // Board filled (draw)
                else if (winCondition == WinType.Draw)
                {
                    Console.WriteLine("DRAW!");
                }
            }
        }
    }
}