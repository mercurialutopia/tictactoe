namespace tictactoe
{
    public class Game
    {
        private Board _board;

        private Tic _currentPlayer;

        private bool _gameOver;

        public Board Board
        {
            get
            {
                return _board;
            }
        }

        public Game()
        {
            _board = new Board();
            StartNew();
        }

        public void StartNew()
        {
            _board.Clear();
            _currentPlayer = Tic.X;
            _gameOver = false;
        }

        public Tic WhoseTurnIsIt()
        {
            return _currentPlayer;
        }

        public bool Play(int cellIndex)
        {
            if(_currentPlayer != Tic.None)
            {
                bool success = _board.Set(cellIndex, _currentPlayer);
                if(success)
                    _currentPlayer = _currentPlayer.FindOpponent();
                return success;
            }
            else
                return false;
        }

        public bool IsGameOver()
        {
            return _gameOver;
        }

        public override string ToString() 
        {
            Tic? winner = IfThereIsAWinnerWhoIsIt();
            System.Text.StringBuilder build = new System.Text.StringBuilder();
            build.AppendLine(_board.ToString());
            build.AppendLine();
            if(!winner.HasValue)
            {
                build.AppendLine($"{_currentPlayer}'s turn");
            }
            else
            {
                if(winner.Value != Tic.None)
                    build.AppendLine($"{winner.Value} has won.");
                else
                    build.AppendLine($"The game is a draw.");
            }
            return build.ToString();
        }

        private Tic? IfThereIsAWinnerWhoIsIt()
        {
            Tic _1 = _board.Get(1);
            Tic _2 = _board.Get(2);
            Tic _3 = _board.Get(3);
            Tic _4 = _board.Get(4);
            Tic _5 = _board.Get(5);
            Tic _6 = _board.Get(6);
            Tic _7 = _board.Get(7);
            Tic _8 = _board.Get(8);
            Tic _9 = _board.Get(9);

            Status row1Status = CheckForWinner(_1, _2, _3);
            if(row1Status.WinnerFound != Tic.None)
                return row1Status.WinnerFound;

            Status row2Status = CheckForWinner(_4, _5, _6);
            if(row2Status.WinnerFound != Tic.None)
                return row2Status.WinnerFound;

            Status row3Status = CheckForWinner(_7, _8, _9);
            if(row3Status.WinnerFound != Tic.None)
                return row3Status.WinnerFound;

            Status col1Status = CheckForWinner(_1, _4, _7);
            if(col1Status.WinnerFound != Tic.None)
                return col1Status.WinnerFound;

            Status col2Status = CheckForWinner(_2, _5, _8);
            if(col2Status.WinnerFound != Tic.None)
                return col2Status.WinnerFound;

            Status col3Status = CheckForWinner(_3, _6, _9);
            if(col3Status.WinnerFound != Tic.None)
                return col3Status.WinnerFound;

            Status cross1Status = CheckForWinner(_1, _5, _9);
            if(cross1Status.WinnerFound != Tic.None)
                return cross1Status.WinnerFound;

            Status cross2Status = CheckForWinner(_3, _5, _7);
            if(cross2Status.WinnerFound != Tic.None)
                return cross2Status.WinnerFound;

            if(row1Status.NoWinnerPossible &&
               row2Status.NoWinnerPossible &&
               row3Status.NoWinnerPossible &&
               col1Status.NoWinnerPossible &&
               col2Status.NoWinnerPossible &&
               col3Status.NoWinnerPossible &&
               cross1Status.NoWinnerPossible &&
               cross2Status.NoWinnerPossible)
            {
                _gameOver = true;
                return Tic.None;
            }
            else
                return null;
        }

        private Status CheckForWinner(Tic _1, Tic _2, Tic _3)
        {
            Tic? lineResult = CheckLine(_1, _2, _3);
            Status status = new Status();
            if(lineResult.HasValue)
            {
                if(lineResult.Value != Tic.None)
                {
                    _gameOver = true;
                    status.WinnerFound = lineResult.Value;
                }
                else
                {
                    status.NoWinnerPossible = true;
                }
            }   
            return status;
        }

        private Tic? CheckLine(Tic _1, Tic _2, Tic _3)
        {
            if(_1 != Tic.None && _2 != Tic.None && _3 != Tic.None)
            {
                if(_1 == _2 && _2 == _3)
                    return _1; // all three match, so return the match
                else
                    return Tic.None; // all threee are claimed, but neither side has all three, so no one wins.
            }
            else
                return null; // at least one is not claimed, so there is no result.
        }

        private class Status
        {
            public Status()
            {
                WinnerFound = Tic.None;
                NoWinnerPossible = false;
            }
            public Tic WinnerFound { get; set; }
            public bool NoWinnerPossible { get; set; }
        }
    }
}