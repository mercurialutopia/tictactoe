using System;

namespace tictactoe
{
    class Program
    {
        static void Main(string[] args)
        {
            Game g = new Game();
            CompPlayer compO = new CompPlayer(g.Board, Tic.O);
            //CompPlayer compX = new CompPlayer(g.Board, Tic.X);

            Console.WriteLine(g);

            while(!g.IsGameOver())
            {
                if(g.WhoseTurnIsIt() == Tic.X)
                {
                    int position = GetPosition();
                    while(!g.Play(position))
                        position = GetPosition();
                    // int position = compX.GetNextMove();
                    // bool success = g.Play(position);
                }
                else
                {
                    int position = compO.GetNextMove();
                    bool success = g.Play(position);
                    if(!success)
                        Console.WriteLine($"Computer player was not successful. Chose {position}.");
                }
                Console.WriteLine(g.ToString());
            }
            Console.WriteLine();
            Console.WriteLine("Game over");
        }

        static int GetPosition()
        {
            int position;
            Console.Write("Position: ");
            string read = Console.ReadLine();
            while(!int.TryParse(read, out position))
                read = Console.ReadLine();
            return position;
        }
    }
}
