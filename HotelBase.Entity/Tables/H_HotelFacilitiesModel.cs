//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="H_HotelfacilitiesModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：H_Hotelfacilities表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using Component.Access.MapAttribute;

namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// H_Hotelfacilities表实体类
    /// </summary>
    [Serializable, Table("H_HotelFacilities")]
	public class H_HotelFacilitiesModel
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
		/// 酒店id
		/// </summary>
		[Column("HIId")]
		public int HIId
		{
			get { return _hIId; }
			set { _hIId = value; }
		}

		/// <summary>
		/// 数据库字段：HFFacilitiyId
		/// </summary>
		private int _hFFacilitiyId = 0;

		/// <summary>
		/// 设施Id
		/// </summary>
		[Column("HFFacilitiyId")]
		public int HFFacilitiyId
		{
			get { return _hFFacilitiyId; }
			set { _hFFacilitiyId = value; }
		}

		/// <summary>
		/// 数据库字段：HFFacilitiyName
		/// </summary>
		private string _hFFacilitiyName = "0";

		/// <summary>
		/// 名称
		/// </summary>
		[Column("HFFacilitiyName")]
		public string HFFacilitiyName
		{
			get { return _hFFacilitiyName; }
			set { _hFFacilitiyName = value; }
		}

		/// <summary>
		/// 数据库字段：HFAddName
		/// </summary>
		private string _hFAddName = "0";

		/// <summary>
		/// 新增人
		/// </summary>
		[Column("HFAddName")]
		public string HFAddName
		{
			get { return _hFAddName; }
			set { _hFAddName = value; }
		}

		/// <summary>
		/// 数据库字段：HFAddTime
		/// </summary>
		private DateTime _hFAddTime = DateTime.Now;

		/// <summary>
		/// 新增时间
		/// </summary>
		[Column("HFAddTime")]
		public DateTime HFAddTime
		{
			get { return _hFAddTime; }
			set { _hFAddTime = value; }
		}

	}
}
