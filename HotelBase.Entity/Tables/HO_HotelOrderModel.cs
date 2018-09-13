//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="ho_hotelorderModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：ho_hotelorder表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using HotelBase.Entity.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// ho_hotelorder表实体类
    /// </summary>
    [Serializable, Table("HO_HotelOrder")]
	public class HO_HotelOrderModel
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
		/// 数据库字段：HOCustomerSerialId
		/// </summary>
		private string _hOCustomerSerialId = string.Empty;

		/// <summary>
		/// 客户单号
		/// </summary>
		[Column("HOCustomerSerialId")]
		public string HOCustomerSerialId
		{
			get { return _hOCustomerSerialId; }
			set { _hOCustomerSerialId = value; }
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
		/// 数据库字段：HRRId
		/// </summary>
		private int _hRRId = 0;

		/// <summary>
		/// 政策Id
		/// </summary>
		[Column("HRRId")]
		public int HRRId
		{
			get { return _hRRId; }
			set { _hRRId = value; }
		}

		/// <summary>
		/// 数据库字段：HOSupplierId
		/// </summary>
		private int _hOSupplierId = 0;

		/// <summary>
		/// 供应商Id
		/// </summary>
		[Column("HOSupplierId")]
		public int HOSupplierId
		{
			get { return _hOSupplierId; }
			set { _hOSupplierId = value; }
		}

		/// <summary>
		/// 数据库字段：HOSupplierSourceId
		/// </summary>
		private int _hOSupplierSourceId = 0;

		/// <summary>
		/// 供应商来源
		/// </summary>
		[Column("HOSupplierSourceId")]
		public int HOSupplierSourceId
		{
			get { return _hOSupplierSourceId; }
			set { _hOSupplierSourceId = value; }
		}

		/// <summary>
		/// 数据库字段：HOOutSerialId
		/// </summary>
		private string _hOOutSerialId = string.Empty;

		/// <summary>
		/// 第三方订单号
		/// </summary>
		[Column("HOOutSerialId")]
		public string HOOutSerialId
		{
			get { return _hOOutSerialId; }
			set { _hOOutSerialId = value; }
		}

		/// <summary>
		/// 数据库字段：HOSupplierSerialId
		/// </summary>
		private string _hOSupplierSerialId = string.Empty;

		/// <summary>
		/// 供应商订单号
		/// </summary>
		[Column("HOSupplierSerialId")]
		public string HOSupplierSerialId
		{
			get { return _hOSupplierSerialId; }
			set { _hOSupplierSerialId = value; }
		}

		/// <summary>
		/// 数据库字段：HOStatus
		/// </summary>
		private int _hOStatus = 0;

		/// <summary>
		/// 订单状态
		/// </summary>
		[Column("HOStatus")]
		public int HOStatus
		{
			get { return _hOStatus; }
			set { _hOStatus = value; }
		}

		/// <summary>
		/// 数据库字段：HOPayStatus
		/// </summary>
		private int _hOPayStatus = 0;

		/// <summary>
		/// 支付状态
		/// </summary>
		[Column("HOPayStatus")]
		public int HOPayStatus
		{
			get { return _hOPayStatus; }
			set { _hOPayStatus = value; }
		}

		/// <summary>
		/// 数据库字段：HORoomCount
		/// </summary>
		private int _hORoomCount = 0;

		/// <summary>
		/// 房间数
		/// </summary>
		[Column("HORoomCount")]
		public int HORoomCount
		{
			get { return _hORoomCount; }
			set { _hORoomCount = value; }
		}

		/// <summary>
		/// 数据库字段：HOChild
		/// </summary>
		private int _hOChild = 0;

		/// <summary>
		/// 儿童数
		/// </summary>
		[Column("HOChild")]
		public int HOChild
		{
			get { return _hOChild; }
			set { _hOChild = value; }
		}

		/// <summary>
		/// 数据库字段：HOAdult
		/// </summary>
		private int _hOAdult = 0;

		/// <summary>
		/// 成人数
		/// </summary>
		[Column("HOAdult")]
		public int HOAdult
		{
			get { return _hOAdult; }
			set { _hOAdult = value; }
		}

		/// <summary>
		/// 数据库字段：HoPlat1
		/// </summary>
		private int _hoPlat1 = 0;

		/// <summary>
		/// 来源渠道
		/// </summary>
		[Column("HoPlat1")]
		public int HoPlat1
		{
			get { return _hoPlat1; }
			set { _hoPlat1 = value; }
		}

		/// <summary>
		/// 数据库字段：HoPlat2
		/// </summary>
		private int _hoPlat2 = 0;

		/// <summary>
		/// 来源渠道
		/// </summary>
		[Column("HoPlat2")]
		public int HoPlat2
		{
			get { return _hoPlat2; }
			set { _hoPlat2 = value; }
		}

		/// <summary>
		/// 数据库字段：HOContractPrice
		/// </summary>
		private decimal _hOContractPrice = 0.00M;

		/// <summary>
		/// 结算价
		/// </summary>
		[Column("HOContractPrice")]
		public decimal HOContractPrice
		{
			get { return _hOContractPrice; }
			set { _hOContractPrice = value; }
		}

		/// <summary>
		/// 数据库字段：HOSellPrice
		/// </summary>
		private decimal _hOSellPrice = 0.00M;

		/// <summary>
		/// 销售价
		/// </summary>
		[Column("HOSellPrice")]
		public decimal HOSellPrice
		{
			get { return _hOSellPrice; }
			set { _hOSellPrice = value; }
		}

		/// <summary>
		/// 数据库字段：HOCustomerName
		/// </summary>
		private string _hOCustomerName = string.Empty;

		/// <summary>
		/// 入住人姓名
		/// </summary>
		[Column("HOCustomerName")]
		public string HOCustomerName
		{
			get { return _hOCustomerName; }
			set { _hOCustomerName = value; }
		}

		/// <summary>
		/// 数据库字段：HOCustomerMobile
		/// </summary>
		private string _hOCustomerMobile = string.Empty;

		/// <summary>
		/// 入住人手机号
		/// </summary>
		[Column("HOCustomerMobile")]
		public string HOCustomerMobile
		{
			get { return _hOCustomerMobile; }
			set { _hOCustomerMobile = value; }
		}

		/// <summary>
		/// 数据库字段：HOLinkerName
		/// </summary>
		private string _hOLinkerName = string.Empty;

		/// <summary>
		/// 联系人姓名
		/// </summary>
		[Column("HOLinkerName")]
		public string HOLinkerName
		{
			get { return _hOLinkerName; }
			set { _hOLinkerName = value; }
		}

		/// <summary>
		/// 数据库字段：HOLinkerMobile
		/// </summary>
		private string _hOLinkerMobile = string.Empty;

		/// <summary>
		/// 联系人手机号
		/// </summary>
		[Column("HOLinkerMobile")]
		public string HOLinkerMobile
		{
			get { return _hOLinkerMobile; }
			set { _hOLinkerMobile = value; }
		}

		/// <summary>
		/// 数据库字段：HORemark
		/// </summary>
		private string _hORemark = string.Empty;

		/// <summary>
		/// 特殊要求
		/// </summary>
		[Column("HORemark")]
		public string HORemark
		{
			get { return _hORemark; }
			set { _hORemark = value; }
		}

		/// <summary>
		/// 数据库字段：HOCheckInDate
		/// </summary>
		private DateTime _hOCheckInDate = Convert.ToDateTime("1970-01-01 00:00:00");

		/// <summary>
		/// 入住时间
		/// </summary>
		[Column("HOCheckInDate")]
		public DateTime HOCheckInDate
		{
			get { return _hOCheckInDate; }
			set { _hOCheckInDate = value; }
		}

		/// <summary>
		/// 数据库字段：HOCheckOutDate
		/// </summary>
		private DateTime _hOCheckOutDate = Convert.ToDateTime("1970-01-01 00:00:00");

		/// <summary>
		/// 离店时间
		/// </summary>
		[Column("HOCheckOutDate")]
		public DateTime HOCheckOutDate
		{
			get { return _hOCheckOutDate; }
			set { _hOCheckOutDate = value; }
		}

		/// <summary>
		/// 数据库字段：HOLastCheckInTime
		/// </summary>
		private DateTime _hOLastCheckInTime = Convert.ToDateTime("1970-01-01 00:00:00");

		/// <summary>
		/// 最晚到店时间
		/// </summary>
		[Column("HOLastCheckInTime")]
		public DateTime HOLastCheckInTime
		{
			get { return _hOLastCheckInTime; }
			set { _hOLastCheckInTime = value; }
		}

		/// <summary>
		/// 数据库字段：HOAddId
		/// </summary>
		private int _hOAddId = 0;

		/// <summary>
		/// 新增人
		/// </summary>
		[Column("HOAddId")]
		public int HOAddId
		{
			get { return _hOAddId; }
			set { _hOAddId = value; }
		}

		/// <summary>
		/// 数据库字段：HOAddName
		/// </summary>
		private string _hOAddName = string.Empty;

		/// <summary>
		/// 新增人
		/// </summary>
		[Column("HOAddName")]
		public string HOAddName
		{
			get { return _hOAddName; }
			set { _hOAddName = value; }
		}

		/// <summary>
		/// 数据库字段：HOAddDepartId
		/// </summary>
		private int _hOAddDepartId = 0;

		/// <summary>
		/// 新增人部门Id
		/// </summary>
		[Column("HOAddDepartId")]
		public int HOAddDepartId
		{
			get { return _hOAddDepartId; }
			set { _hOAddDepartId = value; }
		}

		/// <summary>
		/// 数据库字段：HOAddDepartName
		/// </summary>
		private string _hOAddDepartName = string.Empty;

		/// <summary>
		/// 新增人部门
		/// </summary>
		[Column("HOAddDepartName")]
		public string HOAddDepartName
		{
			get { return _hOAddDepartName; }
			set { _hOAddDepartName = value; }
		}

		/// <summary>
		/// 数据库字段：HOAddTime
		/// </summary>
		private DateTime _hOAddTime = DateTime.Now;

		/// <summary>
		/// 新增时间
		/// </summary>
		[Column("HOAddTime")]
		public DateTime HOAddTime
		{
			get { return _hOAddTime; }
			set { _hOAddTime = value; }
		}

		/// <summary>
		/// 数据库字段：HOUpdateId
		/// </summary>
		private int _hOUpdateId = 0;

		/// <summary>
		/// 修改人
		/// </summary>
		[Column("HOUpdateId")]
		public int HOUpdateId
		{
			get { return _hOUpdateId; }
			set { _hOUpdateId = value; }
		}

		/// <summary>
		/// 数据库字段：HOUpdateName
		/// </summary>
		private string _hOUpdateName = string.Empty;

		/// <summary>
		/// 修改人
		/// </summary>
		[Column("HOUpdateName")]
		public string HOUpdateName
		{
			get { return _hOUpdateName; }
			set { _hOUpdateName = value; }
		}

		/// <summary>
		/// 数据库字段：HOUpdateTime
		/// </summary>
		private DateTime _hOUpdateTime = DateTime.Now;

		/// <summary>
		/// 修改时间
		/// </summary>
		[Column("HOUpdateTime")]
		public DateTime HOUpdateTime
		{
			get { return _hOUpdateTime; }
			set { _hOUpdateTime = value; }
		}

	}
}
