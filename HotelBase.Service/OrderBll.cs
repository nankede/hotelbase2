﻿using HotelBase.DataAccess.Order;
using HotelBase.Entity;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
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
        
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static HO_HotelOrderModel GetModel(int orderid)
        {
            return Ho_HotelOrderAccess.GetModel(orderid);
        }

        /// <summary>
        /// 预定资源查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<BookSearchResponse> GetHotelRuleList(BookSearchRequest request)
        {
            return Ho_HotelOrderAccess.GetHotelRuleList(request);
        }
    }
}
