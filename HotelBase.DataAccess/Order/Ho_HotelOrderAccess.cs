using Dapper;
using HotelBase.Entity;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.DataAccess.Order
{
    public class Ho_HotelOrderAccess
    {
        private static List<OrderSearchResponse> _DicList = new List<OrderSearchResponse>();

        /// <summary>
        /// 订单列表
        /// </summary>
        public static BasePageResponse<OrderSearchResponse> GetOrderList(OrderSearchRequset request)
        {
            var response = new BasePageResponse<OrderSearchResponse>();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT
                        HOCustomerSerialId,
                        HOSupplierSourceId,
                        HIId,
                        HName,
                        HOCheckInDate,
                        HOCheckOutDate,
                        HOLinkerName,
                        HOStatus,
                        HOSellPrice,
                        HOContractPrice
                    FROM
                        Ho_HotelOrder
                    WHERE
                        1 = 1");
            //订单号
            if (!string.IsNullOrWhiteSpace(request.HOCustomerSerialId))
            {
                sb.AppendFormat(" AND HOCustomerSerialId Like '%{0}%'", request.HOCustomerSerialId);
            }
            //人员归属查询
            if (!string.IsNullOrWhiteSpace(request.PeopleMobile) || !string.IsNullOrWhiteSpace(request.PeopleName))
            {
                if (request.CustomerType == 1)
                {
                    sb.AppendFormat(" AND HOCustomerName Like '%{0}%' AND HOCustomerMobile = '{1}'", request.PeopleName, request.PeopleMobile);
                }
                else
                {
                    sb.AppendFormat(" AND HOLinkerName Like '%{0}%' AND HOLinkerMobile = '{1}'", request.PeopleName, request.PeopleMobile);
                }
            }
            //时间
            if (!string.IsNullOrWhiteSpace(request.StartTime) || !string.IsNullOrWhiteSpace(request.EndTime))
            {
                if (request.TimeType == 1)
                {
                    sb.AppendFormat(" AND HOCheckInDate >= '{0}' AND HOCheckInDate<'{0}'", request.StartTime, Convert.ToDateTime(request.EndTime).AddDays(1).ToShortDateString());
                }
                else
                {
                    sb.AppendFormat(" AND HOAddTime >= '{0}' AND HOAddTime<'{0}'", request.StartTime, Convert.ToDateTime(request.EndTime).AddDays(1).ToShortDateString());
                }
            }
            //来源
            if (request.SourceId > 0)
            {
                sb.AppendFormat(" AND HOSupplierSourceId = {0}", request.SourceId);
            }
            //酒店名称
            if (!string.IsNullOrWhiteSpace(request.HotelName))
            {
                sb.AppendFormat(" AND HName Like '%{0}%'", request.HotelName);
            }
            //酒店Id
            if (!string.IsNullOrWhiteSpace(request.HIId))
            {
                sb.AppendFormat(" AND HIId = '{0}'", request.HIId);
            }

            //第三方流水
            if (!string.IsNullOrWhiteSpace(request.HOOutSerialId))
            {
                sb.AppendFormat(" AND HOOutSerialId = '{0}'", request.HOOutSerialId);
            }

            //订单状态
            if (request.HOStatus != null)
            {
                string state = "";
                foreach (var item in state)
                {
                    state += "'" + item + "',";
                }
                sb.AppendFormat(" AND HOStatus IN ({0})", state.Substring(0, state.Length - 1));
            }
            var list = MysqlHelper.GetList<OrderSearchResponse>(sb.ToString());
            var total = list?.Count ?? 0;
            if (total > 0)
            {
                response.IsSuccess = 1;
                response.Total = total;
                response.List = list.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)?.ToList();
            }
            return response;
        }


        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static HO_HotelOrderModel GetModel(int orderid)
        {
            if (orderid <= 0)
            {
                return null;
            }
            var para = new DynamicParameters();
            var sql = "SELECT * FROM ho_hotelorder  WHERE  id=@id  LIMIT 1;   ";
            para.Add("@id", orderid);
            var data = MysqlHelper.GetModel<HO_HotelOrderModel>(sql, para);
            return data;
        }
    }
}
