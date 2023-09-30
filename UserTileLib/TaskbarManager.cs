using System;
using System.Windows.Forms;

namespace UserTileLib {
    public class TaskbarManager {
        private event EventHandler TrayResized;
        private event EventHandler RebarResized;

        public TaskbarManager() {
            this.taskbarHwnd = WinAPI.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
            this.rebarHwnd = WinAPI.FindWindowEx(this.taskbarHwnd, IntPtr.Zero, "ReBarWindow32", null);
            this.trayHwnd = WinAPI.FindWindowEx(this.taskbarHwnd, IntPtr.Zero, "TrayNotifyWnd", null);
            this.minimizeHwnd = WinAPI.FindWindowEx(this.trayHwnd, IntPtr.Zero, "TrayShowDesktopButtonWClass", null);
            this.currentTaskbarPos = this.DetectTaskbarPos();
            this.TrayResized += this.TaskbarManagerTrayResized;
            this.RebarResized += this.TaskbarManagerRebarResized;
        }

        private void TaskbarManagerRebarResized(object sender, EventArgs e) {
            this.ReduceRebarWidth();
        }

        private void TaskbarManagerTrayResized(object sender, EventArgs e) {
            this.MoveTrayToLeft();
            this.ReduceRebarWidth();
        }

        public TaskbarPosition DetectTaskbarPos() {
            WinAPI.GetWindowRect(this.taskbarHwnd, out WinAPI.RECT rect);
            TaskbarPosition result;

            if (rect.Left == 0 && rect.Bottom == SystemInformation.VirtualScreen.Bottom && rect.Top == SystemInformation.WorkingArea.Top) {
                result = TaskbarPosition.Left;
            }
            else if (rect.Left == 0 && rect.Top == 0 && rect.Bottom != SystemInformation.VirtualScreen.Bottom) {
                result = TaskbarPosition.Top;
            }
            else if (rect.Left != 0 && rect.Top == 0 && rect.Bottom == SystemInformation.VirtualScreen.Bottom) {
                result = TaskbarPosition.Right;
            }
            else if (rect.Left == 0 && rect.Top != 0 && rect.Bottom == SystemInformation.VirtualScreen.Bottom) {
                result = TaskbarPosition.Bottom;
            }
            else {
                result = TaskbarPosition.Unknown;
            }

            return result;
        }

        public bool IsTaskbarSmall() {
            WinAPI.GetWindowRect(this.taskbarHwnd, out WinAPI.RECT rect);

            int num = rect.Bottom - rect.Top;

            return num < 35;
        }

        public WinAPI.RECT GetMinimizeRect() {
            WinAPI.GetWindowRect(this.minimizeHwnd, out WinAPI.RECT result);

            return result;
        }

        public WinAPI.RECT GetTrayRect() {
            WinAPI.GetWindowRect(this.trayHwnd, out WinAPI.RECT result);

            return result;
        }

        public WinAPI.RECT GetRebarRect() {
            WinAPI.GetWindowRect(this.rebarHwnd, out WinAPI.RECT result);

            return result;
        }

