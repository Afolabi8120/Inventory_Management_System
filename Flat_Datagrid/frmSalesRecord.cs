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
    public partial class frmSalesRecord : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public static string date1, date2;

        char x = (char)8358;

        public frmSalesRecord()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void getTotalSalesBySearch()
        {
            if (frmLogin.usertype == "Cashier")
            {
                cn.Open();
                cm = new MySqlCommand("SELECT SUM(total) FROM tblcartpayment WHERE invoiceno = @invoiceno ORDER BY customer ASC", cn);
                cm.Parameters.AddWithValue("@invoiceno", txtSearch.Text);
                string num = cm.ExecuteScalar().ToString();
                cn.Close();

                lblSales.Text = x + " " + num.ToString();
            }
            else if (frmLogin.usertype == "Administrator")
            {
                cn.Open();
                cm = new MySqlCommand("SELECT SUM(total) FROM tblcartpayment WHERE invoiceno = @invoiceno ORDER BY customer ASC", cn);
                cm.Parameters.AddWithValue("@invoiceno", txtSearch.Text);
                string num = cm.ExecuteScalar().ToString();
                cn.Close();

                lblSales.Text = x + " " + num.ToString();
            }

        }

        public void getTotalSales()
        {
            if (frmLogin.usertype == "Cashier")
            {
                cn.Open();
                cm = new MySqlCommand("SELECT SUM(total) FROM tblcartpayment WHERE user = @user ORDER BY customer ASC", cn);
                cm.Parameters.AddWithValue("@user", frmLogin.username.ToUpper());
                string num = cm.ExecuteScalar().ToString();
                cn.Close();

                lblSales.Text = x + " " + num.ToString();
            }
            else if (frmLogin.usertype == "Administrator")
            {
                cn.Open();
                cm = new MySqlCommand("SELECT SUM(total) FROM tblcartpayment ORDER BY customer ASC", cn);
                cm.Parameters.AddWithValue("@user", frmLogin.username.ToUpper());
                string num = cm.ExecuteScalar().ToString();
                cn.Close();

                lblSales.Text = x + " " + num.ToString();
            }

        }

        public void getTotalSalesByDate()
        {
            if (frmLogin.usertype == "Cashier")
            {
                cn.Open();
                cm = new MySqlCommand("SELECT SUM(total) FROM tblcartpayment WHERE date BETWEEN '" + dateFrom.Text + "' AND '" + dateTo.Text + "' AND user = @user ORDER BY customer ASC", cn);
                cm.Parameters.AddWithValue("@user", frmLogin.username.ToUpper());
                string num = cm.ExecuteScalar().ToString();
                cn.Close();

                lblSales.Text = x + " " + num.ToString();
            }
            else if (frmLogin.usertype == "Administrator")
            {
                cn.Open();
                cm = new MySqlCommand("SELECT SUM(total) FROM tblcartpayment WHERE date BETWEEN '" + dateFrom.Text + "' AND '" + dateTo.Text + "' ORDER BY customer ASC", cn);
                cm.Parameters.AddWithValue("@user", frmLogin.username.ToUpper());
                string num = cm.ExecuteScalar().ToString();
                cn.Close();

                lblSales.Text = x + " " + num.ToString();
            }

        }

        public void getRecord()
        {
            if (frmLogin.usertype == "Cashier")
            {
                int i = 0;
                dataGridView1.Rows.Clear();
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblcartpayment WHERE user = @user ORDER BY customer ASC", cn);
                cm.Parameters.AddWithValue("@user", frmLogin.username.ToUpper());
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(i, dr["invoiceno"].ToString(), dr["customer"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["cchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["status"].ToString(), dr["user"].ToString());
                }
                dr.Close();
                cn.Close();
                getTotalSales();
            }
            else if (frmLogin.usertype == "Administrator")
            {
                int i = 0;
                dataGridView1.Rows.Clear();
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblcartpayment ORDER BY customer ASC", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(i, dr["invoiceno"].ToString(), dr["customer"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["cchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["status"].ToString(), dr["user"].ToString());
                }
                dr.Close();
                cn.Close();
                getTotalSales();
            }           
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (frmLogin.usertype == "Cashier")
            {
                int i = 0;
                dataGridView1.Rows.Clear();
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblcartpayment WHERE date BETWEEN '" + dateFrom.Text + "' AND '" + dateTo.Text + "' AND user = @user ORDER BY customer ASC", cn);
                cm.Parameters.AddWithValue("@user", frmLogin.username.ToUpper());
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(i, dr["invoiceno"].ToString(), dr["customer"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["cchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["status"].ToString(), dr["user"].ToString());
                }
                dr.Close();
                cn.Close();
                getTotalSalesByDate();
            }
            else if(frmLogin.usertype == "Administrator")
            {
                int i = 0;
                dataGridView1.Rows.Clear();
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblcartpayment WHERE date BETWEEN '" + dateFrom.Text + "' AND '" + dateTo.Text + "' ORDER BY customer ASC", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(i, dr["invoiceno"].ToString(), dr["customer"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["cchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["status"].ToString(), dr["user"].ToString());
                }
                dr.Close();
                cn.Close();
                getTotalSalesByDate();
            }
        }

        private void txtSearch_OnValueChanged(object sender, EventArgs e)
        {
            if (frmLogin.usertype == "Cashier")
            {
                int i = 0;
                dataGridView1.Rows.Clear();
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblcartpayment WHERE invoiceno = @invoiceno ORDER BY customer ASC", cn);
                cm.Parameters.AddWithValue("@invoiceno", txtSearch.Text);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(i, dr["invoiceno"].ToString(), dr["customer"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["cchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["status"].ToString(), dr["user"].ToString());
                }
                dr.Close();
                cn.Close();

                getTotalSalesBySearch();
            }
            else if (frmLogin.usertype == "Administrator")
            {
                int i = 0;
                dataGridView1.Rows.Clear();
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblcartpayment WHERE invoiceno = @invoiceno ORDER BY customer ASC", cn);
                cm.Parameters.AddWithValue("@invoiceno", txtSearch.Text);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(i, dr["invoiceno"].ToString(), dr["customer"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["cchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["status"].ToString(), dr["user"].ToString());
                }
                dr.Close();
                cn.Close();

                getTotalSalesBySearch();
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                app.Visible = true;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "Records";

                try
                {
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            if (dataGridView1.Rows[i].Cells[j].Value != null)
                            {
                                worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[i + 2, j + 1] = "";
                            }
                        }
                    }

                    //Getting the location and file name of the excel to save from user. 
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    saveDialog.FilterIndex = 2;

                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        workbook.SaveAs(saveDialog.FileName);
                        MessageBox.Show("Record has been exported successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                finally
                {
                    app.Quit();
                    workbook = null;
                    worksheet = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        
        //SELECT c.*,p.customer,p.total AS total2,p.amountpaid,p.cchange,p.paymode,p.date,p.time FROM tblcart AS c INNER JOIN tblcartpayment AS p ON c.invoiceno = p.invoiceno 

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            getRecord();
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            date1 = dateFrom.Text;
            date2 = dateTo.Text;
            var f1 = new frmSalesReport();
            f1.date1 = date1;
            f1.date2 = date2;
            f1.LoadHeader();
            f1.LoadRecord();
            f1.ShowDialog();
        }
    }
}
