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



        /// <summary>
        /// 录单页面资源查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<BookSearchResponse> GetHotelRuleList(BookSearchRequest request)
        {
            var response = new BasePageResponse<BookSearchResponse>();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT
	                    b.HIName,
	                    b.HIAddress,
	                    b.HILinkPhone,
	                    r.HRName,
	                    r.HRBedType,
	                    rr.HRRBreakfastRule,
	                    rr.HRRCancelRule,
	                    rp.HRPContractPrice,
	                    rp.HRPSellPrice
                    FROM
	                    h_hotelinfo b
                    INNER JOIN h_hotelroom r ON r.HIId = b.Id
                    INNER JOIN h_hotelroomrule rr ON r.Id = rr.HRId
                    INNER JOIN h_hoteruleprice rp ON rr.Id = rp.HRRId
                    WHERE
	                    b.HIIsValid = 1
                    AND r.HRIsValid = 1
                    AND rr.HRRIsValid = 1
                    AND rp.HRPIsValid = 1
                    WHERE
                        1 = 1");
            //订单号
            if (!string.IsNullOrWhiteSpace(request.HotelName))
            {
                sb.AppendFormat(" AND b.HIName Like '%{0}%'", request.HotelName);
            }
            //酒店id
            if (request.HotelId > 0)
            {
                sb.AppendFormat(" AND  b.Id = {0}", request.HotelId);
            }
            //入离店时间
            if (!string.IsNullOrWhiteSpace(request.InBeginDate))
            {
                sb.AppendFormat(" AND rp.HRPDate >= '{0}'", request.InBeginDate);
            }
            //离店时间
            if (!string.IsNullOrWhiteSpace(request.InEndDate))
            {
                sb.AppendFormat(" AND rp.HRPDate<'{0}'", Convert.ToDateTime(request.InEndDate).AddDays(1).ToShortDateString());
            }
            var list = MysqlHelper.GetList<BookSearchResponse>(sb.ToString());
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
        /// 新增订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int AddOrderModel(HO_HotelOrderModel model)
        {
            var sql = new StringBuilder();
            sql.Append(" INSERT INTO `ho_hotelorder` (`HOCustomerSerialId`, `HIId`, `HName`, `HRId`, `HRName`, `HRRId`, `HRRName`, `HOSupplierId`, `HOSupperlierName`, `HOSupplierSourceId`, `HOSupplierSourceName`, `HOOutSerialId`, `HOSupplierSerialId`, `HOStatus`, `HOPayStatus`, `HORoomCount`, `HOChild`, `HOAdult`, `HoPlat1`, `HoPlat2`, `HOContractPrice`, `HOSellPrice`, `HOCustomerName`, `HOCustomerMobile`, `HOLinkerName`, `HOLinkerMobile`, `HORemark`, `HOCheckInDate`, `HOCheckOutDate`, `HOLastCheckInTime`, `HOAddId`, `HOAddName`, `HOAddDepartId`, `HOAddDepartName`, `HOAddTime`, `HOUpdateId`, `HOUpdateName`, `HOUpdateTime`) VALUES ");
            sql.Append("( @HOCustomerSerialId, @HIId, @HName, @HRId, @HRName, @HRRId, @HRRName, @HOSupplierId, @HOSupperlierName, @HOSupplierSourceId, @HOSupplierSourceName, @HOOutSerialId, @HOSupplierSerialId, @HOStatus, @HOPayStatus, @HORoomCount, @HOChild, @HOAdult, @HoPlat1, @HoPlat2, @HOContractPrice, @HOSellPrice, @HOCustomerName, @HOCustomerMobile, @HOLinkerName, @HOLinkerMobile, @HORemark, @HOCheckInDate, @HOCheckOutDate, @HOLastCheckInTime, @HOAddId, @HOAddName, @HOAddDepartId, @HOAddDepartName, @HOAddTime, @OUpdateId, @HOUpdateName, @HOUpdateTime)");
            var para = new DynamicParameters();
            para.Add("@HOCustomerSerialId", model.HOCustomerSerialId);
            para.Add("@HIId", model.HIId);
            para.Add("@HName", model.HName);
            para.Add("@HRId", model.HRId);
            para.Add("@HRName", model.HRName);
            para.Add("@HRRId", model.HRRId);
            para.Add("@HRRName", model.HRRName);
            para.Add("@HOSupplierId", model.HOSupplierId);
            para.Add("@HOSupperlierName", model.HOSupperlierName);
            para.Add("@HOSupplierSourceId", model.HOSupplierSourceId);
            para.Add("@HOSupplierSourceName", model.HOSupplierSourceName);
            para.Add("@HOOutSerialId", model.HOOutSerialId);
            para.Add("@HOSupplierSerialId", model.HOSupplierSerialId);
            para.Add("@HOStatus", model.HOStatus);
            para.Add("@HOPayStatus", model.HOPayStatus);
            para.Add("@HORoomCount", model.HORoomCount);
            para.Add("@HOChild", model.HOChild);
            para.Add("@HOAdult", model.HOAdult);
            para.Add("@HoPlat1", model.HoPlat1);
            para.Add("@HoPlat2", model.HoPlat2);
            para.Add("@HOContractPrice", model.HOContractPrice);
            para.Add("@HOSellPrice", model.HOSellPrice);
            para.Add("@HOCustomerName", model.HOCustomerName);
            para.Add("@HOCustomerMobile", model.HOCustomerMobile);
            para.Add("@HOLinkerName", model.HOLinkerName);
            para.Add("@HOLinkerMobile", model.HOLinkerMobile);
            para.Add("@HORemark", model.HORemark);
            para.Add("@HOCheckInDate", model.HOCheckInDate);
            para.Add("@HOCheckOutDate", model.HOCheckOutDate);
            para.Add("@HOLastCheckInTime", model.HOLastCheckInTime);
            para.Add("@HOAddId", model.HOAddId);
            para.Add("@HOAddName", model.HOAddName);
            para.Add("@HOAddDepartId", model.HOAddDepartId);
            para.Add("@HOAddDepartName", model.HOAddDepartName);
            para.Add("@HOAddTime", model.HOAddTime);
            para.Add("@HOUpdateId", model.HOUpdateId);
            para.Add("@HOUpdateName", model.HOUpdateName);
            para.Add("@HOUpdateTime ", model.HOUpdateTime);
            var id = MysqlHelper.Insert(sql.ToString(), para);
            //_DicList = null;
            return id;
        }


        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="orderid">订单id</param>
        /// <param name="type">修改类型  0：状态  1：第三方流水号  2：供应商流水号</param>
        /// <param name="serialid"></param>
        /// <returns></returns>

        public static int UpdateOrderSerialid(int orderid,int type,int state,string serialid)
        {
            if (orderid==0) return 0;
            var sql = new StringBuilder();
            sql.Append(" UPDATE `ho_hotelorder` SET ");
            switch (type)
            {
                case 0:
                    sql.AppendFormat(" `HOStatus` = {0}  ", state);
                    break;
                case 1:
                    sql.AppendFormat(" `HOOutSerialId` = {0}  ", serialid);
                    break;
                case 2:
                    sql.AppendFormat(" `HOSupplierSerialId` = {0}  ", serialid);
                    break;
            }
            sql.Append(" WHERE  `Id` =@Id   Limit 1;  ");
            var c = MysqlHelper.Update(sql.ToString());
            return c;
        }
    }
}
