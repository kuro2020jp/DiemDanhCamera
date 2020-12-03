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
    public partial class frmDiemDanh : Form
    {
        #region Var
        SqlConnection conn = new SqlConnection("Data Source=KURO\\SQLEXPRESS;Initial Catalog=FaceRecog;Integrated Security=True");
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        DataSet dtSet = new DataSet();
        int Id_SinhVien;
        string buoi;
        string TenSV;

        //nhận diện
        private Capture videoCapture = null;
        private Image<Bgr, Byte> currentFrame = null;
        Mat frame = new Mat();
        private bool facesDetectionEnable = false;
        Image<Bgr, Byte> faceResult = null;   
        bool EnableSaveImage = false;
        private bool facesDetectionEnabled = false;
        private static bool isTrained = false;
        EigenFaceRecognizer recognizer;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        CascadeClassifier faceCasacdeClassifier = new CascadeClassifier(@"E:\HomeWork\AppDiemDanh\AppDiemDanh\haarcascade_frontalface_alt.xml");
        List<Image<Gray, Byte>> TrainedFaces = new List<Image<Gray, byte>>();
        List<string> PersonsNames = new List<string>();
        List<string> PersonMSSV = new List<string>();
        List<int> PersonsLabes = new List<int>();
        List<int> MSSVLabes = new List<int>();
        #endregion
        public frmDiemDanh()
        {
            InitializeComponent();
        }
        private void LoadCBKhoa()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select IdKhoa,TenKhoa from Khoa", conn);
            DataTable dtKhoa = new DataTable();
            da.Fill(dtKhoa);
            cbKhoa.DataSource = dtKhoa;
            cbKhoa.DisplayMember = "TenKhoa";
            cbKhoa.ValueMember = "IdKhoa";
            conn.Close();
        }
        private void LoadCBLop()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select IdLop,TenLop from Lop", conn);
            DataTable dtLop = new DataTable();
            da.Fill(dtLop);
            cbLop.DataSource = dtLop;
            cbLop.DisplayMember = "TenLop";
            cbLop.ValueMember = "IdLop";
            conn.Close();
        }
        private void LoadCBMonHoc()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select IdMonHoc,TenMH from MonHoc", conn);
            DataTable dtMon = new DataTable();
            da.Fill(dtMon);
            cbMonHoc.DataSource = dtMon;
            cbMonHoc.DisplayMember = "TenMH";
            cbMonHoc.ValueMember = "IdMonHoc";
            conn.Close();
        }
        private void LoadCBBuoi()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select IdBuoi,Buoi from BuoiHoc", conn);
            DataTable dtBuoi = new DataTable();
            da.Fill(dtBuoi);
            cbBuoi  .DataSource = dtBuoi;
            cbBuoi.DisplayMember = "Buoi";
            cbBuoi.ValueMember = "IdBuoi";
            conn.Close();
        }
        private void frmDiemDanh_Load(object sender, EventArgs e)
        {
            LoadCBLop();
            LoadCBKhoa();
            LoadCBMonHoc();
            LoadCBBuoi();
            cbMonHoc.Enabled = false;
            cbLop.Enabled = false;
            cbBuoi.Enabled = false;
            dgvDiemDanh.ReadOnly = true;
            StyteCB();
            ClearCB();
            LoadTime();
            listBox1.DataSource = null;
            btnStart.Enabled = false;
        }
        private void LoadTime()
        {
            label17.Text = DateTime.Now.ToLongDateString();
            label18.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void cbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            string c = cbKhoa.Text;
            string sql = "Select * from Lop,Khoa where Khoa.IdKhoa=Lop.IdKhoa and TenKhoa = N'" + c + "'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dtLop = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dtLop);  // đổ dữ liệu vào kho
            cbLop.DataSource = dtLop;
            conn.Close();
            cbLop.Enabled = true;
            cbLop.Text = null;
            cbMonHoc.Enabled = false;
            cbBuoi.Enabled = false;
            cbBuoi.Text = null;
            dgvDiemDanh.DataSource = null;
            btnStart.Enabled = false;
          
        }
        private void LoadList()
        {
            string lop = cbLop.Text;
            string buoi = cbBuoi.Text;
            string sql = "select distinct MSSV,HoTen,ThoiGian from DiemDanh,SinhVien,Lop,BuoiHoc,MonHoc where Lop.IdLop=SinhVien.IdLop and SinhVien.MSSV=DiemDanh.MaSoSV and DiemDanh.IdMonHoc=MonHoc.IdMonHoc and Lop.TenLop='" + lop + "' and DiemDanh.CoMat='1' and DiemDanh.BuoiHoc='" + buoi + "'";
            SqlCommand com = new SqlCommand(sql, conn);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dtList = new DataTable();
            da.Fill(dtList);
            listBox1.DataSource = dtList;
            listBox1.DisplayMember = "HoTen";
            listBox1.ValueMember = "MSSV";
           // listBox1.Items.Add(dtList);


        }
        private void cbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = cbLop.Text;
            string sql = "Select IdMonHoc,TenMH,TenLop from MonHoc,Lop where Lop.IdLop=MonHoc.IdLop and TenLop = N'" + a + "'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dtMon = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dtMon);  // đổ dữ liệu vào kho
            cbMonHoc.DataSource = dtMon;
            conn.Close();
            cbMonHoc.Enabled = true;     
              cbBuoi.Enabled = false;
            dgvDiemDanh.DataSource = null;
            cbBuoi.Text = null;
            cbMonHoc.Text = null;
            btnStart.Enabled = false;
        }

        private void cbMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string b = cbMonHoc.Text;
            string sql = "Select IdBuoi,MaMH,TenMH,Buoi from MonHoc,BuoiHoc where MonHoc.IdMonHoc=BuoiHoc.IdMonHoc and TenMH = N'" + b + "'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dtBuoi = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dtBuoi);  // đổ dữ liệu vào kho
            cbBuoi.DataSource = dtBuoi;
            conn.Close();
            cbBuoi.Enabled = true;     
            dgvDiemDanh.DataSource = null;
            cbBuoi.Text = null;
            btnStart.Enabled = false;

        }

        private void cbBuoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            string b = cbBuoi.Text;
            string d = cbLop.Text;

            string sql = "select MSSV,HoTen,TenMH,HinhAnh from BuoiHoc,MonHoc,Lop,SinhVien where Lop.IdLop=SinhVien.IdLop and Lop.IdLop=MonHoc.IdLop and Lop.IdLop=BuoiHoc.IdLop and BuoiHoc.Buoi='"+b+"' and Lop.TenLop='"+d+"'";  // lay het du lieu trong bang sinh vien
            SqlCommand com = new SqlCommand(sql, conn); //bat dau truy van
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com); //chuyen du lieu ve
            DataTable dt = new DataTable(); //tạo một kho ảo để lưu trữ dữ liệu
            da.Fill(dt);  // đổ dữ liệu vào kho
            dgvDiemDanh.DataSource = dt;
            conn.Close();
            LoadList();
            btnStart.Enabled = true;

        }
       private  void ClearCB()
        {
            cbLop.Text = null;
            cbMonHoc.Text = null ;
            cbBuoi.Text = null;
        }
        private void StyteCB()
        {
            cbKhoa.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMonHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBuoi.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnDiemDanh_Click(object sender, EventArgs e)
        {
            string insert = "INSERT INTO DiemDanh(IdLop,MaSoSV,IdMonHoc,CoMat,BuoiHoc,ThoiGian) Values (@IdLop,@MaSoSV,@IdMonHoc,@CoMat,@BuoiHoc,@ThoiGian)";
            SqlCommand insertCmd = new SqlCommand(insert, conn);
            conn.Open();
            int id = dgvDiemDanh.Rows.Count;

            //insertCmd.Parameters.AddWithValue("@IdBuoi",);
            insertCmd.Parameters.AddWithValue("@IdLop", Convert.ToInt32(cbLop.SelectedValue.ToString()));
            insertCmd.Parameters.AddWithValue("@MaSoSV", label8.Text.Trim());
            insertCmd.Parameters.AddWithValue("@CoMat", 1);
            insertCmd.Parameters.AddWithValue("@IdMonHoc", Convert.ToInt32(cbMonHoc.SelectedValue.ToString()));
            insertCmd.Parameters.AddWithValue("@BuoiHoc",  Convert.ToInt32(cbBuoi.Text.Trim()));
            insertCmd.Parameters.AddWithValue("@ThoiGian", DateTime.Now.ToShortDateString());
            insertCmd.ExecuteNonQuery();
            conn.Close();
           
            if((MessageBox.Show("Điểm danh thành công", "Thông báo", MessageBoxButtons.OK))==DialogResult.OK)
            {
                LoadList();
            }    
        }

        private void dgvDiemDanh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow rows = this.dgvDiemDanh.Rows[e.RowIndex];
                
                //txtMonHoc.Text = rows.Cells[2].Value.ToString();
                Id_SinhVien = Convert.ToInt32(rows.Cells[1].Value.ToString());
                 buoi = rows.Cells[4].Value.ToString();
                TenSV = rows.Cells[2].Value.ToString();

                //MessageBox.Show("id=" + rows.Cells[1].Value.ToString()); //mssv
                //MessageBox.Show("id=" + rows.Cells[2].Value.ToString()); // ten
                //MessageBox.Show("id=" + rows.Cells[3].Value.ToString()); //mon
                //MessageBox.Show("id=" + rows.Cells[4].Value.ToString()); //buoi
            }
            catch
            {
                MessageBox.Show("Vui lòng không click vào ô trống!!!!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (videoCapture != null) videoCapture.Dispose();
            videoCapture = new Capture();
            Application.Idle += ProcessFrame;
            facesDetectionEnable = true;
            TrainImagesFromDir();
            btnDiemDanh.Enabled = true;
            btnThongKe.Enabled = true;
            btnStart.Enabled = false;

        }
        private void ProcessFrame(object sender, EventArgs e)
        {
            //Step 1: Video Capture
            if (videoCapture != null && videoCapture.Ptr != IntPtr.Zero)
            {
                videoCapture.Retrieve(frame, 0);
                currentFrame = frame.ToImage<Bgr, Byte>().Resize(picCapture.Width, picCapture.Height, Inter.Cubic);

                //Step 2: Face Detection
                if (facesDetectionEnable)
                {

                    //Convert from Bgr to Gray Image
                    Mat grayImage = new Mat();
                    CvInvoke.CvtColor(currentFrame, grayImage, ColorConversion.Bgr2Gray);
                    //Enhance the image to get better result
                    CvInvoke.EqualizeHist(grayImage, grayImage);

                    Rectangle[] faces = faceCasacdeClassifier.DetectMultiScale(grayImage, 1.1, 3, Size.Empty, Size.Empty);
                    //If faces detected
                    if (faces.Length > 0)
                    {

                        foreach (var face in faces)
                        {
                            //Draw square around each face 
                             CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Red).MCvScalar, 2);
                          

                            //Step 3: Add Person 
                            //Assign the face to the picture Box face picDetected
                            Image<Bgr, Byte> resultImage = currentFrame.Convert<Bgr, Byte>();
                            resultImage.ROI = face;
                            picDetected.SizeMode = PictureBoxSizeMode.StretchImage;
                            picDetected.Image = resultImage.Bitmap;                         

                            // Step 5: Recognize the face 
                            if (isTrained)
                            {
                                Image<Gray, Byte> grayFaceResult = resultImage.Convert<Gray, Byte>().Resize(200, 200, Inter.Cubic);
                                CvInvoke.EqualizeHist(grayFaceResult, grayFaceResult);
                                var result = recognizer.Predict(grayFaceResult);
                                pictureBox1.Image = grayFaceResult.Bitmap;                                                           
                                pictureBox2.Image = TrainedFaces[result.Label].Bitmap;
                                Debug.WriteLine(result.Label + ". " + result.Distance);
                             
                              
                                //Here results found known faces
                                if (result.Label !=-1 && result.Distance < 7000)
                                {                                   
                                    CvInvoke.PutText(currentFrame, PersonsNames[result.Label], new Point(face.X - 2, face.Y - 2),
                                        FontFace.HersheyComplex, 1.0, new Bgr(Color.Orange).MCvScalar);
                                    CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Green).MCvScalar, 2);
                                    label8.Text = PersonsNames[result.Label];
                                }
                                //here results did not found any know faces
                                else
                                {
                                    CvInvoke.PutText(currentFrame, "Unknown", new Point(face.X - 2, face.Y - 2),
                                        FontFace.HersheyComplex, 1.0, new Bgr(Color.Orange).MCvScalar);
                                    CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Red).MCvScalar, 2);
                                    label8.Text = "Unknown";
                                }
                            }
                        }
                    }
                }

                //Render the video capture into the Picture Box picCapture
                picCapture.Image = currentFrame.Bitmap;
            }

            //Dispose the Current Frame after processing it to reduce the memory consumption.
            if (currentFrame != null)
                currentFrame.Dispose();
        }
        
        Image ChuyenVeHinh(byte[] b)
        {
            MemoryStream m = new MemoryStream(b);
            return Image.FromStream(m);
        }

        private bool TrainImagesFromDir()
        {
            int ImagesCount = 0;
            double Threshold = 7000;
            TrainedFaces.Clear();
            PersonsLabes.Clear();
            PersonsNames.Clear();
            try
            {
                string path = Directory.GetCurrentDirectory() + @"\TrainedImages";
                string[] files = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    Image<Gray, byte> trainedImage = new Image<Gray, byte>(file).Resize(200, 200, Inter.Cubic);
                    CvInvoke.EqualizeHist(trainedImage, trainedImage);
                    TrainedFaces.Add(trainedImage);
                    PersonsLabes.Add(ImagesCount);
                  
                    string name = file.Split('\\').Last().Split('_')[0];          
                    PersonsNames.Add(name);
                    ImagesCount += 1;
                    Debug.WriteLine(ImagesCount + ". " + name);
                    
                
                }
                
                if (TrainedFaces.Count > 0)
                {
                    recognizer = new EigenFaceRecognizer(ImagesCount, Threshold);
                    recognizer.Train(TrainedFaces.ToArray(), PersonsLabes.ToArray());

                    isTrained = true;
                    //Debug.WriteLine(ImagesCount);
                    //Debug.WriteLine(isTrained);
                    return true;
                }
                else
                {
                    isTrained = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                isTrained = false;
                MessageBox.Show("Error in Train Images: " + ex.Message);
                return false;
            }

        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            
            label15.Text = Convert.ToString(listBox1.Items.Count) + "/" + Convert.ToString(dgvDiemDanh.Rows.Count-1);
            label16.Text = Convert.ToString((dgvDiemDanh.Rows.Count-1) - listBox1.Items.Count);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label18.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }
    }
}
