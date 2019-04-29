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
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClsMember user = new ClsMember();

            user.Username = this.UsertextBox.Text;
            user.Password = this.Pwordtextbox.Text;
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
                this.UsertextBox.Focus();
                this.UsertextBox.SelectAll();
            }
        }

    }
}
