using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Member
{
    public partial class ForgetPassword : Member.Maindesign
    {
        public ForgetPassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (PasswordChangeFun.CheckUser(this.Usertext.Text, this.Emailtext.Text))
            {
                MessageBox.Show("成功");
            }
            else
                MessageBox.Show("失敗");
        }

        private void button2_Click(object sender, EventArgs e)
        { 
        }

        private void ForgetPassword_Load(object sender, EventArgs e)
        {

        }
    }
}
