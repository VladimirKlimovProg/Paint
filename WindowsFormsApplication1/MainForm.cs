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
    public partial class MainForm : Form
    {

        public static Color CurColor = Color.Black;
        public static int CurWidth = 3;
        public MainForm()
        {
            InitializeComponent();
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Canvas frmChild = new Canvas();
            frmChild.MdiParent = this;
            frmChild.Show();
        }

        private void вертикальноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Windows Bitmap (*.bmp)|*.bmp| Файлы JPEG (*.jpeg, *.jpg)|*.jpeg;*.jpg|Все файлы ()*.*|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Canvas frmChild = new Canvas(dlg.FileName);
                frmChild.MdiParent = this;
                frmChild.Show();
            }

        }

        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ОПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutPaint frmAbout = new AboutPaint();
            frmAbout.ShowDialog();
        }

        private void рисунокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            размерХолстаToolStripMenuItem.Enabled = !(ActiveMdiChild == null);
        }

        private void размерХолстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CanvasSize cs = new CanvasSize();
            cs.CanvasWidth.Text = ((Canvas)ActiveMdiChild).CanvasWidth.ToString();
            cs.CanvasHeight.Text = ((Canvas)ActiveMdiChild).CanvasHeight.ToString();
            if (cs.ShowDialog() == DialogResult.OK)
            {
                ((Canvas)ActiveMdiChild).CanvasWidth = Convert.ToInt32(cs.CanvasWidth.Text);
                ((Canvas)ActiveMdiChild).CanvasHeight = Convert.ToInt32(cs.CanvasHeight.Text);
            }

        }

        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurColor = Color.Red;
        }

        private void синийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurColor = Color.Blue;
        }

        private void зеленыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurColor = Color.Green;
        }

        private void другойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
                CurColor = cd.Color;
        }

        private void txtBrushSize_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CurWidth = int.Parse(txtBrushSize.Text);
            }
            catch
            {
                MessageBox.Show("Значение должно быть целым числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ((Canvas)ActiveMdiChild).SaveAs();
            }
           
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ((Canvas)ActiveMdiChild).Save();
            };
        }

        private void слеваНаправоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void сверхуВнизToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void упорядочитьЗначкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Canvas.drawMode = "Линия";

        }

        private void пероToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Canvas.drawMode = "Перо";

        }

        private void ластикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Canvas.drawMode = "Ластик";
            CurColor = Color.White;
        }

        private void эллипсToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Canvas.drawMode = "Эллипс";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtBrushSize.Text = CurWidth.ToString();
            starComboBox.SelectedIndex = 1;
            radiusTextBox.Text = Canvas.radius.ToString();
        }

        private void starComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int numberOfVertices = Convert.ToInt32(starComboBox.SelectedItem.ToString());

            Canvas.angle = 360 / (numberOfVertices * 2);
        }


        private void звездаЗакрашеннаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Canvas.drawMode = "Звезда закрашенная";
        }

        private void звездаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Canvas.drawMode = "Звезда незакрашенная";
        }

        private void radiusTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Canvas.radius = int.Parse(radiusTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Значение должно быть целым числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void масштабToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ((Canvas)ActiveMdiChild).ZoomIn();
            }
        }

        private void масштабToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ((Canvas)ActiveMdiChild).ZoomOut();
            }
        }
    }
}
