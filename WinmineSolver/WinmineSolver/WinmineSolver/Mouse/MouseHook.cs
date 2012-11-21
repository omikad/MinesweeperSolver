using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace WinmineSolver.Mouse
{
	public class MouseHook
	{
		protected Action<MouseLLHookStruct, MouseMessages> action;
		protected int hHook;

		protected MouseHook()
		{ }

		public MouseHook(Action<MouseLLHookStruct, MouseMessages> action)
		{
			this.action = action;
		}

		public virtual void Hook()
		{
			hHook = SetWindowsHookEx((int)HookType.WH_MOUSE_LL, MouseHookProc, IntPtr.Zero, 0);

			if (hHook == 0)
				throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		public virtual void Unhook()
		{
			if (hHook == 0)
				return;

			var ret = UnhookWindowsHookEx(hHook);

			if (ret == false)
				throw new Win32Exception(Marshal.GetLastWin32Error());

			hHook = 0;
		}

		private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode < 0)
			{
				return CallNextHookEx(hHook, nCode, wParam, lParam);
			}

			var data = (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));
			action(data, (MouseMessages)wParam);

			return CallNextHookEx(hHook, nCode, wParam, lParam);
		}

		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class MouseHookStruct
		{
			public POINT pt;
			public int hwnd;
			public int wHitTestCode;
			public int dwExtraInfo;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class MouseLLHookStruct
		{
			public POINT pt;
			public int mouseData;
			public int flags;
			public int time;
			public int dwExtraInfoPtr;
		}

		public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern bool UnhookWindowsHookEx(int idHook);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

		public enum HookType : int
		{
			WH_JOURNALRECORD = 0,
			WH_JOURNALPLAYBACK = 1,
			WH_KEYBOARD = 2,
			WH_GETMESSAGE = 3,
			WH_CALLWNDPROC = 4,
			WH_CBT = 5,
			WH_SYSMSGFILTER = 6,
			WH_MOUSE = 7,
			WH_HARDWARE = 8,
			WH_DEBUG = 9,
			WH_SHELL = 10,
			WH_FOREGROUNDIDLE = 11,
			WH_CALLWNDPROCRET = 12,
			WH_KEYBOARD_LL = 13,
			WH_MOUSE_LL = 14
		}

		public enum MouseMessages
		{
			WM_LBUTTONDOWN = 0x0201,	//513
			WM_LBUTTONUP = 0x0202,		//514
			WM_MOUSEMOVE = 0x0200,		//512
			WM_MOUSEWHEEL = 0x020A,		//522
			WM_RBUTTONDOWN = 0x0204,	//516
			WM_RBUTTONUP = 0x0205,		//517
			WM_LBUTTONDBLCLK = 0x0203,	//515
			WM_MBUTTONDOWN = 0x0207,	//519
			WM_MBUTTONUP = 0x0208		//520
		}
	}
}
