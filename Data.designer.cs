﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CameraDiemDanh
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="FaceRecog")]
	public partial class DataDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public DataDataContext() : 
				base(global::CameraDiemDanh.Properties.Settings.Default.FaceRecogConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DataDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.[Procedure]")]
		public int Procedure([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Id", DbType="Int")] System.Nullable<int> id, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="TenKhoa", DbType="NVarChar(50)")] string tenKhoa)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, tenKhoa);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.[Procedure]")]
		public int Procedure1([global::System.Data.Linq.Mapping.ParameterAttribute(Name="TenKhoa", DbType="NVarChar(50)")] string tenKhoa)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), tenKhoa);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.[Delete]")]
		public int Delete([global::System.Data.Linq.Mapping.ParameterAttribute(Name="TenLop", DbType="NVarChar(50)")] string tenLop)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), tenLop);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.Delete2")]
		public int Delete2([global::System.Data.Linq.Mapping.ParameterAttribute(Name="TenMH", DbType="NVarChar(50)")] string tenMH)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), tenMH);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.Delete3")]
		public int Delete3([global::System.Data.Linq.Mapping.ParameterAttribute(Name="MSSV", DbType="NVarChar(50)")] string mSSV)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), mSSV);
			return ((int)(result.ReturnValue));
		}
	}
}
#pragma warning restore 1591
