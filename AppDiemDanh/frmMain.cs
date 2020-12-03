using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppDiemDanh
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnQuanLy_Click(object sender, EventArgs e)
        {
            if (btnDiemDanh.BackColor == Color.FromArgb(186, 183, 255) || btnTrangChu.BackColor == Color.CornflowerBlue || btnQuanLy.BackColor == Color.FromArgb(186, 183, 255))
            {
                btnDiemDanh.BackColor = Color.FromArgb(186, 183, 255);
                btnTrangChu.BackColor = Color.FromArgb(186, 183, 255);
                btnQuanLy.BackColor = Color.CornflowerBlue;

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

        private void btnDiemDanh_Click(object sender, EventArgs e)
        {
            if (btnDiemDanh.BackColor == Color.FromArgb(186, 183, 255) || btnTrangChu.BackColor == Color.CornflowerBlue || btnQuanLy.BackColor == Color.FromArgb(186, 183, 255))
            {
                btnDiemDanh.BackColor = Color.CornflowerBlue;
                btnTrangChu.BackColor = Color.FromArgb(186, 183, 255);
                btnQuanLy.BackColor = Color.FromArgb(186, 183, 255);

                pnlForm.Controls.Clear();
                frmDiemDanh frmDM = new frmDiemDanh();
                frmDM.TopLevel = false;
                frmDM.AutoScroll = true;
                pnlForm.Controls.Add(frmDM);
                frmDM.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                frmDM.Dock = DockStyle.Fill;
                frmDM.Show();
            }
           
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinisize_Click(object sender, EventArgs e)
        {

        }

      
    }
}
