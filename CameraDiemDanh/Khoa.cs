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
    public partial class Khoa : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=KURO\\SQLEXPRESS;Initial Catalog=FaceRecog;Integrated Security=True");
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        DataSet dtSet = new DataSet();
        bool isChange = false;
        int Id_Khoa;
        public Khoa()
        {
            InitializeComponent();
        }
        private void conSQL()
        {
            conn.Open();
            string sql = "select IdKhoa,TenKhoa from Khoa";
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            conn.Close();  // đóng kết nối

            dgvKhoa.DataSource = dt; //đổ dữ liệu vào datagridview
        }

        private void Khoa_Load(object sender, EventArgs e)
        {
            conSQL();

            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            txtTenKhoa.ReadOnly = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
            btnThem.Enabled = false;
            txtTenKhoa.ReadOnly = false;
            
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
                    string tenKhoa = txtTenKhoa.Text.Trim();
                    string insert = "INSERT INTO Khoa(IdKhoa,TenKhoa) Values ( @IdKhoa,@TenKhoa)";

                    SqlCommand insertCmd = new SqlCommand(insert, conn);
                    conn.Close();
                    conn.Open();

                   
                    insertCmd.Parameters.Add("IdKhoa", SqlDbType.Int);
                    insertCmd.Parameters.Add("TenKhoa", SqlDbType.NVarChar, 50);

                    insertCmd.Parameters["IdKhoa"].Value = id;
                    insertCmd.Parameters["TenKhoa"].Value = tenKhoa;
                    insertCmd.ExecuteNonQuery();
                    conn.Close();
                    conSQL();
                    txtTenKhoa.Text = null;
                    conn.Close();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                }
            }
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            txtTenKhoa.ReadOnly = true;

        }

        private void dgvKhoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow rows = this.dgvKhoa.Rows[e.RowIndex];
                Id_Khoa = Convert.ToInt32(rows.Cells[0].Value.ToString());
                txtTenKhoa.Text = rows.Cells[1].Value.ToString();
                btnThem.Enabled = false;
                btnXoa.Enabled = true;
                btnSua.Enabled = true;
                btnLuu.Enabled = false; ;
                btnBoQua.Enabled = true;
                txtTenKhoa.ReadOnly = true;
            }
            catch
            {
                MessageBox.Show("Vui lòng không click vào ô trống!!!!", "Thông báo", MessageBoxButtons.OK);
            }
          
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtTenKhoa.ReadOnly = false;
            btnLuu.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            isChange = true;
            btnCapNhat.Visible = true;
            btnLuu.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if(dgvKhoa.CurrentRow.Cells["TenKhoa"].Value!= DBNull.Value)
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
           
                txtTenKhoa.Text = null;
                txtTenKhoa.ReadOnly = false;
                btnThem.Enabled = true;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false;
                btnBoQua.Enabled = false;
                btnSua.Enabled = false;
                conSQL();
            }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            txtTenKhoa.ReadOnly = true;
            btnSua.Enabled = false;
            txtTenKhoa.Text = null;
            btnCapNhat.Visible = false;
            
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (btnSua.Enabled == false)
            {
                conn.Open();
                string Update = "Update Khoa set TenKhoa=@TenKhoa where Id='" + Id_Khoa + "'";
                SqlCommand scmd = new SqlCommand(Update, conn);
                //scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@Id", Id_Khoa);
                scmd.Parameters.AddWithValue("@TenKhoa", txtTenKhoa.Text);
                scmd.ExecuteNonQuery();
                MessageBox.Show("Thay đổi thành công", "Thông báo", MessageBoxButtons.OK);
                conn.Close();
                conSQL();
                btnCapNhat.Visible = false;
                btnBoQua.Enabled = false;
                
            }
        }
           
        }
    }
