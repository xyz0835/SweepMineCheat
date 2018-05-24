using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MineSweepCheet
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sweepzone = null;
                return;


                IntPtr handle = FormHelper.FindWindow(null, "扫雷");

                if (handle.ToInt32() > 0)
                {
                    FormHelper.SetForegroundWindow(handle);

                    RECT rc = new RECT();
                    FormHelper.GetWindowRect(handle, ref rc);

                    Point startPoint = new Point(38, 80);
                    Point endPoint = new Point(580, 370);

          
                    int zonewidth = endPoint.X - startPoint.X;  //542
                    int zoneheight = endPoint.Y - startPoint.Y; //290
                    //int x = rc.Left + startPoint.X;
                    //int y = rc.Top + startPoint.Y;

                    int MineWidth = 18;

                    int MineHeight = 18;

                    Bitmap zoneImage = new Bitmap(zonewidth, zoneheight);

                    Bitmap mineImage = new Bitmap(18, 18);

                    int XIndex = 29;
                    int YIndex = 15;

                    int MineX = XIndex * 18;
                    int MineY = YIndex * 18;

                    using (Bitmap windowimage = new Bitmap(rc.Width, rc.Height))
                    {
                        using (Graphics windowgp = Graphics.FromImage(windowimage), zonegp = Graphics.FromImage(zoneImage))
                        {
                            IntPtr windowdc = windowgp.GetHdc();
                            FormHelper.PrintWindow(handle, windowdc, 0);
                            windowgp.ReleaseHdc();

                            zonegp.DrawImage(windowimage, 0, 0, new Rectangle(startPoint.X, startPoint.Y, zonewidth, zoneheight), GraphicsUnit.Pixel);

                            string fileName = string.Format("{0:yyyyMMddHHmmssfff}window.jpg", DateTime.Now);
                            //windowimage.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            fileName = string.Format("{0:yyyyMMddHHmmssfff}zone.jpg", DateTime.Now);
                            //zoneImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                            using (Graphics minegp = Graphics.FromImage(mineImage))
                            {
                                minegp.DrawImage(zoneImage, 0, 0, new Rectangle(MineX, MineY, MineWidth, MineHeight), GraphicsUnit.Pixel);
                            }
                        }

                        picZone.Image = mineImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        SweepZone sweepzone = null;
        private void button2_Click(object sender, EventArgs e)
        {
            Point cursorPoint = Cursor.Position;
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                IntPtr handle = FormHelper.FindWindow(null, "扫雷");

                if (handle.ToInt32() > 0)
                {
                    if (sweepzone == null)
                        sweepzone = new SweepZone(handle);

                    sweepzone.Refresh();
                    sweepzone.Recognize();
                    ShowMineImage();
                    picZone.Image = sweepzone.PredictImage();
                    //picZone.Image = sweepzone.ZoneImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sw.Stop();
                Cursor.Position = cursorPoint;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ShowMineImage();
        }

        private void ShowMineImage()
        {
            try
            {
                if (txtX.Text.Length > 0 && txtY.Text.Length > 0)
                {
                    if (sweepzone != null)
                    {
                        Mine mineItem = sweepzone.Mines.Find(item => item.XIndex == Convert.ToInt32(txtX.Text) && item.YIndex == Convert.ToInt32(txtY.Text));
                        if (mineItem != null)
                        {
                            picMine.Image = mineItem.MineImage;
                            txtResult.Text = mineItem.RecognizeResult;
                            if (txtX.Text != "0" || txtY.Text != "0")
                                Cursor.Position = new Point((int)mineItem.Position.X + (int)mineItem.Width / 2, (int)mineItem.Position.Y + (int)mineItem.Height / 2);
                        }
                    }
                }
            }
            catch  //(Exception ex)
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Point cursorPoint = Cursor.Position;
            try
            {
                if (sweepzone == null)
                    return;
                
                sweepzone.Sweep();
                sweepzone.Refresh();
                sweepzone.Recognize();
                ShowMineImage();
                picZone.Image = sweepzone.PredictImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Cursor.Position = cursorPoint;
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            ThreadStarter.Start(() =>
            {
                Point cursorPoint = Cursor.Position;
                Stopwatch sw = Stopwatch.StartNew();
                try
                {
                    if (sweepzone == null)
                    {
                        IntPtr handle = FormHelper.FindWindow(null, "扫雷");

                        if (handle.ToInt32() > 0)
                        {
                            sweepzone = new SweepZone(handle);
                            sweepzone.Refresh();
                            sweepzone.Recognize();
                            ShowMineImage();
                            picZone.Image = sweepzone.PredictImage();
                        }
                    }
                    while (true)
                    {
                        if (sw.ElapsedMilliseconds > 60000)
                            break;

                        if (!sweepzone.Sweep())
                            break;
                        sweepzone.Refresh();
                        sweepzone.Recognize();
                        this.InvokeMethod(() =>
                        {
                            picZone.Image = sweepzone.PredictImage();
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    //Cursor.Position = cursorPoint;
                    sw.Stop();
                }
            });
        }

        private void picZone_Click(object sender, EventArgs e)
        {
            try
            {
                if (sweepzone != null)
                {
                    Point mousePoint = picZone.PointToClient(Cursor.Position);

                    int xindex = mousePoint.X / 18;
                    int yindex = mousePoint.Y / 18;

                    Mine mine = sweepzone.Mines.Find(item => item.XIndex == xindex && item.YIndex == yindex);
                    if (mine != null)
                    {
                        txtResult.Text = mine.RecognizeResult;
                        picMine.Image = mine.MineImage;
                    }
                }
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.ToString();
            }
        }

        private void picZone_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (sweepzone != null)
                {
                    Point mousePoint = picZone.PointToClient(Cursor.Position);

                    int xindex = mousePoint.X / 18;
                    int yindex = mousePoint.Y / 18;

                    Mine mine = sweepzone.Mines.Find(item => item.XIndex == xindex && item.YIndex == yindex);
                    if (mine != null)
                    {
                        mine.Recognize();
                    }
                }
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.ToString();
            }
        }

        private void txtResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
        }
    }
}
