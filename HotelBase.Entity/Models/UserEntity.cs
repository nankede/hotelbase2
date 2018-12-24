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
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public int IsValid { get; set; }
    }

    /// <summary>
    /// 部门查询
    /// </summary>
    public class DepartistRequest : BaseRequest
    {
        public int Id { get; set; }
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

    /// <summary>
    /// 响应
    /// </summary>
    public class DepartModelResponse : BaseResponse
    {
        public DepartModel Model { get; set; }

    }
    /// <summary>
    /// 部门实体
    /// </summary>
    public class DepartModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartId { get; set; }
        public string DepartName { get; set; }
        public int LearderId { get; set; }
        public string LearderName { get; set; }

    }
}
