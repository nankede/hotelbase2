﻿using HotelBase.DataAccess.Order;
using HotelBase.Entity;
using HotelBase.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Service
{
    public class OrderBll
    {
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<OrderSearchResponse> GetOrderList(OrderSearchRequset request)
        {
            return Ho_HotelOrderAccess.GetOrderList(request);
        }
    }
}
