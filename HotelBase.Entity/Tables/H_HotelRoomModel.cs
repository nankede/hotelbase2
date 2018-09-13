//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="H_HotelroomModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：H_Hotelroom表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using HotelBase.Entity.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// H_Hotelroom表实体类
    /// </summary>
    [Serializable, Table("H_HotelRoom")]
	public class H_HotelRoomModel
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
		/// 数据库字段：HRName
		/// </summary>
		private string _hRName = string.Empty;

		/// <summary>
		/// 房型名称
		/// </summary>
		[Column("HRName")]
		public string HRName
		{
			get { return _hRName; }
			set { _hRName = value; }
		}

		/// <summary>
		/// 数据库字段：HRBedType
		/// </summary>
		private int _hRBedType = 0;

		/// <summary>
		/// 床型
		/// </summary>
		[Column("HRBedType")]
		public int HRBedType
		{
			get { return _hRBedType; }
			set { _hRBedType = value; }
		}

		/// <summary>
		/// 数据库字段：HRBedSize
		/// </summary>
		private int _hRBedSize = 0;

		/// <summary>
		/// 尺寸
		/// </summary>
		[Column("HRBedSize")]
		public int HRBedSize
		{
			get { return _hRBedSize; }
			set { _hRBedSize = value; }
		}

		/// <summary>
		/// 数据库字段：HRRoomSIze
		/// </summary>
		private string _hRRoomSIze = string.Empty;

		/// <summary>
		/// 房间大小
		/// </summary>
		[Column("HRRoomSIze")]
		public string HRRoomSIze
		{
			get { return _hRRoomSIze; }
			set { _hRRoomSIze = value; }
		}

		/// <summary>
		/// 数据库字段：HRFloor
		/// </summary>
		private string _hRFloor = string.Empty;

		/// <summary>
		/// 楼层
		/// </summary>
		[Column("HRFloor")]
		public string HRFloor
		{
			get { return _hRFloor; }
			set { _hRFloor = value; }
		}

		/// <summary>
		/// 数据库字段：HRWindowsType
		/// </summary>
		private int _hRWindowsType = 0;

		/// <summary>
		/// 窗户类型
		/// </summary>
		[Column("HRWindowsType")]
		public int HRWindowsType
		{
			get { return _hRWindowsType; }
			set { _hRWindowsType = value; }
		}

		/// <summary>
		/// 数据库字段：HRPersonCount
		/// </summary>
		private int _hRPersonCount = 0;

		/// <summary>
		/// 人数
		/// </summary>
		[Column("HRPersonCount")]
		public int HRPersonCount
		{
			get { return _hRPersonCount; }
			set { _hRPersonCount = value; }
		}

		/// <summary>
		/// 数据库字段：HRIsValid
		/// </summary>
		private int _hRIsValid = 0;

		/// <summary>
		/// 是否有效
		/// </summary>
		[Column("HRIsValid")]
		public int HRIsValid
		{
			get { return _hRIsValid; }
			set { _hRIsValid = value; }
		}

		/// <summary>
		/// 数据库字段：HRAddName
		/// </summary>
		private string _hRAddName = string.Empty;

		/// <summary>
		/// 新增人
		/// </summary>
		[Column("HRAddName")]
		public string HRAddName
		{
			get { return _hRAddName; }
			set { _hRAddName = value; }
		}

		/// <summary>
		/// 数据库字段：HRUpdateName
		/// </summary>
		private string _hRUpdateName = string.Empty;

		/// <summary>
		/// 修改人
		/// </summary>
		[Column("HRUpdateName")]
		public string HRUpdateName
		{
			get { return _hRUpdateName; }
			set { _hRUpdateName = value; }
		}

		/// <summary>
		/// 数据库字段：HRAddTime
		/// </summary>
		private DateTime _hRAddTime = DateTime.Now;

		/// <summary>
		/// 新增时间
		/// </summary>
		[Column("HRAddTime")]
		public DateTime HRAddTime
		{
			get { return _hRAddTime; }
			set { _hRAddTime = value; }
		}

		/// <summary>
		/// 数据库字段：HRUpdateTime
		/// </summary>
		private DateTime _hRUpdateTime = DateTime.Now;

		/// <summary>
		/// 修改时间
		/// </summary>
		[Column("HRUpdateTime")]
		public DateTime HRUpdateTime
		{
			get { return _hRUpdateTime; }
			set { _hRUpdateTime = value; }
		}

	}
}
