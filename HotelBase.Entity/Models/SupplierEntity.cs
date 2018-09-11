using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Models
{
    /// <summary>
    /// 供应商查询
    /// </summary>
    public class SupplierSearchRequest : BaseRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 是否有效 1 有效  2 无效
        /// </summary>
        public int IsValid { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public int SourceId { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkerName { get; set; }
    }

    /// <summary>
    /// 供应商
    /// </summary>
    public class SupplierModel
    {
        /// <summary>Id</summary>
        public int Id { get; set; }
        /// <summary>Code</summary>
        public string Code { get; set; }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>产品经理</summary>
        public string PmName { get; set; }
        /// <summary>联系人</summary>
        public string Linker { get; set; }
        /// <summary>来源</summary>
        public int SourceId { get; set; }
        /// <summary>来源</summary>
        public string Source { get; set; }
    }
}
