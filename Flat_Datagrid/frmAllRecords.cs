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
    public partial class frmAllRecords : Form
    {

        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public frmAllRecords()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        //get all records of stock history
        public void getStockHistory()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT s.referenceno, p.supplier, p.contact, s.pcode, s.pname, s.quantity, s.price, s.total, s.date, s.stockby FROM tblstockin as s INNER JOIN tblstockinpayment as p on s.referenceno = p.referenceno", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["referenceno"].ToString(), dr["supplier"].ToString(), dr["contact"].ToString(), dr["pcode"].ToString(), dr["pname"].ToString(), dr["quantity"].ToString(), dr["price"].ToString(), dr["total"].ToString(), dr["date"].ToString(), dr["stockby"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        //get all the records of sold products
        public void getSoldProduct()
        {
            int i = 0;
            dataGridView2.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT c.*, p.date, p.time FROM tblcart AS c INNER JOIN tblcartpayment AS p on c.invoiceno = p.invoiceno ", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr["invoiceno"].ToString(), dr["code"].ToString(), dr["name"].ToString(), dr["price"].ToString(), dr["quantity"].ToString(), dr["total"].ToString(), dr["user"].ToString(), dr["status"].ToString(), dr["date"].ToString(), dr["time"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        //load all products
        public void LoadRecord()
        {
            int i = 0;
            dataGridView3.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct ORDER BY name ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView3.Rows.Add(i, dr["code"].ToString(), dr["name"].ToString(), dr["barcode"].ToString(), dr["category"].ToString(), dr["description"].ToString(), dr["quantity"].ToString(), dr["price"].ToString(), Convert.ToBoolean(dr["status"]));
            }
            dr.Close();
            cn.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtSearch_OnValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT s.referenceno, p.supplier, p.contact, s.pcode, s.pname, s.quantity, s.price, s.total, s.date, s.stockby FROM tblstockin as s INNER JOIN tblstockinpayment as p on s.referenceno = p.referenceno WHERE s.referenceno LIKE '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["referenceno"].ToString(), dr["supplier"].ToString(), dr["contact"].ToString(), dr["pcode"].ToString(), dr["pname"].ToString(), dr["quantity"].ToString(), dr["price"].ToString(), dr["total"].ToString(), dr["date"].ToString(), dr["stockby"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT s.referenceno, p.supplier, p.contact, s.pcode, s.pname, s.quantity, s.price, s.total, s.date, s.stockby FROM tblstockin as s INNER JOIN tblstockinpayment as p on s.referenceno = p.referenceno WHERE s.date BETWEEN '" + dateFrom.Text + "' AND '" + dateTo.Text + "'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["referenceno"].ToString(), dr["supplier"].ToString(), dr["contact"].ToString(), dr["pcode"].ToString(), dr["pname"].ToString(), dr["quantity"].ToString(), dr["price"].ToString(), dr["total"].ToString(), dr["date"].ToString(), dr["stockby"].ToString());
            }
            dr.Close();
            cn.Close();
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

        private void button1_Click(object sender, EventArgs e)
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
                    for (int i = 0; i < dataGridView3.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1] = dataGridView3.Columns[i].HeaderText;
                    }
                    for (int i = 0; i < dataGridView3.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView3.Columns.Count; j++)
                        {
                            if (dataGridView3.Rows[i].Cells[j].Value != null)
                            {
                                worksheet.Cells[i + 2, j + 1] = dataGridView3.Rows[i].Cells[j].Value.ToString();
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

        private void txtSearch3_OnValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView3.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct WHERE name LIKE '" + txtSearch3.Text + "%' OR code LIKE '" + txtSearch3.Text + "%' OR category LIKE '" + txtSearch3.Text + "%' ORDER BY name ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView3.Rows.Add(i, dr["code"].ToString(), dr["name"].ToString(), dr["barcode"].ToString(), dr["category"].ToString(), dr["description"].ToString(), dr["quantity"].ToString(), dr["price"].ToString(), Convert.ToBoolean(dr["status"]));
            }
            dr.Close();
            cn.Close();
        }

        private void btnSearchSoldProduct_Click(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView2.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT c.*, p.date, p.time FROM tblcart AS c INNER JOIN tblcartpayment AS p on c.invoiceno = p.invoiceno WHERE date BETWEEN '" + dateSoldProductFrom.Text + "' AND '" + dateSoldProductTo.Text + "'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr["invoiceno"].ToString(), dr["code"].ToString(), dr["name"].ToString(), dr["price"].ToString(), dr["quantity"].ToString(), dr["total"].ToString(), dr["user"].ToString(), dr["status"].ToString(), dr["date"].ToString(), dr["time"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtSearchSoldProduct_OnValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView2.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT c.*, p.date, p.time FROM tblcart AS c INNER JOIN tblcartpayment AS p on c.invoiceno = p.invoiceno WHERE c.invoiceno = @invoiceno", cn);
            cm.Parameters.AddWithValue("@invoiceno", txtSearchSoldProduct.Text);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr["invoiceno"].ToString(), dr["code"].ToString(), dr["name"].ToString(), dr["price"].ToString(), dr["quantity"].ToString(), dr["total"].ToString(), dr["user"].ToString(), dr["status"].ToString(), dr["date"].ToString(), dr["time"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnRefreshSoldProduct_Click(object sender, EventArgs e)
        {
            getSoldProduct();
        }
    }
}
