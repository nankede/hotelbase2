using HotelBase.DataAccess.Order;
using HotelBase.Entity;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Service
{
    public class OrderLogBll
    {
        /// <summary>
        /// 新增订单日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddOrderModel(HO_HotelOrderLogModel model)
        {
            var id = Ho_HotelOrderLogAccess.AddOrderLogModel(model);
            return id >= 1;
        }
    }
}
