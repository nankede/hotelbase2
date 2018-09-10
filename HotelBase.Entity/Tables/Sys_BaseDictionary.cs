using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Tables
{
    public class Sys_BaseDictionary
    {
        /// <summary> Id </summary>
        public int Id { get; set; }
        /// <summary> 上级 </summary>
        public int DParentId { get; set; }
        /// <summary> Code </summary>
        public int DCode { get; set; }
        /// <summary>Name </summary>
        public string DName { get; set; }
        /// <summary>值 </summary>
        public string DValue { get; set; }
        /// <summary>描述 </summary>
        public string DRemark { get; set; }
        /// <summary>是否有效 </summary>
        public int DIsValid { get; set; }
        /// <summary>新增时间 </summary>
        public DateTime DAddTime { get; set; }
        /// <summary> 修改时间</summary>
        public DateTime DUpdateTime { get; set; }
    }
}
