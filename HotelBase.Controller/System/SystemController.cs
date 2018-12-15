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
        public JsonResult GetUserList(UserListRequest request)
        {
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

        /// <summary>
        /// 新增人员
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveUser(UserModel request)
        {
            var model = new Sys_UserInfoModel
            {
                Id = request.Id,
                UIAccount = request.Account,
                UIName = request.Name,
                UIPassWord = request.Id == 0 ? request.Pwd : string.Empty,
                UIDepartId = request.DepartId,
                UIDepartName = request.DepartName,
                UIResponsibility = request.R,

            };
            if (request.Id > 0)
            {
                model.UIUpdateTime = DateTime.Now;
                model.UIUpdateName = CurrtUser.Name;
                var response = SystemBll.UpdateUser(model);
                return Json(response);
            }
            else
            {
                model.UIIsValid = 1;
                model.UIAddName = CurrtUser.Name;
                model.UIAddTime = DateTime.Now;
                ;
                var response = SystemBll.InsertUser(model);
                return Json(response);
            }
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
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepartList(DepartistRequest request)
        {
            var response = SystemBll.GetDepartList(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 部门模糊搜索
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult GetDepartSearch(string name)
        {
            var request = new DepartistRequest
            {
                IsValid = 1,
                Name = HttpUtility.UrlDecode(name),
                PageIndex = 1,
                PageSize = 20

            };
            var list = new List<BaseDic>();
            var response = SystemBll.GetDepartList(request);
            if (response != null && response.List != null && response.List.Count > 0)
            {
                list.AddRange(response.List.Select(x => new BaseDic
                {
                    Code = x.Id,
                    Name = x.DIName

                }));
            }
            return Json(list, JsonRequestBehavior.AllowGet);

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

        /// <summary>
        /// 根据父Code查数据字典列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetDicListByPCode(int pCode)
        {
            var response = SystemBll.GetDicListByPCode(pCode);
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
