//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="H_HoterulepriceModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：H_Hoteruleprice表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using HotelBase.Entity.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// H_Hoteruleprice表实体类
    /// </summary>
    [Serializable, Table("H_HoteRulePrice")]
	public class H_HoteRulePriceModel
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
		/// 数据库字段：HRPDate
		/// </summary>
		private DateTime _hRPDate = Convert.ToDateTime("1970-01-01 00:00:00");

		/// <summary>
		/// 日期
		/// </summary>
		[Column("HRPDate")]
		public DateTime HRPDate
		{
			get { return _hRPDate; }
			set { _hRPDate = value; }
		}

		/// <summary>
		/// 数据库字段：HRPContractPrice
		/// </summary>
		private decimal _hRPContractPrice = 0.00M;

		/// <summary>
		/// 结算价
		/// </summary>
		[Column("HRPContractPrice")]
		public decimal HRPContractPrice
		{
			get { return _hRPContractPrice; }
			set { _hRPContractPrice = value; }
		}

		/// <summary>
		/// 数据库字段：HRPSellPrice
		/// </summary>
		private decimal _hRPSellPrice = 0.00M;

		/// <summary>
		/// 售卖价
		/// </summary>
		[Column("HRPSellPrice")]
		public decimal HRPSellPrice
		{
			get { return _hRPSellPrice; }
			set { _hRPSellPrice = value; }
		}

		/// <summary>
		/// 数据库字段：HRPCount
		/// </summary>
		private int _hRPCount = 0;

		/// <summary>
		/// 数量
		/// </summary>
		[Column("HRPCount")]
		public int HRPCount
		{
			get { return _hRPCount; }
			set { _hRPCount = value; }
		}

		/// <summary>
		/// 数据库字段：HRPRetainCount
		/// </summary>
		private int _hRPRetainCount = 0;

		/// <summary>
		/// 保留数量
		/// </summary>
		[Column("HRPRetainCount")]
		public int HRPRetainCount
		{
			get { return _hRPRetainCount; }
			set { _hRPRetainCount = value; }
		}

		/// <summary>
		/// 数据库字段：HRPStatus
		/// </summary>
		private int _hRPStatus = 0;

		/// <summary>
		/// 状态
		/// </summary>
		[Column("HRPStatus")]
		public int HRPStatus
		{
			get { return _hRPStatus; }
			set { _hRPStatus = value; }
		}

		/// <summary>
		/// 数据库字段：HRPIsValid
		/// </summary>
		private int _hRPIsValid = 0;

		/// <summary>
		/// 是否有效
		/// </summary>
		[Column("HRPIsValid")]
		public int HRPIsValid
		{
			get { return _hRPIsValid; }
			set { _hRPIsValid = value; }
		}

		/// <summary>
		/// 数据库字段：HRPAddTime
		/// </summary>
		private DateTime _hRPAddTime = DateTime.Now;

		/// <summary>
		/// 新增时间
		/// </summary>
		[Column("HRPAddTime")]
		public DateTime HRPAddTime
		{
			get { return _hRPAddTime; }
			set { _hRPAddTime = value; }
		}

		/// <summary>
		/// 数据库字段：HRPUpdateTime
		/// </summary>
		private DateTime _hRPUpdateTime = DateTime.Now;

		/// <summary>
		/// 修改时间
		/// </summary>
		[Column("HRPUpdateTime")]
		public DateTime HRPUpdateTime
		{
			get { return _hRPUpdateTime; }
			set { _hRPUpdateTime = value; }
		}

		/// <summary>
		/// 数据库字段：HRPAddName
		/// </summary>
		private string _hRPAddName = string.Empty;

		/// <summary>
		/// 新增人
		/// </summary>
		[Column("HRPAddName")]
		public string HRPAddName
		{
			get { return _hRPAddName; }
			set { _hRPAddName = value; }
		}

		/// <summary>
		/// 数据库字段：HRPUpdateName
		/// </summary>
		private string _hRPUpdateName = string.Empty;

		/// <summary>
		/// 修改人
		/// </summary>
		[Column("HRPUpdateName")]
		public string HRPUpdateName
		{
			get { return _hRPUpdateName; }
			set { _hRPUpdateName = value; }
		}

	}
}
