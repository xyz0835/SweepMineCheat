using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MineSweepCheet
{
    public static class FormHelper
    {
        static Rectangle PrimaryScreenRectangle = Rectangle.Empty;

        static FormHelper()
        {
            PrimaryScreenRectangle = Screen.PrimaryScreen.WorkingArea;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetWindow(IntPtr hWnd, uint wWcmd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, int fAttach);

        [DllImport("User32.dll")]
        public static extern int GetWindowText(IntPtr handle, StringBuilder text, int MaxLen);

        /// <summary>
        /// 是否是有效窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindow(IntPtr hWnd);

        /// <summary>
        /// 设置窗口到前台，并激活窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 设置由其他线程创建的窗口的显示状态(异步执行，不会因为进程挂起而阻塞)
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// 是否最小化
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern bool IsIconic(IntPtr hWnd);


        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);

        /// <summary>
        /// 打开该程序主窗口
        /// </summary>
        public static void RaiseWindow(string formText)
        {
            IntPtr handle = FindWindow(null, formText);
            if (handle.ToInt32() > 0)
                ActivateWindow(handle);
        }

        /// <summary>
        /// 截取窗口图像(最小化或遮挡也可以截取)
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hdcBlt"></param>
        /// <param name="nFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(
         IntPtr hwnd,               // Window to copy,Handle to the window that will be copied. 
         IntPtr hdcBlt,             // HDC to print into,Handle to the device context. 
         uint nFlags              // Optional flags,Specifies the drawing options. It can be one of the following values. 
         );


        private static void ActivateWindow(IntPtr win)
        {
            IntPtr foreWin = GetForegroundWindow();
            IntPtr foreProID = GetWindowThreadProcessId(foreWin, IntPtr.Zero);
            IntPtr winProID = GetWindowThreadProcessId(win, IntPtr.Zero);

            if (foreProID != winProID)
            {
                AttachThreadInput(winProID, foreProID, 1);
                if (IsIconic(win))
                    ShowWindow(win, (int)ShowWindowFlag.SW_RESTORE);

                SetWindowPos(win, /*HWND_TOPMOST*/-1, 0, 0, 0, 0, 1 | 2);
                SetWindowPos(win, /*HWND_NOTOPMOST*/-2, 0, 0, 0, 0, 1 | 2);
                SetForegroundWindow(win);
                AttachThreadInput(winProID, foreProID, 0);
            }
            else
                SetForegroundWindow(win);
        }

        /// <summary>
        /// 窗口控制代码
        /// </summary>
        private enum ShowWindowFlag
        {
            /// <summary>
            /// 最小化窗口，即是创建线程未响应。该值只能用于最小化其他线程创建的窗口
            /// </summary>
            SW_FORCEMINIMIZE = 11,
            /// <summary>
            /// 隐藏窗口
            /// </summary>
            SW_HIDE = 0,
            /// <summary>
            /// 最大化窗口
            /// </summary>
            SW_MAXIMIZE = 3,
            /// <summary>
            /// 最小化窗口，并激活下一最前窗口
            /// </summary>
            SW_MINIMIZE = 6,
            /// <summary>
            /// 激活并显示一个窗口，如果窗口是最小化或最大化，则系统会将其还原到最初大小及位置
            /// </summary>
            SW_RESTORE = 9,
            /// <summary>
            /// 激活并显示窗口，保持当前的大小和位置
            /// </summary>
            SW_SHOW = 5,
            /// <summary>
            /// 设置为初始状态
            /// </summary>
            SW_SHOWDEFAULT = 10,
            /// <summary>
            /// 激活窗口，并最小化显示
            /// </summary>
            SW_SHOWMINIMIZED = 2,
            /// <summary>
            /// 最小化显示窗口，但不激活窗口
            /// </summary>
            SW_SHOWMINNOACTIVE = 7,
            /// <summary>
            /// 显示窗口，保持当前大小和位置，但不激活窗口
            /// </summary>
            SW_SHOWNA = 8,
            /// <summary>
            /// 以最近的大小和位置显示窗口，但不激活窗口
            /// </summary>
            SW_SHOWNOACTIVATE = 4,
            /// <summary>
            /// 激活并显示一个窗口，如果窗口是最小化或最大化，则系统会将其还原到最初大小及位置。程序在初次显示窗口时，需指明该值
            /// </summary>
            SW_SHOWNORMAL = 1,
        }

        public static bool IsOverlapped(Form form)
        {
            return isoverlapped(form.Handle, form);
        }

        private static bool isoverlapped(IntPtr win, Form form)
        {
            IntPtr preWin = GetWindow(win, 3); //获取显示在Form之上的窗口

            if (preWin == null || preWin == IntPtr.Zero)
                return false;

            if (!IsWindowVisible(preWin))
                return isoverlapped(preWin, form);

            RECT rect = new RECT();
            if (GetWindowRect(preWin, ref rect)) //获取窗体矩形
            {
                Rectangle winrect = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

                if (winrect.Width == PrimaryScreenRectangle.Width && winrect.Y == PrimaryScreenRectangle.Height) //菜单栏。不判断遮挡
                    return isoverlapped(preWin, form);

                if (winrect.X == 0 && winrect.Width == 54 && winrect.Height == 54) //开始按钮。不判断遮挡
                    return isoverlapped(preWin, form);

                Rectangle formRect = new Rectangle(form.Location, form.Size); //Form窗体矩形
                if (formRect.IntersectsWith(winrect)) //判断是否遮挡
                    return true;
            }

            return isoverlapped(preWin, form);
        }
    }

    /// <summary>
    /// 矩形
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public int Width { get { return Right - Left; } }
        public int Height { get { return Bottom - Top; } }
    }
}
