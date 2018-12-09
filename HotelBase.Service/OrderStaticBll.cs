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
        public static BasePageResponse<OrderStaticResponse> GetOrderList(OrderStaticRequest request)
        {
            //return Ho_HotelOrderAccess.GetOrderStaitcList(request);
            return null;
        }
    }
}
