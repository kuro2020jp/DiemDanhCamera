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
    public partial class frmThemSinhVien : Form
    {
        #region Varibles
        SqlConnection conn = new SqlConnection("Data Source=kuro\\sqlexpress;Initial Catalog=FaceRecog;Integrated Security=True");
        private Capture videoCapture = null;
        private Image<Bgr, Byte> currentFrame = null;
        Mat frame = new Mat();
        private bool facesDetectionEnable = false;
        Image<Bgr, Byte> faceResult = null;

        List<string> PersonsNames = new List<string>();
        bool EnableSaveImage = false;
        private static bool isTrained = false;
        EigenFaceRecognizer recognizer;

        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();

        CascadeClassifier faceCasacdeClassifier = new CascadeClassifier(@"E:\HomeWork\AppDiemDanh\AppDiemDanh\haarcascade_frontalface_alt.xml");
        List<Image<Gray, Byte>> TrainedFaces = new List<Image<Gray, byte>>();
        List<int> PersonsLabes = new List<int>();

        #endregion
        public frmThemSinhVien()
        {
            InitializeComponent();
        }
        private void LoadDLComBo()
        {
            conn.Open();
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
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select IdLop,TenLop from Lop", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbLop.DataSource = dt;
            cbLop.DisplayMember = "TenLop";
            cbLop.ValueMember = "IdLop";
            conn.Close();
        }
        private void btnCamera_Click(object sender, EventArgs e)
        {
            if (videoCapture != null) videoCapture.Dispose();
            videoCapture = new Capture();
            Application.Idle += ProcessFrame;
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
                            // CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Red).MCvScalar, 2);

                            //Step 3: Add Person 
                            //Assign the face to the picture Box face picDetected
                            Image<Bgr, Byte> resultImage = currentFrame.Convert<Bgr, Byte>();
                            resultImage.ROI = face;
                            picDetected.SizeMode = PictureBoxSizeMode.StretchImage;
                            picDetected.Image = resultImage.Bitmap;
                            if (EnableSaveImage)
                            {
                                //We will create a directory if does not exists!
                                string path = Directory.GetCurrentDirectory() + @"\TrainedImages";
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                //we will save 10 images with delay a second for each image 
                                //to avoid hang GUI we will create a new task
                                Task.Factory.StartNew(() => {
                                    for (int i = 0; i < 1; i++)
                                    {
                                        //resize the image then saving it
                                        resultImage.Resize(200, 200, Inter.Cubic).Save(path + @"\" + txtMSSV.Text + "_" +txtHoTen.Text + ".jpg");
                                        Thread.Sleep(1000);
                                    }
                                });

                            }
                            EnableSaveImage = false;


                         
                         
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
       
        private bool NhanDienHinhAnh()
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
                    ImagesCount++;
                    Debug.WriteLine(ImagesCount + ". " + name);
                }

                if (TrainedFaces.Count() > 0)
                {
                    // recognizer = new EigenFaceRecognizer(ImagesCount,Threshold);
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

        private void btnThem_Click(object sender, EventArgs e)
        {
                conn.Open();
                SqlCommand Check_Data = new SqlCommand("Select MSSV from SinhVien,Lop where SinhVien.IdLop=Lop.IdLop and MSSV=@MSSV ", conn);
                Check_Data.Parameters.AddWithValue("@MSSV", txtMSSV.Text);
                SqlDataReader reader = Check_Data.ExecuteReader();
                
                if (reader.HasRows)
                {
                    MessageBox.Show("Sinh Viên đã tồn tại");
                    conn.Close();
                }
                else
                {
                    conn.Close();
                    string insert = "insert into SinhVien(IdSinhVien,HoTen,MSSV,NamSinh,HinhAnh,IdLop,IdKhoa) values (@IdSinhVien,@HoTen,@MSSV,@NamSinh,@HinhAnh,@IdLop,@IdKhoa)";
                    SqlCommand insertCmd = new SqlCommand(insert, conn);
                    int id = 1;
                    //byte[] b = ChuyenHinhThanhByte(picDetected.Image);
                    conn.Open();
                    string duongdan = Directory.GetCurrentDirectory() + @"\TrainedImages";
                    string hinh = @"\" + txtMSSV.Text + "_" + txtHoTen.Text + ".jpg";
                    string hinhanh = duongdan + hinh;
                    insertCmd.Parameters.AddWithValue("@IdSinhVien", id);
                    insertCmd.Parameters.AddWithValue("@MSSV", txtMSSV.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@HinhAnh", hinhanh);
                    insertCmd.Parameters.AddWithValue("@IdLop", Convert.ToInt32(cbLop.SelectedValue.ToString()));
                    insertCmd.Parameters.AddWithValue("@IdKhoa", Convert.ToInt32(cbKhoa.SelectedValue.ToString()));
                    insertCmd.Parameters.AddWithValue("@NamSinh", dtNgaySinh.Text.Trim());
                    insertCmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                    EnableSaveImage = true;

                    NhanDienHinhAnh();
                }
        }

        private void frmThemSinhVien_Load(object sender, EventArgs e)
        {
            LoadDLCB();
            LoadDLComBo();
            cbLop.Enabled = false;
        }
        //Image ChuyenVeHinh(byte[] b)
        //{
        //    MemoryStream m = new MemoryStream(b);
        //    return Image.FromStream(m);
        //}

        //byte[] ChuyenHinhThanhByte(Image img)
        //{
        //    MemoryStream m = new MemoryStream();
        //    img.Save(m, System.Drawing.Imaging.ImageFormat.Png);
        //    return m.ToArray();
        //}

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
            if(btnCamera.Enabled==true)
                videoCapture.Dispose();
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

        private void btnDetected_Click(object sender, EventArgs e)
        {
            facesDetectionEnable=true;

        }
    }
}
