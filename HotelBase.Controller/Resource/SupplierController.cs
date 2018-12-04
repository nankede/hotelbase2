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
    public class SupplierController : BaseController
    {
        #region 供应商

        /// <summary>
        /// 供应商列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 供应商详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 查询供应商
        /// </summary>
        /// <returns></returns>
        public JsonResult GetList(SupplierSearchRequest request)
        {
            var list = SupplierBll.GetList(request);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增供应商
        /// </summary>
        /// <returns></returns>
        public JsonResult Save(H_SupplierModel request)
        {
            if (request.Id > 0)
            {
                request.SUpdateName = CurrtUser.Name;
                var response = SupplierBll.Update(request);
                return Json(response);
            }
            else
            {
                request.SIsValid = 1;
                request.SAddName = CurrtUser.Name;
                var response = SupplierBll.Insert(request);
                return Json(response);
            }
        }

        /// <summary>
        /// 查询供应商详情
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDateil(int id)
        {
            var model = SupplierBll.GetDetail(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询供应商-模糊查询
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSupplierList(int sourceId, string name)
        {
            if (string.IsNullOrEmpty(name)) return Json("", JsonRequestBehavior.AllowGet);
            var list = SupplierBll.GetSupplierList(sourceId, name);
            var rtn = list?.Select(x => new { Code = x.Id, Name = x.SName });
            return Json(rtn, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
