using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Member
{
    public partial class SignUp : Maindesign
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClsMember user = new ClsMember();

            user.Username = this.Usertext.Text;
            user.Password = this.Pwordtext.Text;
            if (IDo.IsValidEmail(this.Emailtext.Text))
            {
                user.Email = this.Emailtext.Text;
            }
            else
            {
                MessageBox.Show("Email 格式錯誤");
                return;
            }
            try
            {
                user.CreateUser();

                MessageBox.Show($"註冊成功");

            }
            catch (SqlException ex)
            {
                int n = ex.Number;
                if (n == 2627)
                    MessageBox.Show("帳號重複", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Usertext.Focus();
                this.Usertext.SelectAll();
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
