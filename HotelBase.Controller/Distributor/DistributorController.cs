using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using HotelBase.Service;
using HotelBase.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public JsonResult GetDistributor(int isDefault = 1)
        {
            var response = DistributorBll.GetDistributor(isDefault);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}
