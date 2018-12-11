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
    public class DataController : BaseController
    {
        #region 订单统计

        /// <summary>
        /// 订单统计
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderStatics()
        {
            return View();
        }

        /// <summary>
        /// 订单统计列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetOrderStatics(OrderStaticRequest request)
        {
            //GetOrderList
            var response = OrderStaticBll.GetOrderStaticList(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
