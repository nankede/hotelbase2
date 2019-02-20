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
        public ActionResult Book(string hotelId, string roomId, string ruleId, string supplierid)
        {
            ViewBag.HotelId = hotelId;
            ViewBag.RoomId = roomId;
            ViewBag.RuleId = ruleId;
            ViewBag.SupplierId = supplierid;
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
                HOSupplierSourceId = hotelmodel.HotelSupplierSourceId,
                HOSupplierSourceName = hotelmodel.HotelSupplierSourceName,
                HOOutSerialId = ordermodel.HOOutSerialId ?? string.Empty,
                HOSupplierSerialId = ordermodel.HOSupplierSerialId ?? string.Empty,
                HOStatus = 0,
                HOPayStatus = 0,
                HORoomCount = ordermodel.HORoomCount,
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
    }
}
