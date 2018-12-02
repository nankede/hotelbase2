//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="h_supplierModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：h_supplier表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using Component.Access.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// h_supplier表实体类
    /// </summary>
    [Serializable, Table("H_Supplier")]
	public class H_SupplierModel
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
		/// 数据库字段：SCode
		/// </summary>
		private string _sCode = string.Empty;

		/// <summary>
		/// 供应商编号
		/// </summary>
		[Column("SCode")]
		public string SCode
		{
			get { return _sCode; }
			set { _sCode = value; }
		}

		/// <summary>
		/// 数据库字段：SSourceId
		/// </summary>
		private int _sSourceId = 0;

		/// <summary>
		/// 来源
		/// </summary>
		[Column("SSourceId")]
		public int SSourceId
		{
			get { return _sSourceId; }
			set { _sSourceId = value; }
		}

		/// <summary>
		/// 数据库字段：SName
		/// </summary>
		private string _sName = string.Empty;

		/// <summary>
		/// 供应商名称
		/// </summary>
		[Column("SName")]
		public string SName
		{
			get { return _sName; }
			set { _sName = value; }
		}

		/// <summary>
		/// 数据库字段：SFullName
		/// </summary>
		private string _sFullName = string.Empty;

		/// <summary>
		/// 公司全称
		/// </summary>
		[Column("SFullName")]
		public string SFullName
		{
			get { return _sFullName; }
			set { _sFullName = value; }
		}

		/// <summary>
		/// 数据库字段：SAddress
		/// </summary>
		private string _sAddress = string.Empty;

		/// <summary>
		/// 公司地址
		/// </summary>
		[Column("SAddress")]
		public string SAddress
		{
			get { return _sAddress; }
			set { _sAddress = value; }
		}

		/// <summary>
		/// 数据库字段：SLinker
		/// </summary>
		private string _sLinker = string.Empty;

		/// <summary>
		/// 联系人
		/// </summary>
		[Column("SLinker")]
		public string SLinker
		{
			get { return _sLinker; }
			set { _sLinker = value; }
		}

		/// <summary>
		/// 数据库字段：SLinkPhone
		/// </summary>
		private string _sLinkPhone = string.Empty;

		/// <summary>
		/// 联系人电话
		/// </summary>
		[Column("SLinkPhone")]
		public string SLinkPhone
		{
			get { return _sLinkPhone; }
			set { _sLinkPhone = value; }
		}

		/// <summary>
		/// 数据库字段：SLinkQQ
		/// </summary>
		private string _sLinkQQ = string.Empty;

		/// <summary>
		/// 联系人QQ
		/// </summary>
		[Column("SLinkQQ")]
		public string SLinkQQ
		{
			get { return _sLinkQQ; }
			set { _sLinkQQ = value; }
		}

		/// <summary>
		/// 数据库字段：SLinkMail
		/// </summary>
		private string _sLinkMail = string.Empty;

		/// <summary>
		/// 联系人邮箱
		/// </summary>
		[Column("SLinkMail")]
		public string SLinkMail
		{
			get { return _sLinkMail; }
			set { _sLinkMail = value; }
		}

		/// <summary>
		/// 数据库字段：SLinkFax
		/// </summary>
		private string _sLinkFax = string.Empty;

		/// <summary>
		/// 联系人传真
		/// </summary>
		[Column("SLinkFax")]
		public string SLinkFax
		{
			get { return _sLinkFax; }
			set { _sLinkFax = value; }
		}

		/// <summary>
		/// 数据库字段：SFinanceLinker
		/// </summary>
		private string _sFinanceLinker = string.Empty;

		/// <summary>
		/// 财务联系人
		/// </summary>
		[Column("SFinanceLinker")]
		public string SFinanceLinker
		{
			get { return _sFinanceLinker; }
			set { _sFinanceLinker = value; }
		}

		/// <summary>
		/// 数据库字段：SFinancePhone
		/// </summary>
		private string _sFinancePhone = string.Empty;

		/// <summary>
		/// 财务电话
		/// </summary>
		[Column("SFinancePhone")]
		public string SFinancePhone
		{
			get { return _sFinancePhone; }
			set { _sFinancePhone = value; }
		}

		/// <summary>
		/// 数据库字段：SFinanceBankName
		/// </summary>
		private string _sFinanceBankName = string.Empty;

		/// <summary>
		/// 开户行
		/// </summary>
		[Column("SFinanceBankName")]
		public string SFinanceBankName
		{
			get { return _sFinanceBankName; }
			set { _sFinanceBankName = value; }
		}

		/// <summary>
		/// 数据库字段：SFinanceAccount
		/// </summary>
		private string _sFinanceAccount = string.Empty;

		/// <summary>
		/// 账户
		/// </summary>
		[Column("SFinanceAccount")]
		public string SFinanceAccount
		{
			get { return _sFinanceAccount; }
			set { _sFinanceAccount = value; }
		}

		/// <summary>
		/// 数据库字段：SFinanceName
		/// </summary>
		private string _sFinanceName = string.Empty;

		/// <summary>
		/// 户名
		/// </summary>
		[Column("SFinanceName")]
		public string SFinanceName
		{
			get { return _sFinanceName; }
			set { _sFinanceName = value; }
		}

		/// <summary>
		/// 数据库字段：SInvoiceTitle
		/// </summary>
		private string _sInvoiceTitle = string.Empty;

		/// <summary>
		/// 发票抬头
		/// </summary>
		[Column("SInvoiceTitle")]
		public string SInvoiceTitle
		{
			get { return _sInvoiceTitle; }
			set { _sInvoiceTitle = value; }
		}

		/// <summary>
		/// 数据库字段：SInvoiceTax
		/// </summary>
		private string _sInvoiceTax = string.Empty;

		/// <summary>
		/// 税号
		/// </summary>
		[Column("SInvoiceTax")]
		public string SInvoiceTax
		{
			get { return _sInvoiceTax; }
			set { _sInvoiceTax = value; }
		}

		/// <summary>
		/// 数据库字段：SInvoiceItem
		/// </summary>
		private string _sInvoiceItem = string.Empty;

		/// <summary>
		/// 发票项目
		/// </summary>
		[Column("SInvoiceItem")]
		public string SInvoiceItem
		{
			get { return _sInvoiceItem; }
			set { _sInvoiceItem = value; }
		}

		/// <summary>
		/// 数据库字段：SInvoiceType
		/// </summary>
		private string _sInvoiceType = string.Empty;

		/// <summary>
		/// 发票类型
		/// </summary>
		[Column("SInvoiceType")]
		public string SInvoiceType
		{
			get { return _sInvoiceType; }
			set { _sInvoiceType = value; }
		}

		/// <summary>
		/// 数据库字段：SCooperationBegin
		/// </summary>
		private DateTime _sCooperationBegin = DateTime.Now;

		/// <summary>
		/// 合作开始时间
		/// </summary>
		[Column("SCooperationBegin")]
		public DateTime SCooperationBegin
		{
			get { return _sCooperationBegin; }
			set { _sCooperationBegin = value; }
		}

		/// <summary>
		/// 数据库字段：SCooperationEnd
		/// </summary>
		private DateTime _sCooperationEnd = DateTime.Now;

		/// <summary>
		/// 合作结束时间
		/// </summary>
		[Column("SCooperationEnd")]
		public DateTime SCooperationEnd
		{
			get { return _sCooperationEnd; }
			set { _sCooperationEnd = value; }
		}

		/// <summary>
		/// 数据库字段：SIsValid
		/// </summary>
		private int _sIsValid = 0;

		/// <summary>
		/// 是否有效
		/// </summary>
		[Column("SIsValid")]
		public int SIsValid
		{
			get { return _sIsValid; }
			set { _sIsValid = value; }
		}
        /// <summary>
        /// 数据库字段：SPMId
        /// </summary>
        private int _sPMId = 0;

        /// <summary>
        /// 产品经理
        /// </summary>
        [Column("SPMId")]
        public int SPMId
        {
            get { return _sPMId; }
            set { _sPMId = value; }
        }
        /// <summary>
		/// 数据库字段：SPMName
		/// </summary>
		private string _sPMName = string.Empty;

        /// <summary>
        /// 产品经理
        /// </summary>
        [Column("SPMName")]
        public string SPMName
        {
            get { return _sPMName; }
            set { _sPMName = value; }
        }
 
        /// <summary>
        /// 数据库字段：SAddTime
        /// </summary>
        private DateTime _sAddTime = DateTime.Now;

		/// <summary>
		/// 新增时间
		/// </summary>
		[Column("SAddTime")]
		public DateTime SAddTime
		{
			get { return _sAddTime; }
			set { _sAddTime = value; }
		}

		/// <summary>
		/// 数据库字段：SUpdateTime
		/// </summary>
		private DateTime _sUpdateTime = DateTime.Now;

		/// <summary>
		/// 修改时间
		/// </summary>
		[Column("SUpdateTime")]
		public DateTime SUpdateTime
		{
			get { return _sUpdateTime; }
			set { _sUpdateTime = value; }
		}

		/// <summary>
		/// 数据库字段：SAddName
		/// </summary>
		private string _sAddName = string.Empty;

		/// <summary>
		/// 新增人
		/// </summary>
		[Column("SAddName")]
		public string SAddName
		{
			get { return _sAddName; }
			set { _sAddName = value; }
		}

		/// <summary>
		/// 数据库字段：SUpdateName
		/// </summary>
		private string _sUpdateName = string.Empty;

		/// <summary>
		/// 修改人
		/// </summary>
		[Column("SUpdateName")]
		public string SUpdateName
		{
			get { return _sUpdateName; }
			set { _sUpdateName = value; }
		}

	}
}
