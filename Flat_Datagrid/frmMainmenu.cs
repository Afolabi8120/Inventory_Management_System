using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Flat_Datagrid
{
    public partial class frmMainmenu : Form
    {
        public frmMainmenu()
        {
            InitializeComponent();
        }

        private void frmMainmenu_Load(object sender, EventArgs e)
        {
            var f1 = new frmDashboard();
            //panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.lblName.Text = frmLogin.username;
            f1.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("You are about to logout?, Click yes to confirm!", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var f1 = new frmLogin();
                f1.Show();
                this.Hide();
            }
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            
        }

        private void btnUsermanagement_Click(object sender, EventArgs e)
        {
            
        }

        private void btnHome_Click(object sender, EventArgs e)
        {            
            
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSystemSettings_Click(object sender, EventArgs e)
        {
            
        }

        private void bnProduct_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var f1 = new frmSystemSettings();
            panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.LoadRecord();
            f1.Show();
        }

        private void btnStockEntry_Click(object sender, EventArgs e)
        {
            
        }

        private void btnStockAdjustment_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSalesRecord_Click(object sender, EventArgs e)
        {
            
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            var f1 = new frmPOS();
            f1.ShowDialog();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            
        }

        private void btnReprintReceipt_Click(object sender, EventArgs e)
        {
            
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            
        }

        private void btnCategory_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmCategory();
            panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.LoadRecord();
            f1.btnUpdate.Enabled = false;
            f1.Show();
        }

        private void btnUsermanagement_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmUserManagement();
            panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.LoadRecord();
            f1.btnUpdate.Enabled = false;
            f1.Show();
        }

        private void btnHome_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmDashboard();
            panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.lblName.Text = frmLogin.username;
            f1.LoadRecord();
            f1.Show();
        }

        private void btnSupplier_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmSupplier();
            panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.btnUpdate.Enabled = false;
            f1.LoadRecord();
            f1.Show();
        }

        private void btnSystemSettings_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmSystemSettings();
            panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.LoadRecord();
            f1.Show();
        }

        private void bnProduct_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmProduct();
            panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.btnUpdate.Enabled = false;
            f1.getID();
            f1.getCategory();
            f1.LoadRecord();
            f1.Show();
        }

        private void btnStockEntry_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmStockEntry();
            panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.getReferenceID();
            f1.getSupplier();
            f1.getItemsInTrolley();
            f1.getProduct();
            f1.txtStockInBy.Text = frmLogin.username;
            f1.Show();
        }

        private void btnStockAdjustment_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmStockAdjustment();
            panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.getProduct();
            f1.Show();
        }

        private void btnSalesRecord_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmAllRecords();
            panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.getStockHistory();
            f1.getSoldProduct();
            f1.LoadRecord();
            f1.Show();
        }

        private void btnRecord_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmSalesRecord();
            panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.getTotalSales();
            f1.getRecord();
            f1.Show();
        }

        private void btnPayment_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmPayment();
            panel4.Controls.Clear();
            f1.TopLevel = false;
            panel4.Controls.Add(f1);
            f1.BringToFront();
            f1.getSupplierPayment();
            f1.getCustomerPayment();
            f1.Show();
        }

        private void btnReprintReceipt_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmReprintReceipt();
            f1.Reprint();
            f1.ShowDialog();
        }

        private void btnAbout_Click_1(object sender, EventArgs e)
        {
            var f1 = new frmAbout();
            f1.ShowDialog();         
        }
    }
}
