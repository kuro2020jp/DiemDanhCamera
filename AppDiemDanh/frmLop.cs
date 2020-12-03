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

namespace AppDiemDanh
{
    public partial class frmLop : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=KURO\\SQLEXPRESS;Initial Catalog=FaceRecog;Integrated Security=True");
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        DataSet dtSet = new DataSet();
        int Id_Lop;
        public frmLop()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            // conn.Open();
            string sql = "select IdLop,TenLop,IdKhoa,MaLop from Lop";  // lay het du lieu trong bang lop
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            conn.Close();  // đóng kết nối
            dgvLop.DataSource = dt; //đổ dữ liệu vào datagridview
        }
        private void LoadDataAgain()
        {
            conn.Open();
            string sql = "select IdLop,TenLop,MaLop,TenKhoa from Lop,Khoa where Khoa.IdKhoa=Lop.IdKhoa";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            dgvLop.DataSource = dt;
            conn.Close();
        }
        private void LoadCB()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select IdKhoa,TenKhoa from Khoa", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbKhoa.DataSource = dt;
            cbKhoa.DisplayMember = "TenKhoa";
            cbKhoa.ValueMember = "IdKhoa";
            conn.Close();
        }
        private void enalbeButton(bool enable)
        {
            btnThem.Enabled = enable;
            btnSua.Enabled = enable;
            btnXoa.Enabled = enable;
            btnLuu.Enabled = enable;
            btnBoQua.Enabled = enable;
        }

        private void enableTextbox(bool enabletxt)
        {
            txtTenLop.ReadOnly = enabletxt;
            txtMaLop.ReadOnly = enabletxt;
            if (enabletxt == true)
            {
                cbKhoa.DropDownStyle = ComboBoxStyle.DropDownList;
                
            }
        }
        private void nullTextbox()
        {
            txtTenLop.Text = null;
            txtMaLop.Text = null;

        }
        private void frmLop_Load(object sender, EventArgs e)
        {
           LoadData();
            LoadCB();

            enableTextbox(true);
            enalbeButton(false);
            btnThem.Enabled = true;
            dgvLop.ReadOnly = true;

        }

        private void cbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = cbKhoa.Text;
            string sql = "select IdLop,TenLop,MaLop,TenKhoa from Lop,Khoa where Khoa.IdKhoa=Lop.IdKhoa and TenKhoa = N'" + a + "'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            dgvLop.DataSource = dt;
            conn.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            enalbeButton(false);
            enableTextbox(false);
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand Check_Data = new SqlCommand("Select TenLop from Lop where ([TenLop]=@TenLop)", conn);

            Check_Data.Parameters.AddWithValue("@TenLop", txtTenLop.Text);
            SqlDataReader reader = Check_Data.ExecuteReader();

            if (reader.HasRows)
            {
                MessageBox.Show("Lớp đã tồn tại");
                conn.Close();
            }
            else
            {
                if (btnLuu.Enabled == true)
                {
                    conn.Close();
                    string LoadAgain = "Select * from Lop";
                    SqlCommand scmd = new SqlCommand(LoadAgain, conn);
                    scmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(scmd); //chuyen du lieu ve
                    DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
                    da.Fill(dt);  // đổ dữ liệu vào kho
                    dgvLop.DataSource = dt;
                    int id = dgvLop.Rows.Count;
                    


                    string insert = "INSERT INTO Lop(IdLop,MaLop,TenLop,IdKhoa) Values (@IdLop,@MaLop,@TenLop,@IdKhoa)";
                    string selectIndex = cbKhoa.SelectedValue.ToString();

                    SqlCommand insertCmd = new SqlCommand(insert, conn);
                    conn.Open();

                    insertCmd.Parameters.AddWithValue("@IdLop",id);
                    insertCmd.Parameters.AddWithValue("@IdKhoa", Convert.ToInt32(cbKhoa.SelectedValue.ToString())) ;
                    insertCmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@TenLop", txtTenLop.Text.Trim()); 

                   
                    insertCmd.ExecuteNonQuery();
                    LoadDataAgain();
                    txtTenLop.Text = null;
                    conn.Close();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);



                }
            }
            enableTextbox(true);
            enalbeButton(false);
            btnThem.Enabled=true;
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            enalbeButton(false);
            enableTextbox(true);
            nullTextbox();
            btnThem.Enabled = true;
            btnCapNhat.Visible = false;
        }

        private void dgvLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow rows = this.dgvLop.Rows[e.RowIndex];
                Id_Lop = Convert.ToInt32(rows.Cells[0].Value.ToString());
                txtMaLop.Text = rows.Cells[2].Value.ToString();
                txtTenLop.Text = rows.Cells[1].Value.ToString();
                cbKhoa.Text = rows.Cells[3].Value.ToString();
                enableTextbox(false);
                enalbeButton(false);
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnBoQua.Enabled = true;

            }
            catch
            {
                MessageBox.Show("Vui lòng không click vào ô trống!!!!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (btnSua.Enabled == false)
            {
                conn.Open();
                string Update = "Update Lop set TenLop=@TenLop,MaLop=@MaLop where IdLop='" + Id_Lop + "'";
                SqlCommand scmd = new SqlCommand(Update, conn);
                //scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@Id", Id_Lop);
                scmd.Parameters.AddWithValue("@TenLop", txtTenLop.Text);
                scmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text);
                scmd.ExecuteNonQuery();
                MessageBox.Show("Thay đổi thành công", "Thông báo", MessageBoxButtons.OK);
                conn.Close();
                LoadDataAgain();
                btnCapNhat.Visible = false;
                btnBoQua.Enabled = false;

            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            enalbeButton(false);
            enableTextbox(false);
            btnCapNhat.Visible = true;
            btnBoQua.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvLop.CurrentRow.Cells["TenLop"].Value != DBNull.Value)
            {
                if ((MessageBox.Show("Bạn có chắc xóa ?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    conn.Open();
                    SqlCommand scmd = new SqlCommand("Delete", conn);
                    scmd.CommandType = CommandType.StoredProcedure;
                    scmd.Parameters.AddWithValue("@TenLop", dgvLop.CurrentRow.Cells["TenLop"].Value);
                    scmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);

                }
            }

            enableTextbox(true);
            enalbeButton(false);
            nullTextbox();
            btnThem.Enabled = true;
            LoadDataAgain();
        }

        private void btnDanhSach_Click(object sender, EventArgs e)
        {
            string sql = "select IdLop,TenLop,MaLop,TenKhoa from Lop,Khoa where Khoa.IdKhoa=Lop.IdKhoa";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            dgvLop.DataSource = dt;
            conn.Close();
        }
    }
}
