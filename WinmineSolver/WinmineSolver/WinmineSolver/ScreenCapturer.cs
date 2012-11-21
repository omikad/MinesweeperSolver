using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Text;
using WinmineSolver.WinAPI;
using Point = System.Windows.Point;

namespace WinmineSolver
{
	[InheritedExport]
	public interface IScreenCapturer
	{
		Point GetWindowLUPoint(IntPtr handle);
		Bitmap CaptureWindow(IntPtr handle);
		IntPtr FindFirstWindow();
	}

	public class ScreenCapturer : IScreenCapturer
	{
		public Point GetWindowLUPoint(IntPtr handle)
		{
			var rect = new User32.RECT();
			User32.GetWindowRect(handle, ref rect);
			return new Point(rect.left, rect.top);
		}

		public Bitmap CaptureWindow(IntPtr handle)
		{
			var hdcSrc = User32.GetWindowDC(handle);
			var windowRect = new User32.RECT();
			User32.GetWindowRect(handle, ref windowRect);
			var width = windowRect.right - windowRect.left;
			var height = windowRect.bottom - windowRect.top;
			var hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
			var hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
			var hOld = GDI32.SelectObject(hdcDest, hBitmap);
			GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
			GDI32.SelectObject(hdcDest, hOld);
			GDI32.DeleteDC(hdcDest);
			User32.ReleaseDC(handle, hdcSrc);
			var img = Image.FromHbitmap(hBitmap);
			GDI32.DeleteObject(hBitmap);
			return img;
		}

		public IntPtr FindFirstWindow()
		{
			handles.Clear();
			GDI32.EnumWindows(EnumWindowCallBack, 0);
			return handles.First(h =>
				                     {
										 var bmp = CaptureWindow(h);
										 return bmp.Width >= 100 && bmp.Height >= 100;
				                     });
		}

		private static readonly List<IntPtr> handles = new List<IntPtr>();

		private static bool EnumWindowCallBack(int hwnd, int lParam)
		{
			var windowHandle = (IntPtr)hwnd;

			var sb = new StringBuilder(1024);
			var sbc = new StringBuilder(256);

			GDI32.GetClassName(hwnd, sbc, sbc.Capacity);
			GDI32.GetWindowText((int)windowHandle, sb, sb.Capacity);

			if (sbc.Length > 0)
			{
				var title = sb.ToString();
				if (title == "Minesweeper X")
					handles.Add(windowHandle);
			}
			return true;
		}

	}
}