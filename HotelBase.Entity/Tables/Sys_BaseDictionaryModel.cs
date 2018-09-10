using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// 数据字典表
    /// </summary>
    public class Sys_BaseDictionaryModel
    {
        /// <summary> 自增主键 </summary>
        public int Id { get; set; }
        /// <summary> 上级Id</summary>
        public int DParentId { get; set; }
        /// <summary>int数据 </summary>
        public int DCode { get; set; }
        /// <summary> 是否有效 </summary>
        public int DIsValid { get; set; }
        /// <summary> 排序</summary>
        public int DSort { get; set; }
        /// <summary> 父级名称</summary>
        public string DParentName { get; set; }
        /// <summary> 名称 </summary>
        public string DName { get; set; }
        /// <summary>string数据 </summary>
        public string DValue { get; set; }
        /// <summary>描述 </summary>
        public string DRemark { get; set; }
        /// <summary> 新增时间</summary>
        public DateTime DAddTime { get; set; }
        /// <summary>修改时间 </summary>
        public DateTime DUpdateTime { get; set; }
    }
}
