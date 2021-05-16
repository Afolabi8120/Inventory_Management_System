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
    public partial class frmReprintReceipt : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public frmReprintReceipt()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Reprint()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcartpayment ORDER BY customer ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["invoiceno"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
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
                dataGridView1.Rows.Add(i, dr["invoiceno"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            string invoiceno = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            //this is use to get the name of the category name you want to edit 
            if (ColName == "ColReprint")
            {
                if (MessageBox.Show("Re-print Receipt? Click yes to proceed!\nNOTE: This may take some time", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var f1 = new frmPOSReceipt();
                    f1.invoiceno = invoiceno;
                    f1.LoadHeader();
                    f1.LoadRecord();
                    f1.ShowDialog();
                }
            }
        }
    }
}
