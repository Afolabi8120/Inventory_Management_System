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
    public partial class frmCategory : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public string newname;

        public frmCategory()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //to load all the categories from database into the datagrid
        public void LoadRecord()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcategory", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["name"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        //to save the category name
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                txtName.Focus();
                MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Save Category", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //this will check if the category name is already in the database
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblcategory WHERE name = @name", cn);
                cm.Parameters.AddWithValue("@name", txtName.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    cn.Close();
                    MessageBox.Show("Caegory name already existed!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    //this will save the category name into the database if such name does not exist
                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("INSERT INTO tblcategory(name) VALUES(@name)", cn);
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Category Name has been added successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecord();
                    txtName.Clear();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtName.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        //to update the category name
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                txtName.Focus();
                MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Save Category", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                cm = new MySqlCommand("UPDATE tblcategory SET name = @name WHERE name = '" + newname +"'", cn);
                cm.Parameters.AddWithValue("@name", txtName.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Category Name has been updated successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRecord();
                txtName.Clear();
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            newname = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            //this is use to get the name of the category name you want to edit 
            if (ColName == "ColEdit")
            {
                txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                btnSave.Enabled = false;
                btnUpdate.Enabled = true;
            }
            else if (ColName == "ColDelete") //this will delete the selected category name
            {
                if (MessageBox.Show("Remove Category", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new MySqlCommand("DELETE FROM tblcategory WHERE name = @name", cn);
                    cm.Parameters.AddWithValue("@name", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Category Name has been removed successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecord();
                }
            }
        }

        private void txtSearch_OnValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcategory WHERE name LIKE '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["name"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {

        }
    }
}
