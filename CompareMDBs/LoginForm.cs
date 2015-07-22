using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CompareMDBs
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txt_login.Text == "isodmin")
                DialogResult = DialogResult.OK;
            else MessageBox.Show("Пароль введён неверно!");
        }
    }
}
