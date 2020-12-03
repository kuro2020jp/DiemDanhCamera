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
    public partial class frmBuoiHoc : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=KURO\\SQLEXPRESS;Initial Catalog=FaceRecog;Integrated Security=True");
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        DataSet dtSet = new DataSet();
        int Id_Buoi;
        public frmBuoiHoc()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            // conn.Open();
            string sql = "Select Buoi,IdBuoi,IdMonHoc from BuoiHoc";  // lay het du lieu trong bang lop
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            conn.Close();  // đóng kết nối
           // dgvBuoi.DataSource = dt; //đổ dữ liệu vào datagridview
        }

        private void frmBuoiHoc_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadCB();
            LoadCBLop();
            cbMonHoc.Enabled = false;
            cbMonHoc.Text = null;
            cbLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMonHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            enalbeButton(false);
            btnThem.Enabled = true;
            enableTextbox(true);
        }
        private void LoadCB()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select IdMonHoc,TenMH from MonHoc", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbMonHoc.DataSource = dt;
            cbMonHoc.DisplayMember = "TenMH";
            cbMonHoc.ValueMember = "IdMonHoc";
            conn.Close();
        }
        private void LoadCBLop()
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
            txtSoBuoi.ReadOnly = enabletxt;
            if (enabletxt == true)
            {
                cbLop.DropDownStyle = ComboBoxStyle.DropDownList;
                cbMonHoc.DropDownStyle = ComboBoxStyle.DropDownList;

            }

        }
        private void nullTextbox()
        {
            txtSoBuoi.Text = null;
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
            cbMonHoc.DataSource = dt;
            conn.Close();
            cbMonHoc.Enabled = true;
            cbMonHoc.Text = null;
            txtSoBuoi.Text = null;
        }

        private void cbMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {

            string b = cbMonHoc.Text;
            string sql = "Select IdBuoi,MaMH,TenMH,Buoi from MonHoc,BuoiHoc where MonHoc.IdMonHoc=BuoiHoc.IdMonHoc and TenMH = N'" + b + "'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            dgvBuoi.DataSource = dt;
            conn.Close();
            txtSoBuoi.Text = null;
        }
        private void LoadA()
        {
            string b = cbMonHoc.Text;
            string sql = "Select IdBuoi,MaMH,TenMH,Buoi from MonHoc,BuoiHoc where MonHoc.IdMonHoc=BuoiHoc.IdMonHoc and TenMH = N'" + b + "'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            dgvBuoi.DataSource = dt;
            conn.Close();
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
            //isChange = true;
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
                SqlCommand scmd = new SqlCommand("Delete from BuoiHoc where IdBuoi='"+Id_Buoi+"'", conn);
                scmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);
                LoadA();
            }
            enalbeButton(false);
            enableTextbox(true);
            nullTextbox();
            btnThem.Enabled = true;
            //LoadDataAgain();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand Check_Data = new SqlCommand("Select distinct Buoi,TenMH,TenLop from BuoiHoc,MonHoc,Lop where TenLop=N'"+ cbLop.Text+"' and TenMH=N'"+cbMonHoc.Text+"' and Buoi=@Buoi", conn);

            Check_Data.Parameters.AddWithValue("@Buoi", txtSoBuoi.Text);
            SqlDataReader reader = Check_Data.ExecuteReader();

            if (reader.HasRows)
            {
                MessageBox.Show("Buổi Học đã tồn tại");
                conn.Close();
            }
            else
            {
                if (btnLuu.Enabled == true)
                {

                    string insert = "INSERT INTO BuoiHoc(IdMonHoc,IdLop,Buoi) Values (@IdMonHoc,@IdLop,@Buoi)";
                    SqlCommand insertCmd = new SqlCommand(insert, conn);
                    conn.Close();
                    conn.Open();

                     insertCmd.Parameters.AddWithValue("@IdMonHoc", Convert.ToInt32(cbMonHoc.SelectedValue.ToString()));
                  
                    insertCmd.Parameters.AddWithValue("@Buoi", txtSoBuoi.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@IdLop", Convert.ToInt32(cbLop.SelectedValue.ToString()));


                    insertCmd.ExecuteNonQuery();
                    conn.Close();
                   
                    conn.Close();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                    LoadA();
                }
            }
            enableTextbox(true);
            enalbeButton(false);
            btnThem.Enabled = true;
        }

        private void dgvBuoi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow rows = this.dgvBuoi.Rows[e.RowIndex];
               
                //MessageBox.Show(rows.Cells[1].Value.ToString());
                //MessageBox.Show(rows.Cells[2].Value.ToString());
                //MessageBox.Show(rows.Cells[3].Value.ToString());
                //MessageBox.Show(rows.Cells[4].Value.ToString());
                //cbMonHoc.Text = rows.Cells[3].Value.ToString();
                Id_Buoi = Convert.ToInt32(rows.Cells[1].Value.ToString());
                txtSoBuoi.Text = rows.Cells[4].Value.ToString();
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

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (btnSua.Enabled == false)
            {
                conn.Open();
                string Update = "Update BuoiHoc set IdMonHoc=@IdMonHoc, Buoi=@Buoi where IdBuoi='" + Id_Buoi + "'";
                SqlCommand scmd = new SqlCommand(Update, conn);
                scmd.Parameters.AddWithValue("@IdMonHoc", Convert.ToInt32(cbMonHoc.SelectedValue.ToString()));
                scmd.Parameters.AddWithValue("@Buoi", txtSoBuoi.Text);
                scmd.ExecuteNonQuery();
                MessageBox.Show("Thay đổi thành công", "Thông báo", MessageBoxButtons.OK);
                conn.Close();
                LoadA();
                btnCapNhat.Visible = false;
                enalbeButton(false);
                enableTextbox(true);
                nullTextbox();
                btnThem.Enabled = true;
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            enalbeButton(false);
            btnThem.Enabled = true;
            btnCapNhat.Visible = false;
        }
    }
}
