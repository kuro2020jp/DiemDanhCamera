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
    public partial class MonHoc : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=KURO\\SQLEXPRESS;Initial Catalog=FaceRecog;Integrated Security=True");
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        DataSet dtSet = new DataSet();
        bool isChange = false;
        int Id_MonHoc;
        public MonHoc()
        {
            InitializeComponent();

        }
        private void conSQL()
        {
            conn.Open();
            string sql = "select IdMonHoc,TenMH,TenLop,SoBuoi from MonHoc";
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            conn.Close();  // đóng kết nối

            dgvMonHoc.DataSource = dt; //đổ dữ liệu vào datagridview
        }

        private void LoadDLComBo()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select IdLop,TenLop from Lop", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbLop.DataSource = dt;
            cbLop.DisplayMember = "TenLop";
            cbLop.ValueMember = "TenLop";
            conn.Close();
        }
        private void MonHoc_Load(object sender, EventArgs e)
        {
            conSQL();
            LoadDLComBo();
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            txtMonHoc.ReadOnly = true;
            cbLop.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
            btnThem.Enabled = false;
            txtMonHoc.ReadOnly = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand Check_Data = new SqlCommand("Select TenMH from MonHoc where ([TenMH]=@TenMH)", conn);

            Check_Data.Parameters.AddWithValue("@TenMH", txtMonHoc.Text);
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
                    int id = dgvMonHoc.Rows.Count;
                    string tenMonHoc = txtMonHoc.Text.Trim();
                    string insert = "INSERT INTO MonHoc(IdMonHoc,TenMH) Values ( @IdMonHoc,@TenMH)";
                    string sobuoi = txtSoBuoi.Text.Trim();
                    SqlCommand insertCmd = new SqlCommand(insert, conn);
                    conn.Close();
                    conn.Open();


                    insertCmd.Parameters.Add("IdMonHoc", SqlDbType.Int);
                    insertCmd.Parameters.Add("TenMH", SqlDbType.NVarChar, 50);
                    insertCmd.Parameters.Add("SoBuoi", SqlDbType.Int);

                    insertCmd.Parameters["IdMonHoc"].Value = id;
                    insertCmd.Parameters["TenMH"].Value = tenMonHoc;
                    insertCmd.Parameters["SoBuoi"].Value = sobuoi;
                    insertCmd.ExecuteNonQuery();
                    conn.Close();
                    conSQL();
                    txtMonHoc.Text = null;
                    conn.Close();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                }
            }
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            txtMonHoc.ReadOnly = true;
            txtSoBuoi.ReadOnly = true;
        }

        private void dgvMonHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow rows = this.dgvMonHoc.Rows[e.RowIndex];
                Id_MonHoc = Convert.ToInt32(rows.Cells[0].Value.ToString());
                txtMonHoc.Text = rows.Cells[2].Value.ToString();
                txtSoBuoi.Text = rows.Cells[3].Value.ToString();
                btnThem.Enabled = false;
                btnXoa.Enabled = true;
                btnSua.Enabled = true;
                btnLuu.Enabled = false; ;
                btnBoQua.Enabled = true;
                txtMonHoc.ReadOnly = true;
                txtSoBuoi.ReadOnly = true;
            }
            catch
            {
                MessageBox.Show("Vui lòng không click vào ô trống!!!!", "Thông báo", MessageBoxButtons.OK);
            }
          
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtMonHoc.ReadOnly = false;
            txtSoBuoi.ReadOnly = false;
            btnLuu.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            isChange = true;
            btnCapNhat.Visible = true;
            btnLuu.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvMonHoc.CurrentRow.Cells["TenMH"].Value != DBNull.Value)
            {
                if ((MessageBox.Show("Bạn có chắc xóa ?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    //int rowIndex = dgvKhoa.CurrentCell.RowIndex;
                    //dgvKhoa.Rows.RemoveAt(rowIndex);
                    conn.Open();
                    SqlCommand scmd = new SqlCommand("Delete2", conn);
                    scmd.CommandType = CommandType.StoredProcedure;
                    scmd.Parameters.AddWithValue("@TenMH", dgvMonHoc.CurrentRow.Cells["TenMH"].Value);
                    scmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);

                }
            }

            txtMonHoc.Text = null;
            txtMonHoc.ReadOnly = false;
            txtSoBuoi.Text = null;
            txtSoBuoi.ReadOnly = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            btnSua.Enabled = false;
            conSQL();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (btnSua.Enabled == false)
            {
                conn.Open();
                string Update = "Update MonHoc set TenMH=@TenMH, SoBuoi=@SoBuoi where IdMonHoc='" + Id_MonHoc + "'";
                SqlCommand scmd = new SqlCommand(Update, conn);
                //scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@Id", Id_MonHoc);
                scmd.Parameters.AddWithValue("@TenMH", txtMonHoc.Text);
                scmd.Parameters.AddWithValue("@SoBuoi", txtSoBuoi.Text);
                scmd.ExecuteNonQuery();
                MessageBox.Show("Thay đổi thành công", "Thông báo", MessageBoxButtons.OK);
                conn.Close();
                conSQL();
                btnCapNhat.Visible = false;
                btnBoQua.Enabled = false;
                btnThem.Enabled = true;
                txtMonHoc.Text = null;
                txtMonHoc.ReadOnly = true;
                txtSoBuoi.Text = null;
                txtSoBuoi.ReadOnly = true;
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            txtMonHoc.ReadOnly = true;
            btnSua.Enabled = false;
            txtMonHoc.Text = null;
            txtSoBuoi.Text = null;
            txtSoBuoi.ReadOnly = true;
        }

        private void cbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = cbLop.Text;
            string sql = "Select IdMonHoc,TenLop,TenMH,SoBuoi from MonHoc where TenLop = N'" + a + "'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            dgvMonHoc.DataSource = dt;
            conn.Close();
        }
    }
}
