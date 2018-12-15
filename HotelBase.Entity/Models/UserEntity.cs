using HotelBase.Common;
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

    /// <summary>
    /// 部门查询
    /// </summary>
    public class DepartistRequest : BaseRequest
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否有效 -1  全部
        /// </summary>
        public int IsValid { get; set; } = -1;
    }

    public class UserModelResponse : BaseResponse
    {
        public UserModel Model { get; set; }
    }
}
