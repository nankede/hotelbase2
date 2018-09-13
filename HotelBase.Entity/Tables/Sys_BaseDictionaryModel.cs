//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="sys_basedictionaryModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：sys_basedictionary表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using HotelBase.Entity.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// sys_basedictionary表实体类
    /// </summary>
    [Serializable, Table("Sys_BaseDictionary")]
	public class Sys_BaseDictionaryModel
    {
		/// <summary>
		/// 数据库字段：Id
		/// </summary>
		private int _id = 0;

		/// <summary>
		/// 自增主键
		/// </summary>
		[Key(KeyType.Identity)]
		[Column("Id")]
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		/// <summary>
		/// 数据库字段：DParentId
		/// </summary>
		private int _dParentId = 0;

		/// <summary>
		/// 父级Id
		/// </summary>
		[Column("DParentId")]
		public int DParentId
		{
			get { return _dParentId; }
			set { _dParentId = value; }
		}

		/// <summary>
		/// 数据库字段：DParentName
		/// </summary>
		private string _dParentName = string.Empty;

		/// <summary>
		/// 父级名称
		/// </summary>
		[Column("DParentName")]
		public string DParentName
		{
			get { return _dParentName; }
			set { _dParentName = value; }
		}

		/// <summary>
		/// 数据库字段：DName
		/// </summary>
		private string _dName = string.Empty;

		/// <summary>
		/// 名称
		/// </summary>
		[Column("DName")]
		public string DName
		{
			get { return _dName; }
			set { _dName = value; }
		}

		/// <summary>
		/// 数据库字段：DCode
		/// </summary>
		private int _dCode = 0;

		/// <summary>
		/// int数据
		/// </summary>
		[Column("DCode")]
		public int DCode
		{
			get { return _dCode; }
			set { _dCode = value; }
		}

		/// <summary>
		/// 数据库字段：DValue
		/// </summary>
		private string _dValue = string.Empty;

		/// <summary>
		/// string数据
		/// </summary>
		[Column("DValue")]
		public string DValue
		{
			get { return _dValue; }
			set { _dValue = value; }
		}

		/// <summary>
		/// 数据库字段：DRemark
		/// </summary>
		private string _dRemark = string.Empty;

		/// <summary>
		/// 描述
		/// </summary>
		[Column("DRemark")]
		public string DRemark
		{
			get { return _dRemark; }
			set { _dRemark = value; }
		}

		/// <summary>
		/// 数据库字段：DIsValid
		/// </summary>
		private int _dIsValid = 0;

		/// <summary>
		/// 是否有效
		/// </summary>
		[Column("DIsValid")]
		public int DIsValid
		{
			get { return _dIsValid; }
			set { _dIsValid = value; }
		}

		/// <summary>
		/// 数据库字段：DSort
		/// </summary>
		private int _dSort = 0;

		/// <summary>
		/// 排序
		/// </summary>
		[Column("DSort")]
		public int DSort
		{
			get { return _dSort; }
			set { _dSort = value; }
		}

		/// <summary>
		/// 数据库字段：DAddTime
		/// </summary>
		private DateTime _dAddTime = DateTime.Now;

		/// <summary>
		/// 新增时间
		/// </summary>
		[Column("DAddTime")]
		public DateTime DAddTime
		{
			get { return _dAddTime; }
			set { _dAddTime = value; }
		}

		/// <summary>
		/// 数据库字段：DUpdateTime
		/// </summary>
		private DateTime _dUpdateTime = DateTime.Now;

		/// <summary>
		/// 修改时间
		/// </summary>
		[Column("DUpdateTime")]
		public DateTime DUpdateTime
		{
			get { return _dUpdateTime; }
			set { _dUpdateTime = value; }
		}

	}
}
