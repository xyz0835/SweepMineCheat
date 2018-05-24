using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MineSweepCheet
{
    /// <summary>
    /// 单个方块
    /// </summary>
    public class Mine
    {
        public Mine(Bitmap mineImage, IntPtr winHandle)
        {
            this.MineImage = mineImage;
            this.WinHandle = winHandle;
        }

        /// <summary>
        /// 窗体句柄
        /// </summary>
        IntPtr WinHandle = IntPtr.Zero;

        /// <summary>
        /// 方块位置
        /// </summary>
        public PointF Position = new PointF(0, 0);

        /// <summary>
        /// 宽度
        /// </summary>
        public float Width = 0;

        /// <summary>
        /// 高度
        /// </summary>
        public float Height = 0;

        /// <summary>
        /// 横向索引
        /// </summary>
        public int XIndex = 0;

        /// <summary>
        /// 纵向索引
        /// </summary>
        public int YIndex = 0;

        /// <summary>
        /// 方块的图像
        /// </summary>
        public Bitmap MineImage = null;

        /// <summary>
        /// 该方块的数字
        /// </summary>
        public int MineNumber = -1;

        /// <summary>
        /// 是否已点开
        /// </summary>
        public bool Opened = false;

        /// <summary>
        /// 是否已标记为炸弹
        /// </summary>
        public bool Flagged = false;



        public static Color TopBlueColor = Color.FromArgb(150, 210, 250);
        public static Color BottomBlueColor = Color.FromArgb(70, 90, 200);

        public static Color SplitterColor = Color.FromArgb(40, 75, 100);

        public static Color SpaceColor = Color.FromArgb(200, 200, 220); //近似于白色
        public static Color Number1Color = Color.FromArgb(70, 90, 190); //淡蓝色
        public static Color Number2Color = Color.FromArgb(60, 123, 60); //绿色
        public static Color Number3Color = Color.FromArgb(170, 55, 55);  //红色
        public static Color Number4Color = Color.FromArgb(65, 65, 160);  //深蓝
        public static Color Number5Color = Color.FromArgb(145, 55, 55);  //深红
        public static Color Number6Color = Color.FromArgb(30, 130, 135);  //蓝绿

        public static Color FlagColor = Color.FromArgb(253, 65, 67);  //带有红色


        /// <summary>
        /// 识别结果
        /// </summary>
        public string RecognizeResult = string.Empty;

        /// <summary>
        /// 如果已经识别过
        /// </summary>
        public bool Recoginized
        {
            get
            {
                return Flagged || (Opened && MineNumber > -1);
            }
        }

        /// <summary>
        /// 进行识别
        /// </summary>
        public void Recognize()
        {
            if (Recoginized)  //已经识别过，无需再次识别
                return;

            int BlueCount = 0;
            int SpaceCount = 0;
            int FlagCount = 0;
            int Number1Count = 0;
            int Number2Count = 0;
            int Number3Count = 0;
            int Number4Count = 0;
            int Number5Count = 0;
            int Number6Count = 0;

            Dictionary<Point, Color> PixelColor = new Dictionary<Point, Color>();
            int size = (int)Width;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    PixelColor[new Point(x, y)] = MineImage.GetPixel(x, y);
                }
            }

            foreach (Point point in PixelColor.Keys)
            {
                if (point.X < 2 || point.Y < 2 || point.X > 16 || point.Y > 16)
                    continue;

                Color color = PixelColor[point];
                int Splitter = color.GetSimilarity(SplitterColor);
                int TopBlue = color.GetSimilarity(TopBlueColor);
                int BottomBlue = color.GetSimilarity(BottomBlueColor);
                int Flag = color.GetSimilarity(FlagColor);
                int Space = color.GetSimilarity(SpaceColor);
                int Number1 = color.GetSimilarity(Number1Color);
                int Number2 = color.GetSimilarity(Number2Color);
                int Number3 = color.GetSimilarity(Number3Color);
                int Number4 = color.GetSimilarity(Number4Color);
                int Number5 = color.GetSimilarity(Number5Color);
                int Number6 = color.GetSimilarity(Number6Color);

                //RecognizeResult += string.Format("X{0}Y{1} {2},{3},{4}\r\n", point.X, point.Y, color.R, color.G, color.B);
                //RecognizeResult += string.Format("SIMI Blue{0},Space{1},Flag{2},Number1 {3},Number2 {4},Number3 {5},Number4 {6},Number5 {7},Number6 {8}\r\n\r\n", TopBlue + BottomBlue, Space, Flag, Number1, Number2, Number3, Number4, Number5, Number6, MineNumber);

                List<int> lst = new List<int>() { TopBlue, BottomBlue, Space, Flag, Number1, Number2, Number3, Number4, Number5, Number6 };
                int similarity = lst.Max();
                if (similarity == TopBlue)
                    BlueCount++;
                if (similarity == BottomBlue)
                    BlueCount++;

                if (TopBlue == BottomBlue)
                    BlueCount--;

                if (similarity == Space)
                    SpaceCount++;
                if (similarity == Flag)
                    FlagCount++;
                if (similarity == Number1)
                    Number1Count++;
                if (similarity == Number2)
                    Number2Count++;
                if (similarity == Number3)
                    Number3Count++;
                if (similarity == Number4)
                    Number4Count++;
                if (similarity == Number5)
                    Number5Count++;
                if (similarity == Number6)
                    Number6Count++;
            }

            List<int> lstCount = new List<int>() { BlueCount, SpaceCount, Number1Count, Number2Count, Number3Count, Number4Count, Number5Count, Number6Count };

            if (FlagCount > 10)
            {
                Flagged = true;
            }
            else
            {
                int MaxCount = lstCount.Max();
                if (MaxCount == BlueCount)
                {
                    Opened = false;
                    if (FlagCount > 10)
                    {
                        Flagged = true;
                    }
                    else
                    {
                        if (SpaceCount >= 90)
                        {
                            Opened = true;
                            MineNumber = 0;

                            List<int> lstNumCount = new List<int>() { Number1Count, Number2Count, Number3Count, Number4Count, Number5Count, Number6Count };
                            int numberMax = lstNumCount.Max();

                            if (numberMax == Number1Count)
                                MineNumber = 1;
                            if (numberMax == Number2Count)
                                MineNumber = 2;
                            if (numberMax == Number3Count)
                                MineNumber = 3;
                            if (numberMax == Number4Count)
                                MineNumber = 4;
                            if (numberMax == Number5Count)
                                MineNumber = 5;
                            if (numberMax == Number6Count)
                                MineNumber = 6;

                        }
                        else if (SpaceCount > 30 && SpaceCount < 90)
                        {
                            List<int> lstNumCount = new List<int>() { Number1Count, Number2Count, Number3Count, Number4Count, Number5Count, Number6Count };

                            Opened = true;
                            int numberMax = lstNumCount.Max();
                            if (Number5Count >= 30)
                            {
                                MineNumber = 5;
                            }
                            else if (Number4Count >= 30)
                            {
                                MineNumber = 4;
                            }
                            else if (Number3Count >= 35)
                            {
                                MineNumber = 3;
                            }
                            else if (Number2Count >= 35)
                            {
                                MineNumber = 2;
                            }
                            else if (numberMax >= 20)
                            {
                                if (numberMax == Number1Count)
                                    MineNumber = 1;
                                if (numberMax == Number2Count)
                                    MineNumber = 2;
                                if (numberMax == Number3Count)
                                    MineNumber = 3;
                                if (numberMax == Number4Count)
                                    MineNumber = 4;
                                if (numberMax == Number5Count)
                                    MineNumber = 5;
                                if (numberMax == Number6Count)
                                    MineNumber = 6;
                            }
                            else
                                MineNumber = 0;
                        }
                    }
                }
                else if (MaxCount == SpaceCount)
                {
                    Opened = true;

                    lstCount = new List<int>() { Number1Count, Number2Count, Number3Count, Number4Count, Number5Count, Number6Count };
                    int numberMax = lstCount.Max();


                    if (SpaceCount <= 245)
                    {
                        if (numberMax >= 20)
                        {
                            if (numberMax == Number1Count)
                                MineNumber = 1;
                            else if (numberMax == Number2Count)
                                MineNumber = 2;
                            else if (numberMax == Number3Count)
                                MineNumber = 3;
                            else if (numberMax == Number4Count)
                                MineNumber = 4;
                            else if (numberMax == Number5Count)
                                MineNumber = 5;
                            else if (numberMax == Number6Count)
                                MineNumber = 6;
                        }
                        else
                        {
                            MineNumber = 0;
                        }
                    }
                    else if (SpaceCount > 245)
                    {
                        MineNumber = 0;
                    }
                }
                else if (MaxCount == Number1Count)
                {
                    if (FlagCount > 10)
                    {
                        Flagged = true;
                    }
                    else
                    {
                        if (SpaceCount < 50) //蓝色很容易和数字1的混淆，需要判断空白空间的大小
                        {
                            Opened = false;
                        }
                        else
                        {
                            Opened = true;
                            MineNumber = 1;
                        }
                    }
                }
                else if (MaxCount == Number2Count)
                {
                    Opened = true;
                    MineNumber = 2;
                }
                else if (MaxCount == Number3Count)
                {
                    Opened = true;
                    MineNumber = 3;
                }
                else if (MaxCount == Number4Count)
                {
                    Opened = true;
                    MineNumber = 4;
                }
                else if (MaxCount == Number5Count)
                {
                    Opened = true;
                    MineNumber = 5;
                }
                else if (MaxCount == Number6Count)
                {
                    Opened = true;
                    MineNumber = 6;
                }
                else if (MaxCount == FlagCount)
                {
                    Opened = false;
                    Flagged = true;
                }
            }

            //RecognizeResult = string.Format("Blue{0},Space{1},Flag{2},Number1 {3},Number2 {4},Number3 {5},Number4 {6},Number5 {7},Number6 {8} MineNumber{9}\r\n", BlueCount, SpaceCount, FlagCount, Number1Count, Number2Count, Number3Count, Number4Count, Number5Count, Number6Count, MineNumber);
        }

        /// <summary>
        /// 更新方块的图片
        /// </summary>
        public void RefreshImage()
        {
            if (Recoginized)  //已经正确识别该方块，无需再次获取该图片
                return;
            RECT rc = new RECT();
            FormHelper.GetWindowRect(WinHandle, ref rc);

            Bitmap mineImage = new Bitmap(18, 18);

            using (Bitmap windowimage = new Bitmap(rc.Width, rc.Height))
            {
                using (Graphics windowgp = Graphics.FromImage(windowimage), minegp = Graphics.FromImage(mineImage))
                {
                    IntPtr windowdc = windowgp.GetHdc();
                    FormHelper.PrintWindow(WinHandle, windowdc, 0);
                    windowgp.ReleaseHdc();

                    minegp.DrawImage(windowimage, 0, 0, new RectangleF(Position.X - rc.Left, Position.Y - rc.Top, 18, 18), GraphicsUnit.Pixel);
                }
            }
            MineImage = mineImage;
        }

        /// <summary>
        /// 点开
        /// </summary>
        public void Click()
        {
            int x = (int)Position.X + (int)Width / 2;
            int y = (int)Position.Y + (int)Height / 2;
            Cursor.Position = new Point(x, y);
            MouseHelper.Click(x, y);
            Opened = true;
        }

        /// <summary>
        /// 标记当前方块为炸弹
        /// </summary>
        public void Flag()
        {
            int x = (int)Position.X + (int)Width / 2;
            int y = (int)Position.Y + (int)Height / 2;
            Cursor.Position = new Point(x, y);
            MouseHelper.RightClick(x, y);
            Flagged = true;
        }

        public override string ToString()
        {
            return string.Format("X={0},Y={1}", Position.X, Position.Y);
        }
    }
}
