using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MineSweepCheet
{
    /// <summary>
    /// 多线程封装类
    /// </summary>
    public class ThreadStarter
    {
        /// <summary>
        /// 是否为后台线程，默认为前台线程
        /// </summary>
        private bool isBackGround = false;

        /// <summary>
        /// 工作线程
        /// </summary>
        private Thread thread = null;

        /// <summary>
        /// 多线程具体方法
        /// </summary>
        private ThreadStart ThreadFunction = null;

        /// <summary>
        /// 可设置是否是后台线程
        /// </summary>
        /// <param name="threadFunction"></param>
        /// <param name="isBackGround"></param>
        public ThreadStarter(ThreadStart threadFunction, bool isBackGround = false)
        {
            ThreadFunction = delegate
            {
                threadFunction();
            };
            this.isBackGround = isBackGround;
        }

        /// <summary>
        /// 开始线程操作
        /// </summary>
        public void Start()
        {
            thread = new Thread(ThreadFunction);
            thread.IsBackground = isBackGround;
            thread.Start();
        }

        /// <summary>
        /// 结束线程（）
        /// </summary>
        public void Abort()
        {
            if (thread != null && thread.IsAlive)
                thread.Abort();
        }

        /// <summary>
        /// 前台线程完成任务(静态方法)
        /// </summary>
        /// <param name="func">需要执行的方法</param>
        public static void Start(ThreadStart func)
        {
            new Thread(() =>
            {
                func();
            })
            { IsBackground = true }.Start();
        }


        /// <summary>
        /// 前台线程完成任务(静态方法，带参数)
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="action">需要执行的方法</param>
        /// <param name="t">参数</param>
        public static void Start<T>(Action<T> action, T t)
        {
            new Thread(() =>
            {
                action(t);
            })
            { IsBackground = true }.Start();
        }


        public static void StartNoSkin(ThreadStart func)
        {
            new Thread(() =>
            {
                func();
            })
            { IsBackground = true }.Start();
        }

        public static void StartNoSkin<T>(Action<T> action, T t)
        {
            new Thread(() =>
            {
                action(t);
            })
            { IsBackground = true }.Start();
        }
    }
}
