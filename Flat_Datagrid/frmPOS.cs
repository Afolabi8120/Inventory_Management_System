using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Flat_Datagrid
{
    public partial class frmPOS : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public string pcode, pname, price, quantity, total, stock;

        bool status, found;

        public frmPOS()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var time = DateTime.Now;

            lblDate.Text = time.Date.ToLongDateString();
            lblTime.Text = time.ToLongTimeString();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;

            //this to get date and time
            var time = DateTime.Now;

            lblDate.Text = time.Date.ToLongDateString();
            lblTime.Text = time.ToLongTimeString();

            txtBarcode.Focus();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            var f1 = new frmSearchProduct();
            f1.LoadRecord();
            f1.ShowDialog();
        }

        //this to get total numbers of product in trolley 
        private void getTrolleyCount()
        {            
            int num = dataGridView1.Rows.Count;
            lblCart.Text = "Total Products in Trolley [ " + num.ToString() + " ]";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 1)
            {
                MessageBox.Show("Please remove all product from trolley before logging out!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (MessageBox.Show("You are about to logout?, Click yes to confirm!", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var f1 = new frmLogin();
                    f1.Show();
                    this.Hide();
                }
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            var f1 = new frmChangePassword();
            f1.getPassword();
            f1.ShowDialog();
        }

        public void getTotal()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(total) FROM tblcart WHERE invoiceno = @invoiceno", cn);
            cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            //this is use to get the naira sign
            //char x;
            //x = (char)8358;
            //var mynum = x + " " + num.ToString();
            lblSubTotal.Text = num.ToString();
        }

        public void LoadCart()
        {
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcart WHERE invoiceno = @invoiceno ORDER BY name ASC", cn);
            cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr["code"].ToString(), dr["name"].ToString(), dr["price"].ToString(), dr["quantity"].ToString(), dr["total"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        public void getInvoiceNo()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(DISTINCT(invoiceno)) FROM tblcart", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            DateTime time = DateTime.Now;

            Random myrand = new Random();
            int rand = myrand.Next(10, 1000);

            lblInvoiceNo.Text = "INV" + time.Year + time.Month + time.Day + num + rand;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            string code = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string qty = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

            if (ColName == "ColIncrease") //this will increase product quantity by 1
            {
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblproduct WHERE code = @code AND quantity > 0", cn);
                cm.Parameters.AddWithValue("@code", code);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    stock = dr["quantity"].ToString();

                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblcart SET quantity = quantity + @quantity WHERE invoiceno =@invoiceno AND code = @code", cn);
                    cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                    cm.Parameters.AddWithValue("@code", code);
                    cm.Parameters.AddWithValue("@quantity", 1);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblcart SET total = quantity * price WHERE invoiceno = @invoiceno AND code = @code", cn);
                    cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                    cm.Parameters.AddWithValue("@code", code);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity - @quantity WHERE code = @code", cn);
                    cm.Parameters.AddWithValue("@code", code);
                    cm.Parameters.AddWithValue("@quantity", 1);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    LoadCart();
                    getTotal();
                    getTrolleyCount();
                    txtBarcode.Focus();
                }
                else
                {
                    cn.Close();
                    MessageBox.Show("The selected product is out of stock!\nAvailable Stock: 0", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                dr.Close();
                cn.Close();               
            }
            else if (ColName == "ColDecrease") //this will decrease product quantity by 1
            {
                if (qty == "0")
                {
                    MessageBox.Show("The selected product is 0\nPlease remove product from trolley!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblcart SET quantity = quantity - @quantity WHERE invoiceno =@invoiceno AND code = @code", cn);
                    cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                    cm.Parameters.AddWithValue("@code", code);
                    cm.Parameters.AddWithValue("@quantity", 1);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblcart SET total = quantity * price WHERE invoiceno = @invoiceno AND code = @code", cn);
                    cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                    cm.Parameters.AddWithValue("@code", code);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity + @quantity WHERE code = @code", cn);
                    cm.Parameters.AddWithValue("@code", code);
                    cm.Parameters.AddWithValue("@quantity", 1);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    LoadCart();
                    getTotal();
                    getTrolleyCount();
                    txtBarcode.Focus();
                }
            }
            else if (ColName == "ColDelete") //this will delete the selected user details
            {
                if (MessageBox.Show("Remove Product", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new MySqlCommand("DELETE FROM tblcart WHERE invoiceno = @invoiceno AND code = @code", cn);
                    cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                    cm.Parameters.AddWithValue("@code", code);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity + @quantity WHERE code = @code", cn);
                    cm.Parameters.AddWithValue("@code", code);
                    cm.Parameters.AddWithValue("@quantity", qty);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    LoadCart();
                    getTotal();
                    getTrolleyCount();
                    txtBarcode.Focus();

                    if (dataGridView1.Rows.Count == 0)
                    {
                        lblSubTotal.Text = "0.00";
                    }
                }
            }
        }

        private void btnSettle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Trolley cannot be empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtCustomerName.Text == "" || txtCustomerName.Text == "Enter Customer Name here...")
            {
                txtCustomerName.Focus();
                MessageBox.Show("Please enter a valid customer name", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cboPaymentMode.Text == "")
            {
                cboPaymentMode.Focus();
                MessageBox.Show("Please select a valid Payment Mode", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtAmountPaid.Text == "")
            {
                txtAmountPaid.Focus();
                MessageBox.Show("Please enter an amount to pay", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Convert.ToDecimal(txtAmountPaid.Text) < Convert.ToDecimal(lblSubTotal.Text))
            {
                MessageBox.Show("Insufficient Amount Paid cannot proceed with Transaction", "INSUFFICIENT FUND", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                decimal change = Convert.ToDecimal(txtAmountPaid.Text) - Convert.ToDecimal(lblSubTotal.Text);
                lblChange.Text = change.ToString();
            }

            if (MessageBox.Show("Make Payment? Click yes to proceed!\nNOTE: This transaction cannot be reversed!", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                cm = new MySqlCommand("INSERT INTO tblcartpayment(invoiceno,customer,total,amountpaid,cchange,paymode,date,time,status,user) VALUES(@invoiceno,@customer,@total,@amountpaid,@cchange,@paymode,@date,@time,@status,@user)", cn);
                cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                cm.Parameters.AddWithValue("@customer", txtCustomerName.Text);
                cm.Parameters.AddWithValue("@total", lblSubTotal.Text);
                cm.Parameters.AddWithValue("@amountpaid", txtAmountPaid.Text);
                cm.Parameters.AddWithValue("@cchange", lblChange.Text);
                cm.Parameters.AddWithValue("@paymode", cboPaymentMode.Text);
                cm.Parameters.AddWithValue("@date", DateTime.Now.ToShortDateString());
                cm.Parameters.AddWithValue("@time", DateTime.Now.ToShortTimeString());
                cm.Parameters.AddWithValue("@status", "APPROVED");
                cm.Parameters.AddWithValue("@user", frmLogin.username.ToUpper());
                cm.ExecuteNonQuery();
                cn.Close();

                cn.Open();
                cm = new MySqlCommand("UPDATE tblcart SET status = @status WHERE invoiceno = @invoiceno", cn);
                cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                cm.Parameters.AddWithValue("@status", "APPROVED");
                cm.ExecuteNonQuery();
                cn.Close();

                MessageBox.Show("Transaction with Invoice No: " + lblInvoiceNo.Text + " has been saved successfully!", "TRANSACTION SUCCESSFUL", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (MessageBox.Show("Print Receipt? Click yes to proceed!\nNOTE: This may take some time", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var f1 = new frmPOSReceipt();
                    f1.invoiceno = lblInvoiceNo.Text;
                    f1.LoadHeader();
                    f1.LoadRecord();
                    f1.ShowDialog();
                }
                
                getInvoiceNo();
                LoadCart();
                getTrolleyCount();
                txtAmountPaid.Text = "0";
                lblSubTotal.Text = "0.00";
                lblChange.Text = "0.00";
                txtCustomerName.Text = "Enter Customer Name here...";                
            }

        }

        private void cboPaymentMode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtAmountPaid.Text = "0";
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

        private void btnDoubleZero_Click(object sender, EventArgs e)
        {
            txtAmountPaid.Text = txtAmountPaid.Text + btnDoubleZero.Text;
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            txtAmountPaid.Text = txtAmountPaid.Text + btnDot.Text;
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtAmountPaid.Text = txtAmountPaid.Text + btn0.Text;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Text = btn1.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn1.Text;
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Text = btn2.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn2.Text;
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Text = btn3.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn3.Text;
            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Text = btn4.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn4.Text;
            }
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Text = btn5.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn5.Text;
            }
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Text = btn6.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn6.Text;
            }
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Text = btn7.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn7.Text;
            }
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Text = btn8.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn8.Text;
            }
        }

        //SELECT c.*, p.customer,p.total,p.amountpaid,p.cchange,p.paymode,p.date,p.time,p.user FROM tblcart AS c INNER JOIN tblcartpayment AS p ON c.invoiceno = p.invoiceno
        private void btn9_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Text = btn9.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn9.Text;
            }
        }

        private void btn1000_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Text = btn1000.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn1000.Text;
            }
        }

        private void btn1500_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Text = btn1500.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn1500.Text;
            }
        }

        private void btn2000_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Text = btn2000.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn2000.Text;
            }
        }

        private void txtAmountPaid_TextChanged(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.TextLength < 1)
            {
                var change = Convert.ToDecimal(txtAmountPaid.Text) - Convert.ToDecimal(lblSubTotal.Text);
                lblChange.Text = change.ToString();
            }
            else
            {
                var change = Convert.ToDecimal(txtAmountPaid.Text) - Convert.ToDecimal(lblSubTotal.Text);
                lblChange.Text = change.ToString();
            }
        }

        private void btnSalesRecord_Click(object sender, EventArgs e)
        {
            var f1 = new frmSalesRecord();
            f1.getRecord();
            f1.getTotalSales();
            f1.ShowDialog();
        }

        private void txtBarcode_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            //only allows numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                insertToCart();
            }

            
        }

        private void insertToCart()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct WHERE barcode = @barcode", cn);
            cm.Parameters.AddWithValue("@barcode", txtBarcode.Text + "");
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                pcode = dr["code"].ToString();
                pname = dr["name"].ToString();
                quantity = dr["quantity"].ToString();
                price = dr["price"].ToString();
                status = Convert.ToBoolean(dr["status"].ToString());

                cn.Close();
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblproduct WHERE barcode = @barcode AND status = @status", cn);
                cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                cm.Parameters.AddWithValue("@status", true);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {

                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("SELECT * FROM tblproduct WHERE barcode = @barcode AND quantity > 0", cn);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        found = true;
                    }
                    else
                    {
                        cn.Close();
                        MessageBox.Show("Product with Barcode: " + txtBarcode.Text + " is low on stock!\nRemaining Stock: " + quantity, "STOCK IS ZERO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtBarcode.Text = "";
                        txtBarcode.Focus();
                        return;
                    }
                    dr.Close();
                    cn.Close();

                }
                else
                {
                    cn.Close();
                    MessageBox.Show("Product with Barcode: " + txtBarcode.Text + " is In-Active!", "STATUS IS INACTIVE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                    return;
                }
                dr.Close();
                cn.Close();

            }
            else
            {
                found = false;
            }
            dr.Close();
            cn.Close();

            if (found == true)
            {
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblcart WHERE invoiceno = @invoiceno AND code = @code", cn);
                cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                cm.Parameters.AddWithValue("@code", pcode);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblcart SET quantity = quantity + @quantity, total = @quantity * @price WHERE invoiceno = @invoiceno AND code = @code", cn);
                    cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                    cm.Parameters.AddWithValue("@code", pcode);
                    cm.Parameters.AddWithValue("@price", price);
                    cm.Parameters.AddWithValue("@quantity", 1);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblcart SET total = quantity * price WHERE invoiceno = @invoiceno AND code = @code", cn);
                    cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                    cm.Parameters.AddWithValue("@code", pcode);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity - @quantity WHERE code = @code", cn);
                    cm.Parameters.AddWithValue("@code", pcode);
                    cm.Parameters.AddWithValue("@quantity", 1);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    LoadCart();
                    getTotal();
                    getTrolleyCount();
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                }
                else
                {
                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("INSERT INTO tblcart(invoiceno,code,name,price,quantity,total,user,status) VALUES(@invoiceno,@code,@name,@price,@quantity,@total,@user,@status)", cn);
                    cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                    cm.Parameters.AddWithValue("@code", pcode);
                    cm.Parameters.AddWithValue("@name", pname);
                    cm.Parameters.AddWithValue("@price", price);
                    cm.Parameters.AddWithValue("@quantity", 1);
                    cm.Parameters.AddWithValue("@total", Convert.ToDecimal(price) * 1);
                    cm.Parameters.AddWithValue("@user", frmLogin.username);
                    cm.Parameters.AddWithValue("@status", "PENDING");
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity - @quantity WHERE code = @code", cn);
                    cm.Parameters.AddWithValue("@code", pcode);
                    cm.Parameters.AddWithValue("@quantity", 1);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    LoadCart();
                    getTotal();
                    getTrolleyCount();
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                }
                dr.Close();
                cn.Close();
            }
            //else if(found == false && txtBarcode.TextLength < 13)
            else if (found == false)
            {
                cn.Close();
                MessageBox.Show("Product Barcode does not exist", "BARCODE NOT FOUND", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnBackUp_Click(object sender, EventArgs e)
        {
            try
            {
                Process p = null;
                if (p == null)
                {
                    p = new Process();
                    p.StartInfo.FileName = "Calc.exe";
                    p.Start();
                }
                else
                {
                    p.Close();
                    p.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex.Message);
            }
        }

        private void btnReprintReceipt_Click(object sender, EventArgs e)
        {
            var f1 = new frmReprintReceipt();
            f1.Reprint();
            f1.ShowDialog();
        }

        private void frmPOS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                btnChangePassword.PerformClick();
            }
            else if (e.KeyCode == Keys.F2)
            {
                btnCalculator.PerformClick();
            }
            else if (e.KeyCode == Keys.F3)
            {
                btnProduct.PerformClick();
            }
            else if (e.KeyCode == Keys.F4)
            {
                btnSalesRecord.PerformClick();
            }
            else if (e.KeyCode == Keys.F5)
            {
                btnPay.PerformClick();
            }
            else if (e.KeyCode == Keys.F6)
            {
                btnReprintReceipt.PerformClick();
            }
            else if (e.KeyCode == Keys.F7)
            {
                btnLogout.PerformClick();
            }
            else if (e.KeyCode == Keys.F11)
            {
                txtBarcode.Focus();
            }
        }
      
    }
}
