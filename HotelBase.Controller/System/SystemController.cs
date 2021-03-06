﻿using System;
using System.Collections.Generic;
using System.IO;
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
        public JsonResult SaveUser(Sys_UserInfoModel request)
        {
            if (request.Id > 0)
            {
                request.UIUpdateTime = DateTime.Now;
                request.UIUpdateName = CurrtUser.Name;
                var response = SystemBll.UpdateUser(request);
                return Json(response);
            }
            else
            {
                request.UIIsValid = 1;
                request.UIAddName = CurrtUser.Name;
                request.UIAddTime = DateTime.Now;
                var response = SystemBll.InsertUser(request);
                return Json(response);
            }
        }

        /// <summary>
        /// 人员模糊搜索
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult GetUserSearch(string name)
        {
            var request = new UserListRequest
            {
                IsValid = 1,
                Name = HttpUtility.UrlDecode(name),
                PageIndex = 1,
                PageSize = 20

            };
            var list = new List<BaseDic>();
            var response = SystemBll.GetUserList(request);
            if (response != null && response.List != null && response.List.Count > 0)
            {
                list.AddRange(response.List.Select(x => new BaseDic
                {
                    Code = x.Id,
                    Name = x.Name

                }));
            }
            return Json(list, JsonRequestBehavior.AllowGet);

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

        /// <summary>
        /// 部门详情
        /// </summary>
        /// <returns></returns>
        public ActionResult DepartDetail(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 部门详情
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveDepart(DepartModel model)
        {
            var response = SystemBll.SaveDepart(model, CurrtUser.Name);
            return Json(response);
        }

        /// <summary>
        /// 部门详情
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepartDetail(int id)
        {
            var response = SystemBll.GetDepart(id);
            return Json(response, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetDicListByPCode(int pCode, int isDefault = 1)
        {
            var response = SystemBll.GetDicListByPCode(pCode, isDefault);
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

        /// <summary>
        /// 查询区域列表
        /// pid =1 省份
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAreaList2(int pId)
        {
            var list = SystemBll.GetAreaList(pId);
            var rtnList = list?.Select(x => new BaseDic
            {
                Code = x.Id,
                Name = x.Name
            })?.ToList();
            return Json(rtnList, JsonRequestBehavior.AllowGet);
        }
        //
        #endregion

        #region 上传

        /// <summary>
        /// 查询区域列表
        /// pid =1 省份
        /// </summary>
        /// <returns></returns>
        public JsonResult upload(string mainType, int id, int secType)
        {
            var picList = new List<string>();
            var files = Request.Files;
            if (files != null && files.Count == 0)//判断文件是否存在
                return null;
            var path = $"{Server.MapPath("~/resource/")}{mainType}/{id}/{secType}/";
            var showpath = $"/resource/{mainType}/{id}/{secType}/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            for (var i = 0; i < files.Count; i++)
            {
                string imageFilePath = path + files[i].FileName;
                showpath += files[i].FileName;
                Request.Files[0].SaveAs(imageFilePath);
                picList.Add(showpath);
            }
            return Json(picList);
        }

        #endregion
    }
}
