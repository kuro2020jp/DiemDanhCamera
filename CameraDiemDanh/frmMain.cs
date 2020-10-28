using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CameraDiemDanh
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDanhMuc_Click(object sender, EventArgs e)
        {
            pnlForm.Controls.Clear();
            frmDiemDanh frmDM = new frmDiemDanh();
            frmDM.TopLevel = false;
            frmDM.AutoScroll = true;
            pnlForm.Controls.Add(frmDM);
            frmDM.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            frmDM.Dock = DockStyle.Fill;
            frmDM.Show();
        }

        private void btnQuanLy_Click(object sender, EventArgs e)
        {
            pnlForm.Controls.Clear();
            frmQuanLy frmQL = new frmQuanLy();
            frmQL.TopLevel = false;
            frmQL.AutoScroll = true;
            pnlForm.Controls.Add(frmQL);
            frmQL.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            frmQL.Dock = DockStyle.Fill;
            frmQL.Show();
        }

      
    }
}
