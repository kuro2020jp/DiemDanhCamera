using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using Emgu.CV.CvEnum;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Data.SqlClient;

namespace AppDiemDanh
{
    public partial class frmSinhVien : Form
    {

        SqlConnection conn = new SqlConnection("Data Source=KURO\\SQLEXPRESS;Initial Catalog=FaceRecog;Integrated Security=True");
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        DataSet dtSet = new DataSet();
        int Id_SinhVien;
        List<Image<Gray, Byte>> TrainedFaces = new List<Image<Gray, byte>>();
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> PersonsNames = new List<string>();
        List<int> PersonsLabes = new List<int>();
        public frmSinhVien()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            // conn.Open();
            string sql = "select IdSinhVien,HoTen,MSSV,NamSinh,IdKhoa,IdLop,HinhAnh from SinhVien";  // lay het du lieu trong bang lop
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            conn.Close();  // đóng kết nối
            dgvSinhVien.DataSource = dt; //đổ dữ liệu vào datagridview
        }
        private void LoadDLComBo()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select IdKhoa,TenKhoa from Khoa", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbKhoa.DataSource = dt;
            cbKhoa.DisplayMember = "TenKhoa";
            cbKhoa.ValueMember = "IdKhoa";
            conn.Close();
        }
        private void LoadDLCB()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select IdLop,TenLop from Lop", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbLop.DataSource = dt;
            cbLop.DisplayMember = "TenLop";
            cbLop.ValueMember = "IdLop";
            conn.Close();
        }
        private void enalbeButton(bool enable)
        {
          //  btnThem.Enabled = enable;
            btnSua.Enabled = enable;
            btnXoa.Enabled = enable;
          //  btnLuu.Enabled = enable;
            btnBoQua.Enabled = enable;
            btnCapNhat.Enabled = enable;
        }

        private void enableTextbox(bool enabletxt)
        {
            txtHoTen.ReadOnly = enabletxt;
            txtMSSV.ReadOnly = enabletxt;
           
            if (enabletxt == true)
            {
                cbLop.DropDownStyle = ComboBoxStyle.DropDownList;
                cbKhoa.DropDownStyle = ComboBoxStyle.DropDownList;
            }

        }
        private void nullTextbox()
        {
            txtHoTen.Text = null;
            txtMSSV.Text = null;
        }
        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadDLCB();
            LoadDLComBo();
            Readtxt();
            enableTextbox(true);
            dgvSinhVien.ReadOnly = true;
            enalbeButton(false);
            dtNamSinh.Format = DateTimePickerFormat.Custom;
            dtNamSinh.CustomFormat = "dd/MM/yyyy";
            dtNamSinh.Enabled = false;
            cbLop.Enabled = false;
        }

        private void cbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = cbKhoa.Text;
            string sql = "Select IdLop,TenLop,TenKhoa from Lop,Khoa where Khoa.IdKhoa=Lop.IdKhoa and TenKhoa = N'" + a + "'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            cbLop.DataSource = dt;
            conn.Close();
            cbLop.Enabled = true;
        }
        private void Readtxt()
        {
            txtHoTen.ReadOnly = true;
            txtMSSV.ReadOnly = true;
            dtNamSinh.Enabled = true;

            cbKhoa.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLop.DropDownStyle = ComboBoxStyle.DropDownList;
            dgvSinhVien.ReadOnly = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            enalbeButton(false);
            enableTextbox(false);

            //btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
            dtNamSinh.Enabled = true;

        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            enableTextbox(true);
            enalbeButton(false);
            nullTextbox();
            //btnThem.Enabled = true;
            dtNamSinh.Enabled = false;
           
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.CurrentRow.Cells["MSSV"].Value != DBNull.Value)
            {
                if ((MessageBox.Show("Bạn có chắc xóa ?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    conn.Open();
                    SqlCommand scmd = new SqlCommand("Delete3", conn);
                    scmd.CommandType = CommandType.StoredProcedure;
                    scmd.Parameters.AddWithValue("@MSSV", dgvSinhVien.CurrentRow.Cells["MSSV"].Value);
                    scmd.ExecuteNonQuery();
                    LoadAign();
                    conn.Close();
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);
                }
                enalbeButton(false);
                enableTextbox(true);
                nullTextbox();
              //  btnThem.Enabled = true;
            }
        }


        
        private void LoadAign()
        {
            string a = cbLop.Text;
            string sql = "Select IdSinhVien,HoTen,MSSV,NamSinh,HinhAnh from SinhVien where TenLop = N'" + a + "'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            dgvSinhVien.DataSource = dt;
        }

        private void cbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = cbLop.Text;
            string sql = "Select IdSinhVien,HoTen,MSSV,NamSinh,HinhAnh from SinhVien,Lop where Lop.IdLop=SinhVien.IdLop and TenLop = N'" + a + "'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            dgvSinhVien.DataSource = dt;
            conn.Close();
           
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            enalbeButton(false);
            enableTextbox(false);
            btnBoQua.Enabled = true;           
            btnCapNhat.Visible = true;
            dtNamSinh.Enabled = true;
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (btnSua.Enabled == false)
            {
                conn.Open();
                string Update = "Update SinhVien set HoTen=@HoTen,MSSV=@MSSV,NamSinh=@NamSinh where IdSinhVien='" + Id_SinhVien + "'";
                SqlCommand scmd = new SqlCommand(Update, conn);
                //scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@IdSinhVien", Id_SinhVien);
                scmd.Parameters.AddWithValue("@MSSV", txtMSSV.Text);
                scmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                scmd.Parameters.AddWithValue("@NamSinh", Convert.ToDateTime(dtNamSinh.Text));

                scmd.ExecuteNonQuery();
                MessageBox.Show("Thay đổi thành công", "Thông báo", MessageBoxButtons.OK);
                conn.Close();
                LoadAign();
                btnCapNhat.Visible = false;
                btnBoQua.Enabled = false;
                Readtxt();
                enalbeButton(false);
                enableTextbox(true);
                dtNamSinh.Enabled = false;
            }
        }

        private void btnThemHinh_Click(object sender, EventArgs e)
        {
            frmThemSinhVien ThemSV = new frmThemSinhVien();
            ThemSV.Show();
        }

     
        Image ChuyenVeHinh(byte[] b)
        {
            MemoryStream m = new MemoryStream(b);
            return Image.FromStream(m);
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select IdSinhVien,HoTen,MSSV,NamSinh,IdKhoa,IdLop,HinhAnh from SinhVien");
                int rows = dgvSinhVien.CurrentCell.RowIndex;
                txtHoTen.Text = dgvSinhVien.Rows[rows].Cells[1].Value.ToString();
                txtMSSV.Text = dgvSinhVien.Rows[rows].Cells[2].Value.ToString();
                //picSinhVien.Image = new Bitmap(Application.StartupPath + @"\\..\\TrainedImages\\");
                ////System.IO.Directory.GetFiles(dgvSinhVien.Rows[rows].Cells[4].Value.ToString(), "*.jpg");
                //Image image = Image.FromFile(@"..\..\TrainedImages" + txtMSSV.Text +"_"+txtHoTen.Text+ ".jpg");
                //this.picSinhVien.Image = image;
                conn.Close();           
                enalbeButton(false);
                enableTextbox(true);
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnBoQua.Enabled = true;

            }
            catch
            {
                MessageBox.Show("Vui lòng không click vào ô trống!!!!", "Thông báo", MessageBoxButtons.OK);
            }
        }
       
    }
}
