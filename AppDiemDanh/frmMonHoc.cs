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
    public partial class frmMonHoc : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=KURO\\SQLEXPRESS;Initial Catalog=FaceRecog;Integrated Security=True");
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        DataSet dtSet = new DataSet();
        bool isChange = false;
        int Id_MonHoc;
        public frmMonHoc()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            conn.Open();
            string sql = "select IdMonHoc,MaMH,TenMH,IdLop from MonHoc";
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            conn.Close();  // đóng kết nối

            dgvMonHoc.DataSource = dt; //đổ dữ liệu vào datagridview
        }
        private void LoadDataAgain()
        {
            conn.Open();
            string sql = "Select IdMonHoc,TenLop,TenMH,SoBuoi,MaMH from MonHoc,Lop where Lop.IdLop=MonHoc.IdLop";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            dgvMonHoc.DataSource = dt;
            conn.Close();
        }
        private void LoadCB()
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
            btnThem.Enabled = enable;
            btnSua.Enabled = enable;
            btnXoa.Enabled = enable;
            btnLuu.Enabled = enable;
            btnBoQua.Enabled = enable;
        }

        private void enableTextbox(bool enabletxt)
        {
            txtMonHoc.ReadOnly = enabletxt;
            //txtSoBuoi.ReadOnly = enabletxt;
            txtMaMH.ReadOnly = enabletxt;
            if (enabletxt == true)
            {
                cbLop.DropDownStyle = ComboBoxStyle.DropDownList;

            }

        }
        private void nullTextbox()
        {
            txtMaMH.Text = null;
            //txtSoBuoi.Text = null;
            txtMonHoc.Text = null;
        }
        private void frmMonHoc_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadCB();
            enableTextbox(true);
            enalbeButton(false);
            dgvMonHoc.ReadOnly = true;
            btnThem.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            enalbeButton(false);
            enableTextbox(false);
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            isChange = true;
            enableTextbox(false);
            enalbeButton(false);
            btnCapNhat.Visible = true;
            btnBoQua.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
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
            enalbeButton(false);
            enableTextbox(true);
            nullTextbox();
            btnThem.Enabled = true;
            LoadDataAgain();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand Check_Data = new SqlCommand("Select TenMH from MonHoc where ([TenMH]=@TenMH)", conn);

            Check_Data.Parameters.AddWithValue("@TenMH", txtMonHoc.Text);
            SqlDataReader reader = Check_Data.ExecuteReader();

            if (reader.HasRows)
            {
                MessageBox.Show("Môn Học đã tồn tại");
                conn.Close();
            }
            else
            {
                if (btnLuu.Enabled == true)
                {
                    int id = dgvMonHoc.Rows.Count;
                    string insert = "INSERT INTO MonHoc(IdMonHoc,MaMH,TenMH,IdLop,SoBuoi) Values ( @IdMonHoc,@MaMH,@TenMH,@IdLop,@SoBuoi)";
                    SqlCommand insertCmd = new SqlCommand(insert, conn);
                    conn.Close();
                    conn.Open();


                    insertCmd.Parameters.AddWithValue("@IdMonHoc",id);
                    insertCmd.Parameters.AddWithValue("@MaMH", txtMaMH.Text.Trim()) ;
                    insertCmd.Parameters.AddWithValue("@TenMH",txtMonHoc.Text.Trim());
                    //insertCmd.Parameters.AddWithValue("@SoBuoi", txtSoBuoi.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@IdLop",Convert.ToInt32(cbLop.SelectedValue.ToString()));

                   
                    insertCmd.ExecuteNonQuery();
                    conn.Close();
                    LoadDataAgain();
                    txtMonHoc.Text = null;
                    conn.Close();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                }
            }
            enableTextbox(true);
            enalbeButton(false);
            btnThem.Enabled = true;
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (btnSua.Enabled == false)
            {
                conn.Open();
                string Update = "Update MonHoc set TenMH=@TenMH, SoBuoi=@SoBuoi, MaMH=@MaMH where IdMonHoc='" + Id_MonHoc + "'";
                SqlCommand scmd = new SqlCommand(Update, conn);
                //scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@Id", Id_MonHoc);
                scmd.Parameters.AddWithValue("@TenMH", txtMonHoc.Text);
               // scmd.Parameters.AddWithValue("@SoBuoi", txtSoBuoi.Text);
                scmd.Parameters.AddWithValue("@MaMH", txtMaMH.Text);
                scmd.ExecuteNonQuery();
                MessageBox.Show("Thay đổi thành công", "Thông báo", MessageBoxButtons.OK);
                conn.Close();
                LoadDataAgain();
                btnCapNhat.Visible = false;
                enalbeButton(false);
                enableTextbox(true);
                nullTextbox();
                btnThem.Enabled = true;
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            enableTextbox(true);
            nullTextbox();
            enalbeButton(false);
            btnThem.Enabled = true;
            btnCapNhat.Visible = false;
        }

        private void cbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = cbLop.Text;
            string sql = "Select IdMonHoc,TenLop,TenMH,MaMH from MonHoc,Lop where Lop.IdLop=MonHoc.IdLop and TenLop = N'" + a + "'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            dgvMonHoc.DataSource = dt;
            conn.Close();
            txtMaMH.Text = null;
            txtMonHoc.Text = null;
        }

        private void dgvMonHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow rows = this.dgvMonHoc.Rows[e.RowIndex];
              
                txtMonHoc.Text = rows.Cells[2].Value.ToString();
                Id_MonHoc = Convert.ToInt32(rows.Cells[0].Value.ToString());
                //txtMaMH.Text = rows.Cells[3].Value.ToString();
               
                txtMaMH.Text = rows.Cells[1].Value.ToString();
                enalbeButton(false);
                enableTextbox(true);
                btnXoa.Enabled = true;
                btnSua.Enabled = true;
                btnBoQua.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Vui lòng không click vào ô trống!!!!", "Thông báo", MessageBoxButtons.OK);
            }
        }
    }
}

