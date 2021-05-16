using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Flat_Datagrid
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            lblAbout.Text = "Hi there!\nI'm Afolabi Temidayo Timothy, a System Software Developer focused on C#, VB.Net && MySQL.\nThis Project was implemented using C# and MySQL.\n\tContact Me\nPhone No: +2348090949669\nEmail Address: Afolabi8120@gmail.com";
        }

        private void btnFacebook_Click(object sender, EventArgs e)
        {
            Process.Start("https://facebook.com/profile.php?id=100056265665208");
        }

        private void btnInstagram_Click(object sender, EventArgs e)
        {
            Process.Start("https://instagram.com/life_of_temidayo/");
        }

        private void btnTwitter_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/afolabitemidee");
        }

        private void btnGitHub_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/afolabi8120/");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnLinkedIn_Click(object sender, EventArgs e)
        {
            Process.Start("https://linkedin.com/in/afolabi-temidayo-timothy-6ab2261a5/");
        }

    }
}
