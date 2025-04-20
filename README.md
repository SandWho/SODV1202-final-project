# SODV1202-final-project

Team Name: Ain't Nothing But a Work Crew

Team Memners: Harpreet Singh, Gurniaz Singh.

using System;

namespace FourConnect
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectFourGame game = new ConnectFourGame();
            game.Start();
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
            player1 = new GamePlayer('X');
            player2 = new GamePlayer('O');
        }
        public void Start() 
        {
            GamePlayer currentPlayer = player1;
            bool gameIsRunning = true;
            
            while (gameIsRunning) 
            {
                Console.Clear();
                board.Print();
                Console.WriteLine($"{currentPlayer.Name} turn {currentPlayer.symbol} select between column 1&7");

                if (int.TryParse(Console.ReadLine(), out int column))
                {
                    column _= 1;

                    if(board.dropDisc(column,currentPlayer.symbol))
                    {
                        if (board.checkWinner(currentPlayer.symbol))
                        {
                            Console.Clear();
                            board.Print();
                            Console.WriteLine($"{currentPlayer.symbol}{currentPlayer.Name} won");
                            gameIsRunning = false;
                        }
                        else if(board.isFull())
                        {
                            board.Print();
                            Console.WriteLine("this match is tie");
                            gameIsRunning = false;
                        }

                        else
                        {
                            currentPlayer = currentPlayer = player1 ? player2 : player1;

                        }

                    }
                }
    }


}
---- last edited by Harpreet singh

