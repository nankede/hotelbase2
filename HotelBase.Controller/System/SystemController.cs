using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HotelBase.Common;
using HotelBase.Service;
using HotelBase.Web.Controllers;

namespace HotelBase.Web.Controller.System
{
    public class SystemController : BaseController
    {
        /// <summary>
        /// 人员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserList()
        {
            return View();
        }

        /// <summary>
        /// 人员列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserList()
        {
            var lsit = TestBll.GetUser();
            return Json(lsit, JsonRequestBehavior.AllowGet);
        }
    }
}
