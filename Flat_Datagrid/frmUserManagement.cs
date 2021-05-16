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
    public partial class frmUserManagement : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public string newname;

        public frmUserManagement()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cboUsertype_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        //to load all the user's records from database into the datagrid
        public void LoadRecord()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tbluser", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["username"].ToString(), dr["fullname"].ToString(), dr["usertype"].ToString(), Convert.ToBoolean(dr["status"]));
            }
            dr.Close();
            cn.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtFullname.Text == "" || txtPassword.Text == "" || txtCPassword.Text == "")
            {
                MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtPassword.Text != txtCPassword.Text)
            {
                MessageBox.Show("Password do not match!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cboUsertype.Text == "")
            {
                MessageBox.Show("Please selec a valid usertype!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Save User Details", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //this will check if the username is already in the database
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tbluser WHERE username = @username", cn);
                cm.Parameters.AddWithValue("@username", txtUsername.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    cn.Close();
                    MessageBox.Show("Username is already taken!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    //this will save the username into the database if such name does not exist
                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("INSERT INTO tbluser(username,fullname,password,usertype,status) VALUES(@username,@fullname,@password,@usertype,@status)", cn);
                    cm.Parameters.AddWithValue("@username", txtUsername.Text);
                    cm.Parameters.AddWithValue("@fullname", txtFullname.Text);
                    cm.Parameters.AddWithValue("@password", txtPassword.Text);
                    cm.Parameters.AddWithValue("@usertype", cboUsertype.Text);
                    cm.Parameters.AddWithValue("@status", Convert.ToBoolean(cbActive.Checked.ToString()));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("User detalis has been added successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecord();
                    Clear();
                }
            }
        }

        void Clear()
        {
            txtUsername.Clear();
            txtFullname.Clear();
            txtPassword.Clear();
            txtCPassword.Clear();
            cboUsertype.Text = "";
            cbActive.Checked = false;
        }

        private void cbActive_CheckedChanged(object sender, EventArgs e)
        {
            if (cbActive.Checked == true)
            {
                cbActive.Text = "Active";
            }
            else if (cbActive.Checked == false)
            {
                cbActive.Text = "In-Active";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            txtUsername.ReadOnly = false;
            txtUsername.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            newname = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            //this is use to get the name of the category name you want to edit 
            if (ColName == "ColEdit")
            {
                txtUsername.ReadOnly = true;

                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tbluser WHERE username = @username", cn);
                cm.Parameters.AddWithValue("@username", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    txtUsername.Text = dr["username"].ToString();
                    txtFullname.Text = dr["fullname"].ToString();
                    txtPassword.Text = dr["password"].ToString();
                    txtCPassword.Text = dr["password"].ToString();
                    cboUsertype.Text = dr["usertype"].ToString();
                    cbActive.Checked = Convert.ToBoolean(dr["status"]);
                    dr.Close();
                    cn.Close();

                    btnSave.Enabled = false;
                    btnUpdate.Enabled = true;
                }
            }
            else if (ColName == "ColDelete") //this will delete the selected user details
            {
                    if (MessageBox.Show("Remove User", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cn.Open();
                        cm = new MySqlCommand("DELETE FROM tbluser WHERE username = @username", cn);
                        cm.Parameters.AddWithValue("@username", newname);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("User has been removed successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadRecord();
                        Clear();
                    }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e) // update user starts here
        {
            if (txtUsername.Text == "" || txtFullname.Text == "" || txtPassword.Text == "" || txtCPassword.Text == "")
            {
                MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtPassword.Text != txtCPassword.Text)
            {
                MessageBox.Show("Password do not match!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cboUsertype.Text == "")
            {
                MessageBox.Show("Please select a valid usertype!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Update User Details", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                    //this will update the user details already in the database
                    cn.Open();
                    cm = new MySqlCommand("UPDATE tbluser SET fullname = @fullname, password = @password, usertype = @usertype, status = @status WHERE username = @username", cn);
                    cm.Parameters.AddWithValue("@username", txtUsername.Text);
                    cm.Parameters.AddWithValue("@fullname", txtFullname.Text);
                    cm.Parameters.AddWithValue("@password", txtPassword.Text);
                    cm.Parameters.AddWithValue("@usertype", cboUsertype.Text);
                    cm.Parameters.AddWithValue("@status", Convert.ToBoolean(cbActive.Checked.ToString()));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("User details has been updated successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecord();
                    Clear();
                }
            
        } // update user ends here

        private void txtSearch_OnValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tbluser WHERE username LIKE '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["username"].ToString(), dr["fullname"].ToString(), dr["password"].ToString(), dr["usertype"].ToString(), Convert.ToBoolean(dr["status"]));
            }
            dr.Close();
            cn.Close();
        }
    }
}
