using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Member
{
    public partial class ChangePWpage : UserControl
    {
        public ChangePWpage()
        {
            InitializeComponent();
            this.AuthCodetext.Enabled = false;
            this.button1.Click += Button1_Click;
        }
        public event EventHandler Tc;
        private void Button1_Click(object sender, EventArgs e)
        {
            ClsMember user = new ClsMember();
            user.Username = this.Usertext.Text;
            user.Password = this.Pwordtext.Text;
            if (user.ValidateUser())
            {
                if (PasswordChangeFun.CheckAuthCode(user.Username, this.AuthCodetext.Text) && this.AuthCodetext.Text != "")
                {
                    if (PasswordChangeFun.UpdateUser(this.Usertext.Text, this.NPWtext.Text))
                    {
                        MessageBox.Show("更改成功");
                        if (this.Tc != null)
                            this.Tc(this.button1, e);
                    }
                    else
                        MessageBox.Show("更改失敗");
                }
                else
                    this.label5.Text = "驗證碼錯誤";
            }
            else
                this.label5.Text = "會員資料錯誤";
        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.AuthCodetext.Enabled = true;
            PasswordChangeFun.SendAuthCode(this.Usertext.Text);
            this.label5.Text = "已寄送驗證碼到註冊Email";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
