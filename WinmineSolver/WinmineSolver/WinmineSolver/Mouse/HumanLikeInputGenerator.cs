using System;
using System.ComponentModel.Composition;
using System.Threading;
using WinmineSolver.WinAPI;

namespace WinmineSolver.Mouse
{
	[Export(typeof(IInputGenerator))]
	public class HumanLikeInputGenerator : IInputGenerator
	{
		private readonly IMouseTrailCollection trails;
		private readonly int width;
		private readonly int height;

		[ImportingConstructor]
		public HumanLikeInputGenerator(IMouseTrailCollection trails)
		{
			width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
			height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
			this.trails = trails;
		}

		public void MoveMouseAbsolute(int x, int y)
		{
			var cursor = User32.GetCursorPosition();
			MoveMouseRelative(x - cursor.X, y - cursor.Y);
		}

		public void DoMouseClick()
		{
			User32.mouse_event((uint)(User32.MouseEvent.MOUSEEVENTF_LEFTDOWN | User32.MouseEvent.MOUSEEVENTF_LEFTUP), 0, 0, 0, IntPtr.Zero);
		}

		public override string ToString()
		{
			return "HumanLikeInputGenerator based on: " + trails;
		}

		private void DoMouseMoveAbsolute(int x, int y, MouseHook.MouseMessages message)
		{
			var xx = x * 65536 / width;
			var yy = y * 65536 / height;

			var dwFlags = User32.MouseEvent.MOUSEEVENTF_MOVE | User32.MouseEvent.MOUSEEVENTF_ABSOLUTE;

			if (message == MouseHook.MouseMessages.WM_LBUTTONDOWN) dwFlags |= User32.MouseEvent.MOUSEEVENTF_LEFTDOWN;
			if (message == MouseHook.MouseMessages.WM_LBUTTONUP) dwFlags |= User32.MouseEvent.MOUSEEVENTF_LEFTUP;

			User32.mouse_event((uint)dwFlags, xx, yy, 0, IntPtr.Zero);
		}

		private void MoveMouseRelative(int dx, int dy)
		{
			var trail = trails.GetRandomTrail();
			var matrix = ExtraMath.GetMatrix(dx, dy, trail.DX, trail.DY);
			var moves = trail.Moves;

			var first = moves[0];
			var time = first.Time;

			var start = User32.GetCursorPosition();

			foreach (var m in moves)
			{
				Thread.Sleep(m.Time - time);

				time = m.Time;

				var p = ExtraMath.MultiplyMatrix22ToPoint(matrix, m.X - first.X, m.Y - first.Y);

				DoMouseMoveAbsolute(start.X + p.X, start.Y + p.Y, m.Type);
			}
		}
	}
}