using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MineSweepCheet
{
    public class SweepZone
    {
        /// <summary>
        /// 扫雷区域类
        /// </summary>
        /// <param name="handle"></param>
        public SweepZone(IntPtr handle)
        {
            WindowHandle = handle;
            //Refresh();
        }

        /// <summary>
        /// 扫雷窗口句柄
        /// </summary>
        private IntPtr WindowHandle = IntPtr.Zero;

        /// <summary>
        /// 雷区的图像
        /// </summary>
        public Bitmap ZoneImage = null;


        /// <summary>
        /// 雷区宽度
        /// </summary>
        private int Width = 0;

        /// <summary>
        /// 雷区高度
        /// </summary>
        private int Height = 0;

        /// <summary>
        /// 横向方块个数
        /// </summary>
        private int MineXCount = 0;
        /// <summary>
        /// 纵向方块个数
        /// </summary>
        private int MineYCount = 0;


        /// <summary>
        /// 各个操作的间隔
        /// </summary>
        public int SleepMiliSeconds = 15;

        /// <summary>
        /// 刷新整个扫雷区的图片，并更新各个方块的图片
        /// </summary>
        public void Refresh()
        {
            FormHelper.SetForegroundWindow(WindowHandle);

            RECT rc = new RECT();
            FormHelper.GetWindowRect(WindowHandle, ref rc);

            int windowwidth = rc.Right - rc.Left;
            int windowheight = rc.Bottom - rc.Top;

            Point startPoint = new Point(38, 80);
            Point endPoint = new Point(580, 370);
            MineXCount = 30;
            MineYCount = 16;

            if (windowwidth < 300 && windowheight < 300)
            {
                startPoint = new Point(38, 80);
                endPoint = new Point(202, 244);
                MineXCount = 9;
                MineYCount = 9;
            }
            else if (windowwidth < 380 && windowheight < 420)
            {
                startPoint = new Point(38, 80);
                endPoint = new Point(328, 370);
                MineXCount = 16;
                MineYCount = 16;
            }

            int zonewidth = Width = endPoint.X - startPoint.X;
            int zoneheight = Height = endPoint.Y - startPoint.Y;

            //Mines.Clear();

            Bitmap zoneImage = new Bitmap(zonewidth, zoneheight);

            using (Bitmap windowimage = new Bitmap(windowwidth, windowheight))
            {
                using (Graphics windowgp = Graphics.FromImage(windowimage), zonegp = Graphics.FromImage(zoneImage))
                {
                    IntPtr windowdc = windowgp.GetHdc();
                    FormHelper.PrintWindow(WindowHandle, windowdc, 0);
                    windowgp.ReleaseHdc();

                    zonegp.DrawImage(windowimage, 0, 0, new Rectangle(startPoint.X, startPoint.Y, zonewidth, zoneheight), GraphicsUnit.Pixel);
                    float MineWidth = 18.06f;
                    float MineHeight = 18.06f;
                    float MineY = 0;
                    float MineX = 0;
                    Bitmap mineImage = null;
                    for (int xindex = 0; xindex < MineXCount; xindex++)
                    {
                        for (int yindex = 0; yindex < MineYCount; yindex++)
                        {
                            Mine mineItem = Mines.Find(item => item.XIndex == xindex && item.YIndex == yindex);
                            if (mineItem == null)
                            {
                                MineX = xindex * MineWidth + startPoint.X;
                                MineY = yindex * MineHeight + startPoint.Y;
                                mineImage = new Bitmap(18, 18);
                                using (Graphics minegp = Graphics.FromImage(mineImage))
                                {
                                    minegp.DrawImage(windowimage, 0, 0, new RectangleF(MineX, MineY, 18, 18), GraphicsUnit.Pixel);
                                }
                                mineItem = new Mine(mineImage, WindowHandle);
                                mineItem.XIndex = xindex;
                                mineItem.YIndex = yindex;
                                mineItem.Width = MineWidth;
                                mineItem.Height = MineHeight;
                                mineItem.Position.X = rc.Left + MineX;
                                mineItem.Position.Y = rc.Top + MineY;
                                Mines.Add(mineItem);
                            }
                            else // if (!mineItem.Recoginized)
                            {
                                MineX = xindex * MineWidth + startPoint.X;
                                MineY = yindex * MineHeight + startPoint.Y;
                                mineImage = new Bitmap(18, 18);
                                using (Graphics minegp = Graphics.FromImage(mineImage))
                                {
                                    minegp.DrawImage(windowimage, 0, 0, new RectangleF(MineX, MineY, 18, 18), GraphicsUnit.Pixel);
                                }
                                mineItem.MineImage = mineImage;
                            }
                        }
                    }
                }
            }
            ZoneImage = zoneImage;
        }

        /// <summary>
        /// 扫雷块
        /// </summary>
        public List<Mine> Mines = new List<Mine>();

        /// <summary>
        /// 根据图像识别结果组装出来的图像
        /// </summary>
        /// <returns></returns>
        public Bitmap PredictImage()
        {
            Bitmap bitMap = new Bitmap(Width, Height);
            Font myFont = new Font("微软雅黑", 12, FontStyle.Bold);
            SolidBrush s1brush = new SolidBrush(Mine.Number1Color);
            SolidBrush s2brush = new SolidBrush(Mine.Number2Color);
            SolidBrush s3brush = new SolidBrush(Mine.Number3Color);
            SolidBrush s4brush = new SolidBrush(Mine.Number4Color);
            SolidBrush s5brush = new SolidBrush(Mine.Number5Color);
            SolidBrush s6brush = new SolidBrush(Mine.Number6Color);
            using (Graphics gp = Graphics.FromImage(bitMap))
            {
                gp.FillRectangle(Brushes.Gray, 0, 0, Width, Height);
                foreach (Mine mineItem in Mines)
                {
                    float x = mineItem.XIndex * mineItem.Width;
                    float y = mineItem.YIndex * mineItem.Height;
                    if (!mineItem.Opened)
                    {
                        if (mineItem.Flagged)
                            gp.FillRectangle(Brushes.Red, new RectangleF(x, y, mineItem.Width, mineItem.Height));
                        else
                            gp.FillRectangle(Brushes.Blue, new RectangleF(x, y, mineItem.Width, mineItem.Height));
                    }
                    else
                    {
                        if (mineItem.MineNumber == 0)
                            gp.FillRectangle(Brushes.White, new RectangleF(x, y, mineItem.Width, mineItem.Height));
                        else if (mineItem.MineNumber > 0)
                        {
                            gp.FillRectangle(Brushes.White, new RectangleF(x, y, mineItem.Width, mineItem.Height));
                            if (mineItem.MineNumber == 1)
                                gp.DrawString(mineItem.MineNumber.ToString(), myFont, s1brush, x, y);
                            else if (mineItem.MineNumber == 2)
                                gp.DrawString(mineItem.MineNumber.ToString(), myFont, s2brush, x, y);
                            else if (mineItem.MineNumber == 3)
                                gp.DrawString(mineItem.MineNumber.ToString(), myFont, s3brush, x, y);
                            else if (mineItem.MineNumber == 4)
                                gp.DrawString(mineItem.MineNumber.ToString(), myFont, s4brush, x, y);
                            else if (mineItem.MineNumber == 5)
                                gp.DrawString(mineItem.MineNumber.ToString(), myFont, s5brush, x, y);
                            else if (mineItem.MineNumber == 6)
                                gp.DrawString(mineItem.MineNumber.ToString(), myFont, s6brush, x, y);
                        }
                    }
                }
            }

            return bitMap;
        }

        public void Recognize()
        {
            foreach (Mine mineItem in Mines)
            {
                mineItem.Recognize();
            }
        }

        /// <summary>
        /// 扫雷
        /// </summary>
        public bool Sweep()
        {
            if (!FormHelper.IsWindow(WindowHandle))
                return false;

            IntPtr Handle = FormHelper.FindWindow(null, "游戏胜利");
            if (Handle.ToInt32() > 0)
                return false;
            Handle = FormHelper.FindWindow(null, "退出游戏");
            if (Handle.ToInt32() > 0)
            {
                return false;
            }
            Handle = FormHelper.FindWindow(null, "游戏失败");
            if (Handle.ToInt32() > 0)
            {
                if (AutoRestart)
                {
                    RECT rc = new RECT();
                    FormHelper.GetWindowRect(Handle, ref rc);
                    Cursor.Position = new Point(rc.Right - 20, rc.Bottom - 30);
                    MouseHelper.Click(rc.Right - 20, rc.Bottom - 30);
                    Mines.Clear();
                    Thread.Sleep(1000);
                    Refresh();
                    return Sweep();
                }
                else
                    return false;
            }

            FormHelper.SetForegroundWindow(WindowHandle);
            Thread.Sleep(20);

            var opened = Mines.FindAll(item => item.Opened);
            if (opened.Count == 0)
            {
                RandomClick();
                Thread.Sleep(200);
                return true;
            }
            int move = 0;
            foreach (Mine mine in Mines)
            {
                if (SweepMine(mine))
                    move++;

                if (Failed)
                    break;
            }

            if (!Failed)
            {
                if (move == 0) //没有动作时，随便点击一个
                {
                    RandomClick();
                    Thread.Sleep(300);
                    move++;
                }
            }

            Failed = false;

            return move > 0;
        }

        bool Failed = false;

        /// <summary>
        /// 随机点击
        /// </summary>
        private void RandomClick()
        {
            while (true)
            {
                Random random = new Random(DateTime.Now.Millisecond);
                var needClickMines = Mines.FindAll(item => !item.Opened && !item.Flagged);
                if (needClickMines.Count == 0)
                    break;
                int index = random.Next(0, needClickMines.Count);
                needClickMines[index].Click();
                break;
            }
        }

        /// <summary>
        /// 是否自动重开游戏
        /// </summary>
        public bool AutoRestart = false;

        /// <summary>
        /// 处理单个方块
        /// </summary>
        /// <param name="mine"></param>
        /// <returns></returns>
        private bool SweepMine(Mine mine)
        {
            if (!mine.Opened && !mine.Flagged)  //如果没打开，也没标记，则不处理当前方块
                return false;

            if (mine.Opened && mine.MineNumber == 0) //如果是空白块，也不处理
                return false;

            if (!FormHelper.IsWindow(WindowHandle))
            {
                Failed = true;
                return false;
            }

            IntPtr Handle = FormHelper.FindWindow(null, "游戏胜利");
            if (Handle.ToInt32() > 0)
            {
                Failed = true;
                return false;
            }
            Handle = FormHelper.FindWindow(null, "退出游戏");
            if (Handle.ToInt32() > 0)
            {
                Failed = true;
                return false;
            }
            Handle = FormHelper.FindWindow(null, "游戏失败");
            if (Handle.ToInt32() > 0)
            {
                Failed = true;
                if (AutoRestart)
                {
                    RECT rc = new RECT();
                    FormHelper.GetWindowRect(Handle, ref rc);
                    Cursor.Position = new Point(rc.Right - 20, rc.Bottom - 30);
                    MouseHelper.Click(rc.Right - 20, rc.Bottom - 30);
                    Mines.Clear();
                    Thread.Sleep(1000);
                    Refresh();
                    return Sweep();
                }
                else
                    return false;
            }

            List<Mine> Neighbors = GetNeighbors(mine);

            bool hasMove = false;

            if (mine.MineNumber > 0)
            {
                var NotOpenedMines = Neighbors.FindAll(item => !item.Opened); //周围没有点开的数量
                if (NotOpenedMines.Count > 0 && NotOpenedMines.Count == mine.MineNumber)  //如果方格周围有等同数量未打开的方格，则标记
                {
                    NotOpenedMines.ForEach(item =>
                    {
                        if (!item.Opened) //有必要进行判断，集合里的属性有可能已经变动
                        {
                            if (!item.Flagged)
                            {
                                item.Flag();
                                hasMove = true;
                                Thread.Sleep(SleepMiliSeconds);
                            }
                        }
                    });
                }

                var flaggedmines = Neighbors.FindAll(item => item.Flagged);
                if (flaggedmines.Count > 0 && flaggedmines.Count == mine.MineNumber) //如果格子周围已标记了等同个数的方格，则每一个都点一遍
                {
                    Neighbors.ForEach(item =>
                    {
                        if (!flaggedmines.Contains(item) && !item.Opened && !item.Flagged) //有必要进行判断，集合里的属性有可能已经变动
                        {
                            item.Click();  //点开
                            Thread.Sleep(SleepMiliSeconds);
                            //item.RefreshImage(); //刷新图片
                            //item.Recognize(); //再识别当前图片
                            //SweepMine(item); //对已经点开的方块进行排雷
                            hasMove = true;
                        }
                    });
                }
            }

            if (mine.Flagged)
            {
                SweepFlag(mine);
                Thread.Sleep(SleepMiliSeconds);
            }

            return hasMove;
        }

        /// <summary>
        /// 处理标记了地雷周边的方块
        /// </summary>
        /// <param name="mine"></param>
        private void SweepFlag(Mine mine)
        {
            if (mine.Flagged)
            {
                List<Mine> Neighbors = GetNeighbors(mine);

                Neighbors.ForEach(item =>
                {
                    if (item.Opened)
                        ClickNeighbors(item);
                });
            }
        }

        /// <summary>
        /// 点击该方块周围所有未标记未打开的方块
        /// </summary>
        /// <param name="mine"></param>
        private void ClickNeighbors(Mine mine)
        {
            List<Mine> Neighbors = GetNeighbors(mine);

            if (Neighbors.Count(item => item.Flagged) == mine.MineNumber) //已标记数量等同于当前块的地雷数量，则点击周围的方块
            {
                Neighbors.ForEach(clickItem =>
                {
                    if (!clickItem.Opened && !clickItem.Flagged)
                    {
                        clickItem.Click();
                        Thread.Sleep(SleepMiliSeconds);
                    }
                });
            }
        }

        /// <summary>
        /// 获取当前方块的周围8个方块
        /// </summary>
        /// <param name="mine"></param>
        /// <returns></returns>
        private List<Mine> GetNeighbors(Mine mine)
        {
            Mine LeftUP = Mines.Find(item => item.XIndex == mine.XIndex - 1 && item.YIndex == mine.YIndex - 1);
            Mine Left = Mines.Find(item => item.XIndex == mine.XIndex - 1 && item.YIndex == mine.YIndex);
            Mine LeftDown = Mines.Find(item => item.XIndex == mine.XIndex - 1 && item.YIndex == mine.YIndex + 1);
            Mine UP = Mines.Find(item => item.XIndex == mine.XIndex && item.YIndex == mine.YIndex - 1);
            Mine Down = Mines.Find(item => item.XIndex == mine.XIndex && item.YIndex == mine.YIndex + 1);
            Mine RightUP = Mines.Find(item => item.XIndex == mine.XIndex + 1 && item.YIndex == mine.YIndex - 1);
            Mine Right = Mines.Find(item => item.XIndex == mine.XIndex + 1 && item.YIndex == mine.YIndex);
            Mine RightDown = Mines.Find(item => item.XIndex == mine.XIndex + 1 && item.YIndex == mine.YIndex + 1);

            List<Mine> Neighbors = new List<Mine>() { LeftUP, UP, RightUP, Left, Right, LeftDown, Down, RightDown };
            Neighbors.RemoveNull();
            return Neighbors;
        }
    }
}
