using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Flat_Datagrid
{
    public partial class frmChangePassword : Form
    {

        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public static string password;

        public frmChangePassword()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        public void getPassword()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tbluser WHERE username = @username", cn);
            cm.Parameters.AddWithValue("@username", frmLogin.username);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                password = dr["password"].ToString();
            }
            dr.Close();
            cn.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtOldPassword.Text == "" || txtNewPassword.Text == "" || txtRetypePassword.Text == "")
            {
                MessageBox.Show("Please fill all required fields!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(txtOldPassword.Text != password)
            {
                txtOldPassword.Focus();
                MessageBox.Show("Old Password is Incorrect!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtNewPassword.Text != txtRetypePassword.Text)
            {
                MessageBox.Show("New Password does not match!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                cn.Open();
                cm = new MySqlCommand("UPDATE tbluser SET password = @password WHERE username = @username", cn);
                cm.Parameters.AddWithValue("@username", frmLogin.username);
                cm.Parameters.AddWithValue("@password", txtNewPassword.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Password has been changed  successfully!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                getPassword();
                txtOldPassword.Text = "";
                txtNewPassword.Text = "";
                txtRetypePassword.Text = "";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
