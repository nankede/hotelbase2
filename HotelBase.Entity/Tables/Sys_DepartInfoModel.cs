//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="sys_departinfoModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：sys_departinfo表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using HotelBase.Entity.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// sys_departinfo表实体类
    /// </summary>
    [Serializable, Table("Sys_DepartInfo")]
	public class Sys_DepartInfoModel
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
		/// 数据库字段：DIParentId
		/// </summary>
		private int _dIParentId = 0;

		/// <summary>
		/// 父级Id
		/// </summary>
		[Column("DIParentId")]
		public int DIParentId
		{
			get { return _dIParentId; }
			set { _dIParentId = value; }
		}

		/// <summary>
		/// 数据库字段：DIName
		/// </summary>
		private string _dIName = string.Empty;

		/// <summary>
		/// 部门名称
		/// </summary>
		[Column("DIName")]
		public string DIName
		{
			get { return _dIName; }
			set { _dIName = value; }
		}

		/// <summary>
		/// 数据库字段：DILeaderId
		/// </summary>
		private int _dILeaderId = 0;

		/// <summary>
		/// 负责人
		/// </summary>
		[Column("DILeaderId")]
		public int DILeaderId
		{
			get { return _dILeaderId; }
			set { _dILeaderId = value; }
		}

		/// <summary>
		/// 数据库字段：DILeaderName
		/// </summary>
		private string _dILeaderName = string.Empty;

		/// <summary>
		/// 负责人
		/// </summary>
		[Column("DILeaderName")]
		public string DILeaderName
		{
			get { return _dILeaderName; }
			set { _dILeaderName = value; }
		}

		/// <summary>
		/// 数据库字段：DIIsValid
		/// </summary>
		private int _dIIsValid = 0;

		/// <summary>
		/// 是否有效
		/// </summary>
		[Column("DIIsValid")]
		public int DIIsValid
		{
			get { return _dIIsValid; }
			set { _dIIsValid = value; }
		}

		/// <summary>
		/// 数据库字段：DIAddTime
		/// </summary>
		private DateTime _dIAddTime = DateTime.Now;

		/// <summary>
		/// 新增时间
		/// </summary>
		[Column("DIAddTime")]
		public DateTime DIAddTime
		{
			get { return _dIAddTime; }
			set { _dIAddTime = value; }
		}

		/// <summary>
		/// 数据库字段：DIUpdateTime
		/// </summary>
		private DateTime _dIUpdateTime = DateTime.Now;

		/// <summary>
		/// 修改时间
		/// </summary>
		[Column("DIUpdateTime")]
		public DateTime DIUpdateTime
		{
			get { return _dIUpdateTime; }
			set { _dIUpdateTime = value; }
		}

		/// <summary>
		/// 数据库字段：DIAddName
		/// </summary>
		private string _dIAddName = string.Empty;

		/// <summary>
		/// 新增人
		/// </summary>
		[Column("DIAddName")]
		public string DIAddName
		{
			get { return _dIAddName; }
			set { _dIAddName = value; }
		}

		/// <summary>
		/// 数据库字段：DIUpdateName
		/// </summary>
		private string _dIUpdateName = string.Empty;

		/// <summary>
		/// 修改人
		/// </summary>
		[Column("DIUpdateName")]
		public string DIUpdateName
		{
			get { return _dIUpdateName; }
			set { _dIUpdateName = value; }
		}

	}
}
