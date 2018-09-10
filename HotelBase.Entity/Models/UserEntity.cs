using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Models
{
    public class UserListRequest : BaseRequest
    {

    }
    public class UserModelResponse : BaseResponse
    {
        public UserModel Model { get; set; }
    }

    public class UserModel
    {
        /// <summary> Id </summary>
        public int Id { get; set; }
        /// <summary> 登陆账户 </summary>
        public string Account { get; set; }
        /// <summary> 姓名 </summary>
        public string Name { get; set; }
        /// <summary> 部门Id </summary>
        public int DepartId { get; set; }
        /// <summary> 部门名称 </summary>
        public string DepartName { get; set; }
        /// <summary> 职责 </summary>
        public int R { get; set; }
        /// <summary> 职责 </summary>
        public string Responsibility { get; set; }
        /// <summary> 是否有效 </summary>
        public int IsValid { get; set; }
    }
}
