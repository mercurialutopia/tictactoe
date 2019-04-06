using System;
using System.Collections.Generic;

namespace tictactoe
{
    /// Logic borrowed from:
    /// Kevin Crowley, Robert S. Siegler (1993). "Flexible Strategy Use in Young Children's Tic-Tac-Toe". Cognitive Science. 17 (4): 531–561. doi:10.1016/0364-0213(93)90003-Q
    /// https://doi.org/10.1016%2F0364-0213%2893%2990003-Q
    public class CompPlayer
    {
        private Board _board;
        private readonly Tic _thisPlayer;

        public CompPlayer(Board board, Tic thisPlayer)
        {
            _board = board;
            _thisPlayer = thisPlayer;
        }

        public int GetNextMove()
        {
            Tic opponent = _thisPlayer.FindOpponent();
            IList<int> nextMove = FindWinOrBlock(_thisPlayer);
            if(nextMove.Count > 0) 
            {
                int position1 = nextMove.GetOneAtRandom();
                Console.WriteLine($"{_thisPlayer} found a win: {position1}");
                return position1;
            }

            nextMove = FindWinOrBlock(opponent);
            if(nextMove.Count > 0) 
            {
                int position2 = nextMove.GetOneAtRandom();
                Console.WriteLine($"{_thisPlayer} found a block: {position2}");
                return position2;
            }

            nextMove = FindForks(_thisPlayer);
            if(nextMove.Count > 0) 
            {
                int position3 = nextMove.GetOneAtRandom();
                Console.WriteLine($"{_thisPlayer} found a fork: {position3}");
                return position3;
            }

            nextMove = FindForks(opponent);
            if(nextMove.Count > 0) 
            {
                int position4 = nextMove.GetOneAtRandom();
                Console.WriteLine($"{_thisPlayer} found a block fork: {position4}");
                return position4;
            }

            int center = FindCenter();
            if(center != 0)
            {
                Console.WriteLine($"{_thisPlayer} found the center: 5");
                return center;
            }

            nextMove = FindOppositeCorners();
            if(nextMove.Count > 0) 
            {
                int position5 = nextMove.GetOneAtRandom();
                Console.WriteLine($"{_thisPlayer} found an opposite corner: {position5}");
                return position5;
            }

            nextMove = FindEmptyCorners();
            if(nextMove.Count > 0) 
            {
                int position6 = nextMove.GetOneAtRandom();
                Console.WriteLine($"{_thisPlayer} found an empty corner: {position6}");
                return position6;
            }

            nextMove = FindEmptySides();
            int position = nextMove.GetOneAtRandom();
            Console.WriteLine($"{_thisPlayer} found an empty side: {position}");
            return position;
        }

        // Win: If the player has two in a row, they can place a third to get three in a row.
        // Block: If the opponent has two in a row, the player must play the third themselves to block the opponent.
        private IList<int> FindWinOrBlock(Tic find)
        {
            IList<int> indicies = new List<int>();

            int row1Index = FindEmptyIndexInALineWithTwoMatching(1,2,3,find);
            indicies.AddIfNotZero(row1Index);

            int row2Index = FindEmptyIndexInALineWithTwoMatching(4,5,6,find);
            indicies.AddIfNotZero(row2Index);

            int row3Index = FindEmptyIndexInALineWithTwoMatching(7,8,9,find);
            indicies.AddIfNotZero(row3Index);

            int col1Index = FindEmptyIndexInALineWithTwoMatching(1,4,7,find);
            indicies.AddIfNotZero(col1Index);

            int col2Index = FindEmptyIndexInALineWithTwoMatching(2,5,8,find);
            indicies.AddIfNotZero(col2Index);

            int col3Index = FindEmptyIndexInALineWithTwoMatching(3,6,9,find);
            indicies.AddIfNotZero(col3Index);

            int cross1Index = FindEmptyIndexInALineWithTwoMatching(1,5,9,find);
            indicies.AddIfNotZero(cross1Index);

            int cross2Index = FindEmptyIndexInALineWithTwoMatching(3,5,7,find);
            indicies.AddIfNotZero(cross2Index);

            return indicies;
        }

        private int FindEmptyIndexInALineWithTwoMatching(int cell1, int cell2, int cell3, Tic find)
        {
            Tic cell1Value = _board.Get(cell1);
            Tic cell2Value = _board.Get(cell2);
            Tic cell3Value = _board.Get(cell3);

            if(cell1Value == find && cell2Value == find && cell3Value == Tic.None)
                return cell3;
            if(cell1Value == find && cell2Value == Tic.None && cell3Value == find)
                return cell2;
            if(cell1Value == Tic.None && cell2Value == find && cell3Value == find)
                return cell1;
            return 0;
        }

        // Fork: Create an opportunity where the player has two threats to win (two non-blocked lines of 2).
        // Blocking an opponent's fork: If there is only one possible fork for the opponent, the player should block it. 
        //  ** Otherwise, the player should block any forks in any way that simultaneously allows them to create two in a row. 
        //  ** Otherwise, the player should create a two in a row to force the opponent into defending, as long as it doesn't result in them creating a fork. For example, if "X" has two opposite corners and "O" has the center, "O" must not play a corner in order to win. (Playing a corner in this scenario creates a fork for "X" to win.)
        // HACK: Need to add the above **.
        
