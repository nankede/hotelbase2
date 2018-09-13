//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="sys_userinfoModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：sys_userinfo表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using HotelBase.Entity.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// sys_userinfo表实体类
    /// </summary>
    [Serializable, Table("Sys_UserInfo")]
	public class Sys_UserInfoModel
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
		/// 数据库字段：UIAccount
		/// </summary>
		private string _uIAccount = string.Empty;

		/// <summary>
		/// 登陆账户
		/// </summary>
		[Column("UIAccount")]
		public string UIAccount
		{
			get { return _uIAccount; }
			set { _uIAccount = value; }
		}

		/// <summary>
		/// 数据库字段：UIPassWord
		/// </summary>
		private string _uIPassWord = string.Empty;

		/// <summary>
		/// 密码
		/// </summary>
		[Column("UIPassWord")]
		public string UIPassWord
		{
			get { return _uIPassWord; }
			set { _uIPassWord = value; }
		}

		/// <summary>
		/// 数据库字段：UIName
		/// </summary>
		private string _uIName = string.Empty;

		/// <summary>
		/// 姓名
		/// </summary>
		[Column("UIName")]
		public string UIName
		{
			get { return _uIName; }
			set { _uIName = value; }
		}

		/// <summary>
		/// 数据库字段：UIResponsibility
		/// </summary>
		private int _uIResponsibility = 0;

		/// <summary>
		/// 职责
		/// </summary>
		[Column("UIResponsibility")]
		public int UIResponsibility
		{
			get { return _uIResponsibility; }
			set { _uIResponsibility = value; }
		}

		/// <summary>
		/// 数据库字段：UIDepartId
		/// </summary>
		private int _uIDepartId = 0;

		/// <summary>
		/// 部门Id
		/// </summary>
		[Column("UIDepartId")]
		public int UIDepartId
		{
			get { return _uIDepartId; }
			set { _uIDepartId = value; }
		}

		/// <summary>
		/// 数据库字段：UIDepartName
		/// </summary>
		private string _uIDepartName = string.Empty;

		/// <summary>
		/// 部门名称
		/// </summary>
		[Column("UIDepartName")]
		public string UIDepartName
		{
			get { return _uIDepartName; }
			set { _uIDepartName = value; }
		}

		/// <summary>
		/// 数据库字段：UIIsValid
		/// </summary>
		private int _uIIsValid = 0;

		/// <summary>
		/// 是否有效
		/// </summary>
		[Column("UIIsValid")]
		public int UIIsValid
		{
			get { return _uIIsValid; }
			set { _uIIsValid = value; }
		}

		/// <summary>
		/// 数据库字段：UIAddTime
		/// </summary>
		private DateTime _uIAddTime = DateTime.Now;

		/// <summary>
		/// 新增时间
		/// </summary>
		[Column("UIAddTime")]
		public DateTime UIAddTime
		{
			get { return _uIAddTime; }
			set { _uIAddTime = value; }
		}

		/// <summary>
		/// 数据库字段：UIUpdateTime
		/// </summary>
		private DateTime _uIUpdateTime = DateTime.Now;

		/// <summary>
		/// 修改时间
		/// </summary>
		[Column("UIUpdateTime")]
		public DateTime UIUpdateTime
		{
			get { return _uIUpdateTime; }
			set { _uIUpdateTime = value; }
		}

		/// <summary>
		/// 数据库字段：UIAddName
		/// </summary>
		private string _uIAddName = string.Empty;

		/// <summary>
		/// 新增人
		/// </summary>
		[Column("UIAddName")]
		public string UIAddName
		{
			get { return _uIAddName; }
			set { _uIAddName = value; }
		}

		/// <summary>
		/// 数据库字段：UIUpdateName
		/// </summary>
		private string _uIUpdateName = string.Empty;

		/// <summary>
		/// 修改人
		/// </summary>
		[Column("UIUpdateName")]
		public string UIUpdateName
		{
			get { return _uIUpdateName; }
			set { _uIUpdateName = value; }
		}

	}
}
