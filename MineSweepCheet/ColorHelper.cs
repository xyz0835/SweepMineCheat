using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MineSweepCheet
{
    public static class ColorHelper
    {
        /// <summary>
        /// 获取相似度
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns></returns>
        public static int GetSimilarity(this Color color1, Color color2)
        {
            int r1 = Math.Abs(color1.R - color2.R);
            int g1 = Math.Abs(color1.G - color2.G);
            int b1 = Math.Abs(color1.B - color2.B);

            return 444 - (int)Math.Sqrt(r1 * r1 + g1 * g1 + b1 * b1);
        }

        public static bool EqualColor(this Color color1, Color color2)
        {
            return color1.R == color2.R && color1.G == color2.G && color1.B == color2.B;
        }
    }
}
