//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="sys_logModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：sys_log表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using HotelBase.Entity.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// sys_log表实体类
    /// </summary>
    [Serializable, Table("Sys_Log")]
	public class Sys_LogModel
	{
		/// <summary>
		/// 数据库字段：Id
		/// </summary>
		private long _id = 0;

		/// <summary>
		/// ID
		/// </summary>
		[Key(KeyType.Identity)]
		[Column("Id")]
		public long Id
		{
			get { return _id; }
			set { _id = value; }
		}

		/// <summary>
		/// 数据库字段：SLLogLevel
		/// </summary>
		private string _sLLogLevel = string.Empty;

		/// <summary>
		/// 日志级别
		/// </summary>
		[Column("SLLogLevel")]
		public string SLLogLevel
		{
			get { return _sLLogLevel; }
			set { _sLLogLevel = value; }
		}

		/// <summary>
		/// 数据库字段：SLOperation
		/// </summary>
		private string _sLOperation = string.Empty;

		/// <summary>
		/// 标题
		/// </summary>
		[Column("SLOperation")]
		public string SLOperation
		{
			get { return _sLOperation; }
			set { _sLOperation = value; }
		}

		/// <summary>
		/// 数据库字段：SLMessage
		/// </summary>
		private string _sLMessage = string.Empty;

		/// <summary>
		/// 错误信息
		/// </summary>
		[Column("SLMessage")]
		public string SLMessage
		{
			get { return _sLMessage; }
			set { _sLMessage = value; }
		}

		/// <summary>
		/// 数据库字段：SLIP
		/// </summary>
		private string _sLIP = string.Empty;

		/// <summary>
		/// IP
		/// </summary>
		[Column("SLIP")]
		public string SLIP
		{
			get { return _sLIP; }
			set { _sLIP = value; }
		}

		/// <summary>
		/// 数据库字段：SLIPAddress
		/// </summary>
		private string _sLIPAddress = string.Empty;

		/// <summary>
		/// IP地址
		/// </summary>
		[Column("SLIPAddress")]
		public string SLIPAddress
		{
			get { return _sLIPAddress; }
			set { _sLIPAddress = value; }
		}

		/// <summary>
		/// 数据库字段：SLBrowser
		/// </summary>
		private string _sLBrowser = string.Empty;

		/// <summary>
		/// 浏览器
		/// </summary>
		[Column("SLBrowser")]
		public string SLBrowser
		{
			get { return _sLBrowser; }
			set { _sLBrowser = value; }
		}

		/// <summary>
		/// 数据库字段：SLStackTrace
		/// </summary>
		private string _sLStackTrace = string.Empty;

		/// <summary>
		/// 堆栈信息
		/// </summary>
		[Column("SLStackTrace")]
		public string SLStackTrace
		{
			get { return _sLStackTrace; }
			set { _sLStackTrace = value; }
		}

		/// <summary>
		/// 数据库字段：SLCreateUser
		/// </summary>
		private int _sLCreateUser = 0;

		/// <summary>
		/// 创建人
		/// </summary>
		[Column("SLCreateUser")]
		public int SLCreateUser
		{
			get { return _sLCreateUser; }
			set { _sLCreateUser = value; }
		}

		/// <summary>
		/// 数据库字段：SLUserName
		/// </summary>
		private string _sLUserName = string.Empty;

		/// <summary>
		/// 创建人
		/// </summary>
		[Column("SLUserName")]
		public string SLUserName
		{
			get { return _sLUserName; }
			set { _sLUserName = value; }
		}

		/// <summary>
		/// 数据库字段：SLCreateTime
		/// </summary>
		private DateTime _sLCreateTime = Convert.ToDateTime("1900-01-01 00:00:00");

		/// <summary>
		/// 发生时间
		/// </summary>
		[Column("SLCreateTime")]
		public DateTime SLCreateTime
		{
			get { return _sLCreateTime; }
			set { _sLCreateTime = value; }
		}

	}
}
