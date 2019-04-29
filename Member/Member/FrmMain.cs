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
    public partial class FrmMain : Form
    {
        public FrmMain(string Username)
        {
            InitializeComponent();
            this.label1.Text = "Hello!!"+Username;
        }

    }
}
