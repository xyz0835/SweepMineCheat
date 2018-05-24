using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MineSweepCheet
{
    public class MouseHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int LEFTDOWN = 0x02;
        private const int LEFTUP = 0x04;
        private const int RIGHTDOWN = 0x08;
        private const int RIGHTUP = 0x10;

        public static void Click(int x, int y)
        {
            mouse_event(LEFTDOWN | LEFTUP, (uint)x, (uint)y, 0, 0);
        }

        public static void RightClick(int x, int y)
        {
            mouse_event(RIGHTDOWN | RIGHTUP, (uint)x, (uint)y, 0, 0);
        }

        public static void BothClick(int x, int y)
        {
            mouse_event(LEFTDOWN | LEFTUP | RIGHTDOWN | RIGHTUP, (uint)x, (uint)y, 0, 0);
        }
    }
}
