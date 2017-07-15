using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinglingCai
{
    public partial class FormSelectCategory : Form
    {
        public FormSelectCategory()
        {
            InitializeComponent();
        }

        private void FormSelectCategory_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            this.Bounds = Screen.PrimaryScreen.Bounds;


            try
            {
                ImageMgr.Category[] categories = ImageMgr.ListCategories();

                for (int i = 0; i < categories.Length; i++)
                {
                    ImageMgr.Category c = categories[i];
                    createBox(c.name, c.image, i);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("出错啦：\n" + e);
            }
        }

        private void createBox(string name, Image img, int i)
        {
            PictureBox box;
            if (this.flowLayoutPanel1.Controls.Count > i + 1)
            {
                box = (PictureBox) this.flowLayoutPanel1.Controls[i + 1];
            }
            else
            {
                box = new PictureBox();
                box.Size = new Size(360, 240);
                box.SizeMode = PictureBoxSizeMode.CenterImage;
                box.Cursor = Cursors.Hand;
                box.Tag = name;
                box.Click += box_Click;

                this.toolTip1.SetToolTip(box, name);
                this.flowLayoutPanel1.Controls.Add(box);
            }

            int w = box.Size.Width - box.Margin.Left - box.Margin.Right;
            int h = box.Size.Height - box.Margin.Top - box.Margin.Bottom;
            box.Image = ImageMgr.ResizeImage(img, new Size(w, h));
        }

        void box_Click(object sender, EventArgs e)
        {
            StartCategory((string)((Control)sender).Tag);
        }

        private void FormSelectCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)    //esc
                this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            StartCategory(null);
        }

        private void StartCategory(string name)
        {
            try
            {
                ImageMgr.InitCategory(name);
                Form1 form = new Form1();
                form.ShowDialog(this);
                Init();
            }
            catch (Exception e)
            {
                MessageBox.Show("出错啦：\n" + e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
