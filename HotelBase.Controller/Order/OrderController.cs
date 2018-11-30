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
        public JsonResult GetDicList(OrderSearchRequset request)
        {
            var response = OrderBll.GetOrderList(request);
            return Json(response, JsonRequestBehavior.AllowGet);
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
        public ActionResult Book(string hotelId,string roomId, string ruleId)
        {
            ViewBag.HotelId = hotelId;
            ViewBag.RoomId = roomId;
            ViewBag.RuleId = ruleId;
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
        /// 新增订单
        /// </summary>
        /// <returns></returns>
        public ActionResult AddOrder()
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
