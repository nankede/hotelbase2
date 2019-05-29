using HotelBase.DataAccess.Distributor;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using HotelBase.Service;
using HotelBase.Web.Controllers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HotelBase.Web.Controller.Distributor
{
    public class DistributorController : BaseController
    {

        #region 分销商列表

        /// <summary>
        /// 分销商列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 分销商查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetList(DistributorSearchRequest request)
        {
            var response = DistributorBll.GetDistributorList(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分销商详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(int did)
        {
            ViewBag.Id = did;
            return View();
        }

        /// <summary>
        /// 分销商详情
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public JsonResult GetDetial(int id)
        {
            var response = DistributorBll.GetModel(id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分销商产品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProduceList()
        {
            return View();
        }


        /// <summary>
        /// 分配资源
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        public ActionResult Give(int did)
        {
            ViewBag.DId = did;
            return View();
        }

        /// <summary>
        /// 分销商操作
        /// </summary>
        /// <returns></returns>
        public JsonResult Save(H_DistributorInfoModel request)
        {
            if (request.Id > 0)
            {
                request.DUpdateName = CurrtUser.Name;
                var response = DistributorBll.UpdateModel(request);
                return Json(response);
            }
            else
            {
                request.DIsValid = 1;
                request.DAddName = CurrtUser.Name;
                var response = DistributorBll.AddModel(request);
                return Json(response);
            }
        }

        #endregion


        /// <summary>
        /// 设置有效性
        /// </summary>
        /// <returns></returns>
        public JsonResult SetValid(int id, int valid)
        {
            var model = DistributorBll.SetValid(id, valid, CurrtUser.Name);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分销商列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDistributor(int IsValid, int isDefault = 1)
        {
            var response = DistributorBll.GetDistributor(IsValid, isDefault);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分销商资源匹配
        /// </summary>
        /// <param name="resourceids"></param>
        /// <param name="distributorid"></param>
        /// <returns></returns>
        public JsonResult BathGive(string resourceids, string distributorid)
        {
            var ids = JsonConvert.DeserializeObject<List<int>>(resourceids);
            var response = DistributorBll.BathGive(ids, distributorid);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 分销商资源解除匹配
        /// </summary>
        /// <param name="resourceids"></param>
        /// <param name="distributorid"></param>
        /// <returns></returns>
        public JsonResult OutGive(string resourceids, string distributorid)
        {
            var ids = JsonConvert.DeserializeObject<List<int>>(resourceids);
            var response = DistributorBll.OutGive(ids, distributorid);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
