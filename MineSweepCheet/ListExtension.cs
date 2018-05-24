using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MineSweepCheet
{
    public static class ListExtension
    {
        /// <summary>
        /// 移除空值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        public static void RemoveNull<T>(this List<T> lst)
        {
            List<T> lstNotNull = new List<T>();

            foreach (T t in lst)
                if (t != null)
                    lstNotNull.Add(t);

            lst.Clear();
            lst.AddRange(lstNotNull);
        }

        /// <summary>
        /// 显示集合字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static string ToArrayString<T>(this IEnumerable<T> lst)
        {
            return string.Join(" ", lst);
        }
    }
}
