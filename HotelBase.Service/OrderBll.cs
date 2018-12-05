using HotelBase.DataAccess.Order;
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

        /// <summary>
        /// 录单详情页酒店信息查询
        /// </summary>
        /// <param name="hotelid"></param>
        /// <param name="roomid"></param>
        /// <param name="rooleid"></param>
        /// <returns></returns>
        public static BookSearchResponse GetHotelRuleDetial(int hotelid, int roomid, int rooleid,int supplierid)
        {
            return Ho_HotelOrderAccess.GetHotelRuleDetial(hotelid,roomid,rooleid, supplierid);
        }

        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BaseResponse AddOrderModel(HO_HotelOrderModel model)
        {
            var res = new BaseResponse();
            var id = 0;
            id = Ho_HotelOrderAccess.AddOrderModel(model);
            if (id <= 0)
            {
                res.Msg = "新增失败";
                return res;
            }
            else
            {
                res = new BaseResponse
                {
                    AddId = id,
                    IsSuccess = 1
                };
            }
            return res;
        }


        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BaseResponse SetOrder(int id, int type, int state, string serialid)
        {
            var i = Ho_HotelOrderAccess.UpdateOrderSerialid(id,type, state, serialid);
            var res = new BaseResponse
            {
                IsSuccess = i > 0 ? 1 : 0,
                Msg = i > 0 ? string.Empty : "更新失败",
            };
            return res;
        }

        /// <summary>
        /// 获取订单日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<HO_HotelOrderLogModel> GetOrderLogList(OrderLogSearchRequset request)
        {
            return Ho_HotelOrderAccess.GetOrderLogList(request);
        }
    }
}
