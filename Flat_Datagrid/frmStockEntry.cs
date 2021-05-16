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
    public partial class frmStockEntry : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public string pname, pcode, pstatus;

        bool checkstatus;

        public frmStockEntry()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                MessageBox.Show("Remove all items from Trolley before exiting!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                this.Dispose();
            }
        }

        private void cboSupplier_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public void getReferenceID()
        {
            string mynum;
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(DISTINCT(referenceno)) FROM tblstockin", cn);
            mynum = cm.ExecuteScalar().ToString();
            cn.Close();

            Random myrand = new Random();
            int num = myrand.Next(20, 1000);

            txtReferenceNo.Text = "REF-" + DateTime.Now.Year + mynum + num;
        }

        public void getTotal()
        {
            string mynum;
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(total) FROM tblstockin WHERE referenceno = @referenceno", cn);
            cm.Parameters.AddWithValue("@referenceno", txtReferenceNo.Text);
            mynum = cm.ExecuteScalar().ToString();
            cn.Close();

            lblTotal.Text = mynum;
        }

        public void getSupplier()
        {
            cn.Open();
            cboSupplier.Items.Clear();
            cm = new MySqlCommand("SELECT * FROM tblsupplier", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cboSupplier.Items.Add(dr["name"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        //this will get the total number of quantity in the trolley based on the reference number
        public void getItemsInTrolley()
        {
            string mynum;
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(quantity) FROM tblstockin WHERE referenceno = @referenceno", cn);
            cm.Parameters.AddWithValue("@referenceno", txtReferenceNo.Text);
            mynum = cm.ExecuteScalar().ToString();
            cn.Close();

            lblItemInTrolley.Text = mynum;
        }

        private void txtSearch_OnValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct WHERE code LIKE '" + txtSearch.Text + "%' OR name LIKE '" + txtSearch.Text + "%' OR category LIKE '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["code"].ToString(), dr["name"].ToString(), Convert.ToBoolean(dr["status"]));
            }
            dr.Close();
            cn.Close();
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            //only allows numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //this will get the list of all products into the first datagrid
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
                dataGridView1.Rows.Add(i, dr["code"].ToString(), dr["name"].ToString(), Convert.ToBoolean(dr["status"]));
            }
            dr.Close();
            cn.Close();
        }

        //this will load data all products to the trolley
        public void loadTrolley()
        {
            int i = 0;
            dataGridView2.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblstockin WHERE referenceno = @referenceno", cn);
            cm.Parameters.AddWithValue("@referenceno", txtReferenceNo.Text);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr["pcode"].ToString(), dr["pname"].ToString(), dr["price"].ToString(), dr["quantity"].ToString(), dr["total"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            pcode = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            pname = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            pstatus = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

            bool status;

            status = bool.Parse(pstatus);

            //this is use to get the name of the category name you want to edit 
            if (ColName == "ColInsert")
            {
                if (status == false)
                {
                    MessageBox.Show("The product selected is In-Active!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    txtCode.Text = pcode;
                    txtName.Text = pname;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Please select a product to proceed!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtCode.Text == "")
            {
                MessageBox.Show("Please select a product to proceed!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtQuantity.Text == "" || txtQuantity.Text == "0")
            {
                txtQuantity.Focus();
                MessageBox.Show("Please enter a valid quantity!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtSellingPrice.Text == "" || txtSellingPrice.Text == "0.00")
            {
                txtSellingPrice.Focus();
                MessageBox.Show("Please enter a valid selling price!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtBuyingPrice.Text == "" || txtBuyingPrice.Text == "0.00")
            {
                txtBuyingPrice.Focus();
                MessageBox.Show("Please enter a valid selling price!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Add To Trolley", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblstockin WHERE referenceno = @referenceno AND pcode = @pcode", cn);
                cm.Parameters.AddWithValue("@referenceno", txtReferenceNo.Text);
                cm.Parameters.AddWithValue("@pcode", txtCode.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblstockin SET quantity = quantity + @quantity, total = quantity * price WHERE referenceno = @referenceno AND pcode = @pcode", cn);
                    cm.Parameters.AddWithValue("@referenceno", txtReferenceNo.Text);
                    cm.Parameters.AddWithValue("@pcode", txtCode.Text);
                    cm.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                    cm.Parameters.AddWithValue("@price", Convert.ToDecimal(txtBuyingPrice.Text));                   
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity + @quantity, price = @price WHERE code = @code", cn);
                    cm.Parameters.AddWithValue("@code", txtCode.Text);
                    cm.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                    cm.Parameters.AddWithValue("@price", txtSellingPrice.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    loadTrolley();
                    getItemsInTrolley();
                    getTotal();
                    txtBuyingPrice.Text = "0.00";
                    txtSellingPrice.Text = "0.00";
                    txtQuantity.Text = "0";
                }
                else
                {
                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("INSERT INTO tblstockin(referenceno,stockby,date,pcode,pname,price,quantity,total) VALUES(@referenceno,@stockby,@date,@pcode,@pname,@price,@quantity,@total)", cn);
                    cm.Parameters.AddWithValue("@referenceno", txtReferenceNo.Text);
                    cm.Parameters.AddWithValue("@stockby", txtStockInBy.Text);
                    cm.Parameters.AddWithValue("@date", dateStock.Value);
                    cm.Parameters.AddWithValue("@pcode", txtCode.Text);
                    cm.Parameters.AddWithValue("@pname", txtName.Text);
                    cm.Parameters.AddWithValue("@price", Convert.ToDecimal(txtBuyingPrice.Text));
                    cm.Parameters.AddWithValue("@quantity", Convert.ToInt32(txtQuantity.Text));
                    cm.Parameters.AddWithValue("@total", Convert.ToDecimal(txtBuyingPrice.Text) * Convert.ToInt32(txtQuantity.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity + @quantity, price = @price WHERE code = @code", cn);
                    cm.Parameters.AddWithValue("@code", txtCode.Text);
                    cm.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                    cm.Parameters.AddWithValue("@price", txtSellingPrice.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    loadTrolley();
                    getItemsInTrolley();
                    getTotal();
                    txtBuyingPrice.Text = "0.00";
                    txtSellingPrice.Text = "0.00";
                    txtQuantity.Text = "0";
                }
            }

        }

        private void txtBuyingPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtSellingPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtAmountPaid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void cboSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblsupplier WHERE name = @name", cn);
            cm.Parameters.AddWithValue("@name", cboSupplier.Text);
            dr = cm.ExecuteReader();
            dr.Read();
            if(dr.HasRows)
            {
                txtContact.Text = dr["contactperson"].ToString();
                txtAddress.Text = dr["address"].ToString();
                checkstatus = Convert.ToBoolean(dr["status"].ToString());

                if (checkstatus == false)
                {
                    cn.Close();
                    MessageBox.Show("The selected supplier status is In-Active!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {

                }
            }
            dr.Close();
            cn.Close();
        }

        private void frmStockEntry_Load(object sender, EventArgs e)
        {
            lblItemInTrolley.Text = "0";
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView2.Columns[e.ColumnIndex].Name;
            pcode = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
            string qty = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();


            if (MessageBox.Show("Remove Product from Trolley", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (ColName == "ColDelete")
                {
                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity - @quantity WHERE code = @code", cn);
                    cm.Parameters.AddWithValue("@code", pcode);
                    cm.Parameters.AddWithValue("@quantity", qty);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("DELETE FROM tblstockin WHERE referenceno = @referenceno AND pcode = @pcode", cn);
                    cm.Parameters.AddWithValue("@referenceno", txtReferenceNo.Text);
                    cm.Parameters.AddWithValue("@pcode", pcode);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    loadTrolley();
                    getTotal();
                    getItemsInTrolley();
                    txtBuyingPrice.Text = "0.00";
                    txtSellingPrice.Text = "0.00";
                    txtQuantity.Text = "0";
                }
            }
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (checkstatus == false)
            {
                cn.Close();
                MessageBox.Show("The selected supplier status is In-Active!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataGridView2.Rows.Count < 1)
            {
                MessageBox.Show("You haven't selected any product yet!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cboSupplier.Text == "")
            {
                cboSupplier.Focus();
                MessageBox.Show("Please select a valid supplier!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtReferenceNo.Text == "")
            {
                txtReferenceNo.Focus();
                MessageBox.Show("Reference No Field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtStockInBy.Text == "")
            {
                txtStockInBy.Focus();
                MessageBox.Show("Stock By Field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0.00" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Focus();
                MessageBox.Show("Please enter an amount to Pay!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Convert.ToDecimal(txtAmountPaid.Text) < Convert.ToDecimal(lblTotal.Text))
            {
                MessageBox.Show("Can't proceed with transaction, please enter a valid amount", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Save Transaction", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                decimal change = Convert.ToDecimal(lblChange.Text);
                change = Convert.ToDecimal(txtAmountPaid.Text) - Convert.ToDecimal(lblTotal.Text);
                lblChange.Text = change.ToString();


                cn.Open();
                cm = new MySqlCommand("INSERT INTO tblstockinpayment(referenceno,stockby,date,supplier,contact,address,total,schange,amountpaid) VALUES(@referenceno,@stockby,@date,@supplier,@contact,@address,@total,@schange,@amountpaid)", cn);
                cm.Parameters.AddWithValue("@referenceno", txtReferenceNo.Text);
                cm.Parameters.AddWithValue("@stockby", txtStockInBy.Text);
                cm.Parameters.AddWithValue("@date", dateStock.Text);
                cm.Parameters.AddWithValue("@supplier", cboSupplier.Text);
                cm.Parameters.AddWithValue("@contact", txtContact.Text);
                cm.Parameters.AddWithValue("@address", txtAddress.Text);
                cm.Parameters.AddWithValue("@total", lblTotal.Text);
                cm.Parameters.AddWithValue("@schange", lblChange.Text);
                cm.Parameters.AddWithValue("@amountpaid", txtAmountPaid.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Transaction has been saved successfully!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                getReferenceID();
                loadTrolley();
                lblItemInTrolley.Text = "0";
                lblChange.Text = "0.00";
                lblTotal.Text = "0.00";
                txtAmountPaid.Text = "0.00";
                txtBuyingPrice.Text = "0.00";
                txtSellingPrice.Text = "0.00";
                txtQuantity.Text = "0";
                txtAddress.Clear();
                txtContact.Clear();
                cboSupplier.Text = "";
            }
        }
    }
}
