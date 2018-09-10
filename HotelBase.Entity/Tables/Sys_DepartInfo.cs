using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Tables
{
    public class Sys_DepartInfo
    {
        /// <summary> Id </summary>
        public int Id { get; set; }
        /// <summary> 部门名称 </summary>
        public string DIName { get; set; }
        /// <summary> 上级部门 </summary>
        public int DIParentId { get; set; }
        /// <summary> 负责人 </summary>
        public int DILeaderId { get; set; }
        /// <summary>负责人 </summary>
        public string DILeaderName { get; set; }
        /// <summary>是否有效 </summary>
        public int DIIsValid { get; set; }
        /// <summary>新增时间 </summary>
        public DateTime DIAddTime { get; set; }
        /// <summary> 修改时间</summary>
        public DateTime DIUpdateTime { get; set; }
        /// <summary> 新增人</summary>
        public string DIAddName { get; set; }
        /// <summary> 修改人</summary>
        public string DIUpdateName { get; set; }
    }
}
