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
    public partial class Lop : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=KURO\\SQLEXPRESS;Initial Catalog=FaceRecog;Integrated Security=True");
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        DataSet dtSet = new DataSet();
        int Id_Lop;
        public Lop()
        {
            InitializeComponent();
        }
        private void conSQL()
        {
           // conn.Open();
            string sql = "select IdLop,TenLop,TenKhoa from Lop";  // lay het du lieu trong bang lop
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            conn.Close();  // đóng kết nối
            dgvLop.DataSource = dt; //đổ dữ liệu vào datagridview
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

        private void Lop_Load(object sender, EventArgs e)
        {
            conSQL();
            LoadDLComBo();

            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            cbKhoa.DropDownStyle = ComboBoxStyle.DropDownList;
            dgvLop.ReadOnly = true;
            txtTenLop.ReadOnly = true;
           
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
                dgvLop.DataSource = dt;
                conn.Close();
          
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
            btnThem.Enabled = false;
            txtTenLop.ReadOnly =false;
            
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
                        string tenLop = txtTenLop.Text.Trim();

                       
                        string insert = "INSERT INTO Lop(IdLop,TenKhoa,TenLop) Values (@IdLop,@TenKhoa,@TenLop)";
                        string selectIndex = cbKhoa.SelectedValue.ToString();

                        SqlCommand insertCmd = new SqlCommand(insert, conn);
                        conn.Open();

                        insertCmd.Parameters.Add("IdLop", SqlDbType.Int);
                        insertCmd.Parameters.Add("TenKhoa", SqlDbType.NVarChar, 50);
                        insertCmd.Parameters.Add("TenLop", SqlDbType.NVarChar, 50);

                        insertCmd.Parameters["IdLop"].Value = id;
                        insertCmd.Parameters["TenKhoa"].Value = selectIndex;
                        insertCmd.Parameters["TenLop"].Value = tenLop;
                        insertCmd.ExecuteNonQuery();
                        conSQL();
                        txtTenLop.Text = null;
                        conn.Close();
                        MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                    
                    
                   
                }
            }
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            txtTenLop.ReadOnly = true;
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnCapNhat.Visible = false;
            txtTenLop.ReadOnly = true;
            txtTenLop.Text = null;
        }

        private void dgvLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow rows = this.dgvLop.Rows[e.RowIndex];
                Id_Lop = Convert.ToInt32(rows.Cells[0].Value.ToString());
                txtTenLop.Text = rows.Cells[1].Value.ToString();
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnBoQua.Enabled = true;
            btnThem.Enabled = false;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnCapNhat.Visible = true;
            txtTenLop.ReadOnly = false;
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (btnSua.Enabled == false)
            {
                conn.Open();
                string Update = "Update Lop set TenLop=@TenLop where Id='" + Id_Lop + "'";
                SqlCommand scmd = new SqlCommand(Update, conn);
                //scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@Id", Id_Lop);
                scmd.Parameters.AddWithValue("@TenLop", txtTenLop.Text);
                scmd.ExecuteNonQuery();
                MessageBox.Show("Thay đổi thành công", "Thông báo", MessageBoxButtons.OK);
                conn.Close();
                conSQL();
                btnCapNhat.Visible = false;
                btnBoQua.Enabled = false;

            }
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

            txtTenLop.Text = null;
            txtTenLop.ReadOnly = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            btnSua.Enabled = false;
            conSQL();
        }

        private void btnDanhSach_Click(object sender, EventArgs e)
        {
            conSQL();
        }
    }
}
