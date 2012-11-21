using System;
using System.ComponentModel.Composition;
using System.Drawing;
using WinmineSolver.WinAPI;

namespace WinmineSolver
{
	[InheritedExport]
	public interface IEyes
	{
		void Look();
		void LookFleetly();
	}

	public class Eyes : IEyes
	{
		private readonly IScreenCapturer screenCapturer;
		private readonly IBrain brain;
		private readonly IBoardLogger boardLogger;

		private const int cellSize = 16;
		private const int boardWidth = 30;
		private const int boardHeight = 16;

		private IntPtr hwnd = IntPtr.Zero;

		[ImportingConstructor]
		public Eyes(IScreenCapturer screenCapturer, IBrain brain, IBoardLogger boardLogger)
		{
			this.screenCapturer = screenCapturer;
			this.brain = brain;
			this.boardLogger = boardLogger;
		}

		public void Look()
		{
			EnsureHwnd();

			var board = GetBoard();

			var viewInfo = new ViewInfo
				               {
					               BoardHeight = boardHeight,
					               BoardWidth = boardWidth,
					               CellSize = cellSize,
					               Hwnd = hwnd,
				               };

			brain.Think(viewInfo, board);
		}

		public void LookFleetly()
		{
			EnsureHwnd();
			boardLogger.Log(GetBoard());
		}

		private Board GetBoard()
		{
			var bmp = screenCapturer.CaptureWindow(hwnd);

			var board = new Cell[boardWidth,boardHeight];

			try
			{
				for (var y = 0; y < boardHeight; y++)
				{
					for (var x = 0; x < boardWidth; x++)
					{
						var mean = CalcMeanColor(bmp, 15 + cellSize * x + 1, 100 + cellSize * y + 1, cellSize - 2);

						var cell = GetCell(mean);

						board[x, y] = cell;
					}
				}
			}
			catch (ArgumentOutOfRangeException)
			{
				bmp.Save("error.bmp");
				throw;
			}

			return new Board(board);
		}

		private void EnsureHwnd()
		{
			if (hwnd == IntPtr.Zero)
			{
				hwnd = screenCapturer.FindFirstWindow();
				User32.SetForegroundWindow(hwnd);
			}
		}

		private static Cell GetCell(Color color)
		{
			if (color.R == 132 && color.G == 139 && color.B == 157) return Cell.Unopened;
			if (color.R == 192 && color.G == 192 && color.B == 192) return Cell.Zero;
			if (color.R == 157 && color.G == 157 && color.B == 195) return Cell.One;
			if (color.R == 135 && color.G == 178 && color.B == 135) return Cell.Two;
			if (color.R == 203 && color.G == 131 && color.B == 131) return Cell.Three;
			if (color.R == 137 && color.G == 137 && color.B == 173) return Cell.Four;
			if (color.R == 169 && color.G == 169 && color.B == 123) return Cell.Five;
			if (color.R == 84 && color.G == 88 && color.B == 99) return Cell.Mine;
			if (color.R == 154 && color.G == 10 && color.B == 10) return Cell.Mine;
			if (color.R == 100 && color.G == 106 && color.B == 138) return Cell.Flag;
			return Cell.Unknown;
		}

		private static Color CalcMeanColor(Bitmap bmp, int x, int y, int size)
		{
			var r = 0;
			var g = 0;
			var b = 0;

			for (int xx = 0; xx < size; xx++)
				for (int yy = 0; yy < size; yy++)
				{
					var color = bmp.GetPixel(x + xx, y + yy);
					r += color.R;
					g += color.G;
					b += color.B;
				}

			var total = size * size;

			return Color.FromArgb(r / total, g / total, b / total);
		}
	}
}