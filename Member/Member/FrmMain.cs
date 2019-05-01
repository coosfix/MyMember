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
    public partial class FrmMain : Maindesign
    {
        public FrmMain(string Username)
        {
            InitializeComponent();
            this.changePWpage1.Hide();

            this.label1.Text = "Hello!!"+Username;
            this.changePWpage1.Tc += new EventHandler(Button1_Click);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.changePWpage1.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.changePWpage1.Hide();
        }
    }
}
