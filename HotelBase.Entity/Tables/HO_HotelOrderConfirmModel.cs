//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="ho_hotelorderconfirmModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：ho_hotelorderconfirm表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using HotelBase.Entity.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// ho_hotelorderconfirm表实体类
    /// </summary>
    [Serializable, Table("HO_HotelOrderConfirm")]
	public class HO_HotelOrderConfirmModel
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
		/// 数据库字段：HOCOrderId
		/// </summary>
		private int _hOCOrderId = 0;

		/// <summary>
		/// 订单表Id
		/// </summary>
		[Column("HOCOrderId")]
		public int HOCOrderId
		{
			get { return _hOCOrderId; }
			set { _hOCOrderId = value; }
		}

		/// <summary>
		/// 数据库字段：HOCConfirmType
		/// </summary>
		private int _hOCConfirmType = 0;

		/// <summary>
		/// 确认类型
		/// </summary>
		[Column("HOCConfirmType")]
		public int HOCConfirmType
		{
			get { return _hOCConfirmType; }
			set { _hOCConfirmType = value; }
		}

		/// <summary>
		/// 数据库字段：HOCConfirmValue
		/// </summary>
		private string _hOCConfirmValue = string.Empty;

		/// <summary>
		/// 确认方式
		/// </summary>
		[Column("HOCConfirmValue")]
		public string HOCConfirmValue
		{
			get { return _hOCConfirmValue; }
			set { _hOCConfirmValue = value; }
		}

		/// <summary>
		/// 数据库字段：HOCConfirmStatus
		/// </summary>
		private int _hOCConfirmStatus = 0;

		/// <summary>
		/// 确认状态
		/// </summary>
		[Column("HOCConfirmStatus")]
		public int HOCConfirmStatus
		{
			get { return _hOCConfirmStatus; }
			set { _hOCConfirmStatus = value; }
		}

	}
}
