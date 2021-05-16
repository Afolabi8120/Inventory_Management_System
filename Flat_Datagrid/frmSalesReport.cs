using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.Reporting.WinForms;

namespace Flat_Datagrid
{
    public partial class frmSalesReport : Form
    {
        MySqlConnection cn;
        MySqlDataReader dr;
        MySqlDataAdapter da;
        MySqlCommand cm;
        ClassDB db = new ClassDB();

        public string date1, date2, name, address, website, phone, email;

        public frmSalesReport()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void frmSalesReport_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        public void LoadHeader()
        {
            ReportDataSource rptDS = new ReportDataSource();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblsettings", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                name = dr["name"].ToString();
                website = dr["website"].ToString();
                email = dr["email"].ToString();
                phone = dr["phoneno"].ToString();
                address = dr["address"].ToString();

                dr.Close();
                cn.Close();

                reportViewer1.LocalReport.ReportPath = Application.StartupPath + "/Reports/SalesReport.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                da = new MySqlDataAdapter();


                cn.Open();
                da.SelectCommand = new MySqlCommand("SELECT * FROM tblsettings WHERE name = '" + name + "'", cn);
                da.Fill(ds, "dtName");
                cn.Close();

                ReportParameter pName = new ReportParameter("pName", name);
                ReportParameter pWebsite = new ReportParameter("pWebsite", website);
                ReportParameter pEmail = new ReportParameter("pEmail", email);
                ReportParameter pPhone = new ReportParameter("pPhone", phone);
                ReportParameter pAddress = new ReportParameter("pAddress", address);
                ReportParameter pDate1 = new ReportParameter("pDate1", date1);
                ReportParameter pDate2 = new ReportParameter("pDate2", date2);

                reportViewer1.LocalReport.SetParameters(pName);
                reportViewer1.LocalReport.SetParameters(pWebsite);
                reportViewer1.LocalReport.SetParameters(pEmail);
                reportViewer1.LocalReport.SetParameters(pPhone);
                reportViewer1.LocalReport.SetParameters(pAddress);
                reportViewer1.LocalReport.SetParameters(pDate1);
                reportViewer1.LocalReport.SetParameters(pDate2);

                rptDS = new ReportDataSource("DataSet1", ("dtName"));
                reportViewer1.LocalReport.DataSources.Add(rptDS);
            }
            else
            {
                name = "";
                website = "";
                email = "";
                phone = "";
                address = "";
            }
            dr.Close();
            cn.Close();
        }

        public void LoadRecord()
        {
            cn.Open();
            da = new MySqlDataAdapter("SELECT c.*,p.customer,p.total AS total2,p.amountpaid,p.cchange,p.paymode,p.date,p.time FROM tblcart AS c INNER JOIN tblcartpayment AS p ON c.invoiceno = p.invoiceno WHERE p.date BETWEEN '" + date1 + "' AND '" + date2 + "'", cn);
            DataSet1 ds = new DataSet1();
            da.Fill(ds, "dtSalesReport");

            ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[2]);

            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(datasource);
            this.reportViewer1.RefreshReport();
            cn.Close();
        }
    }
}