        public void MoveTrayToLeft() {
            WinAPI.RECT trayRect = this.GetTrayRect();
            WinAPI.RECT minimizeRect = this.GetMinimizeRect();

            int num = minimizeRect.Right - minimizeRect.Left;
            WinAPI.RECT rebarRect = this.GetRebarRect();
            IntPtr hwnd = WinAPI.FindWindowEx(this.trayHwnd, IntPtr.Zero, "TrayClockWClass", null);
            WinAPI.GetWindowRect(hwnd, out WinAPI.RECT rect);

            int num2 = rect.Right - rect.Left;
            IntPtr hwnd2 = WinAPI.FindWindowEx(this.trayHwnd, IntPtr.Zero, "SysPager", null);
            WinAPI.GetWindowRect(hwnd2, out WinAPI.RECT rect2);

            int num3 = rect2.Right - rect2.Left;
            IntPtr hwnd3 = WinAPI.FindWindowEx(this.trayHwnd, IntPtr.Zero, "Button", null);
            WinAPI.GetWindowRect(hwnd3, out WinAPI.RECT rect3);

            int num4 = rect3.Right - rect3.Left;
            WinAPI.SetWindowPos(this.trayHwnd, IntPtr.Zero, SystemInformation.VirtualScreen.Right - num - this.reservedWidth - num2 - num3 - num4, 0, num + this.reservedWidth + num2 + num3 + num4, trayRect.Bottom - trayRect.Top, (WinAPI.SetWindowPosFlags)0U);
            trayRect = this.GetTrayRect();
            
            int num5 = trayRect.Right - trayRect.Left;
            WinAPI.SetWindowPos(this.minimizeHwnd, IntPtr.Zero, num5 - num, 0, num, trayRect.Bottom - trayRect.Top, WinAPI.SetWindowPosFlags.SWP_NOSIZE);
            
            if (num == 0) {
                num = 15;
            }

            this.control.Left = num5 - this.control.Width - num - 4;
        }

        public void PlaceMinimizeOnTaskbar() {
            WinAPI.SetParent(this.minimizeHwnd, this.taskbarHwnd);
            WinAPI.RECT minimizeRect = this.GetMinimizeRect();
            
            int num = minimizeRect.Right - minimizeRect.Left;
            WinAPI.SetWindowPos(this.minimizeHwnd, IntPtr.Zero, SystemInformation.WorkingArea.Right - num, 0, num, minimizeRect.Bottom - minimizeRect.Top, WinAPI.SetWindowPosFlags.SWP_NOSIZE);
            WinAPI.RECT trayRect = this.GetTrayRect();
            
            int num2 = trayRect.Right - trayRect.Left;
            WinAPI.SetWindowPos(this.trayHwnd, IntPtr.Zero, SystemInformation.WorkingArea.Right - this.reservedWidth - num2 - num, 0, num2 - num, trayRect.Bottom - trayRect.Top, (WinAPI.SetWindowPosFlags)0U);
        }

        public void PlaceMinimizeOnTray() {
            WinAPI.SetParent(this.minimizeHwnd, this.trayHwnd);
        }

        public bool CheckTrayWidth() {
            WinAPI.RECT trayRect = this.GetTrayRect();
            
            int num = trayRect.Right - trayRect.Left;
            WinAPI.RECT minimizeRect = this.GetMinimizeRect();
            
            int num2 = minimizeRect.Right - minimizeRect.Left;
            IntPtr hwnd = WinAPI.FindWindowEx(this.trayHwnd, IntPtr.Zero, "TrayClockWClass", null);
            WinAPI.RECT rect;
            WinAPI.GetWindowRect(hwnd, out rect);
            
            int num3 = rect.Right - rect.Left;
            IntPtr hwnd2 = WinAPI.FindWindowEx(this.trayHwnd, IntPtr.Zero, "SysPager", null);
            WinAPI.RECT rect2;
            WinAPI.GetWindowRect(hwnd2, out rect2);
            
            int num4 = rect2.Right - rect2.Left;
            IntPtr hwnd3 = WinAPI.FindWindowEx(this.trayHwnd, IntPtr.Zero, "Button", null);
            WinAPI.RECT rect3;
            WinAPI.GetWindowRect(hwnd3, out rect3);
            
            int num5 = rect3.Right - rect3.Left;
            
            return num == num2 + num3 + num4 + num5 + this.reservedWidth;
        }

        public void ReduceRebarWidth() {
            WinAPI.RECT rebarRect = this.GetRebarRect();
            WinAPI.RECT trayRect = this.GetTrayRect();
            
            int num = trayRect.Right - trayRect.Left;
            WinAPI.RECT minimizeRect = this.GetMinimizeRect();
            
            int num2 = minimizeRect.Right - minimizeRect.Left;
            int num3 = rebarRect.Right - rebarRect.Left;
            
            WinAPI.SetWindowPos(this.rebarHwnd, IntPtr.Zero, rebarRect.Left, 0, SystemInformation.WorkingArea.Right - num - this.reservedWidth - num2 - 3, rebarRect.Bottom - rebarRect.Top, WinAPI.SetWindowPosFlags.SWP_NOMOVE);
        }

