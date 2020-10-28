using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CameraDiemDanh
{
    public partial class SinhVien : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=KURO\\SQLEXPRESS;Initial Catalog=FaceRecog;Integrated Security=True");
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        DataSet dtSet = new DataSet();
        int Id_SinhVien;
        public SinhVien()
        {
            InitializeComponent();
        }

        private void btnThemHinh_Click(object sender, EventArgs e)
        {
            ThemSinhVien ThemSV = new ThemSinhVien();
            ThemSV.Show();
        }
        private void conSQL()
        {
            // conn.Open();
            string sql = "select IdSinhVien,HoTen,MSSV,NamSinh from SinhVien";  // lay het du lieu trong bang lop
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
            cbKhoa.ValueMember = "TenKhoa";
            conn.Close();
        }
        private void LoadDLCB()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select IdLop,TenLop from Lop", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbLop.DataSource = dt;
            cbLop.DisplayMember = "TenLop";
            cbLop.ValueMember = "TenLop";
            conn.Close();
        }

        private void SinhVien_Load(object sender, EventArgs e)
        {
            conSQL();
            LoadDLCB();
            LoadDLComBo();
            Readtxt();
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            dtNgaySinh.Format = DateTimePickerFormat.Custom;
            dtNgaySinh.CustomFormat = "dd/MM/yyyy";
            dtNgaySinh.Enabled = false;
        }

        private void cbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = cbKhoa.Text;
            string sql = "Select IdLop,TenLop,TenKhoa from Lop where TenKhoa = N'" + a + "'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            cbLop.DataSource = dt;
            conn.Close();
        }
        private void Readtxt()
        {
            txtHoTen.ReadOnly = true;
            txtMSSV.ReadOnly = true;
           dtNgaySinh.Enabled = true;
           
            cbKhoa.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLop.DropDownStyle = ComboBoxStyle.DropDownList;
            dgvSinhVien.ReadOnly = true;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
          
            txtHoTen.ReadOnly = false;
            txtMSSV.ReadOnly = false;
           dtNgaySinh.Enabled = true;
            
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            txtMSSV.Text = null;
            txtHoTen.Text = null;
            dtNgaySinh.Text = null;
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
                txtHoTen.Text = null;
                txtMSSV.Text = null;
                dtNgaySinh.Text = null;
                btnThem.Enabled = true;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false;
                btnBoQua.Enabled = false;
                btnSua.Enabled = false;
            }

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand Check_Data = new SqlCommand("Select MSSV from SinhVien where ([MSSV]=@MSSV)", conn);

            Check_Data.Parameters.AddWithValue("@MSSV", txtMSSV.Text);
            SqlDataReader reader = Check_Data.ExecuteReader();

            if (reader.HasRows)
            {
                MessageBox.Show("Vui lòng kiểm tra lại MSSV");
                conn.Close();
            }
            else
            {
                if (btnLuu.Enabled == true)
                {
                    conn.Close();
                    string LoadAgain = "Select IdSinhVien,HoTen,MSSV,NamSinh from SinhVien";
                    SqlCommand scmd = new SqlCommand(LoadAgain, conn);
                    scmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(scmd); //chuyen du lieu ve
                    DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
                    da.Fill(dt);  // đổ dữ liệu vào kho
                    dgvSinhVien.DataSource = dt;
                    int id = dgvSinhVien.Rows.Count;
                    string HoTen = txtHoTen.Text.Trim();
                    string mssv = txtMSSV.Text.Trim();
                    string insert = "INSERT INTO SinhVien(IdSinhVien,TenKhoa,TenLop,MSSV,HoTen,NamSinh) Values (@IdSinhVien,@TenKhoa,@TenLop,@MSSV,@HoTen,@NamSinh)";
                    string selectIndex = cbKhoa.SelectedValue.ToString();
                    string selectIndex2 = cbLop.SelectedValue.ToString();
                    string date = dtNgaySinh.Text.ToString() ;

                    SqlCommand insertCmd = new SqlCommand(insert, conn);
                    conn.Open();


                    insertCmd.Parameters.Add("IdSinhVien", SqlDbType.Int);
                    insertCmd.Parameters.Add("TenKhoa", SqlDbType.NVarChar, 50);
                    insertCmd.Parameters.Add("TenLop", SqlDbType.NVarChar, 50);
                    insertCmd.Parameters.Add("MSSV", SqlDbType.VarChar, 20);
                    insertCmd.Parameters.Add("HoTen", SqlDbType.NVarChar, 50);
                    insertCmd.Parameters.Add("NamSinh", SqlDbType.Date);


                    insertCmd.Parameters["IdSinhVien"].Value = id;
                    insertCmd.Parameters["TenKhoa"].Value = selectIndex;
                    insertCmd.Parameters["TenLop"].Value = selectIndex2;
                    insertCmd.Parameters["MSSV"].Value = mssv;
                    insertCmd.Parameters["HoTen"].Value = HoTen;
                    insertCmd.Parameters["NamSinh"].Value = date;
                    insertCmd.ExecuteNonQuery();
                    LoadAign();
                    conn.Close();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);

                    btnBoQua.Enabled = false;
                    btnThem.Enabled = true;
                    btnLuu.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;

                    txtHoTen.ReadOnly = true;
                    txtMSSV.ReadOnly = true;
                    txtHoTen.Text = null;
                    txtMSSV.Text = null;
                    dtNgaySinh.Enabled = false;
                    dtNgaySinh.Text = null;
                    
                }
            }
           
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow rows = this.dgvSinhVien.Rows[e.RowIndex];
                Id_SinhVien = Convert.ToInt32(rows.Cells[0].Value.ToString());
                txtMSSV.Text = rows.Cells[2].Value.ToString();
                txtHoTen.Text = rows.Cells[1].Value.ToString();
                dtNgaySinh.Text = rows.Cells[3].Value.ToString();
                btnBoQua.Enabled = true;
                btnThem.Enabled = false;
                btnLuu.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;

            }
            catch
            {
                MessageBox.Show("Vui lòng không click vào ô trống!!!!", "Thông báo", MessageBoxButtons.OK);
            }
        }
        private void LoadAign()
        {
            string a = cbLop.Text;
            string sql = "Select IdSinhVien,HoTen,MSSV,NamSinh from SinhVien where TenLop = N'" + a + "'";  // lay het du lieu trong bang sinh vien
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
            string sql = "Select IdSinhVien,HoTen,MSSV,NamSinh from SinhVien where TenLop = N'" + a + "'";  // lay het du lieu trong bang sinh vien
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
            btnBoQua.Enabled = true;
            btnThem.Enabled = false;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnCapNhat.Visible = true;
            txtHoTen.ReadOnly = false;
            txtMSSV.ReadOnly = false;
            dtNgaySinh.Enabled = true;
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
                scmd.Parameters.AddWithValue("@NamSinh", Convert.ToDateTime(dtNgaySinh.Text));
               
                scmd.ExecuteNonQuery();
                MessageBox.Show("Thay đổi thành công", "Thông báo", MessageBoxButtons.OK);
                conn.Close();
                LoadAign();
                btnCapNhat.Visible = false;
                btnBoQua.Enabled = false;
                Readtxt();
                txtHoTen.Text = null;
                txtMSSV.Text = null;
                dtNgaySinh.Text = null;
                dtNgaySinh.Enabled = false;
            }
        }
    }
}