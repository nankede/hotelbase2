using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HotelBase.Common;
using HotelBase.Entity.Models;
using HotelBase.Service;
using HotelBase.Web.Controllers;

namespace HotelBase.Web.Controller.System
{
    public class SystemController : BaseController
    {
        #region 人员

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
        public JsonResult GetUserList(int index)
        {
            var request = new UserListRequest
            {
                PageIndex = index
            };
            var response = SystemBll.GetUserList(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 人员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserDetail(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 人员列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserDetail(int id)
        {
            var request = new UserListRequest
            {
                PageIndex = id
            };
            var response = SystemBll.GetUserModel(id, string.Empty);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 部门

        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DepartList()
        {
            return View();
        }

        /// <summary>
        /// 人员列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepartList()
        {
            //var lsit = SystemBll.GetUserList();
            //return Json(lsit, JsonRequestBehavior.AllowGet);
            return Json(new { });
        }
        #endregion

    }
}