        public void ReserveSpace(int width) {
            if (!this.spaceReserved) {
                this.reservedWidth = width;
                this.spaceReserved = true;
            }
        }

        public void FreeSpace() {
            this.reservedWidth = 0;
            
            WinAPI.RECT trayRect = this.GetTrayRect();
            WinAPI.RECT rebarRect = this.GetRebarRect();
            WinAPI.RECT minimizeRect = this.GetMinimizeRect();
            
            int num = minimizeRect.Right - minimizeRect.Left;
            IntPtr hwnd = WinAPI.FindWindowEx(this.trayHwnd, IntPtr.Zero, "TrayClockWClass", null);
            WinAPI.RECT rect;
            WinAPI.GetWindowRect(hwnd, out rect);
            
            int num2 = rect.Right - rect.Left;
            IntPtr hwnd2 = WinAPI.FindWindowEx(this.trayHwnd, IntPtr.Zero, "SysPager", null);
            WinAPI.RECT rect2;
            WinAPI.GetWindowRect(hwnd2, out rect2);
            
            int num3 = rect2.Right - rect2.Left;
            IntPtr hwnd3 = WinAPI.FindWindowEx(this.trayHwnd, IntPtr.Zero, "Button", null);
            WinAPI.RECT rect3;
            WinAPI.GetWindowRect(hwnd3, out rect3);
            
            int num4 = rect3.Right - rect3.Left;
            int num5 = num + num2 + num3 + num4;
            
            WinAPI.SetWindowPos(this.trayHwnd, IntPtr.Zero, SystemInformation.WorkingArea.Right - num5, 0, num5, trayRect.Bottom - trayRect.Top, (WinAPI.SetWindowPosFlags)0U);
            WinAPI.SetWindowPos(this.rebarHwnd, IntPtr.Zero, rebarRect.Left, 0, SystemInformation.WorkingArea.Right - num5 - rebarRect.Left, trayRect.Bottom - trayRect.Top, WinAPI.SetWindowPosFlags.SWP_NOMOVE);
            WinAPI.SetWindowPos(this.minimizeHwnd, IntPtr.Zero, num5 - num, 0, num, minimizeRect.Bottom - minimizeRect.Top, WinAPI.SetWindowPosFlags.SWP_NOSIZE);
        }

        public void CheckTaskbar() {
            if (this.spaceReserved) {
                if (!this.CheckTrayWidth()) {
                    this.currentTaskbarPos = this.DetectTaskbarPos();

                    if (this.currentTaskbarPos == TaskbarPosition.Bottom) {
                        this.TrayResized(null, EventArgs.Empty);
                    }
                }
            }
            else {
                this.ReserveSpace(36);
            }
        }

        public void AddControl(UserControl control) {
            WinAPI.RECT trayRect = this.GetTrayRect();
            
            int num = trayRect.Right - trayRect.Left;
            WinAPI.RECT minimizeRect = this.GetMinimizeRect();
            
            int num2 = minimizeRect.Right - minimizeRect.Left;
            this.reservedWidth = control.Width + 5;
            
            control.Left = num - num2;
            
            WinAPI.SetParent(control.Handle, this.trayHwnd);
            this.control = control;
        }

        public void Dispose() {
            if (this.spaceReserved) {
                this.FreeSpace();
            }
        }

        private readonly IntPtr taskbarHwnd;
        private readonly IntPtr trayHwnd;
        private readonly IntPtr rebarHwnd;
        private readonly IntPtr minimizeHwnd;
        private TaskbarPosition currentTaskbarPos;
        private int reservedWidth = 32;
        private bool spaceReserved;
        private Control control;
    }
}
