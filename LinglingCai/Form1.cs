using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LinglingCai
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == System.Windows.Forms.MouseButtons.Right)
                this.Close();
            else
                NextImage();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Bounds = Screen.PrimaryScreen.Bounds;


            int w = pictureBox1.Size.Width - pictureBox1.Margin.Left - pictureBox1.Margin.Right;
            int h = pictureBox1.Size.Height - pictureBox1.Margin.Top - pictureBox1.Margin.Bottom;
            ImageMgr.ContainerSize = new Size(w, h);
            NextImage();
        }

        private void NextImage(bool backward = false)
        {
            try
            {

                string name;
                Image img = backward ? ImageMgr.Prev(out name) : ImageMgr.Next(out name);

                pictureBox1.Image = img;
                label1.Text = name;
            }
            catch (Exception e)
            {
                MessageBox.Show("出错啦：\n" + e);
            }
        }


        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)    //esc
                this.Close();
            else if (e.KeyChar == 'b')
                NextImage(true);
            else
                NextImage();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
