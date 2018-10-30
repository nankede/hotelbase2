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


        #region 图片
        /// <summary>
        /// 图片
        /// </summary>
        /// <returns></returns>
        public ActionResult PicList()
        {
            return View();
        }

        #endregion

        #region 房型
        /// <summary>
        /// 房型
        /// </summary>
        /// <returns></returns>
        public ActionResult Room()
        {
            return View();
        }

        /// <summary>
        /// 酒店房型列表
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        public JsonResult GetHotelRoom(HotelRoomSearchRequest request)
        {
            var data = HotelRoomBll.GetList(request);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  新增 修改房型
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        public JsonResult SaveRoom(int hotelId)
        {

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置有效性
        /// </summary>
        /// <returns></returns>
        public JsonResult SetRoomValid(int id, int valid)
        {
            var model = HotelBll.SetValid(id, valid, CurrtUser.Name);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region 房型政策
        /// <summary>
        /// 房型政策
        /// </summary>
        /// <returns></returns>
        public ActionResult RoomRule()
        {
            return View();
        }

        #endregion

        #region 库存日历
        /// <summary>
        /// 库存日历
        /// </summary>
        /// <returns></returns>
        public ActionResult StoreList()
        {
            return View();
        }

        #endregion

        #region 价格日历
        /// <summary>
        /// 价格日历
        /// </summary>
        /// <returns></returns>
        public ActionResult PriceList()
        {
            return View();
        }

        #endregion
    }
}
