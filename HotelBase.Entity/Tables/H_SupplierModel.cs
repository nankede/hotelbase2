using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// 供应商表实体
    /// </summary>
    public class H_SupplierModel
    {
        /// <summary>自增主键</summary>
        public int Id { get; set; }
        /// <summary>来源</summary>
        public int SSourceId { get; set; }
        /// <summary>供应商编号</summary>
        public string SCode { get; set; }
        /// <summary>供应商名称</summary>
        public string SName { get; set; }
        /// <summary>公司全称</summary>
        public string SFullName { get; set; }
        /// <summary>公司地址</summary>
        public string SAddress { get; set; }
        /// <summary>联系人</summary>
        public string SLinker { get; set; }
        /// <summary>联系人电话</summary>
        public string SLinkPhone { get; set; }
        /// <summary>联系人QQ</summary>
        public string SLinkQQ { get; set; }
        /// <summary>联系人邮箱</summary>
        public string SLinkMail { get; set; }
        /// <summary>联系人传真</summary>
        public string SLinkFax { get; set; }
        /// <summary>财务联系人</summary>
        public string SFinanceLinker { get; set; }
        /// <summary>财务电话</summary>
        public string SFinancePhone { get; set; }
        /// <summary>开户行</summary>
        public string SFinanceBankName { get; set; }
        /// <summary>账户</summary>
        public string SFinanceAccount { get; set; }
        /// <summary>户名</summary>
        public string SFinanceName { get; set; }
        /// <summary>发票抬头</summary>
        public string SInvoiceTitle { get; set; }
        /// <summary>税号</summary>
        public string SInvoiceTax { get; set; }
        /// <summary>发票项目</summary>
        public string SInvoiceItem { get; set; }
        /// <summary>发票类型</summary>
        public string SInvoiceType { get; set; }
        /// <summary>合作开始时间</summary>
        public DateTime SCooperationBegin { get; set; }
        /// <summary>合作结束时间</summary>
        public DateTime SCooperationEnd { get; set; }
        /// <summary>是否有效</summary>
        public int SIsValid { get; set; }
        /// <summary>产品经理</summary>
        public int SPMId { get; set; }
        /// <summary>产品经理</summary>
        public string SPMName { get; set; }
        /// <summary>新增人</summary>
        public string SAddName { get; set; }
        /// <summary>新增时间</summary>
        public DateTime SAddTime { get; set; }
        /// <summary>修改人</summary>
        public string SUpdateName { get; set; }
        /// <summary>修改时间</summary>
        public DateTime SUpdateTime { get; set; }

    }
}
