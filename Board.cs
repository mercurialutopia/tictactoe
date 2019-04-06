using System;
namespace tictactoe
{
    public class Board
    {
        private Cell[] cells;

        public Board()
        {
            Clear();
        }

        public void Clear()
        {
            cells = new Cell[] {
                new Cell(Tic.None),new Cell(Tic.None),new Cell(Tic.None),
                new Cell(Tic.None),new Cell(Tic.None),new Cell(Tic.None),
                new Cell(Tic.None),new Cell(Tic.None),new Cell(Tic.None)
            };
        }

        public bool Set(int cellIndex, Tic tic)
        {
            if(!IsIndexValid(cellIndex))
                return false;

            System.Diagnostics.Trace.WriteLine($"Trying to set {cellIndex} to {tic}");
            bool success = SetCell(cells[cellIndex - 1], tic);
            System.Diagnostics.Trace.WriteLineIf(success, $"Set {cellIndex} to {tic}");
            return success;
        }

        public Tic Get(int cellIndex)
        {
            if(!IsIndexValid(cellIndex))
                return Tic.None;

            System.Diagnostics.Trace.WriteLine($"Cell {cellIndex} is {cells[cellIndex-1]}");
            
            return cells[cellIndex-1].Value;
        }

        private bool SetCell(Cell cell, Tic value)
        {
            if(cell.Value == Tic.None)
            {
                cell.Value = value;
                return true;
            }
            else
            {
                System.Diagnostics.Trace.WriteLine($"Cell is already set.");
                return false;
            }
        }

        private bool IsIndexValid(int cellIndex)
        {
            if(cellIndex < 1 || cellIndex > 9)
            {
                System.Diagnostics.Trace.WriteLine($"Cell index {cellIndex} is out of bounds (1-9 incl).");
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.AppendLine($"{cells[0].ToString("1")} | {cells[1].ToString("2")} | {cells[2].ToString("3")}");
            builder.AppendLine("---------");
            builder.AppendLine($"{cells[3].ToString("4")} | {cells[4].ToString("5")} | {cells[5].ToString("6")}");
            builder.AppendLine("---------");
            builder.Append($"{cells[6].ToString("7")} | {cells[7].ToString("8")} | {cells[8].ToString("9")}");
            return builder.ToString();
        }

        private class Cell
        {
            public Cell(Tic initValue) => _value = initValue;

            private Tic _value;
            public Tic Value 
            { 
                get { return _value; } 
                set 
                { 
                    if(_value == Tic.None)
                        _value = value;
                }
            }

            public override string ToString()
            {
                return ToString("?");
            }

            public string ToString(string blank)
            {
                return (_value == Tic.None ? blank : _value.ToString());
            }
        }

    }
}