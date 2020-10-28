namespace CameraDiemDanh
{
    partial class frmQuanLy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pnlFormTrong = new System.Windows.Forms.Panel();
            this.btnSinhVien = new System.Windows.Forms.Button();
            this.btnLop = new System.Windows.Forms.Button();
            this.btnMonHoc = new System.Windows.Forms.Button();
            this.btnKhoa = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSinhVien);
            this.groupBox1.Controls.Add(this.btnLop);
            this.groupBox1.Controls.Add(this.btnMonHoc);
            this.groupBox1.Controls.Add(this.btnKhoa);
            this.groupBox1.Location = new System.Drawing.Point(77, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1011, 102);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // pnlFormTrong
            // 
            this.pnlFormTrong.Location = new System.Drawing.Point(12, 120);
            this.pnlFormTrong.Name = "pnlFormTrong";
            this.pnlFormTrong.Size = new System.Drawing.Size(1186, 537);
            this.pnlFormTrong.TabIndex = 1;
            // 
            // btnSinhVien
            // 
            this.btnSinhVien.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSinhVien.Image = global::CameraDiemDanh.Properties.Resources.man_icon;
            this.btnSinhVien.Location = new System.Drawing.Point(335, 10);
            this.btnSinhVien.Name = "btnSinhVien";
            this.btnSinhVien.Size = new System.Drawing.Size(111, 86);
            this.btnSinhVien.TabIndex = 0;
            this.btnSinhVien.Text = "Sinh Viên";
            this.btnSinhVien.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSinhVien.UseVisualStyleBackColor = true;
            this.btnSinhVien.Click += new System.EventHandler(this.btnSinhVien_Click);
            // 
            // btnLop
            // 
            this.btnLop.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLop.Image = global::CameraDiemDanh.Properties.Resources.Actions_view_list_details_icon;
            this.btnLop.Location = new System.Drawing.Point(189, 10);
            this.btnLop.Name = "btnLop";
            this.btnLop.Size = new System.Drawing.Size(120, 86);
            this.btnLop.TabIndex = 0;
            this.btnLop.Text = "Lớp";
            this.btnLop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnLop.UseVisualStyleBackColor = true;
            this.btnLop.Click += new System.EventHandler(this.btnLop_Click);
            // 
            // btnMonHoc
            // 
            this.btnMonHoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMonHoc.Image = global::CameraDiemDanh.Properties.Resources.Actions_view_list_tree_icon;
            this.btnMonHoc.Location = new System.Drawing.Point(476, 10);
            this.btnMonHoc.Name = "btnMonHoc";
            this.btnMonHoc.Size = new System.Drawing.Size(120, 86);
            this.btnMonHoc.TabIndex = 0;
            this.btnMonHoc.Text = "Môn Học";
            this.btnMonHoc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMonHoc.UseVisualStyleBackColor = true;
            this.btnMonHoc.Click += new System.EventHandler(this.btnMonHoc_Click);
            // 
            // btnKhoa
            // 
            this.btnKhoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKhoa.Image = global::CameraDiemDanh.Properties.Resources.Actions_view_list_icons_icon;
            this.btnKhoa.Location = new System.Drawing.Point(44, 10);
            this.btnKhoa.Name = "btnKhoa";
            this.btnKhoa.Size = new System.Drawing.Size(120, 86);
            this.btnKhoa.TabIndex = 0;
            this.btnKhoa.Text = "Khoa";
            this.btnKhoa.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnKhoa.UseVisualStyleBackColor = true;
            this.btnKhoa.Click += new System.EventHandler(this.btnKhoa_Click);
            // 
            // frmQuanLy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1210, 669);
            this.ControlBox = false;
            this.Controls.Add(this.pnlFormTrong);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmQuanLy";
            this.Text = "frmQuanLy";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSinhVien;
        private System.Windows.Forms.Button btnMonHoc;
        private System.Windows.Forms.Button btnLop;
        private System.Windows.Forms.Button btnKhoa;
        private System.Windows.Forms.Panel pnlFormTrong;
    }
}