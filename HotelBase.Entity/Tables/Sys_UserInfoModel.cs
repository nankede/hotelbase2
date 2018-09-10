using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Tables
{
    public class Sys_UserInfoModel
    {
        /// <summary> Id </summary>
        public int Id { get; set; }
        /// <summary> 账户 </summary>
        public string UIAccount { get; set; }
        /// <summary>密码 </summary>
        public string UIPassWord { get; set; }
        /// <summary>姓名 </summary>
        public string UIName { get; set; }
        /// <summary> 职责 </summary>
        public int UIResponsibility { get; set; }
        /// <summary>是否有效 </summary>
        public int UIIsValid { get; set; }
        /// <summary>新增时间 </summary>
        public DateTime UIAddTime { get; set; }
        /// <summary> 修改时间</summary>
        public DateTime UIUpdateTime { get; set; }
        /// <summary> 新增人</summary>
        public string UIAddName { get; set; }
        /// <summary> 修改人</summary>
        public string UIUpdateName { get; set; }
        
    }
}
