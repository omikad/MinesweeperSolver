using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace WinmineSolver.WinAPI
{
	public static class User32
	{
		[DllImport("User32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		[DllImport("User32.dll")]
		public static extern int SetForegroundWindow(IntPtr hWnd);
		[DllImport("User32.dll")]
		public static extern IntPtr GetForegroundWindow();

		public delegate bool CallBack(int handle, IntPtr param);
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

		[DllImport("user32.dll")]
		public static extern bool EnumWindows(CallBack cb, IntPtr param);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(int hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(int hWnd);

		[DllImport("user32.dll")]
		public static extern bool EnumDesktopWindows(IntPtr hDesktop, CallBack lpfn, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern bool IsWindow(int hWnd);

		[DllImport("user32.dll")]
		public static extern int GetKeyboardLayout(uint idThread);

		// When you don't want the ProcessId, use this overload and pass IntPtr.Zero for the second parameter
		[DllImport("user32.dll")]
		public static extern uint GetWindowThreadProcessId(int hWnd, IntPtr ProcessId);

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		public static extern int GetParent(int hWnd);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool MoveWindow(int hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void mouse_event(UInt32 dwFlags, Int32 dx, Int32 dy, UInt32 cButtons, IntPtr dwExtraInfo);

		/// <summary>
		/// Retrieves the cursor's position, in screen coordinates (pixels from up-left corner)
		/// </summary>
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out Point lpPoint);

		public static Point GetCursorPosition()
		{
			Point p;
			GetCursorPos(out p);
			return p;
		}

		[Flags]
		public enum MouseEvent : uint
		{
			MOUSEEVENTF_ABSOLUTE = 0x8000,
			MOUSEEVENTF_LEFTDOWN = 0x0002,
			MOUSEEVENTF_LEFTUP = 0x0004,
			MOUSEEVENTF_MIDDLEDOWN = 0x0020,
			MOUSEEVENTF_MIDDLEUP = 0x0040,
			MOUSEEVENTF_MOVE = 0x0001,
			MOUSEEVENTF_RIGHTDOWN = 0x0008,
			MOUSEEVENTF_RIGHTUP = 0x0010,
			MOUSEEVENTF_WHEEL = 0x0800,
			MOUSEEVENTF_XDOWN = 0x0080,
			MOUSEEVENTF_XUP = 0x0100
		}
	}
}