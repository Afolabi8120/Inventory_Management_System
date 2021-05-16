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
    public partial class frmProduct : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public string newname;

        public frmProduct()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cboCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public void getCategory()
        {
            cboCategory.Items.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcategory ORDER BY name ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cboCategory.Items.Add(dr["name"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void getID()
        {
            string mynum;
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(*) FROM tblproduct", cn);
            mynum = cm.ExecuteScalar().ToString();
            cn.Close();

            Random myrand = new Random();
            int num = myrand.Next(20, 1000);

            txtCode.Text = "P-" + DateTime.Now.Year + mynum + num;
        }

        public void LoadRecord()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct ORDER BY name ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["code"].ToString(), dr["name"].ToString(), dr["barcode"].ToString(), dr["category"].ToString(), dr["description"].ToString(), Convert.ToBoolean(dr["status"]));
            }
            dr.Close();
            cn.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)//save product details starts here
        {
            if (txtName.Text == "" || txtCode.Text == "" || txtDescription.Text == "" || cboCategory.Text == "")
            {
                MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Save Product Details", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblproduct WHERE name = @name", cn);
                cm.Parameters.AddWithValue("@name", txtName.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    cn.Close();
                    MessageBox.Show("Product Name already existed!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("INSERT INTO tblproduct(code,name,barcode,category,description,status,quantity,price) VALUES(@code,@name,@barcode,@category,@description,@status,@quantity,@price)", cn);
                    cm.Parameters.AddWithValue("@code", txtCode.Text);
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@category", cboCategory.Text);
                    cm.Parameters.AddWithValue("@description", txtDescription.Text);
                    cm.Parameters.AddWithValue("@status", Convert.ToBoolean(cbActive.Checked.ToString()));
                    cm.Parameters.AddWithValue("@quantity", 0);
                    cm.Parameters.AddWithValue("@price", 0.00);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Product details has been added successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecord();                   
                    Clear();
                }
            }
        }//save product details ends here

        void Clear()
        {
            txtName.Clear();
            txtCode.Clear();
            txtDescription.Clear();
            txtBarcode.Text = "";
            cbActive.Checked = false;
            txtName.Focus();
            getID();
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
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblproduct WHERE code = @code", cn);
                cm.Parameters.AddWithValue("@code", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    txtCode.Text = dr["code"].ToString();
                    txtName.Text = dr["name"].ToString();
                    txtBarcode.Text = dr["barcode"].ToString();
                    cboCategory.Text = dr["category"].ToString();
                    txtDescription.Text = dr["description"].ToString();
                    cbActive.Checked = Convert.ToBoolean(dr["status"]);
                    dr.Close();
                    cn.Close();

                    btnSave.Enabled = false;
                    btnUpdate.Enabled = true;
                }
            }
            else if (ColName == "ColDelete") //this will delete the selected user details
            {
                if (MessageBox.Show("Remove Product", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new MySqlCommand("DELETE FROM tblproduct WHERE code = @code", cn);
                    cm.Parameters.AddWithValue("@code", newname);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Product details has been removed successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecord();
                    Clear();
                }
            }
        }

        private void txtSearch_OnValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct WHERE code LIKE '" + txtSearch.Text + "%' OR name LIKE '" + txtSearch.Text + "%' ORDER BY name ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["code"].ToString(), dr["name"].ToString(), dr["barcode"].ToString(), dr["category"].ToString(), dr["description"].ToString(), Convert.ToBoolean(dr["status"]));
            }
            dr.Close();
            cn.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtCode.Text == "" || txtDescription.Text == "" || cboCategory.Text == "")
            {
                MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Update Supplier Details", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //this will update the supplier details already in the database
                cn.Open();
                cm = new MySqlCommand("UPDATE tblproduct SET code = @code, name = @name, barcode = @barcode, category = @category, description = @description, status = @status WHERE code = @code", cn);
                cm.Parameters.AddWithValue("@code", txtCode.Text);
                cm.Parameters.AddWithValue("@name", txtName.Text);
                cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                cm.Parameters.AddWithValue("@category", cboCategory.Text);
                cm.Parameters.AddWithValue("@description", txtDescription.Text);
                cm.Parameters.AddWithValue("@status", Convert.ToBoolean(cbActive.Checked.ToString()));
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Product details has been updated successfully", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRecord();
                Clear();
            }
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {

        }
    }
}
