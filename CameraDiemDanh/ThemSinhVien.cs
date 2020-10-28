using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;

namespace CameraDiemDanh
{
    public partial class ThemSinhVien : Form
    {
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        //HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;
        MCvAvgComp[][] facesDetected;
        EigenObjectRecognizer recognizer;
        string a, b;
        bool isOpenCamera = false;

        SqlConnection conn = new SqlConnection("Data Source=KURO\\SQLEXPRESS;Initial Catalog=FaceRecog;Integrated Security=True");
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        DataSet dtSet = new DataSet();
        int Id_SinhVien;
        public ThemSinhVien()
        {
            InitializeComponent();
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split(' ');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);

                }

               
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                //MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Add Face Button).", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCamera_Click(object sender, EventArgs e)
        {

            isOpenCamera = true;
            if (isOpenCamera == true)
            {
                grabber = new Capture();
                grabber.QueryFrame();
                //Initialize the FrameGraber event
                Application.Idle += new EventHandler(FrameGrabber);
            }
            else
            {
                MessageBox.Show("Camera bị lỗi", "Thông báo");
            }
        }
        private void LoadCombo()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select IdKhoa,TenKhoa from Khoa", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbKhoa.DataSource = dt;
            cbKhoa.DisplayMember = "TenKhoa";
            cbKhoa.ValueMember = "TenKhoa";
            conn.Close();
        }
        private void LoadComboBox()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select IdLop,TenLop from Lop", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbLop.DataSource = dt;
            cbLop.DisplayMember = "TenLop";
            cbLop.ValueMember = "TenLop";
            conn.Close();
        }
            void FrameGrabber(object sender, EventArgs e)
        {

          //  label3.Text = "0";
            NamePersons.Add("");

            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(450, 300, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            facesDetected = gray.DetectHaarCascade(
            face,
            1.2,
            10,
            Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
            new Size(20, 20));

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                    //Eigen face recognizer
                    recognizer = new EigenObjectRecognizer(
                    trainingImages.ToArray(),
                    labels.ToArray(),
                    3000,
                    ref termCrit);

                    name = recognizer.Recognize(result);

                    //Draw the label for each face detected and recognized
                    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                }

                NamePersons[t - 1] = name;
                NamePersons.Add(",");
                //Set the number of faces detected on the scene
               // label3.Text = facesDetected[0].Length.ToString();
            }
            t = 0;
            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                names = names + NamePersons[nnn] + " ";
            }
            imageBoxFrameGrabber.Image = currentFrame;
            // label4.Text = name;
            names = "";
            NamePersons.Clear();
        }

            private void btnDong_Click(object sender, EventArgs e)
            {
                this.Close();
            }

            private void btnThem_Click(object sender, EventArgs e)
            {
               
                try
                {
                    //Trained face counter
                    ContTrain = ContTrain + 1;

                    //Get a gray frame from capture device
                    gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                    //Face Detector
                    facesDetected = gray.DetectHaarCascade(
                    face,
                    1.2,
                    10,
                    Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                    new Size(20, 20));

                    //Action for each element detected
                    foreach (MCvAvgComp f in facesDetected[0])
                    {
                        TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                        break;
                    }

                    //resize face detected image for force to compare the same size with the 
                    //test image with cubic interpolation type method
                    TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    trainingImages.Add(TrainedFace);
                    
                    labels.Add(txtHoTen.Text.ToUpper());

                   
                    //Show face added in gray scale
                    imageBox1.Image = TrainedFace;
                //       byte[] b = ImageToByteArray(imageBox1.Image);
                

                   
                   

                    //Write the number of triained faces in a file text for further load
                    File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                    //Write the labels of triained faces in a file text for further load
                    for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                    {
                        trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/User" + i + ".bmp");
                        File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                       
                    }


                    //string insert = "INSERT INTO SinhVien(IdSinhVien,HoTen,TenKhoa,TenLop,MSSV,HinhAnh) Values ( @IdSinhVien,@HoTen,@TenKhoa,@TenLop,@MSSV,@HinhAnh)";
                    //string mssv = txtMSSV.Text.Trim();

                    //int id = 10;
                    //string tenSinhVien = txtHoTen.Text.Trim();
                    //string LoadFaces;
                    //for (int tf = 1; tf < NumLabels + 1; tf++)
                    //{
                    //    LoadFaces = "face" + tf + ".bmp";
                    //     trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                        

                    //}
                    
                    //string khoa = cbKhoa.SelectedValue.ToString();
                    //string lop = cbLop.SelectedValue.ToString();
                    //SqlCommand insertCmd = new SqlCommand(insert, conn);
                    //conn.Close();
                    //conn.Open();

                    //insertCmd.Parameters.Add("IdSinhVien", SqlDbType.Int);
                    //insertCmd.Parameters.Add("TenKhoa", SqlDbType.NVarChar, 50);
                    //insertCmd.Parameters.Add("TenLop", SqlDbType.NVarChar, 50);
                    //insertCmd.Parameters.Add("HoTen", SqlDbType.NVarChar, 50);
                    //insertCmd.Parameters.Add("MSSV", SqlDbType.VarChar, 20);
                    //insertCmd.Parameters.Add("HinhAnh", SqlDbType.NVarChar, 200);

                    //insertCmd.Parameters["IdSinhVien"].Value = id;
                    //insertCmd.Parameters["TenKhoa"].Value = khoa;
                    //insertCmd.Parameters["TenLop"].Value = lop;
                    //insertCmd.Parameters["HoTen"].Value = tenSinhVien;
                    //insertCmd.Parameters["MSSV"].Value = mssv;
                    //insertCmd.Parameters["HinhAnh"].Value = image;
                    //insertCmd.ExecuteNonQuery();
                    //conn.Close();
                    ////      conSQL();
                    ////txtMonHoc.Text = null;
                    //conn.Close();
                    //MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                    //MessageBox.Show(textBox1.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    // MessageBox.Show("Enable the face detection first", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
               
             
            }
            byte[] ImageToByteArray(Image img)
            {
                MemoryStream m = new MemoryStream();
                img.Save(m,System.Drawing.Imaging.ImageFormat.Png);
                return m.ToArray();
            }
            private void ThemSinhVien_Load(object sender, EventArgs e)
            {
                LoadCombo();
                LoadComboBox();
            }
    }
}

       
