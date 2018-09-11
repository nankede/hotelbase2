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

        #region 字典

        /// <summary>
        /// 字典列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DicList()
        {
            return View();
        }

        /// <summary>
        /// 数据字典列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDicList(GetDicListRequest request)
        {
            var response = SystemBll.GetDicList(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 字典新增
        /// </summary>
        /// <returns></returns>
        public JsonResult AddDic(Sys_BaseDictionaryModel request)
        {
            if (request.Id <= 0)
            {
                var res = SystemBll.AddDicModel(request);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var res = SystemBll.UpdateDicModel(request);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 字典列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DicDetail(int id, int pId)
        {
            ViewBag.Id = id;
            ViewBag.PId = pId;
            return View();
        }

        /// <summary>
        /// 数据字典
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDicDetail(int id)
        {
            var response = SystemBll.GetDicModel(id, 0);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 数据字典
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDicNew(int pid)
        {
            var response = SystemBll.GetNewDicModel(pid);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 国家地区
        /// <summary>
        /// 查询区域列表
        /// pid =1 省份
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAreaList(int pId)
        {
            var list = SystemBll.GetAreaList(pId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
