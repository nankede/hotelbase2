using Component.Access;
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
    public class Ho_HotelOrderAccess : BaseAccess<HO_HotelOrderModel>
    {
        public Ho_HotelOrderAccess() : base(MysqlHelper.Db_HotelBase)
        {

        }
        //private static List<OrderSearchResponse> List = new List<OrderSearchResponse>();

        /// <summary>
        /// 订单列表
        /// </summary>
        public static BasePageResponse<OrderSearchResponse> GetOrderList(OrderSearchRequset request)
        {
            var response = new BasePageResponse<OrderSearchResponse>();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT
                        Id,
                        HODistributorSerialId,
                        HODistributorName,
                        HOSupplierSerialId,
                        HOCustomerSerialId,
                        HOSupplierSourceName,
                        HIId,
                        HName,
                        DATE_FORMAT(HOCheckInDate,'%Y-%m-%d') AS HOCheckInDate,
                        DATE_FORMAT(HOCheckOutDate,'%Y-%m-%d') AS HOCheckOutDate,
                        HOAddTime,
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
            if (request.CustomerType == 1)
            {
                if (!string.IsNullOrWhiteSpace(request.PeopleName))
                {
                    sb.AppendFormat(" AND HOCustomerName Like '%{0}%'", request.PeopleName);
                }
                if (!string.IsNullOrWhiteSpace(request.PeopleMobile))
                {
                    sb.AppendFormat(" AND HOCustomerMobile = '{1}'", request.PeopleMobile);
                }
            }
            if (request.CustomerType == 2)
            {
                if (!string.IsNullOrWhiteSpace(request.PeopleName))
                {
                    sb.AppendFormat(" AND HOLinkerName Like '%{0}%'", request.PeopleName);
                }
                if (!string.IsNullOrWhiteSpace(request.PeopleMobile))
                {
                    sb.AppendFormat(" AND HOLinkerMobile = '{1}'", request.PeopleMobile);
                }

            }
            //时间
            if (request.TimeType == 1)
            {
                if (!string.IsNullOrWhiteSpace(request.StartTime))
                {
                    sb.AppendFormat(" AND HOCheckInDate >= '{0}'", request.StartTime);
                }
                if (!string.IsNullOrWhiteSpace(request.EndTime))
                {
                    sb.AppendFormat(" AND HOCheckInDate<'{0}'", Convert.ToDateTime(request.EndTime).AddDays(1).ToShortDateString());
                }
            }
            if (request.TimeType == 2)
            {
                if (!string.IsNullOrWhiteSpace(request.StartTime))
                {
                    sb.AppendFormat(" AND HOAddTime >= '{0}'", request.StartTime);
                }
                if (!string.IsNullOrWhiteSpace(request.EndTime))
                {
                    sb.AppendFormat(" AND HOAddTime<'{0}'", Convert.ToDateTime(request.EndTime).AddDays(1).ToShortDateString());
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

            //供应商流水号
            if (!string.IsNullOrWhiteSpace(request.HOSupplierSerialId))
            {
                sb.AppendFormat(" AND HOSupplierSerialId = '{0}'", request.HOSupplierSerialId);
            }

            //分销商流水号
            if (!string.IsNullOrWhiteSpace(request.HODistributorSerialId))
            {
                sb.AppendFormat(" AND HODistributorSerialId = '{0}'", request.HODistributorSerialId);
            }

            //订单状态
            if (!string.IsNullOrWhiteSpace(request.HOStatus))
            {
                //var state = request.HOStatus.Split(',');
                //var searchstatus = "";
                //foreach (var item in state)
                //{
                //    searchstatus += "'" + item + "',";
                //}
                //sb.AppendFormat(" AND HOStatus IN ({0})", searchstatus.Substring(0, state.Length - 1));
                sb.AppendFormat(" AND HOStatus IN ({0})", request.HOStatus);
            }
            sb.Append(" Order By HOAddTime DESC");
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
        public static OrdrModel GetModel(int orderid)
        {
            if (orderid <= 0)
            {
                return null;
            }
            var para = new DynamicParameters();
            var sql = @"SELECT
                        hs.SSubWay,
	                    hs.SLinkMail,
	                    hs.SLinkFax,
                        hi.HIOutId,
						hr.HROutId,
	                    ho.*
                    FROM
                        ho_hotelorder ho
                        INNER join h_hotelinfo hi ON hi.Id=ho.HIId
                        INNER join h_hotelroom hr ON hr.Id=ho.HRId
                        INNER JOIN h_supplier hs ON ho.HOSupplierId = hs.Id
                    WHERE
                        ho.id = @id
                        LIMIT 1; ";
            para.Add("@id", orderid);
            var data = MysqlHelper.GetModel<OrdrModel>(sql, para);
            return data;
        }


        /// <summary>
        /// 导出订单查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static List<OrdrModel> GetExportOrder(OrderStaticRequest request)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            if (request.TimeType == 1)
            {
                if (!string.IsNullOrWhiteSpace(request.StartTime))
                {
                    sbwhere.AppendFormat(" AND ho.HOAddTime>= '{0}'", request.StartTime);
                }
                if (!string.IsNullOrWhiteSpace(request.EndTime))
                {
                    sbwhere.AppendFormat(" AND ho.HOAddTime< '{0}'", request.EndTime);
                }
            }
            if (request.TimeType == 2)
            {
                if (!string.IsNullOrWhiteSpace(request.StartTime))
                {
                    sbwhere.AppendFormat(" AND ho.HOCheckOutDate>= '{0}'", request.StartTime);
                }
                if (!string.IsNullOrWhiteSpace(request.EndTime))
                {
                    sbwhere.AppendFormat(" AND ho.HOCheckOutDate< '{0}'", request.EndTime);
                }
            }
            if (request.PrivoceId > 0)
            {
                sbwhere.AppendFormat(" AND hb.HIProvinceId= {0}", request.PrivoceId);
            }
            if (request.CityId > 0)
            {
                sbwhere.AppendFormat(" AND hb.HICityId= {0}", request.CityId);
            }
            if (request.Part1 > 0)
            {
                sbwhere.AppendFormat(" AND ho.HoPlat1= {0}", request.Part1);
            }
            if (request.Part2 > 0)
            {
                sbwhere.AppendFormat(" AND ho.HoPlat2= {0}", request.Part2);
            }
            if (!string.IsNullOrWhiteSpace(request.HotelName))
            {
                sbwhere.AppendFormat(" AND ho.HName IN ({0})", request.HotelName);
            }
            if (!string.IsNullOrWhiteSpace(request.HotelId))
            {
                sbwhere.AppendFormat(" AND ho.HId IN ({0})", request.HotelId);
            }
            if (!string.IsNullOrWhiteSpace(request.SupplierName))
            {
                sbwhere.AppendFormat(" AND ho.HOSupperlierName Like '%{0}%'", request.SupplierName);
            }
            if (request.SupplierSource > 0)
            {
                sbwhere.AppendFormat(" AND ho.HOSupplierSourceId ={0}", request.SupplierSource);
            }
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                sbwhere.AppendFormat(" AND ho.HOStatus IN ({0})", request.Status);
            }
            sb.AppendFormat(@" SELECT
	                                ho.*,
	                                hb.HIProvince AS ProviceName,
	                                hb.HICity AS CityName
                                FROM
	                                ho_hotelorder ho
                                INNER JOIN h_hotelinfo hb ON hb.Id = ho.HIId
                                WHERE
	                                1 = 1
                                     {0}", sbwhere.ToString());
            var list = MysqlHelper.GetList<OrdrModel>(sb.ToString());
            var total = list?.Count ?? 0;
            return list;
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
	                        b.Id AS HotelId,
	                        b.HIName AS HotelName,
	                        b.HIAddress AS HotelAddress,
	                        b.HILinkPhone AS HotelTel,
	                        MIN(rp.HRPSellPrice) AS HoteRoomRuleSellPrice
                        FROM
	                        h_hotelinfo b
                        INNER JOIN h_hotelroom r ON r.HIId = b.Id
                        INNER JOIN h_hotelroomrule rr ON r.Id = rr.HRId
                        INNER JOIN h_hoteruleprice rp ON rr.Id = rp.HRRId
                        WHERE
	                        b.HIIsValid = 1
                        AND r.HRIsValid = 1
                        AND rr.HRRIsValid = 1
                        AND rp.HRPIsValid = 1");
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
            sb.Append(@" GROUP BY b.Id ,
	                        b.HIName,
	                        b.HIAddress,
	                        b.HILinkPhone");
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
        /// 资源详情查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<BookSearchResponse> GetHotelRuleDetialList(BookSearchRequest request)
        {
            var response = new BasePageResponse<BookSearchResponse>();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT DISTINCT
	                        b.Id AS HotelId,
	                        r.Id AS HotelRoomId,
	                        rr.Id AS HotelRoomRuleId,
                            rr.HRRSupplierId AS HotelSupplierId,
                            rr.HRRSupplierName AS HotelSupplierName,
	                        b.HIName AS HotelName,
	                        b.HIAddress AS HotelAddress,
	                        b.HILinkPhone AS HotelTel,
	                        r.HRName AS HotelRoomName,
	                        r.HRBedType AS HotelRoomBedType,
                            rr.HRRName as HotelRoomRuleName,
	                        rr.HRRBreakfastRule AS HotelRoomBreakfastRule,
                            rr.HRRBreakfastRuleName AS HotelRoomBreakfastRuleName,
	                        rr.HRRCancelRule AS HotelRoomCancelRule,
	                        rr.HRRCancelRuleName AS HotelRoomCancelRuleName,
	                        rps.HRPSellPrice AS HoteRoomRuleSellPrice
                        FROM
	                        h_hotelinfo b
                        INNER JOIN h_hotelroom r ON r.HIId = b.Id
                        INNER JOIN h_hotelroomrule rr ON r.Id = rr.HRId
                        INNER JOIN (
	                                SELECT DISTINCT
		                                rp.HRRId,
		                                rp.HRPSellPrice,
		                                rp.HRPIsValid,
		                                rp.HRPDate
	                                FROM
		                                h_hoteruleprice rp
	                                WHERE
		                                rp.HRPDate >= '{0}'
	                                AND rp.HRPDate < '{1}'
	                                GROUP BY
		                                rp.HRRId
	                                ORDER BY
		                                rp.HRPDate ASC
                                ) AS rps ON rr.Id = rps.HRRId
                        WHERE
	                        b.HIIsValid = 1
                        AND r.HRIsValid = 1
                        AND rr.HRRIsValid = 1
                        AND rps.HRPIsValid = 1", !string.IsNullOrWhiteSpace(request.InBeginDate) ? request.InBeginDate : DateTime.Now.ToShortDateString(), 
                        !string.IsNullOrWhiteSpace(request.InEndDate) ? request.InEndDate : DateTime.Now.AddDays(1).ToShortDateString());
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
            ////入离店时间
            //if (!string.IsNullOrWhiteSpace(request.InBeginDate))
            //{
            //    sb.AppendFormat(" AND rp.HRPDate >= '{0}'", request.InBeginDate);
            //}
            ////离店时间
            //if (!string.IsNullOrWhiteSpace(request.InEndDate))
            //{
            //    sb.AppendFormat(" AND rp.HRPDate<'{0}'", Convert.ToDateTime(request.InEndDate).AddDays(1).ToShortDateString());
            //}
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
        /// 录单详情页酒店信息查询
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="roomid"></param>
        /// <param name="ruleid"></param>
        /// <param name="supplierid"></param>
        /// <returns></returns>
        public static BookSearchResponse GetHotelRuleDetial(int hid, int roomid, int ruleid, int supplierid)
        {
            var response = new BookSearchResponse();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT
	                            b.Id AS HotelId,
	                            r.Id AS HotelRoomId,
	                            rr.Id AS HotelRoomRuleId,
	                            b.HIName AS HotelName,
	                            b.HIAddress AS HotelAddress,
	                            b.HILinkPhone AS HotelTel,
	                            r.HRName AS HotelRoomName,
                                rr.HRRName AS HotelRoomRuleName,
	                            r.HRBedType AS HotelRoomBedType,
	                            rr.HRRBreakfastRuleName AS HotelRoomBreakfastRuleName,
	                            rr.HRRCancelRule AS HotelRoomCancelRule,
	                            rr.HRRCancelRuleName AS HotelRoomCancelRuleName,
	                            rr.HRRSourceId AS HotelSupplierSourceId,
	                            rr.HRRSourceName AS HotelSupplierSourceName,
	                            rr.HRRSupplierId AS HotelSupplierId,
	                            rr.HRRSupplierName AS HotelSupplierName,
	                            rp.HRPSellPrice AS HoteRoomRuleSellPrice,
	                            rp.HRPContractPrice AS HoteRoomRuleContractPrice,
	                            s.SSubWay AS HotelSupplierSubWay,
	                            s.SLinkMail AS HotelSupplierLinkMail
                            FROM
	                            h_hotelinfo b
                            INNER JOIN h_hotelroom r ON r.HIId = b.Id
                            INNER JOIN h_hotelroomrule rr ON r.Id = rr.HRId
                            INNER JOIN h_hoteruleprice rp ON rr.Id = rp.HRRId
                            INNER JOIN h_supplier s ON s.Id = rr.HRRSupplierId
                            WHERE
	                            b.HIIsValid = 1
                            AND r.HRIsValid = 1
                            AND rr.HRRIsValid = 1
                            AND rp.HRPIsValid = 1
                            AND s.SIsValid = 1");
            //酒店id
            if (hid > 0)
            {
                sb.AppendFormat(" AND  b.Id = {0}", hid);
            }
            //房间id
            if (roomid > 0)
            {
                sb.AppendFormat(" AND  r.Id = {0}", roomid);
            }
            //房型id
            if (ruleid > 0)
            {
                sb.AppendFormat(" AND  rr.Id = {0}", ruleid);
            }
            //供应商id
            if (ruleid > 0)
            {
                sb.AppendFormat(" AND  s.Id = {0}", supplierid);
            }
            var list = MysqlHelper.GetList<BookSearchResponse>(sb.ToString());
            if (list != null && list.Any())
            {
                response = list.FirstOrDefault();
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
            sql.Append(" INSERT INTO `ho_hotelorder` (`HOCustomerSerialId`, `HIId`, `HName`, `HRId`, `HRName`, `HRRId`, `HRRName`, `HOSupplierId`, `HOSupperlierName`,`HODistributorId`, `HODistributorName`, `HOSupplierSourceId`, `HOSupplierSourceName`, `HOOutSerialId`,`HODistributorSerialId`,`HOSupplierCorfirmSerialId`,`HONight`,`HOSupplierSerialId`, `HOStatus`, `HOPayStatus`, `HORoomCount`, `HOChild`, `HOAdult`, `HoPlat1`, `HoPlat2`, `HOContractPrice`, `HOSellPrice`, `HOCustomerName`, `HOCustomerMobile`, `HOLinkerName`, `HOLinkerMobile`, `HORemark`, `HOCheckInDate`, `HOCheckOutDate`, `HOLastCheckInTime`, `HOAddId`, `HOAddName`, `HOAddDepartId`, `HOAddDepartName`, `HOAddTime`, `HOUpdateId`, `HOUpdateName`, `HOUpdateTime`) VALUES ");
            sql.Append("( @HOCustomerSerialId, @HIId, @HName, @HRId, @HRName, @HRRId, @HRRName, @HOSupplierId, @HOSupperlierName,@HODistributorId, @HODistributorName, @HOSupplierSourceId, @HOSupplierSourceName, @HOOutSerialId, @HODistributorSerialId, @HOSupplierCorfirmSerialId, @HONight, @HOSupplierSerialId, @HOStatus, @HOPayStatus, @HORoomCount, @HOChild, @HOAdult, @HoPlat1, @HoPlat2, @HOContractPrice, @HOSellPrice, @HOCustomerName, @HOCustomerMobile, @HOLinkerName, @HOLinkerMobile, @HORemark, @HOCheckInDate, @HOCheckOutDate, @HOLastCheckInTime, @HOAddId, @HOAddName, @HOAddDepartId, @HOAddDepartName, @HOAddTime, @HOUpdateId, @HOUpdateName, @HOUpdateTime)");
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
            para.Add("@HODistributorId", model.HODistributorId);
            para.Add("@HODistributorName", model.HODistributorName);
            para.Add("@HOSupplierSourceId", model.HOSupplierSourceId);
            para.Add("@HOSupplierSourceName", model.HOSupplierSourceName);
            para.Add("@HOOutSerialId", model.HOOutSerialId);
            para.Add("@HODistributorSerialId", model.HODistributorSerialId);
            para.Add("@HOSupplierCorfirmSerialId", model.HOSupplierCorfirmSerialId);
            para.Add("@HONight", model.HONight);
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

        public static int UpdateOrderSerialid(string orderid, string type, string state, string serialid)
        {
            if (string.IsNullOrWhiteSpace(orderid)) return 0;
            var sql = new StringBuilder();
            sql.Append(" UPDATE `ho_hotelorder` SET ");
            switch (type)
            {
                case "1":
                    if (!string.IsNullOrWhiteSpace(state) && Convert.ToInt32(state) > 0)
                    {
                        sql.AppendFormat(" `HOStatus` = {0}  ", state);
                    }
                    break;
                case "3":
                    if (!string.IsNullOrWhiteSpace(serialid))
                    {
                        sql.AppendFormat(" `HOSupplierCorfirmSerialId` = '{0}'  ", serialid);
                    }
                    break;
                case "2":
                    if (!string.IsNullOrWhiteSpace(serialid))
                    {
                        sql.AppendFormat(" `HOOutSerialId` = '{0}'  ", serialid);
                    }
                    break;
            }
            sql.AppendFormat(" WHERE  `HOCustomerSerialId` ='{0}'   Limit 1;  ", orderid);
            var c = MysqlHelper.Update(sql.ToString());
            return c;
        }

        /// <summary>
        /// 获取订单日志
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static BasePageResponse<HO_HotelOrderLogModel> GetOrderLogList(OrderLogSearchRequset request)
        {
            var response = new BasePageResponse<HO_HotelOrderLogModel>();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT * FROM ho_hotelorderlog  WHERE  HOLOrderId='{0}'", request.CustomerSerialId);
            var list = MysqlHelper.GetList<HO_HotelOrderLogModel>(sb.ToString());
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
        /// 统计订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<OrderStaticResponse> GetOrderStaticList(OrderStaticRequest request)
        {
            var response = new BasePageResponse<OrderStaticResponse>();
            StringBuilder sb = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            if (request.Type == 1 && request.TimeType != 0)
            {
                if (request.TimeType == 1)
                {
                    sbwhere.AppendFormat(" AND ho.HOAddTime>= '{0}'", request.StartTime);
                    sbwhere.AppendFormat(" AND ho.HOAddTime< '{0}'", request.EndTime);
                }
                else
                {
                    sbwhere.AppendFormat(" AND ho.HOCheckOutDate>= '{0}'", request.StartTime);
                    sbwhere.AppendFormat(" AND ho.HOCheckOutDate< '{0}'", request.EndTime);
                }
            }
            if (request.PrivoceId > 0)
            {
                sbwhere.AppendFormat(" AND hb.HIProvinceId= {0}", request.PrivoceId);
            }
            if (request.CityId > 0)
            {
                sbwhere.AppendFormat(" AND hb.HICityId= {0}", request.CityId);
            }
            if (request.Part1 > 0)
            {
                sbwhere.AppendFormat(" AND ho.HoPlat1= {0}", request.Part1);
            }
            if (request.Part2 > 0)
            {
                sbwhere.AppendFormat(" AND hb.HoPlat2= {0}", request.Part2);
            }
            if (!string.IsNullOrWhiteSpace(request.HotelName))
            {
                sbwhere.AppendFormat(" AND ho.HName IN ({0})", request.HotelName);
            }
            if (!string.IsNullOrWhiteSpace(request.HotelId))
            {
                sbwhere.AppendFormat(" AND ho.HId IN ({0})", request.HotelId);
            }
            if (!string.IsNullOrWhiteSpace(request.SupplierName))
            {
                sbwhere.AppendFormat(" AND ho.HOSupperlierName Like '%{0}%'", request.SupplierName);
            }
            if (request.SupplierId > 0)
            {
                sbwhere.AppendFormat(" AND ho.HOSupperlierId = {0}", request.SupplierId);
            }
            if (request.DistributorId > 0)
            {
                sbwhere.AppendFormat(" AND ho.HODistributorId = {0}", request.DistributorId);
            }
            if (request.SupplierSource > 0)
            {
                sbwhere.AppendFormat(" AND ho.HOSupplierSourceId ={0}", request.SupplierSource);
            }
            sb.AppendFormat(@"SELECT
	                                DATE_FORMAT(ho.HOAddTime, '%Y-%m-%d') AS CreateTime,
	                                DATE_FORMAT(
		                                ho.HOCheckOutDate,
		                                '%Y-%m-%d'
	                                ) AS CheckOutDate,
	                                hb.HIProvinceId AS ProvinceId,
	                                hb.HIProvince AS ProvinceName,
	                                hb.HICityId AS CityId,
	                                hb.HICity AS CityName,
	                                ho.HName AS HotelName,
	                                ho.HIId AS HotelId,
	                                ho.HOSupperlierName AS SupperlierName,
	                                ho.HOSupplierId AS SupperlierId,
	                                count(ho.Id) AS TotalCreate,
	                                count(
		                                CASE
		                                WHEN ho.HOStatus = 1 THEN
			                                1
		                                END
	                                ) AS TotalSuccess,
	                                sum(ho.HOSellPrice) AS TotalSell,
	                                sum(ho.HOContractPrice) AS TotalContract,
	                                sum(
		                                ho.HOSellPrice - ho.HOContractPrice
	                                ) AS TotalRevenue
                                FROM
	                                ho_hotelorder ho
                                INNER JOIN h_hotelinfo hb ON hb.Id = ho.HIId
                                WHERE
	                                1 = 1
                                    {0}
                                GROUP BY
	                                DATE_FORMAT(ho.HOAddTime, '%Y-%m-%d'),
	                                DATE_FORMAT(
		                                ho.HOCheckOutDate,
		                                '%Y-%m-%d'
	                                ),
	                                hb.HIProvinceId,
	                                hb.HIProvince,
	                                hb.HICityId,
	                                hb.HICity,
	                                ho.HName,
	                                ho.HIId,
	                                ho.HOSupperlierName,
	                                ho.HOSupplierId", sbwhere.ToString());
            var list = MysqlHelper.GetList<OrderStaticResponse>(sb.ToString());
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
        /// 统计订单---新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<OrderStaticResponse> GetOrderStatic(OrderStaticRequest request)
        {
            var response = new BasePageResponse<OrderStaticResponse>();
            StringBuilder sb = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            if (request.TimeType == 1)
            {
                if (!string.IsNullOrWhiteSpace(request.StartTime))
                {
                    sbwhere.AppendFormat(" AND ho.HOAddTime>= '{0}'", request.StartTime);
                }
                if (!string.IsNullOrWhiteSpace(request.EndTime))
                {
                    sbwhere.AppendFormat(" AND ho.HOAddTime< '{0}'", request.EndTime);
                }
            }
            if (request.TimeType == 2)
            {
                if (!string.IsNullOrWhiteSpace(request.StartTime))
                {
                    sbwhere.AppendFormat(" AND ho.HOCheckOutDate>= '{0}'", request.StartTime);
                }
                if (!string.IsNullOrWhiteSpace(request.EndTime))
                {
                    sbwhere.AppendFormat(" AND ho.HOCheckOutDate< '{0}'", request.EndTime);
                }
            }
            if (request.PrivoceId > 0)
            {
                sbwhere.AppendFormat(" AND hb.HIProvinceId= {0}", request.PrivoceId);
            }
            if (request.CityId > 0)
            {
                sbwhere.AppendFormat(" AND hb.HICityId= {0}", request.CityId);
            }
            if (request.Part1 > 0)
            {
                sbwhere.AppendFormat(" AND ho.HoPlat1= {0}", request.Part1);
            }
            if (request.Part2 > 0)
            {
                sbwhere.AppendFormat(" AND ho.HoPlat2= {0}", request.Part2);
            }
            if (!string.IsNullOrWhiteSpace(request.HotelName))
            {
                sbwhere.AppendFormat(" AND ho.HName IN ({0})", request.HotelName);
            }
            if (!string.IsNullOrWhiteSpace(request.HotelId))
            {
                sbwhere.AppendFormat(" AND ho.HId IN ({0})", request.HotelId);
            }
            if (!string.IsNullOrWhiteSpace(request.SupplierName))
            {
                sbwhere.AppendFormat(" AND ho.HOSupperlierName Like '%{0}%'", request.SupplierName);
            }
            if (request.SupplierSource > 0)
            {
                sbwhere.AppendFormat(" AND ho.HOSupplierSourceId ={0}", request.SupplierSource);
            }
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                sbwhere.AppendFormat(" AND ho.HOStatus IN ({0})", request.Status);
            }
            switch (request.Type)
            {
                //时间
                case 1:
                    //创建时间
                    if (request.TimeType == 1)
                    {
                        sb.AppendFormat(@"SELECT
	                                            DATE_FORMAT(ho.HOAddTime, '%Y-%m-%d') AS StaticTime,
	                                            count(ho.Id) AS TotalCreate,
	                                            count(
		                                            CASE
		                                            WHEN ho.HOStatus = 1 THEN
			                                            1
		                                            END
	                                            ) AS TotalSuccess,
	                                            sum(ho.HOSellPrice) AS TotalSell,
	                                            sum(ho.HOContractPrice) AS TotalContract,
	                                            sum(
		                                            CASE WHEN ho.HOStatus = 1 THEN
			                                            ho.HOSellPrice - ho.HOContractPrice
		                                            ELSE
			                                            0
		                                            END
	                                            ) AS TotalRevenue
                                            FROM
	                                            ho_hotelorder ho
                                            INNER JOIN h_hotelinfo hb ON hb.Id = ho.HIId
                                            WHERE
	                                            1 = 1
                                                {0}
                                            GROUP BY
                                            DATE_FORMAT(ho.HOAddTime, '%Y-%m-%d')", sbwhere.ToString());
                    }
                    //离店时间
                    else
                    {
                        sb.AppendFormat(@"SELECT
	                                            DATE_FORMAT(
		                                            ho.HOCheckOutDate,
		                                            '%Y-%m-%d'
	                                            ) AS StaticTime,
	                                            count(ho.Id) AS TotalCreate,
	                                            count(
		                                            CASE
		                                            WHEN ho.HOStatus = 1 THEN
			                                            1
		                                            END
	                                            ) AS TotalSuccess,
	                                            sum(ho.HOSellPrice) AS TotalSell,
	                                            sum(ho.HOContractPrice) AS TotalContract,
	                                            sum(
		                                            CASE WHEN ho.HOStatus = 1 THEN
			                                            ho.HOSellPrice - ho.HOContractPrice
		                                            ELSE
			                                            0
		                                            END
	                                            ) AS TotalRevenue
                                            FROM
	                                            ho_hotelorder ho
                                            INNER JOIN h_hotelinfo hb ON hb.Id = ho.HIId
                                            WHERE
	                                            1 = 1
                                                {0}
                                            GROUP BY
	                                            DATE_FORMAT(
		                                            ho.HOCheckOutDate,
		                                            '%Y-%m-%d')", sbwhere.ToString());
                    }
                    break;
                //省市
                case 2:
                    sb.AppendFormat(@"SELECT
	                                        DATE_FORMAT(ho.HOAddTime, '%Y-%m-%d') AS StaticTime,
	                                        hb.HIProvinceId AS ProvinceId,
	                                        hb.HIProvince AS ProviceName,
	                                        hb.HICityId AS CityId,
	                                        hb.HICity AS CityName,
	                                        count(ho.Id) AS TotalCreate,
	                                        count(
		                                        CASE
		                                        WHEN ho.HOStatus = 1 THEN
			                                        1
		                                        END
	                                        ) AS TotalSuccess,
	                                        sum(ho.HOSellPrice) AS TotalSell,
	                                        sum(ho.HOContractPrice) AS TotalContract,
	                                        sum(
		                                            CASE WHEN ho.HOStatus = 1 THEN
			                                            ho.HOSellPrice - ho.HOContractPrice
		                                            ELSE
			                                            0
		                                            END
	                                            ) AS TotalRevenue
                                        FROM
	                                        ho_hotelorder ho
                                        INNER JOIN h_hotelinfo hb ON hb.Id = ho.HIId
                                        WHERE
	                                        1 = 1
                                            {0}
                                        GROUP BY
	                                        DATE_FORMAT(ho.HOAddTime, '%Y-%m-%d'),
	                                        hb.HIProvinceId,
	                                        hb.HIProvince,
	                                        hb.HICityId,
	                                        hb.HICity", sbwhere.ToString());
                    break;
                //酒店
                case 3:
                    sb.AppendFormat(@"SELECT
	                                        DATE_FORMAT(ho.HOAddTime, '%Y-%m-%d') AS StaticTime,
	                                        ho.HName AS HotelName,
	                                        ho.HIId AS HotelId,
	                                        count(ho.Id) AS TotalCreate,
	                                        count(
		                                        CASE
		                                        WHEN ho.HOStatus = 1 THEN
			                                        1
		                                        END
	                                        ) AS TotalSuccess,
	                                        sum(ho.HOSellPrice) AS TotalSell,
	                                        sum(ho.HOContractPrice) AS TotalContract,
	                                        sum(
		                                            CASE WHEN ho.HOStatus = 1 THEN
			                                            ho.HOSellPrice - ho.HOContractPrice
		                                            ELSE
			                                            0
		                                            END
	                                            ) AS TotalRevenue
                                        FROM
	                                        ho_hotelorder ho
                                        INNER JOIN h_hotelinfo hb ON hb.Id = ho.HIId
                                        WHERE
	                                        1 = 1
                                            {0}
                                        GROUP BY
	                                        DATE_FORMAT(ho.HOAddTime, '%Y-%m-%d'),
	                                        ho.HName,
	                                        ho.HIId", sbwhere.ToString());
                    break;
                //供应商
                case 4:
                    sb.AppendFormat(@"SELECT
	                                        DATE_FORMAT(ho.HOAddTime, '%Y-%m-%d') AS StaticTime,
	                                        ho.HOSupperlierName AS SupperlierName,
	                                        ho.HOSupplierId AS SupperlierId,
	                                        count(ho.Id) AS TotalCreate,
	                                        count(
		                                        CASE
		                                        WHEN ho.HOStatus = 1 THEN
			                                        1
		                                        END
	                                        ) AS TotalSuccess,
	                                        sum(ho.HOSellPrice) AS TotalSell,
	                                        sum(ho.HOContractPrice) AS TotalContract,
	                                        sum(
		                                            CASE WHEN ho.HOStatus = 1 THEN
			                                            ho.HOSellPrice - ho.HOContractPrice
		                                            ELSE
			                                            0
		                                            END
	                                            ) AS TotalRevenue
                                        FROM
	                                        ho_hotelorder ho
                                        INNER JOIN h_hotelinfo hb ON hb.Id = ho.HIId
                                        WHERE
	                                        1 = 1
                                            {0}
                                        GROUP BY
	                                        DATE_FORMAT(ho.HOAddTime, '%Y-%m-%d'),
	                                        ho.HOSupperlierName,
	                                        ho.HOSupplierId", sbwhere.ToString());
                    break;
                //分销商
                case 5:
                    sb.AppendFormat(@"SELECT
	                                        DATE_FORMAT(ho.HOAddTime, '%Y-%m-%d') AS StaticTime,
	                                        hd.Id AS DistributorId,
	                                        hd.DName AS DistributorName,
	                                        count(ho.Id) AS TotalCreate,
	                                        count(
		                                        CASE
		                                        WHEN ho.HOStatus = 1 THEN
			                                        1
		                                        END
	                                        ) AS TotalSuccess,
	                                        sum(ho.HOSellPrice) AS TotalSell,
	                                        sum(ho.HOContractPrice) AS TotalContract,
	                                        sum(
		                                            CASE WHEN ho.HOStatus = 1 THEN
			                                            ho.HOSellPrice - ho.HOContractPrice
		                                            ELSE
			                                            0
		                                            END
	                                            ) AS TotalRevenue
                                        FROM
	                                        ho_hotelorder ho
										INNER JOIN h_hotelinfo hb ON hb.Id = ho.HIId
                                        INNER JOIN h_distributorinfo hd ON hd.Id = ho.HoPlat2
                                        WHERE
	                                        1 = 1
                                            {0}
                                        GROUP BY
	                                        DATE_FORMAT(ho.HOAddTime, '%Y-%m-%d'),
	                                        hd.Id,
	                                        hd.DName", sbwhere.ToString());
                    break;
            }
            var list = MysqlHelper.GetList<OrderStaticResponse>(sb.ToString());
            var TotalCreate = list.Sum(x => Convert.ToInt32(x.TotalCreate));
            var TotalSuccess = list.Sum(x => Convert.ToInt32(x.TotalSuccess));
            var TotalSell = list.Sum(x => Convert.ToInt32(x.TotalSell));
            var TotalContract = list.Sum(x => Convert.ToInt32(x.TotalContract));
            var TotalRevenue = list.Sum(x => Convert.ToInt32(x.TotalRevenue));
            var total = list?.Count ?? 0;
            if (total >= 0)
            {
                response.IsSuccess = 1;
                response.Total = total;
                response.List = list.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)?.ToList();
                response.Msg = TotalCreate + "|" + TotalSuccess + "|" + TotalSell + "|" + TotalContract + "|" + TotalRevenue;
            }
            return response;
        }
    }
}
