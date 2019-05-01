using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Member
{
    public partial class Maindesign : Form
    {
        public Maindesign()
        {
            InitializeComponent();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDown)
            {
                this.Location = new Point(this.Left += e.X - mx, this.Top += e.Y - my);
            }
        }
        bool isDown = false;
        int mx, my;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isDown = true;
            mx = e.X;
            my = e.Y;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDown = false;
        }
    }
}
