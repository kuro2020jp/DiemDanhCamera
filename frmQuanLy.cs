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
    public partial class frmQuanLy : Form
    {
        public frmQuanLy()
        {
            InitializeComponent();
        }

        private void btnKhoa_Click(object sender, EventArgs e)
        {
            pnlFormTrong.Controls.Clear();
            Khoa khoa = new Khoa();
            khoa.TopLevel = false;
            khoa.AutoScroll = true;
            pnlFormTrong.Controls.Add(khoa);
            khoa.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            khoa.Dock = DockStyle.Fill;
            khoa.Show();
        }

        private void btnLop_Click(object sender, EventArgs e)
        {
            pnlFormTrong.Controls.Clear();
            Lop lop = new Lop();
            lop.TopLevel = false;
            lop.AutoScroll = true;
            pnlFormTrong.Controls.Add(lop);
            lop.Dock = DockStyle.Fill;
            lop.Show();
        }

        private void btnMonHoc_Click(object sender, EventArgs e)
        {
            pnlFormTrong.Controls.Clear();
            MonHoc monHoc = new MonHoc();
            monHoc.TopLevel = false;
            monHoc.AutoScroll = true;
            pnlFormTrong.Controls.Add(monHoc);
            monHoc.Dock = DockStyle.Fill;
            monHoc.Show();
        }

        private void btnSinhVien_Click(object sender, EventArgs e)
        {
            pnlFormTrong.Controls.Clear();
            SinhVien sinhVien = new SinhVien();
            sinhVien.TopLevel = false;
            sinhVien.AutoScroll = true;
            pnlFormTrong.Controls.Add(sinhVien);
            sinhVien.Dock = DockStyle.Fill;
            sinhVien.Show();
        }

       
    }
}
