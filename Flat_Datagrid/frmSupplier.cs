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
    public partial class frmSupplier : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public string newname;

        public frmSupplier()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e) //save supplier details starts here
        {
            if (txtName.Text == "" || txtAddress.Text == "" || txtContact.Text == "" || txtEmail.Text == "" || txtPhoneNo.Text == "")
            {
                MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Save Supplier Details", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                    //this will save the supplier information into the database if such name does not exist
                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("INSERT INTO tblsupplier(name,address,contactperson,phoneno,email,status) VALUES(@name,@address,@contactperson,@phoneno,@email,@status)", cn);
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.Parameters.AddWithValue("@address", txtAddress.Text);
                    cm.Parameters.AddWithValue("@contactperson", txtContact.Text);
                    cm.Parameters.AddWithValue("@phoneno", txtPhoneNo.Text);
                    cm.Parameters.AddWithValue("@email", txtEmail.Text);
                    cm.Parameters.AddWithValue("@status", Convert.ToBoolean(cbActive.Checked.ToString()));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("User details has been added successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecord();
                    Clear();
                }
            }//save supplier details ends here

        //to load all the supplier's records from database into the datagrid
        public void LoadRecord()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblsupplier", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["name"].ToString(), dr["address"].ToString(), dr["contactperson"].ToString(), dr["phoneno"].ToString(), dr["email"].ToString(), Convert.ToBoolean(dr["status"]));
            }
            dr.Close();
            cn.Close();
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

        private void txtSearch_OnValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblsupplier WHERE name LIKE '" + txtSearch.Text + "%' OR contactperson LIKE '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["name"].ToString(), dr["address"].ToString(), dr["contactperson"].ToString(), dr["phoneno"].ToString(), dr["email"].ToString(), Convert.ToBoolean(dr["status"]));
            }
            dr.Close();
            cn.Close();
        }

        void Clear()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtPhoneNo.Clear();
            cbActive.Checked = false;
            txtName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e) //update supplier details starts here
        {
            if (txtName.Text == "" || txtAddress.Text == "" || txtContact.Text == "" || txtEmail.Text == "" || txtPhoneNo.Text == "")
            {
                MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Update Supplier Details", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //this will update the supplier details already in the database
                cn.Open();
                cm = new MySqlCommand("UPDATE tblsupplier SET name = @name, address = @address, contactperson = @contactperson, phoneno = @phoneno, email = @email, status = @status WHERE name = @name", cn);
                cm.Parameters.AddWithValue("@name", txtName.Text);
                cm.Parameters.AddWithValue("@address", txtAddress.Text);
                cm.Parameters.AddWithValue("@contactperson", txtContact.Text);
                cm.Parameters.AddWithValue("@phoneno", txtPhoneNo.Text);
                cm.Parameters.AddWithValue("@email", txtEmail.Text);
                cm.Parameters.AddWithValue("@status", Convert.ToBoolean(cbActive.Checked.ToString()));
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Supplier details has been updated successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRecord();
                Clear();
            }
        }//update supplier details ends here

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            newname = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            //this is use to get the name of the category name you want to edit 
            if (ColName == "ColEdit")
            {
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblsupplier WHERE name = @name", cn);
                cm.Parameters.AddWithValue("@name", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    txtName.Text = dr["name"].ToString();
                    txtAddress.Text = dr["address"].ToString();
                    txtContact.Text = dr["contactperson"].ToString();
                    txtPhoneNo.Text = dr["phoneno"].ToString();
                    txtEmail.Text = dr["email"].ToString();
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
                    cm = new MySqlCommand("DELETE FROM tblsupplier WHERE name = @name", cn);
                    cm.Parameters.AddWithValue("@name", newname);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Supplier details has been removed successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecord();
                    Clear();
                }
            }
        }//update supplier details ends here

        private void txtPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //only allows numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        } 


    }
}
