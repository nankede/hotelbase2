//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="H_HotelroomruleModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：H_Hotelroomrule表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using HotelBase.Entity.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// H_Hotelroomrule表实体类
    /// </summary>
    [Serializable, Table("H_HotelRoomRule")]
	public class H_HotelRoomRuleModel
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
		/// 数据库字段：HRId
		/// </summary>
		private int _hRId = 0;

		/// <summary>
		/// 房型Id
		/// </summary>
		[Column("HRId")]
		public int HRId
		{
			get { return _hRId; }
			set { _hRId = value; }
		}

		/// <summary>
		/// 数据库字段：HRRBreakfastRule
		/// </summary>
		private int _hRRBreakfastRule = 0;

		/// <summary>
		/// 早餐政策
		/// </summary>
		[Column("HRRBreakfastRule")]
		public int HRRBreakfastRule
		{
			get { return _hRRBreakfastRule; }
			set { _hRRBreakfastRule = value; }
		}

		/// <summary>
		/// 数据库字段：HRRCancelRule
		/// </summary>
		private int _hRRCancelRule = 0;

		/// <summary>
		/// 取消政策
		/// </summary>
		[Column("HRRCancelRule")]
		public int HRRCancelRule
		{
			get { return _hRRCancelRule; }
			set { _hRRCancelRule = value; }
		}

		/// <summary>
		/// 数据库字段：HRRSourceId
		/// </summary>
		private int _hRRSourceId = 0;

		/// <summary>
		/// 供应商来源-字典
		/// </summary>
		[Column("HRRSourceId")]
		public int HRRSourceId
		{
			get { return _hRRSourceId; }
			set { _hRRSourceId = value; }
		}

		/// <summary>
		/// 数据库字段：HRRSupplierId
		/// </summary>
		private int _hRRSupplierId = 0;

		/// <summary>
		/// 供应商Id
		/// </summary>
		[Column("HRRSupplierId")]
		public int HRRSupplierId
		{
			get { return _hRRSupplierId; }
			set { _hRRSupplierId = value; }
		}

		/// <summary>
		/// 数据库字段：HRRIsValid
		/// </summary>
		private int _hRRIsValid = 0;

		/// <summary>
		/// 是否有效
		/// </summary>
		[Column("HRRIsValid")]
		public int HRRIsValid
		{
			get { return _hRRIsValid; }
			set { _hRRIsValid = value; }
		}

		/// <summary>
		/// 数据库字段：HRRAddTime
		/// </summary>
		private DateTime _hRRAddTime = DateTime.Now;

		/// <summary>
		/// 新增时间
		/// </summary>
		[Column("HRRAddTime")]
		public DateTime HRRAddTime
		{
			get { return _hRRAddTime; }
			set { _hRRAddTime = value; }
		}

		/// <summary>
		/// 数据库字段：HRRUpdateTime
		/// </summary>
		private DateTime _hRRUpdateTime = DateTime.Now;

		/// <summary>
		/// 修改时间
		/// </summary>
		[Column("HRRUpdateTime")]
		public DateTime HRRUpdateTime
		{
			get { return _hRRUpdateTime; }
			set { _hRRUpdateTime = value; }
		}

		/// <summary>
		/// 数据库字段：HRRAddName
		/// </summary>
		private string _hRRAddName = string.Empty;

		/// <summary>
		/// 新增人
		/// </summary>
		[Column("HRRAddName")]
		public string HRRAddName
		{
			get { return _hRRAddName; }
			set { _hRRAddName = value; }
		}

		/// <summary>
		/// 数据库字段：HRRUpdateName
		/// </summary>
		private string _hRRUpdateName = string.Empty;

		/// <summary>
		/// 修改人
		/// </summary>
		[Column("HRRUpdateName")]
		public string HRRUpdateName
		{
			get { return _hRRUpdateName; }
			set { _hRRUpdateName = value; }
		}

	}
}
