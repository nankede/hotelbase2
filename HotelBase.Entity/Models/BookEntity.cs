using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Models
{
    public class BookSearchRequest:BaseRequest
    {
        /// <summary>
        /// 酒店名称
        /// </summary>
        public string HotelName { get; set; }

        /// <summary>
        /// 酒店Id
        /// </summary>
        public int HotelId { get; set; }

        /// <summary>
        /// 入住开始时间
        /// </summary>
        public string InBeginDate { get; set; }

        /// <summary>
        /// 入住结束时间
        /// </summary>
        public string InEndDate { get; set; }
    }

    /// <summary>
    /// 录单资源查询页面
    /// </summary>
    public class BookSearchResponse : BaseResponse
    {
        /// <summary>
        /// 酒店id
        /// </summary>
        public int HotelId { get; set; }

        /// <summary>
        /// 房型id
        /// </summary>
        public int HotelRoomId { get; set; }

        /// <summary>
        /// 政策id
        /// </summary>
        public int HotelRoomRuleId { get; set; }

        /// <summary>
        /// 酒店名称
        /// </summary>
        public string HotelName { get; set; }

        /// <summary>
        /// 酒店电话
        /// </summary>
        public string HotelTel { get; set; }

        /// <summary>
        /// 酒店地址
        /// </summary>
        public string HotelAddress { get; set; }

        /// <summary>
        /// 房型名称
        /// </summary>
        public string HotelRoomName { get; set; }

        /// <summary>
        /// 床型名称
        /// </summary>
        public string HotelRoomBedType { get; set; }

        /// <summary>
        /// 早餐规则
        /// </summary>
        public string HotelRoomBreakfastRule { get; set; }

        /// <summary>
        /// 取消规则
        /// </summary>
        public string HotelRoomCancelRule { get; set; }

        /// <summary>
        /// 售卖价
        /// </summary>
        public decimal HoteRoomRuleSellPrice { get; set; }
    }
}
