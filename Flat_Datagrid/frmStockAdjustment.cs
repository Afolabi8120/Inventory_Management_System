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
    public partial class frmStockAdjustment : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public frmStockAdjustment()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cboOperation_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        //this will get the list of all products into the datagrid
        public void getProduct()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["code"].ToString(), dr["name"].ToString(), dr["barcode"].ToString(), dr["category"].ToString(), dr["description"].ToString(), dr["quantity"].ToString(), dr["price"].ToString(), Convert.ToBoolean(dr["status"]));
            }
            dr.Close();
            cn.Close();
        }

        //to esarch for product by name, category and code
        private void txtSearch_OnValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct WHERE code LIKE '%" + txtSearch.Text + "%' OR name LIKE '" + txtSearch.Text + "%' OR category LIKE '" + txtSearch.Text + "%' ", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["code"].ToString(), dr["name"].ToString(), dr["barcode"].ToString(), dr["category"].ToString(), dr["description"].ToString(), dr["quantity"].ToString(), dr["price"].ToString(), Convert.ToBoolean(dr["status"]));
            }
            dr.Close();
            cn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            string pstatus = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();

            bool status;

            status = bool.Parse(pstatus);

            //this is use to get the name of the category name you want to edit 
            if (ColName == "ColEdit")
            {
                if (status == false)
                {
                    MessageBox.Show("The product selected is In-Active!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    txtProductCode.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                }              
            }
        }

        private void addToInventory()
        {
            cn.Open();
            cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity + @quantity WHERE code = @code", cn);
            cm.Parameters.AddWithValue("@code", txtProductCode.Text);
            cm.Parameters.AddWithValue("@quantity", txtQuantity.Text);
            cm.ExecuteNonQuery();
            cn.Close();
        }

        private void removeFromInventory()
        {
            cn.Open();
            cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity - @quantity WHERE code = @code", cn);
            cm.Parameters.AddWithValue("@code", txtProductCode.Text);
            cm.Parameters.AddWithValue("@quantity", txtQuantity.Text);
            cm.ExecuteNonQuery();
            cn.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtProductCode.Text == "" || txtName.Text == "" || txtDescription.Text == "" || txtQuantity.Text == "" || txtUser.Text == "" || cboOperation.Text == "")
            {
                MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtQuantity.Text == "0")
            {
                MessageBox.Show("Please enter a valid quantity!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Save this Record? Click Yes to Confirm!", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (cboOperation.Text == "ADD TO INVENTORY")
                {
                    addToInventory();
                    getProduct();
                    Clear();
                    MessageBox.Show("Product has been added successfully!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (cboOperation.Text == "REMOVE FROM INVENTORY")
                {
                    removeFromInventory();
                    getProduct();
                    Clear();
                    MessageBox.Show("Product has been removed successfully!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        void Clear()
        {
            txtDescription.Clear();
            txtName.Clear();
            txtProductCode.Clear();
            txtQuantity.Text = "0";
            cboOperation.Text = "";
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            //only allows numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
