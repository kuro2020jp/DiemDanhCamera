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
    public partial class frmKhoa : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=KURO\\SQLEXPRESS;Initial Catalog=FaceRecog;Integrated Security=True");
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        DataSet dtSet = new DataSet();
        bool isChange = false;
        int Id_Khoa;
        public frmKhoa()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            conn.Open();
            string sql = "select IdKhoa,MaKhoa,TenKhoa from Khoa";
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            conn.Close();  // đóng kết nối

            dgvKhoa.DataSource = dt; //đổ dữ liệu vào datagridview
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
            txtTenKhoa.ReadOnly = enabletxt;
            txtMaKhoa.ReadOnly = enabletxt;
        }
        private void nullTextbox()
        {
            txtTenKhoa.Text = null;
            txtMaKhoa.Text = null;
        }
        private void frmKhoa_Load(object sender, EventArgs e)
        {
            LoadData();
            enalbeButton(false);
            enableTextbox(true);
            btnThem.Enabled = true;
            dgvKhoa.ReadOnly = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            enalbeButton(false);
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
            enableTextbox(false);

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            enableTextbox(false);
            enalbeButton(false);
            btnCapNhat.Visible = true;
            btnBoQua.Enabled = true;
            isChange = true;
           
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKhoa.CurrentRow.Cells["TenKhoa"].Value != DBNull.Value)
            {
                if ((MessageBox.Show("Bạn có chắc xóa ?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    //int rowIndex = dgvKhoa.CurrentCell.RowIndex;
                    //dgvKhoa.Rows.RemoveAt(rowIndex);
                    conn.Open();
                    SqlCommand scmd = new SqlCommand("Procedure", conn);
                    scmd.CommandType = CommandType.StoredProcedure;
                    scmd.Parameters.AddWithValue("@TenKhoa", dgvKhoa.CurrentRow.Cells["TenKhoa"].Value);
                    scmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);

                }
            }
            enalbeButton(false);
            enableTextbox(true);
            nullTextbox();
            btnThem.Enabled = true;
            LoadData();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (btnSua.Enabled == false)
            {
                conn.Open();
                string Update = "Update Khoa set TenKhoa=@TenKhoa,MaKhoa=@MaKhoa where IdKhoa='" + Id_Khoa + "'";
                SqlCommand scmd = new SqlCommand(Update, conn);
                //scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@Id", Id_Khoa);
                scmd.Parameters.AddWithValue("@TenKhoa", txtTenKhoa.Text);
                scmd.Parameters.AddWithValue("@MaKhoa", txtMaKhoa.Text);
                scmd.ExecuteNonQuery();
                MessageBox.Show("Thay đổi thành công", "Thông báo", MessageBoxButtons.OK);
                conn.Close();
                LoadData();
                btnCapNhat.Visible = false;
                enalbeButton(false);
                enableTextbox(true);
                btnThem.Enabled = true;
                nullTextbox();
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            enalbeButton(false);
            enableTextbox(true);
            nullTextbox();
            btnThem.Enabled = true;
            btnCapNhat.Visible = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand Check_Data = new SqlCommand("Select TenKhoa from Khoa where ([TenKhoa]=@TenKhoa)", conn);

            Check_Data.Parameters.AddWithValue("@TenKhoa", txtTenKhoa.Text);
            SqlDataReader reader = Check_Data.ExecuteReader();

            if (reader.HasRows)
            {
                MessageBox.Show("Khoa đã tồn tại");
                conn.Close();
            }
            else
            {
                if (btnLuu.Enabled == true)
                {
                    int id = dgvKhoa.Rows.Count;                  
                    string insert = "INSERT INTO Khoa(IdKhoa,MaKhoa,TenKhoa) Values ( @IdKhoa,@MaKhoa,@TenKhoa)";

                    SqlCommand insertCmd = new SqlCommand(insert, conn);
                    conn.Close();
                    conn.Open();

                    insertCmd.Parameters.AddWithValue("@IdKhoa", id);
                    insertCmd.Parameters.AddWithValue("@MaKhoa", txtMaKhoa.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@TenKhoa", txtTenKhoa.Text.Trim());              
                    insertCmd.ExecuteNonQuery();
                    conn.Close();
                    LoadData();
                    txtTenKhoa.Text = null;
                    conn.Close();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                }
            }
            enableTextbox(true);
            enalbeButton(false);
            btnThem.Enabled = true;
        }

        private void dgvKhoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow rows = this.dgvKhoa.Rows[e.RowIndex];
                Id_Khoa = Convert.ToInt32(rows.Cells[0].Value.ToString());
                txtMaKhoa.Text = rows.Cells[1].Value.ToString();
                txtTenKhoa.Text = rows.Cells[2].Value.ToString();
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
