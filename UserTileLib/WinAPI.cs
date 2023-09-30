using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace UserTileLib {
    public static class WinAPI {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WinAPI.WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WinAPI.WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out WinAPI.RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        public static IntPtr MakeLParam(int LoWord, int HiWord) {
            int value = (HiWord << 16) | (LoWord & 65535);
            return new IntPtr(value);
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, WinAPI.SetWindowPosFlags uFlags);

        public const int WM_SIZE = 5;

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        public struct WINDOWPLACEMENT {
            public int length;
            public int flags;
            public int showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rectangle rcNormalPosition;
        }

        public struct RECT {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [Flags]
        public enum SetWindowPosFlags : uint {
            SWP_ASYNCWINDOWPOS = 16384U,
            SWP_DEFERERASE = 8192U,
            SWP_DRAWFRAME = 32U,
            SWP_FRAMECHANGED = 32U,
            SWP_HIDEWINDOW = 128U,
            SWP_NOACTIVATE = 16U,
            SWP_NOCOPYBITS = 256U,
            SWP_NOMOVE = 2U,
            SWP_NOOWNERZORDER = 512U,
            SWP_NOREDRAW = 8U,
            SWP_NOREPOSITION = 512U,
            SWP_NOSENDCHANGING = 1024U,
            SWP_NOSIZE = 1U,
            SWP_NOZORDER = 4U,
            SWP_SHOWWINDOW = 64U
        }
    }
}
