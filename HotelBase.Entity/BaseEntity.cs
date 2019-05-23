using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity
{
    /// <summary>
    /// 请求基类
    /// </summary>
    public class BaseRequest
    {
        /// <summary>操作人 </summary>
        public int UserId { get; set; }
        /// <summary>操作人 </summary>
        public string UserName { get; set; }
        /// <summary>操作人 </summary>
        public int DepartId { get; set; }
        /// <summary>操作人 </summary>
        public string DetpartName { get; set; }
        /// <summary>页码 </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>每页个数 </summary>
        public int PageSize { get; set; } = 10;
    }
    /// <summary>
    /// 响应基类
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public int IsSuccess { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int AddId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Msg { get; set; }
    }

    /// <summary>
    /// 响应基类
    /// </summary>
    public class BasePageResponse<T> : BaseResponse
    {
        /// <summary>
        /// 列表
        /// </summary>
        public List<T> List { get; set; }

        public List<T> AllList { get; set; }
    }
}
