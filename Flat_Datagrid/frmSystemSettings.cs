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
    public partial class frmSystemSettings : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public frmSystemSettings()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadRecord()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblsettings", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if(dr.HasRows)
            {
                txtName.Text = dr["name"].ToString();
                txtWebsite.Text = dr["website"].ToString();
                txtPhoneNo.Text = dr["phoneno"].ToString();
                txtEmail.Text = dr["email"].ToString();
                txtAddress.Text = dr["address"].ToString();
            }
            else
            {
                txtName.Text = "";
                txtWebsite.Text = "";
                txtPhoneNo.Text = "";
                txtEmail.Text = "";
                txtAddress.Text = "";
            }
            dr.Close();
            cn.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtAddress.Text == "" || txtWebsite.Text == "" || txtEmail.Text == "" || txtPhoneNo.Text == "")
            {
                MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Save Store Info", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                    //this will save the store information into the database 
                    cn.Open();
                    cm = new MySqlCommand("DELETE FROM tblsettings", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("INSERT INTO tblsettings(name,website,phoneno,email,address) VALUES(@name,@website,@phoneno,@email,@address)", cn);
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.Parameters.AddWithValue("@website", txtWebsite.Text);
                    cm.Parameters.AddWithValue("@phoneno", txtPhoneNo.Text);
                    cm.Parameters.AddWithValue("@email", txtEmail.Text);
                    cm.Parameters.AddWithValue("@address", txtAddress.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Store Info has been saved successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecord();
                }
            }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete Store Info", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //this will delete the store information from the database
                cn.Open();
                cm = new MySqlCommand("DELETE FROM tblsettings", cn);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Store Info has been deleted successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRecord();
            }
        }

        private void txtPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //only allows numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtWebsite.Text = "";
            txtPhoneNo.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            txtName.Focus();
        }
    }
}
