USE [FaceRecog]
GO
/****** Object:  Table [dbo].[Khoa]    Script Date: 12/03/2020 14:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Khoa](
	[IdKhoa] [int] NOT NULL,
	[MaKhoa] [nvarchar](50) NULL,
	[TenKhoa] [nvarchar](50) NULL,
 CONSTRAINT [PK_Khoa] PRIMARY KEY CLUSTERED 
(
	[IdKhoa] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Khoa] ([IdKhoa], [MaKhoa], [TenKhoa]) VALUES (1, N'MK01', N'Công Nghệ')
INSERT [dbo].[Khoa] ([IdKhoa], [MaKhoa], [TenKhoa]) VALUES (2, N'MK02', N'Luật')
INSERT [dbo].[Khoa] ([IdKhoa], [MaKhoa], [TenKhoa]) VALUES (3, N'MK03', N'Dược')
/****** Object:  Table [dbo].[DiemDanh]    Script Date: 12/03/2020 14:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DiemDanh](
	[IdBuoi] [int] NULL,
	[IdLop] [int] NULL,
	[MaSoSV] [varchar](20) NULL,
	[IdMonHoc] [int] NULL,
	[CoMat] [int] NULL,
	[BuoiHoc] [nvarchar](50) NULL,
	[ThoiGian] [date] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[DiemDanh] ([IdBuoi], [IdLop], [MaSoSV], [IdMonHoc], [CoMat], [BuoiHoc], [ThoiGian]) VALUES (NULL, 1, N'178274', 1, 1, N'1', CAST(0xC9410B00 AS Date))
/****** Object:  StoredProcedure [dbo].[DangNhap_Search]    Script Date: 12/03/2020 14:47:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[DangNhap_Search](
	@userName nvarchar(50),
	@passWord nvarchar(50)
)
as
begin
	select * from DangNhap where userName=@userName and passWord=@passWord
End
GO
/****** Object:  StoredProcedure [dbo].[DangNhap_Check]    Script Date: 12/03/2020 14:47:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[DangNhap_Check](
	@userName nvarchar(50)
)
as
begin
	select * from DangNhap where userName=@userName
End
GO
/****** Object:  Table [dbo].[BuoiHoc]    Script Date: 12/03/2020 14:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BuoiHoc](
	[IdBuoi] [int] IDENTITY(1,1) NOT NULL,
	[IdLop] [int] NULL,
	[IdMonHoc] [int] NULL,
	[Buoi] [nvarchar](50) NULL,
 CONSTRAINT [PK_BuoiHoc] PRIMARY KEY CLUSTERED 
(
	[IdBuoi] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[BuoiHoc] ON
INSERT [dbo].[BuoiHoc] ([IdBuoi], [IdLop], [IdMonHoc], [Buoi]) VALUES (1, 1, 1, N'1')
INSERT [dbo].[BuoiHoc] ([IdBuoi], [IdLop], [IdMonHoc], [Buoi]) VALUES (2, 4, 2, N'1')
INSERT [dbo].[BuoiHoc] ([IdBuoi], [IdLop], [IdMonHoc], [Buoi]) VALUES (3, 1, 1, N'2')
INSERT [dbo].[BuoiHoc] ([IdBuoi], [IdLop], [IdMonHoc], [Buoi]) VALUES (4, 4, 2, N'')
SET IDENTITY_INSERT [dbo].[BuoiHoc] OFF
/****** Object:  StoredProcedure [dbo].[Admin_Login]    Script Date: 12/03/2020 14:47:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Admin_Login](
	@AdminName nvarchar(50),
	@AdminPass nvarchar(50)
)
as
begin
	select * from AdminLogin where AdminName=@AdminName and AdminPass=@AdminPass
end
GO
/****** Object:  StoredProcedure [dbo].[UserDelete]    Script Date: 12/03/2020 14:47:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[UserDelete]
@IDName int
as
begin
	delete from DangNhap
	where IDName=@IDName
	end
GO
/****** Object:  StoredProcedure [dbo].[TruyenSearch]    Script Date: 12/03/2020 14:47:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[TruyenSearch]
@Search nvarchar(50)
as
begin	
select * from Truyen where @Search=TenTruyen and TenTruyen like '%Search%'
end
GO
/****** Object:  StoredProcedure [dbo].[TruyenDelete]    Script Date: 12/03/2020 14:47:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[TruyenDelete]
@IDT int
as
begin
	delete from Truyen
	where IDT=@IDT
	end
GO
/****** Object:  StoredProcedure [dbo].[TruyenCreateOrUpdate]    Script Date: 12/03/2020 14:47:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[TruyenCreateOrUpdate]
@IDT int,
@TenTruyen nvarchar(max),
@TacGia nvarchar(50),
@Mota nvarchar(max),
@NoiDung nvarchar(max)
As
begin
if (@IDT=0)
	begin
		insert into Truyen(TenTruyen,TacGia,MoTa,NoiDung)
		values (@TenTruyen,@TacGia,@Mota,@NoiDung)
		select SCOPE_IDENTITY() as LastID
		end
else
	begin
	UPDATE Truyen
	set
		TenTruyen=@TenTruyen,
		TacGia=@TacGia,
		MoTa=@Mota,
		NoiDung=@NoiDung
		where IDT=@IDT
		end
	
end
GO
/****** Object:  Table [dbo].[SinhVien]    Script Date: 12/03/2020 14:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SinhVien](
	[IdSinhVien] [int] NOT NULL,
	[HoTen] [nvarchar](50) NOT NULL,
	[MSSV] [varchar](20) NOT NULL,
	[NamSinh] [date] NULL,
	[HinhAnh] [nvarchar](100) NULL,
	[IdKhoa] [int] NULL,
	[IdLop] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[SinhVien] ([IdSinhVien], [HoTen], [MSSV], [NamSinh], [HinhAnh], [IdKhoa], [IdLop]) VALUES (1, N'Nguyễn Hải Toàn', N'178274', CAST(0xD3230B00 AS Date), N'E:\HomeWork\AppDiemDanh\AppDiemDanh\bin\Debug\TrainedImages\178274_Nguyễn Hải Toàn.jpg', 1, 1)
INSERT [dbo].[SinhVien] ([IdSinhVien], [HoTen], [MSSV], [NamSinh], [HinhAnh], [IdKhoa], [IdLop]) VALUES (1, N'Trương Tiến Minh', N'177300', CAST(0x42230B00 AS Date), N'E:\HomeWork\AppDiemDanh\AppDiemDanh\bin\Debug\TrainedImages\177300_Trương Tiến Minh.jpg', 1, 1)
INSERT [dbo].[SinhVien] ([IdSinhVien], [HoTen], [MSSV], [NamSinh], [HinhAnh], [IdKhoa], [IdLop]) VALUES (1, N'Lương Nguyễn Ngọc Tài', N'1754215', CAST(0x40230B00 AS Date), N'E:\HomeWork\AppDiemDanh\AppDiemDanh\bin\Debug\TrainedImages\1754215_Lương Nguyễn Ngọc Tài.jpg', 1, 1)
/****** Object:  Table [dbo].[MonHoc]    Script Date: 12/03/2020 14:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonHoc](
	[IdMonHoc] [int] NOT NULL,
	[IdLop] [int] NULL,
	[TenMH] [nvarchar](50) NULL,
	[MaMH] [nvarchar](50) NULL
) ON [PRIMARY]
GO
INSERT [dbo].[MonHoc] ([IdMonHoc], [IdLop], [TenMH], [MaMH]) VALUES (1, 1, N'Xử Lý Ảnh', N'MH01')
INSERT [dbo].[MonHoc] ([IdMonHoc], [IdLop], [TenMH], [MaMH]) VALUES (2, 4, N'Hóa đại cương', N'HH01')
/****** Object:  Table [dbo].[Lop]    Script Date: 12/03/2020 14:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lop](
	[IdLop] [int] NOT NULL,
	[MaLop] [nvarchar](50) NOT NULL,
	[TenLop] [nvarchar](50) NOT NULL,
	[IdKhoa] [int] NULL,
 CONSTRAINT [PK_Lop] PRIMARY KEY CLUSTERED 
(
	[IdLop] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Lop] ([IdLop], [MaLop], [TenLop], [IdKhoa]) VALUES (1, N'CN01', N'Lop11', 1)
INSERT [dbo].[Lop] ([IdLop], [MaLop], [TenLop], [IdKhoa]) VALUES (2, N'Lop2', N'Lop12', 1)
INSERT [dbo].[Lop] ([IdLop], [MaLop], [TenLop], [IdKhoa]) VALUES (3, N'LU01', N'Lop20', 2)
INSERT [dbo].[Lop] ([IdLop], [MaLop], [TenLop], [IdKhoa]) VALUES (4, N'DUO01', N'DH17DUO01', 3)
/****** Object:  ForeignKey [FK_Lop_Khoa]    Script Date: 12/03/2020 14:47:54 ******/
ALTER TABLE [dbo].[Lop]  WITH CHECK ADD  CONSTRAINT [FK_Lop_Khoa] FOREIGN KEY([IdKhoa])
REFERENCES [dbo].[Khoa] ([IdKhoa])
GO
ALTER TABLE [dbo].[Lop] CHECK CONSTRAINT [FK_Lop_Khoa]
GO
