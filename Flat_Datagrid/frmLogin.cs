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
    public partial class frmLogin : Form
    {

        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        bool found, status;

        public static string username, fullname, usertype, password;

        public frmLogin()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Please fill all required field!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tbluser WHERE username = @username AND password = @password", cn);
                cm.Parameters.AddWithValue("@username", txtUsername.Text);
                cm.Parameters.AddWithValue("@password", txtPassword.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    username = dr["username"].ToString();
                    fullname = dr["fullname"].ToString();
                    password = dr["password"].ToString();
                    usertype = dr["usertype"].ToString();
                    status = Convert.ToBoolean(dr["status"].ToString());

                    found = true;
                }
                else
                {
                    cn.Close();
                    MessageBox.Show("Incorrect Username or Password!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                dr.Close();
                cn.Close();
            }

            if (found == true)
            {
                if (usertype == "Administrator" && status == true)
                {
                    var f1 = new frmMainmenu();
                    f1.Show();
                    this.Hide();
                }
                else if (usertype == "Cashier" && status == true)
                {
                    var f1 = new frmPOS();
                    f1.lblFullName.Text = fullname;
                    f1.lblUsername.Text = username;
                    f1.lblUsertype.Text = usertype;
                    f1.getInvoiceNo();
                    f1.LoadCart();
                    f1.Show();
                    this.Hide();
                }
                else if (usertype == "Administrator" && status == false)
                {
                    MessageBox.Show("Your Account is In-Active, Please contact the Admin!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (usertype == "Cashier" && status == false)
                {
                    MessageBox.Show("Your Account is In-Active, Please contact the Admin!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTwitter_Click(object sender, EventArgs e)
        {

        }
    }
}
