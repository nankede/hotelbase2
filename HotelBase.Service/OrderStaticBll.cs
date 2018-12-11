using HotelBase.DataAccess.Order;
using HotelBase.Entity;
using HotelBase.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Service
{
    public class OrderStaticBll
    {
        /// <summary>
        /// 订单统计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<OrderStaticResponse> GetOrderStaticList(OrderStaticRequest request)
        {
            return Ho_HotelOrderAccess.GetOrderStaticList(request);
        }
    }
}
