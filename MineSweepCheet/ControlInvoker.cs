using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MineSweepCheet
{
    public static class ControlInvoker
    {
        /// <summary>
        /// 控件调用委托方法，更新控件
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="method"></param>
        public static void InvokeMethod(this Control ctl, MethodInvoker method)
        {
            if (!ctl.ControlAvailable())
                return;

            if (ctl.InvokeRequired)
            {
                MethodInvoker invokeMethod = delegate
                {
                    try
                    {
                        method();
                    }
                    catch (ObjectDisposedException ex)
                    {
                        // AppLog.Write(ex, "Exception when invoking method", true);
                    }
                };

                ctl.Invoke(invokeMethod);
            }
            else
            {
                method();
            }
        }

        /// <summary>
        /// 设置控件的可用性 (简便方法)
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="enabled"></param>
        public static void SetEnabled(this Control ctl, bool enabled)
        {
            ctl.InvokeMethod(() => ctl.Enabled = enabled);
        }


        public static bool ControlAvailable(this Control ctl)
        {
            if (ctl == null)
                return false;

            if (ctl.IsDisposed)
                return false;

            //if (!ctl.IsHandleCreated)
            //    return false;

            return true;
        }
    }
}
