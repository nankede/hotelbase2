//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="H_HotelpictureModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：H_Hotelpicture表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using Component.Access.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// H_Hotelpicture表实体类
    /// </summary>
    [Serializable, Table("H_HotelPicture")]
	public class H_HotelPictureModel
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
		/// 数据库字段：HIId
		/// </summary>
		private int _hIId = 0;

		/// <summary>
		/// 酒店Id
		/// </summary>
		[Column("HIId")]
		public int HIId
		{
			get { return _hIId; }
			set { _hIId = value; }
		}

		/// <summary>
		/// 数据库字段：HPOutId
		/// </summary>
		private int _hPOutId = 0;

		/// <summary>
		/// 外部id
		/// </summary>
		[Column("HPOutId")]
		public int HPOutId
		{
			get { return _hPOutId; }
			set { _hPOutId = value; }
		}

		/// <summary>
		/// 数据库字段：HPType
		/// </summary>
		private int _hPType = 0;

		/// <summary>
		/// 图片类型（字典）
		/// </summary>
		[Column("HPType")]
		public int HPType
		{
			get { return _hPType; }
			set { _hPType = value; }
		}

		/// <summary>
		/// 数据库字段：HPUrl
		/// </summary>
		private string _hPUrl = string.Empty;

		/// <summary>
		/// 图片地址
		/// </summary>
		[Column("HPUrl")]
		public string HPUrl
		{
			get { return _hPUrl; }
			set { _hPUrl = value; }
		}

		/// <summary>
		/// 数据库字段：HPIsValid
		/// </summary>
		private int _hPIsValid = 0;

		/// <summary>
		/// 是否有效
		/// </summary>
		[Column("HPIsValid")]
		public int HPIsValid
		{
			get { return _hPIsValid; }
			set { _hPIsValid = value; }
		}

		/// <summary>
		/// 数据库字段：HPAddTime
		/// </summary>
		private DateTime _hPAddTime = DateTime.Now;

		/// <summary>
		/// 新增时间
		/// </summary>
		[Column("HPAddTime")]
		public DateTime HPAddTime
		{
			get { return _hPAddTime; }
			set { _hPAddTime = value; }
		}

		/// <summary>
		/// 数据库字段：HPAddName
		/// </summary>
		private string _hPAddName = string.Empty;

		/// <summary>
		/// 新增人
		/// </summary>
		[Column("HPAddName")]
		public string HPAddName
		{
			get { return _hPAddName; }
			set { _hPAddName = value; }
		}

		/// <summary>
		/// 数据库字段：HPUpdateTime
		/// </summary>
		private DateTime _hPUpdateTime = DateTime.Now;

		/// <summary>
		/// 修改时间
		/// </summary>
		[Column("HPUpdateTime")]
		public DateTime HPUpdateTime
		{
			get { return _hPUpdateTime; }
			set { _hPUpdateTime = value; }
		}

		/// <summary>
		/// 数据库字段：HPUpdateName
		/// </summary>
		private string _hPUpdateName = string.Empty;

		/// <summary>
		/// 修改人
		/// </summary>
		[Column("HPUpdateName")]
		public string HPUpdateName
		{
			get { return _hPUpdateName; }
			set { _hPUpdateName = value; }
		}

	}
}
