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
    public partial class frmDashboard : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public char x;

        public string active, inactive,allproduct;

        public frmDashboard()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            //this is use to get the naira sign           
            x = (char)8358;
            var num = x + " " + lblDailySales.Text;
            var num1 = x + " " + lblAllTimeSales.Text;

            lblDailySales.Text = num;
            lblAllTimeSales.Text = num1;

            getInactiveProduct();
            getActiveProduct();
            getAllUsers();
            getCategory();
            getProduct();
            getStockInHand();
            getSuppliers();
            LoadRecord();
            getActivePercent();

            var time = DateTime.Now;

            lblDate.Text = time.Date.ToLongDateString();
            lblTime.Text = time.ToLongTimeString();
        }

        //get the total numbers of categories
        public void getCategory()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(*) FROM tblcategory", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblCategories.Text = num;
        }

        //get the sum of daily sales
        public void getDailySales()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(total) FROM tblcartpayment WHERE date = @date", cn);
            cm.Parameters.AddWithValue("@date", DateTime.Now.ToShortDateString());
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            if (num == "" || num == "0")
            {
                lblDailySales.Text = x + " 0";
            }
            else
            {
                lblDailySales.Text = x + " " + num;
            }
        }

        //get the sum of all sales
        public void getAllTimeSales()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(total) FROM tblcartpayment", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblAllTimeSales.Text = x + " " + num;
        }

        //get the total numbers of suppliers
        public void getSuppliers()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(*) FROM tblsupplier", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblAllSuppliers.Text = num;
        }

        //get the total numbers of users
        public void getAllUsers()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(*) FROM tbluser", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblAllUsers.Text = num;
        }

        //get the total numbers of users
        public void getStockInHand()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(quantity) FROM tblproduct", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblStockInHand.Text = num;
        }

        //get the total numbers of products
        public void getProduct()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(*) FROM tblproduct", cn);
            allproduct = cm.ExecuteScalar().ToString();
            cn.Close();

            lblProduct.Text = allproduct;
        }

        //get the total numbers of inactive products
        public void getInactiveProduct()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(*) FROM tblproduct WHERE status = @status", cn);
            cm.Parameters.AddWithValue("@status", Convert.ToBoolean(0));
            inactive = cm.ExecuteScalar().ToString();
            cn.Close();

            lblInactiveProduct.Text = inactive;
        }

        //get the total numbers of active products
        public void getActiveProduct()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(*) FROM tblproduct WHERE status = @status", cn);
            cm.Parameters.AddWithValue("@status", Convert.ToBoolean(1));
            active = cm.ExecuteScalar().ToString();
            cn.Close();

            lblActiveProduct.Text = active;
        }

        //this will load all the record of all products into the datagrid
        public void LoadRecord()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["code"].ToString(), dr["name"].ToString(), dr["barcode"].ToString(), dr["category"].ToString(), dr["quantity"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            
        }

        public void getDailySalesByDate()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(total) FROM tblcartpayment WHERE date BETWEEN '" + dateFrom.Text + "' AND '" + dateTo.Text + "'", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblDailySales.Text = x + " " + num;
        }

        public void getAllTimeSalesByDate()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(total) FROM tblcartpayment WHERE date BETWEEN '" + dateFrom.Text + "' AND '" + dateTo.Text + "'", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblAllTimeSales.Text = x + " " + num;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            
        }

        public void getActivePercent()
        {
            int num = Convert.ToInt32(active);
            int num2 = Convert.ToInt32(inactive);
            int num3 = Convert.ToInt32(allproduct);

            int x = (num * 100) / num3;
            int y = (num2 * 100) / num3;

            cpActive.Value = x;
            cpInActive.Value = y;

        }

        private void btnNotepad_Click(object sender, EventArgs e)
        {
            
        }

        private void btnCalculator_Click(object sender, EventArgs e)
        {
           
        }

        private void btnNotepad_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe");
        }

        private void btnCalculator_Click_1(object sender, EventArgs e)
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

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
            getDailySalesByDate();
            getAllTimeSalesByDate();
        }

        private void btnReset_Click_1(object sender, EventArgs e)
        {
            getAllTimeSales();
            getDailySales();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var time = DateTime.Now;

            lblDate.Text = time.Date.ToLongDateString();
            lblTime.Text = time.ToLongTimeString();

            getInactiveProduct();
            getActiveProduct();
            getAllUsers();
            getCategory();
            getProduct();
            getStockInHand();
            getSuppliers();
            getAllTimeSales();
            getDailySales();
            getActivePercent();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToInt32(row.Cells["Column7"].Value) < 1)
                {
                    row.Cells["Column7"].Style.BackColor = Color.Maroon;
                }
                else if (Convert.ToInt32(row.Cells["Column7"].Value) < 50)
                {
                    row.Cells["Column7"].Style.BackColor = Color.Chartreuse;
                }
                else if (Convert.ToInt32(row.Cells["Column7"].Value) > 50)
                {
                    row.Cells["Column7"].Style.BackColor = Color.DarkGreen;
                }
            }
        }

        private void btnBackUp_Click(object sender, EventArgs e)
        {
            string constring = "server=localhost;username=root;password=;database=ui_inventory;";
            string file = "C:ui_inventory.sql";
            using (MySqlConnection cn = new MySqlConnection(constring))
            {
                using (MySqlCommand cm = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cm))
                    {
                        cm.Connection = cn;
                        cn.Open();
                        mb.ExportToFile(file);
                        cn.Close();
                        MessageBox.Show("Database Backup Completed...", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            string constring = "server=localhost;username=root;password=;database=ui_inventory;";
            string file = "C:ui_inventory.sql";
            using (MySqlConnection cn = new MySqlConnection(constring))
            {
                using (MySqlCommand cm = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cm))
                    {
                        cm.Connection = cn;
                        cn.Open();
                        mb.ImportFromFile(file);
                        cn.Close();
                        MessageBox.Show("Database Restore Completed...", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}
