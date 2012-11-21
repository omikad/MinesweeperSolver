using System;
using System.Text;

namespace WinmineSolver
{
	public class Board
	{
		public Board(Cell[,] cells)
		{
			Cells = cells;
		}

		public readonly Cell[,] Cells;

		public int Width { get { return Cells.GetUpperBound(0) + 1; } }
		public int Height { get { return Cells.GetUpperBound(1) + 1; } }

		public override string ToString()
		{
			var sb = new StringBuilder();

			for (var y = 0; y < Height; y++)
			{
				for (var x = 0; x < Width; x++)
				{
					var cell = Cells[x, y];
					sb.Append(CellTypeToString(cell));
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}

		private static string CellTypeToString(Cell cell)
		{
			if (cell == Cell.Unknown) return "?";
			if (cell == Cell.Unopened) return ".";
			if (cell == Cell.Zero) return "0";
			if (cell == Cell.One) return "1";
			if (cell == Cell.Two) return "2";
			if (cell == Cell.Three) return "3";
			if (cell == Cell.Four) return "4";
			if (cell == Cell.Five) return "5";
			if (cell == Cell.Six) return "6";
			if (cell == Cell.Seven) return "7";
			if (cell == Cell.Eight) return "8";
			if (cell == Cell.Mine) return "*";
			if (cell == Cell.Flag) return "#";
			throw new InvalidOperationException();
		} 
	}
}