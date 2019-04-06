using System.Collections.Generic;

namespace tictactoe
{
    public static class Helper
    {
        private static System.Random _rand = new System.Random();

        public static void AddIfNotZero(this IList<int> list, int value)
        {
            if(value > 0)
                list.Add(value);
        }

        public static int GetOneAtRandom(this IList<int> list)
        {
            if(list.Count == 1)
                return list[0];
            else
            {
                int index = _rand.Next(0, list.Count);
                return list[index];
            }
        }

        public static Tic FindOpponent(this Tic player)
        {
            return (player == Tic.X ? Tic.O : Tic.X);
        }
    }
}