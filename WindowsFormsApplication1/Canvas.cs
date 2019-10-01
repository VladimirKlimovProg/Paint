using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Canvas : Form
    {
        public static string drawMode = "Перо";
        public static int angle = 36;
        public static int radius = 50;
        public static string filePath = null;
        public static ImageFormat format = null; 
        private int oldX, oldY;
        private int startX, startY;
        private static Bitmap bmp;
        public Canvas()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
        }

        public Canvas(string FileName)
        {
            InitializeComponent();
            bmp = new Bitmap(FileName);
            Graphics g = Graphics.FromImage(bmp);
            pictureBox1.Width = bmp.Width;
            pictureBox1.Height = bmp.Height;
            pictureBox1.Image = bmp;
        }

        public int CanvasWidth
        {
            get
            {
                return pictureBox1.Width;
            }
            set
            {
                pictureBox1.Width = value;
                Bitmap tbmp = new Bitmap(value, pictureBox1.Width);
                Graphics g = Graphics.FromImage(tbmp);
                g.Clear(Color.White);
                g.DrawImage(bmp, new Point(0, 0));
                bmp = tbmp;
                pictureBox1.Image = bmp;
            }
        }

        public int CanvasHeight
        {
            get
            {
                return pictureBox1.Height;
            }
            set
            {
                pictureBox1.Height = value;
                Bitmap tbmp = new Bitmap(pictureBox1.Width, value);
                Graphics g = Graphics.FromImage(tbmp);
                g.Clear(Color.White);
                g.DrawImage(bmp, new Point(0, 0));
                bmp = tbmp;
                pictureBox1.Image = bmp;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = Graphics.FromImage(bmp);
            if (drawMode == "Перо" || drawMode == "Ластик")
            {
                if (e.Button == MouseButtons.Left)
                {
                    g.DrawLine(new Pen(MainForm.CurColor, MainForm.CurWidth), oldX, oldY, e.X, e.Y);
                    oldX = e.X;
                    oldY = e.Y;
                    pictureBox1.Invalidate();
                }
            }

            (MdiParent as MainForm).toolStripStatusLabel1.Text = $"X:{e.X} Y:{e.Y}";
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            (MdiParent as MainForm).toolStripStatusLabel1.Text = string.Empty;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawMode == "Линия" || drawMode == "Эллипс")
            {
                if (e.Button == MouseButtons.Left)
                {
                    startX = e.X;
                    startY = e.Y;
                }
            }
            else
            {
                oldX = e.X;
                oldY = e.Y;
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Graphics g = Graphics.FromImage(bmp);
            if (drawMode == "Линия")
            {
                if (e.Button == MouseButtons.Left)
                {
                    g.DrawLine(new Pen(MainForm.CurColor, MainForm.CurWidth), startX, startY, e.X, e.Y);
                    pictureBox1.Invalidate();
                }
            }
            else if (drawMode == "Эллипс")
            {
                if (e.Button == MouseButtons.Left)
                {
                    g.DrawEllipse(new Pen(MainForm.CurColor, MainForm.CurWidth), startX, startY, e.X - startX, e.Y - startY);
                    pictureBox1.Invalidate();
                }
            }
            else if (drawMode == "Звезда незакрашенная" || drawMode == "Звезда закрашенная")
            {
                if (e.Button == MouseButtons.Left)
                {
                    startX = e.X;
                    startY = e.Y;
                    PointF[] points = new PointF[360 / angle];
                    int curAngle = angle;
                    for (int i = 0; i < points.Length; i++)
                    {
                        PointF point;
                        if (i % 2 == 0)
                        {
                            float x = (float)(startX + Math.Cos(curAngle * (Math.PI / 180)) * (radius / 2));
                            float y = (float)(startY - Math.Sin(curAngle * (Math.PI / 180)) * (radius / 2));
                            point = new PointF(x, y);

                        }
                        else
                        {
                            float x = (float)(startX + Math.Cos(curAngle * (Math.PI / 180)) * radius);
                            float y = (float)(startY - Math.Sin(curAngle * (Math.PI / 180)) * radius);
                            point = new PointF(x, y);
                        }
                        points[i] = point;
                        curAngle = curAngle + angle;
                    }

                    if (drawMode == "Звезда незакрашенная")
                    {
                        g.DrawPolygon(new Pen(MainForm.CurColor, MainForm.CurWidth), points);
                    }
                    else
                    {
                        SolidBrush brush = new SolidBrush(MainForm.CurColor);
                        g.FillPolygon(brush, points);
                    }

                    pictureBox1.Invalidate();
                }
            }
        }

        public void SaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.Filter = "Windows Bitmap (*.bmp)|*.bmp| Файлы JPEG (*.jpg)|*.jpg";
            ImageFormat[] ff = { ImageFormat.Bmp, ImageFormat.Jpeg };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(dlg.FileName, ff[dlg.FilterIndex - 1]);
            }
        }

        public void Save()
        {
          
            if (filePath == null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.AddExtension = true;
                dlg.Filter = "Windows Bitmap (*.bmp)|*.bmp| Файлы JPEG (*.jpg)|*.jpg";
                ImageFormat[] ff = { ImageFormat.Bmp, ImageFormat.Jpeg };
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(dlg.FileName, ff[dlg.FilterIndex - 1]);
                    filePath = dlg.FileName;
                    format = ff[dlg.FilterIndex - 1];
                }
            }
            else
            {
                bmp.Save(filePath, format);
            }

        }

        public void ZoomIn()
        {
            Size newSize = new Size(bmp.Width * 2, bmp.Height * 2);
            bmp = new Bitmap(bmp, newSize);
            pictureBox1.Image = bmp;

            //pictureBox1.Width = pictureBox1.Width * 2;
            //pictureBox1.Height = pictureBox1.Height * 2;

        }

        public void ZoomOut()
        {
            Size newSize = new Size(bmp.Width / 2, bmp.Height / 2);
            bmp = new Bitmap(bmp, newSize);
            pictureBox1.Image = bmp;
        }

    }

}