        private IList<int> FindForks(Tic find)
        {
            IList<int> indicies = new List<int>();

            bool corner1Fork = FindCornerFork(1, 2, 3, 9, 4, 7, find);
            if(corner1Fork) indicies.Add(1);
            bool corner3Fork = FindCornerFork(3, 1, 2, 7, 6, 9, find);
            if(corner3Fork) indicies.Add(3);
            bool corner7Fork = FindCornerFork(7, 8, 9, 3, 4, 1, find);
            if(corner7Fork) indicies.Add(7);
            bool corner9Fork = FindCornerFork(9, 7, 8, 1, 6, 3, find);
            if(corner9Fork) indicies.Add(9);

            bool edge2Fork = FindNonCornerFork(2, 1, 3, 5, 8, find);
            if(edge2Fork) indicies.Add(2);
            bool edge4Fork = FindNonCornerFork(4, 5, 6, 1, 7, find);
            if(edge4Fork) indicies.Add(4);
            bool edge6Fork = FindNonCornerFork(6, 4, 5, 3, 9, find);
            if(edge6Fork) indicies.Add(6);
            bool edge8Fork = FindNonCornerFork(8, 7, 9, 2, 5, find);
            if(edge8Fork) indicies.Add(8);

            bool centerFork = FindNonCornerFork(5, 4, 6, 2, 8, find);
            if(centerFork) indicies.Add(5);

            return indicies;
        }

        private bool FindCornerFork(int cornerIndex, int row2, int row3, int cross3, int col2, int col3, Tic find)
        {
            Tic corner = _board.Get(cornerIndex);
            if(corner == Tic.None)
            {
                int rowNoneIndex = FindNoneIndexWhenTheOtherMatchs(row2, row3, find);
                int colNoneIndex = FindNoneIndexWhenTheOtherMatchs(col2, col3, find);
                int crossNoneIndex = FindNoneIndexWhenTheOtherMatchs(5, cross3, find);

                if((rowNoneIndex > 0 && colNoneIndex > 0) ||
                    (crossNoneIndex > 0 && colNoneIndex > 0) ||
                    (rowNoneIndex > 0 && crossNoneIndex > 0))
                {
                    return true;
                }
            }
            return false;
        }

        private bool FindNonCornerFork(int nonCornerIndex, int row2, int row3, int col2, int col3, Tic find)
        {
            Tic corner = _board.Get(nonCornerIndex);
            if(corner == Tic.None)
            {
                int rowNoneIndex = FindNoneIndexWhenTheOtherMatchs(row2, row3, find);
                int colNoneIndex = FindNoneIndexWhenTheOtherMatchs(col2, col3, find);

                if(rowNoneIndex > 0 && colNoneIndex > 0)
                {
                    return true;
                }
            }
            return false;
        }

        // Center: A player marks the center. (If it is the first move of the game, playing on a corner gives the second player more opportunities to make a mistake and may therefore be the better choice; however, it makes no difference between perfect players.)
        private int FindCenter()
        {
            Tic center = _board.Get(5);
            if(center == Tic.None)
                return 5;
            return 0;
        }

        // Opposite corner: If the opponent is in the corner, the player plays the opposite corner.
        private IList<int> FindOppositeCorners()
        {
            IList<int> indicies = new List<int>();

            Tic opponent = _thisPlayer.FindOpponent();

            int cross1Corner = FindNoneIndexWhenTheOtherMatchs(1,9,opponent);
            indicies.AddIfNotZero(cross1Corner);

            int cross2Corner = FindNoneIndexWhenTheOtherMatchs(3,7,opponent);
            indicies.AddIfNotZero(cross2Corner);

            return indicies;
        }

        private int FindNoneIndexWhenTheOtherMatchs(int position1, int position2, Tic match)
        {
            Tic corner1Tic = _board.Get(position1);
            Tic corner2Tic = _board.Get(position2);

            if(corner1Tic == match && corner2Tic == Tic.None)
                return position2;
            if(corner2Tic == match && corner1Tic == Tic.None)
                return position1;
            return 0;
        }

        // Empty corner: The player plays in a corner square.
        private IList<int> FindEmptyCorners()
        {
            IList<int> indicies = new List<int>();

            Tic corner1 = _board.Get(1);
            if(corner1 == Tic.None)
                indicies.Add(1);

            Tic corner3 = _board.Get(3);
            if(corner3 == Tic.None)
                indicies.Add(3);

            Tic corner7 = _board.Get(7);
            if(corner7 == Tic.None)
                indicies.Add(7);

            Tic corner9 = _board.Get(9);
            if(corner9 == Tic.None)
                indicies.Add(9);

            return indicies;
        }

        // Empty side: The player plays in a middle square on any of the 4 sides.
        private IList<int> FindEmptySides()
        {
            IList<int> indicies = new List<int>();
            
            Tic side2 = _board.Get(2);
            if(side2 == Tic.None)
                indicies.Add(2);

            Tic side4 = _board.Get(4);
            if(side4 == Tic.None)
                indicies.Add(4);

            Tic side6 = _board.Get(6);
            if(side6 == Tic.None)
                indicies.Add(6);

            Tic side8 = _board.Get(8);
            if(side8 == Tic.None)
                indicies.Add(8);

            return indicies;
        }
        
    }
}