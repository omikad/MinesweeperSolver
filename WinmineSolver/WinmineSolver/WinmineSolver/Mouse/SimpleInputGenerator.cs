using System;
using System.ComponentModel.Composition;
using WinmineSolver.WinAPI;

namespace WinmineSolver.Mouse
{
	public class SimpleInputGenerator : IInputGenerator
	{
		private readonly int width;
		private readonly int height;

		public SimpleInputGenerator()
		{
			width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
			height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
		}

		public void MoveMouseAbsolute(int x, int y)
		{
			var xx = x * 65536 / width;
			var yy = y * 65536 / height;

			const User32.MouseEvent dwFlags = User32.MouseEvent.MOUSEEVENTF_MOVE | User32.MouseEvent.MOUSEEVENTF_ABSOLUTE;

			User32.mouse_event((uint)dwFlags, xx, yy, 0, IntPtr.Zero);
		}

		public void DoMouseClick()
		{
			User32.mouse_event((uint)(User32.MouseEvent.MOUSEEVENTF_LEFTDOWN | User32.MouseEvent.MOUSEEVENTF_LEFTUP), 0, 0, 0, IntPtr.Zero);
		}
	}
}