using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HotelBase.Common;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using HotelBase.Service;
using HotelBase.Web.Controllers;
using HotelBase.Common.Extension;
using HotelBase.Entity;
using Newtonsoft.Json;

namespace HotelBase.Web.Controller.System
{
    public class OrderController : BaseController
    {
        #region 订单列表

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetOrderList(OrderSearchRequset request)
        {
            var response = OrderBll.GetOrderList(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderDetail(int orderid, string serialid)
        {
            ViewBag.OrderId = orderid;
            ViewBag.CustomerSerialId = serialid;
            return View();
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public JsonResult GetDetial(int orderid)
        {
            var response = OrderBll.GetModel(orderid);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 手动录单

        /// <summary>
        /// 手动录单
        /// </summary>
        /// <returns></returns>
        public ActionResult Book(string hotelId, string roomId, string ruleId, string supplierid, string indate)
        {
            ViewBag.HotelId = hotelId;
            ViewBag.RoomId = roomId;
            ViewBag.RuleId = ruleId;
            ViewBag.SupplierId = supplierid;
            ViewBag.Indate = indate;
            return View();
        }


        /// <summary>
        /// 手动录单
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderResourceList()
        {
            return View();
        }

        /// <summary>
        /// 资源查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetResource(BookSearchRequest request)
        {
            var response = OrderBll.GetHotelRuleList(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 手动录单详情
        /// </summary>
        /// <returns></returns>
        public ActionResult ResourceDetail(string hotelId, string hotelname, string hoteladdress)
        {
            ViewBag.HotelId = hotelId;
            ViewBag.HotelName = hotelname;
            ViewBag.HotelAddress = hoteladdress;
            return View();
        }

        /// <summary>
        /// 资源详情查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetResourceDetial(BookSearchRequest request)
        {
            var response = OrderBll.GetHotelRuleDetialList(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增订单
        /// </summary>
        /// <returns></returns>
        public ActionResult Save(BookSearchResponse hotelmodel, HO_HotelOrderModel ordermodel)
        {
            var newmodel = new HO_HotelOrderModel
            {
                HOCustomerSerialId = ExtOrderNum.Gener("Z", 1),
                HIId = hotelmodel.HotelId,
                HName = hotelmodel.HotelName,
                HRId = hotelmodel.HotelRoomId,
                HRName = hotelmodel.HotelRoomName,
                HRRId = hotelmodel.HotelRoomRuleId,
                HRRName = hotelmodel.HotelRoomRuleName,
                HOSupplierId = hotelmodel.HotelSupplierId,
                HOSupperlierName = hotelmodel.HotelSupplierName,
                HODistributorId = ordermodel.HODistributorId,
                HODistributorName = ordermodel.HODistributorName ?? string.Empty,
                HOSupplierSourceId = hotelmodel.HotelSupplierSourceId,
                HOSupplierSourceName = hotelmodel.HotelSupplierSourceName,
                HODistributorSerialId = ordermodel.HODistributorSerialId ?? string.Empty,
                HOOutSerialId = ordermodel.HOOutSerialId ?? string.Empty,
                HOSupplierSerialId = ordermodel.HOSupplierSerialId ?? string.Empty,
                HOSupplierCorfirmSerialId = ordermodel.HOSupplierCorfirmSerialId ?? string.Empty,
                HOStatus = 0,
                HOPayStatus = 0,
                HORoomCount = ordermodel.HORoomCount,
                HONight = ordermodel.HONight,
                HOChild = ordermodel.HOChild,
                HOAdult = ordermodel.HOAdult,
                HoPlat1 = ordermodel.HoPlat1,
                HoPlat2 = ordermodel.HoPlat2,
                HOContractPrice = ordermodel.HOContractPrice,
                HOSellPrice = ordermodel.HOSellPrice,
                HOCustomerName = ordermodel.HOCustomerName,
                HOCustomerMobile = ordermodel.HOCustomerMobile,
                HOLinkerName = ordermodel.HOLinkerName,
                HOLinkerMobile = ordermodel.HOLinkerMobile,
                HOCheckInDate = ordermodel.HOCheckInDate,
                HOCheckOutDate = ordermodel.HOCheckOutDate,
                HOLastCheckInTime = ordermodel.HOLastCheckInTime,
                HOAddId = CurrtUser.Id,
                HOAddName = CurrtUser.Name,
                HOAddDepartId = CurrtUser.DepartId,
                HOAddDepartName = CurrtUser.DepartName,
                HOAddTime = DateTime.Now,
                HORemark = ordermodel.HORemark,
                HOUpdateId = ordermodel.HOUpdateId,
                HOUpdateName = ordermodel.HOUpdateName,
                HOUpdateTime = DateTime.MinValue
            };
            //日志
            var logmodel = new HO_HotelOrderLogModel
            {
                HOLOrderId = newmodel.HOCustomerSerialId,
                HOLLogType = 1,//订单日志
                HOLRemark = "创建订单：" + newmodel.HOCustomerSerialId,
                HOLAddId = CurrtUser.Id,
                HOLAddName = CurrtUser.Name,
                HOLAddDepartId = CurrtUser.DepartId,
                HOLAddDepartName = CurrtUser.DepartName,
                HOLAddTime = DateTime.Now
            };
            OrderLogBll.AddOrderModel(logmodel);
            var response = OrderBll.AddOrderModel(newmodel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 更新订单
        /// </summary>
        /// <returns></returns>
        public JsonResult SetOrder(string id, string type, string state, string serialid, HO_HotelOrderLogModel logmodel)
        {
            var model = new BaseResponse();
            //日志
            logmodel.HOLAddId = CurrtUser.Id;
            logmodel.HOLLogType = 1;
            logmodel.HOLAddDepartId = CurrtUser.DepartId;
            logmodel.HOLAddTime = DateTime.Now;
            var savelog = OrderLogBll.AddOrderModel(logmodel);
            if (!string.IsNullOrWhiteSpace(type) && Convert.ToInt32(type) > 0)
            {
                model = OrderBll.SetOrder(id, type, state, serialid);
                if (type == "1" && model.IsSuccess == 1)//更新成功通知飞猪
                {
                    var order = OrderBll.GetModel(id);
                    if (order != null)
                    {
                        if (order.HODistributorId == 2)//飞猪
                        {
                            //状态通知
                            int status = 0;
                            switch (state)
                            {
                                case "1":
                                    status = 2;
                                    break;
                                case "2":
                                    status = 6;
                                    break;
                                case "3":
                                    status = 5;
                                    break;
                            }
                            if (status > 0)
                            {
                                var api = OpenApi.HotelOrderStatus(id, status);
                                logmodel.HOLRemark = "状态更改通知飞猪：请求参数{id=" + id + ",状态：" + state + "}；返回结果：" + api;
                                OrderLogBll.AddOrderModel(logmodel);
                            }
                        }
                        //if (order.HOSupplierId == 2 && state == "3")//致和订单取消订单
                        //{
                        //    var api = OpenApi.HotelOrderCancel(id);
                        //    logmodel.HOLRemark = "致和手动取消订单：请求参数{id=" + id + ",状态：" + state + "}；返回结果：" + api;
                        //    OrderLogBll.AddOrderModel(logmodel);
                        //}

                    }
                }
            }
            else
            {
                model = new BaseResponse
                {
                    IsSuccess = savelog ? 1 : 0,
                    Msg = savelog ? string.Empty : "更新失败",
                };
            }
            //var model = OrderBll.SetOrder(id, type, state, serialid);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 导出订单
        /// </summary>
        /// <param name="request"></param>
        public void ExportOrder(string request)
        {
            if (!string.IsNullOrWhiteSpace(request))
            {
                try
                {
                    var modelrequest = JsonConvert.DeserializeObject<OrderStaticRequest>(request);
                    var data = OrderBll.GetOrderStaticList(modelrequest);
                    if (data != null && data.Any())
                    {
                        List<ExeclOrder> emlist = new List<ExeclOrder>();
                        for (int i = 0; i < data.Count(); i++)
                        {
                            var detail = new ExeclOrder();
                            detail.HOCustomerSerialId = data[i].HOCustomerSerialId ?? "";//订单号
                            detail.ProviceName = data[i].ProviceName ?? "";//省份
                            detail.CityName = data[i].CityName ?? "";//城市
                            detail.HOSupperlierName = data[i].HOSupperlierName ?? "";//供应商
                            detail.HName = data[i].HName ?? "";//酒店
                            detail.HIId = data[i].HIId;//酒店id
                            detail.HRName = data[i].HRName ?? "";//房型
                            detail.HRId = data[i].HRId;//房型id
                            detail.HORoomCount = data[i].HORoomCount;//房间数
                            detail.HOSupplierCorfirmSerialId = data[i].HOSupplierCorfirmSerialId;//供应商确认号
                            detail.HOSupplierSerialId = data[i].HOSupplierSerialId;//供应商订单号
                            detail.HONight = data[i].HONight;//间夜
                            detail.HOCheckInDate = data[i].HOCheckInDate;//入店时间
                            detail.HOCheckOutDate = data[i].HOCheckOutDate;//离店时间
                            detail.HOAddTime = data[i].HOAddTime;//下单时间
                            detail.HOLinkerName = data[i].HOLinkerName ?? "";//联系人
                            detail.HOLinkerMobile = data[i].HOLinkerMobile ?? "";//手机号
                            detail.HOSellPrice = data[i].HOSellPrice;//订单价
                            detail.HOContractPrice = data[i].HOContractPrice;//酒店价
                                                                             //if (data[i].HOStatus == 1)
                                                                             //{
                            detail.Reven = data[i].HOSellPrice - data[i].HOContractPrice;//营收
                                                                                         //}
                                                                                         //订单状态
                            switch (data[i].HOStatus)
                            {
                                case 0:
                                    detail.Status = "待处理";
                                    break;
                                case 1:
                                    detail.Status = "确认成功";
                                    break;
                                case 2:
                                    detail.Status = "确认失败";
                                    break;
                                case 3:
                                    detail.Status = "取消";
                                    break;
                            }
                            detail.HODistributorName = data[i].HODistributorName;//分销商名称
                            detail.HODistributorSerialId = data[i].HODistributorSerialId;//分销商订单号
                            detail.HOPart1 = data[i].HoPlat1.ToString();
                            emlist.Add(detail);
                        }
                        var newdata = ConvertHelper.ListToDataTable(emlist);
                        var filed = "HOCustomerSerialId;ProviceName;CityName;HOSupperlierName;HName;HIId;HRName;HRId;HORoomCount;HOSupplierCorfirmSerialId;HOSupplierSerialId;HONight;HOCheckInDate;HOCheckOutDate;HOAddTime;HOLinkerName;HOLinkerMobile;HOSellPrice;HOContractPrice;Reven;Status;HODistributorName;HODistributorSerialId;HOPart1";
                        var headName = "订单号#省份#城市#供应商#酒店#酒店id#房型#房型id#房间数#供应商确认号#供应商订单号#间夜#入店时间#离店时间#下单时间#联系人#手机号#订单价#酒店价#营收#订单状态#分销商名称#分销商订单号#测试";
                        IList<ExcelHelper.NPOIModel> list = new List<ExcelHelper.NPOIModel>();
                        list.Add(new ExcelHelper.NPOIModel(newdata, filed, "Sheet1", headName));
                        var fileName = "导出订单" + DateTime.Now.ToString("yyyyMMddhhssmm");
                        ExcelHelper.NewExport(fileName, list, 0);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("异常", ex);
                }

            }
        }

        #endregion

        #region 日志

        /// <summary>
        /// 日志
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderLog(string orderid, string type, string state, string serialid)
        {
            ViewBag.OrderId = orderid;
            ViewBag.Type = type;
            ViewBag.State = state;
            ViewBag.CustomerSerialId = serialid;
            return View();
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public JsonResult GetOrderLogList(OrderLogSearchRequset requset)
        {
            var response = OrderBll.GetOrderLogList(requset);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="logmodel"></param>
        /// <returns></returns>
        public ActionResult SaveLog(HO_HotelOrderLogModel logmodel)
        {
            //日志
            logmodel.HOLAddId = CurrtUser.Id;
            logmodel.HOLAddDepartId = CurrtUser.DepartId;
            logmodel.HOLAddTime = DateTime.Now;
            var response = OrderLogBll.AddOrderModel(logmodel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Common
        /// <summary>
        /// 价格日历
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPriceList(int rrid, string begin, string end)
        {
            var list = HotelPriceBll.GetOrderPriceList(rrid, begin, end);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
