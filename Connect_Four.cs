using System;
using System.Threading;//used for pausing the game

namespace FourConnect
{
    public abstract class GamePlayer
    {
        public string Name { get; }
        public char Symbol { get; }
        public ConsoleColor Color { get; }//For adding color

        protected GamePlayer(string name, char symbol, ConsoleColor color)
        {
            Name = name;
            Symbol = symbol;
            Color = color;
        }

        public abstract int SelectColumn(GameBoard board);
    }

    public class HumanPlayer : GamePlayer//2 player game
    {
        public HumanPlayer(string name, char symbol, ConsoleColor color) : base(name, symbol, color) { }

        public override int SelectColumn(GameBoard board)
        {
            int column = -1;
            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine($"{Name}({Symbol}), select a column between 1 and 7:");
                string input = Console.ReadLine();

                if (int.TryParse(input, out column) && column >= 1 && column <= 7 && board.IsColumnAvailable(column - 1))
                {
                    validInput = true;
                }
                else if (column >= 1 && column <= 7)
                {
                    Console.WriteLine("This Column is Full! Try other column");//if column is full 
                }
                else
                {
                    Console.WriteLine("Invalid! Please enter the between 1 and 7");
                }
            }

            return column - 1;
        }
    }

    public class PlayWithComputer : GamePlayer//Random guessing when playing with computer
    {
        private Random r = new Random();

        public PlayWithComputer(string name, char symbol, ConsoleColor color) : base(name, symbol, color) { }

        public override int SelectColumn(GameBoard board)
        {
            Console.WriteLine($"{Name} is guessing...");
            Thread.Sleep(1000);//guessing time for computer pause for 1 second
            return r.Next(0, 7);
        }
    }

    public class GameBoard
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private char[,] grid = new char[Rows, Columns];

        public GameBoard()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    grid[r, c] = '#';
        }

        public bool DropDisc(int column, char symbol)
        {
            if (column < 0 || column >= Columns)
                return false;

            for (int row = Rows - 1; row >= 0; row--)
            {
                if (grid[row, column] == '#')
                {
                    grid[row, column] = symbol;
                    return true;
                }
            }

            return false;
        }

        public void Print(GamePlayer player1, GamePlayer player2)
        {
            Console.WriteLine("\nConnect 4 Game Development Project:\n");

            for (int r = 0; r < Rows; r++)
            {
                Console.Write("| ");
                for (int c = 0; c < Columns; c++)
                {
                    char symbol = grid[r, c];

                    if (symbol == player1.Symbol)
                    {
                        Console.ForegroundColor = player1.Color;
                    }
                    else if (symbol == player2.Symbol)
                    {
                        Console.ForegroundColor = player2.Color;
                    }
                    else
                    {
                        Console.ResetColor();
                    }

                    Console.Write($" {symbol} ");
                }
                Console.ResetColor();
                Console.WriteLine(" |");
            }

            Console.ResetColor();
            Console.WriteLine("   1  2  3  4  5  6  7\n");//added for let the player know which cloumn 
        }


        public bool IsFull()//for checking that all the column are full
        {
            for (int c = 0; c < Columns; c++)
                if (grid[0, c] == '#')
                    return false;

            return true;
        }

        public bool IsColumnAvailable(int column)
        {
            return grid[0, column] == '#';
        }

        public bool CheckWinner(char symbol)// for checking winner 
        {
            // Horizontal checking from left to right
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c <= Columns - 4; c++)
                    if (grid[r, c] == symbol &&
                        grid[r, c + 1] == symbol &&
                        grid[r, c + 2] == symbol &&
                        grid[r, c + 3] == symbol)
                        return true;

            // Vertical checking from top to bottom
            for (int c = 0; c < Columns; c++)
                for (int r = 0; r <= Rows - 4; r++)
                    if (grid[r, c] == symbol &&
                        grid[r + 1, c] == symbol &&
                        grid[r + 2, c] == symbol &&
                        grid[r + 3, c] == symbol)
                        return true;

            // Diagonal checking from top left to bottom right
            for (int r = 0; r <= Rows - 4; r++)
                for (int c = 0; c <= Columns - 4; c++)
                    if (grid[r, c] == symbol &&
                        grid[r + 1, c + 1] == symbol &&
                        grid[r + 2, c + 2] == symbol &&
                        grid[r + 3, c + 3] == symbol)
                        return true;

            // Diagonal checking from bottom left to top right
            for (int r = 3; r < Rows; r++)
                for (int c = 0; c <= Columns - 4; c++)
                    if (grid[r, c] == symbol &&
                        grid[r - 1, c + 1] == symbol &&
                        grid[r - 2, c + 2] == symbol &&
                        grid[r - 3, c + 3] == symbol)
                        return true;

            return false;
        }
    }

    public class ConnectFourGame
    {
        private GameBoard board;
        private GamePlayer player1;
        private GamePlayer player2;

        public ConnectFourGame()
        {
            board = new GameBoard();

            Console.WriteLine("Do you want to play against a computer(press 1) or for 2 players(press 0)?");
            string input = Console.ReadLine().ToLower();

            if (input == "1")
            {
                player1 = new HumanPlayer("Player1", 'X', ConsoleColor.Red);
                player2 = new PlayWithComputer("Computer", 'O', ConsoleColor.Blue);
            }
            else if (input == "0")
            {
                Console.WriteLine("Enter the name for Player 1:");
                string player1Name = Console.ReadLine();
                Console.WriteLine("Enter the name for Player 2:");
                string player2Name = Console.ReadLine();

                player1 = new HumanPlayer(player1Name, 'X', ConsoleColor.Red); // red color for (X) player one
                player2 = new HumanPlayer(player2Name, 'O', ConsoleColor.Blue);//Blue color for (O) player two
            }
            else
            {
                Console.WriteLine("Invalid input. Please restart the game and choose either 0 or 1.");
                Environment.Exit(0);
            }
        }

        public void Start()
        {
            GamePlayer currentPlayer = player1;
            bool gameIsRunning = true;

            while (gameIsRunning)
            {
                Console.Clear();
                board.Print(player1, player2);

                int column = currentPlayer.SelectColumn(board);

                if (column < 0 || column >= 7)
                {
                    Console.WriteLine("Invalid input! Try again.");
                    Console.ReadLine();
                    continue;
                }

                if (board.DropDisc(column, currentPlayer.Symbol))
                {
                    if (board.CheckWinner(currentPlayer.Symbol))
                    {
                        Console.Clear();
                        board.Print(player1, player2);
                        Console.WriteLine($"{currentPlayer.Symbol} {currentPlayer.Name} has Won the Match");
                        gameIsRunning = false;
                    }
                    else if (board.IsFull())
                    {
                        Console.Clear();
                        board.Print(player1, player2);
                        Console.WriteLine("The match is a tie"); // when no one win
                        gameIsRunning = false;
                    }
                    else
                    {
                        currentPlayer = (currentPlayer == player1) ? player2 : player1;
                    }
                }
                else
                {
                    Console.WriteLine("Move is not valid! Try again.");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Game End! Press any key to Exit.");
            Console.ReadKey();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ConnectFourGame game = new ConnectFourGame();
            game.Start();
        }
    }
}