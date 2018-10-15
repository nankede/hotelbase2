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
    public class HotelController : BaseController
    {
        #region 酒店

        /// <summary>
        /// 酒店列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 酒店列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetList(HotelSearchRequest request)
        {
            var response = HotelBll.GetList(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 酒店详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 查询酒店详情
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDateil(int id)
        {
            var model = HotelBll.GetDetail(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 新增酒店
        /// </summary>
        /// <returns></returns>
        public JsonResult Save(H_HotelInfoModel request)
        {
            if (request.Id > 0)
            {
                request.HIUpdateName = CurrtUser.Name;
                var response = HotelBll.Update(request);
                return Json(response);
            }
            else
            {
                request.HIAddName = CurrtUser.Name;
                var response = HotelBll.Insert(request);
                return Json(response);
            }
        }

        /// <summary>
        /// 设置有效性
        /// </summary>
        /// <returns></returns>
        public JsonResult SetValid(int id, int valid)
        {
            var model = HotelBll.SetValid(id, valid, CurrtUser.Name);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
