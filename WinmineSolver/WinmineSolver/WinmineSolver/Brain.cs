using System;
using System.ComponentModel.Composition;

namespace WinmineSolver
{
	[InheritedExport]
	public interface IBrain
	{
		void Think(ViewInfo viewInfo, Board board);
	}

	public class Brain : IBrain
	{
		private readonly IHands hands;
		private readonly Random rand;

		[ImportingConstructor]
		public Brain(IHands hands)
		{
			this.hands = hands;
			rand = new Random();
		}

		public void Think(ViewInfo viewInfo, Board board)
		{
			if (GameFinished(board))
				hands.ClickToNewGame(viewInfo);

			hands.Click(viewInfo, rand.Next(viewInfo.BoardWidth), rand.Next(viewInfo.BoardHeight));
		}

		private static bool GameFinished(Board board)
		{
			for (var y = 0; y < board.Height; y++)
				for (var x = 0; x < board.Width; x++)
					if (board.Cells[x, y] == Cell.Mine)
						return true;
			return false;
		}
	}
}