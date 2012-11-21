using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WinmineSolver.WinAPI
{
	public static class GDI32
	{
		public delegate bool CallBack(int hwnd, int lParam);

		public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
										 int nWidth, int nHeight, IntPtr hObjectSource,
										 int nXSrc, int nYSrc, int dwRop);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
														   int nHeight);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteDC(IntPtr hDC);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

		[DllImport("user32.Dll")]
		public static extern int EnumWindows(CallBack x, int y);

		[DllImport("User32.Dll")]
		public static extern void GetWindowText(int h, StringBuilder s, int nMaxCount);

		[DllImport("User32.Dll")]
		public static extern void GetClassName(int h, StringBuilder s, int nMaxCount);

		[DllImport("User32.Dll")]
		public static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
	}
}