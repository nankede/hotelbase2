using HotelBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Service
{
    /// <summary>
    /// 内部API数据同步
    /// </summary>
    public static class OpenApi
    {
        /// <summary>
        /// 同步订单状态
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        public static string HotelOrderStatus(string serialId, int optType)
        {
            var url = $"http://openapi.lyqllx.com/order/update?serialId={serialId}&optType={optType}&distributor=2";
            return ApiHelper.HttpGet(url);
        }
    }
}
