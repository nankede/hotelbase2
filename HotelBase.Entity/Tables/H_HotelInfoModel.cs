//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="H_HotelinfoModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：H_Hotelinfo表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using HotelBase.Entity.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// H_Hotelinfo表实体类
    /// </summary>
    [Serializable, Table("H_HotelInfo")]
	public class H_HotelInfoModel
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
		/// 数据库字段：HIName
		/// </summary>
		private string _hIName = string.Empty;

		/// <summary>
		/// 酒店名称
		/// </summary>
		[Column("HIName")]
		public string HIName
		{
			get { return _hIName; }
			set { _hIName = value; }
		}

		/// <summary>
		/// 数据库字段：HIProvinceId
		/// </summary>
		private int _hIProvinceId = 0;

		/// <summary>
		/// 省份Id
		/// </summary>
		[Column("HIProvinceId")]
		public int HIProvinceId
		{
			get { return _hIProvinceId; }
			set { _hIProvinceId = value; }
		}

		/// <summary>
		/// 数据库字段：HIProvince
		/// </summary>
		private string _hIProvince = string.Empty;

		/// <summary>
		/// 省份
		/// </summary>
		[Column("HIProvince")]
		public string HIProvince
		{
			get { return _hIProvince; }
			set { _hIProvince = value; }
		}

		/// <summary>
		/// 数据库字段：HICityId
		/// </summary>
		private int _hICityId = 0;

		/// <summary>
		/// 城市Id
		/// </summary>
		[Column("HICityId")]
		public int HICityId
		{
			get { return _hICityId; }
			set { _hICityId = value; }
		}

		/// <summary>
		/// 数据库字段：HICity
		/// </summary>
		private string _hICity = string.Empty;

		/// <summary>
		/// 城市
		/// </summary>
		[Column("HICity")]
		public string HICity
		{
			get { return _hICity; }
			set { _hICity = value; }
		}

		/// <summary>
		/// 数据库字段：HICountyId
		/// </summary>
		private int _hICountyId = 0;

		/// <summary>
		/// 区县Id
		/// </summary>
		[Column("HICountyId")]
		public int HICountyId
		{
			get { return _hICountyId; }
			set { _hICountyId = value; }
		}

		/// <summary>
		/// 数据库字段：HICounty
		/// </summary>
		private string _hICounty = string.Empty;

		/// <summary>
		/// 区县
		/// </summary>
		[Column("HICounty")]
		public string HICounty
		{
			get { return _hICounty; }
			set { _hICounty = value; }
		}

		/// <summary>
		/// 数据库字段：HIAddress
		/// </summary>
		private string _hIAddress = string.Empty;

		/// <summary>
		/// 详细地址
		/// </summary>
		[Column("HIAddress")]
		public string HIAddress
		{
			get { return _hIAddress; }
			set { _hIAddress = value; }
		}

		/// <summary>
		/// 数据库字段：HIShoppingAreaId
		/// </summary>
		private int _hIShoppingAreaId = 0;

		/// <summary>
		/// 商圈Id
		/// </summary>
		[Column("HIShoppingAreaId")]
		public int HIShoppingAreaId
		{
			get { return _hIShoppingAreaId; }
			set { _hIShoppingAreaId = value; }
		}

		/// <summary>
		/// 数据库字段：HIShoppingArea
		/// </summary>
		private string _hIShoppingArea = string.Empty;

		/// <summary>
		/// 商圈
		/// </summary>
		[Column("HIShoppingArea")]
		public string HIShoppingArea
		{
			get { return _hIShoppingArea; }
			set { _hIShoppingArea = value; }
		}

		/// <summary>
		/// 数据库字段：HIFacilities
		/// </summary>
		private string _hIFacilities = string.Empty;

		/// <summary>
		/// 设施（冗余）
		/// </summary>
		[Column("HIFacilities")]
		public string HIFacilities
		{
			get { return _hIFacilities; }
			set { _hIFacilities = value; }
		}

		/// <summary>
		/// 数据库字段：HICheckIn
		/// </summary>
		private int _hICheckIn = 0;

		/// <summary>
		/// 入住时间
		/// </summary>
		[Column("HICheckIn")]
		public int HICheckIn
		{
			get { return _hICheckIn; }
			set { _hICheckIn = value; }
		}

		/// <summary>
		/// 数据库字段：HICheckOut
		/// </summary>
		private int _hICheckOut = 0;

		/// <summary>
		/// 离店时间
		/// </summary>
		[Column("HICheckOut")]
		public int HICheckOut
		{
			get { return _hICheckOut; }
			set { _hICheckOut = value; }
		}

		/// <summary>
		/// 数据库字段：HIChildRemark
		/// </summary>
		private string _hIChildRemark = string.Empty;

		/// <summary>
		/// 儿童政策
		/// </summary>
		[Column("HIChildRemark")]
		public string HIChildRemark
		{
			get { return _hIChildRemark; }
			set { _hIChildRemark = value; }
		}

		/// <summary>
		/// 数据库字段：HIPetRemark
		/// </summary>
		private string _hIPetRemark = string.Empty;

		/// <summary>
		/// 宠物政策
		/// </summary>
		[Column("HIPetRemark")]
		public string HIPetRemark
		{
			get { return _hIPetRemark; }
			set { _hIPetRemark = value; }
		}

		/// <summary>
		/// 数据库字段：HIHotelIntroduction
		/// </summary>
		private string _hIHotelIntroduction = string.Empty;

		/// <summary>
		/// 酒店介绍
		/// </summary>
		[Column("HIHotelIntroduction")]
		public string HIHotelIntroduction
		{
			get { return _hIHotelIntroduction; }
			set { _hIHotelIntroduction = value; }
		}

		/// <summary>
		/// 数据库字段：HIIsValid
		/// </summary>
		private int _hIIsValid = 0;

		/// <summary>
		/// 是否有效
		/// </summary>
		[Column("HIIsValid")]
		public int HIIsValid
		{
			get { return _hIIsValid; }
			set { _hIIsValid = value; }
		}

		/// <summary>
		/// 数据库字段：HIAddName
		/// </summary>
		private string _hIAddName = string.Empty;

		/// <summary>
		/// 新增人
		/// </summary>
		[Column("HIAddName")]
		public string HIAddName
		{
			get { return _hIAddName; }
			set { _hIAddName = value; }
		}

		/// <summary>
		/// 数据库字段：HIUpdateName
		/// </summary>
		private string _hIUpdateName = string.Empty;

		/// <summary>
		/// 修改人
		/// </summary>
		[Column("HIUpdateName")]
		public string HIUpdateName
		{
			get { return _hIUpdateName; }
			set { _hIUpdateName = value; }
		}

		/// <summary>
		/// 数据库字段：HIAddTime
		/// </summary>
		private DateTime _hIAddTime = DateTime.Now;

		/// <summary>
		/// 新增时间
		/// </summary>
		[Column("HIAddTime")]
		public DateTime HIAddTime
		{
			get { return _hIAddTime; }
			set { _hIAddTime = value; }
		}

		/// <summary>
		/// 数据库字段：HIUpdateTime
		/// </summary>
		private DateTime _hIUpdateTime = DateTime.Now;

		/// <summary>
		/// 修改时间
		/// </summary>
		[Column("HIUpdateTime")]
		public DateTime HIUpdateTime
		{
			get { return _hIUpdateTime; }
			set { _hIUpdateTime = value; }
		}

	}
}
