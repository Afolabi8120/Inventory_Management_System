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
    public partial class frmPayment : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public frmPayment()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //get all records of payment made to suppliers/vendor
        public void getSupplierPayment()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblstockinpayment ORDER BY date ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["referenceno"].ToString(), dr["supplier"].ToString(), dr["contact"].ToString(), dr["address"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["schange"].ToString(), dr["date"].ToString(), dr["stockby"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        //get all records of payment customer made when buying goods
        public void getCustomerPayment()
        {
            int i = 0;
            dataGridView2.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcartpayment ORDER BY customer ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr["invoiceno"].ToString(), dr["customer"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["cchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["status"].ToString(), dr["user"].ToString());
            }
            dr.Close();
            cn.Close();     
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView2.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcartpayment WHERE date BETWEEN '" + dateFromCustomer.Text + "' AND '" + dateToCustomer.Text + "' ORDER BY customer ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr["invoiceno"].ToString(), dr["customer"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["cchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["status"].ToString(), dr["user"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtSearchCustomer_OnValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView2.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcartpayment WHERE invoiceno = @invoiceno ORDER BY customer ASC", cn);
            cm.Parameters.AddWithValue("@invoiceno", txtSearchCustomer.Text);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr["invoiceno"].ToString(), dr["customer"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["cchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["status"].ToString(), dr["user"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnRefreshCustomer_Click(object sender, EventArgs e)
        {
            getCustomerPayment();
        }

        private void btnRefreshSupplier_Click(object sender, EventArgs e)
        {
            getSupplierPayment();
        }

        private void btnSearchSupplier_Click(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblstockinpayment WHERE date BETWEEN '" + dateFromSupplier.Text + "' AND '" + dateToSupplier.Text + "' ORDER BY date ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["referenceno"].ToString(), dr["supplier"].ToString(), dr["contact"].ToString(), dr["address"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["schange"].ToString(), dr["date"].ToString(), dr["stockby"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtSearchSupplier_OnValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblstockinpayment WHERE referenceno = @referenceno ORDER BY date ASC", cn);
            cm.Parameters.AddWithValue("@referenceno", txtSearchSupplier.Text);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["referenceno"].ToString(), dr["supplier"].ToString(), dr["contact"].ToString(), dr["address"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["schange"].ToString(), dr["date"].ToString(), dr["stockby"].ToString());
            }
            dr.Close();
            cn.Close();
        }

    }
}
