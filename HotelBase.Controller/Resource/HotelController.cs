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
        /// 酒店详情
        /// </summary>
        /// <returns></returns>
        public ActionResult HotelBase(int id)
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
                request.HIIsValid = 1;
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
        public ActionResult PicList(int id)
        {
            ViewBag.HotelId = id;
            return View();
        }

        /// <summary>
        /// 图片
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPicList(int id, int index)
        {
            var request = new HotelPicSearchRequest
            {
                HotelId = id,
                PageIndex = index
            };
            var List = HotelBll.GetPicList(request);

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 房型

        /// <summary>
        /// 房型
        /// </summary>
        /// <returns></returns>
        public ActionResult RoomList(int hotelId)
        {
            ViewBag.HotelId = hotelId;
            return View();
        }

        /// <summary>
        /// 房型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoomList(HotelRoomSearchRequest request)
        {
            var data = HotelRoomBll.GetList(request);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 房型
        /// </summary>
        /// <returns></returns>
        public ActionResult Room(int hotelId, int id)
        {
            ViewBag.HotelId = hotelId;
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 酒店房型列表
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        public JsonResult GetRoomDetail(int id)
        {
            var data = HotelRoomBll.GetDetail(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  新增 修改房型
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        public JsonResult SaveRoom(H_HotelRoomModel request)
        {
            if (request.Id > 0)
            {
                request.HRUpdateName = CurrtUser.Name;
                var response = HotelRoomBll.Update(request);
                return Json(response);
            }
            else
            {
                request.HRIsValid = 1;
                request.HRAddName = CurrtUser.Name;
                var response = HotelRoomBll.Insert(request);
                return Json(response);
            }
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
        /// 房型政策-列表
        /// </summary>
        /// <returns></returns>
        public ActionResult RoomRuleList(int hotelId, int roomId)
        {
            ViewBag.HotelId = hotelId;
            ViewBag.RoomId = roomId;
            return View();
        }

        /// <summary>
        /// 房型政策-详情
        /// </summary>
        /// <returns></returns>
        public ActionResult RoomRule(int hotelId, int roomId, int id)
        {
            ViewBag.HotelId = hotelId;
            ViewBag.RoomId = roomId;
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 房型政策
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoomRuleList(HotelRoomRuleSearchRequest request)
        {
            var data = HotelRoomRuleBll.GetList(request);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 酒店房型列表
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        public JsonResult GetRoomRuleDetail(int id)
        {
            var data = HotelRoomRuleBll.GetDetail(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  新增 修改房型
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        public JsonResult SaveRoomRule(H_HotelRoomRuleModel request)
        {
            if (request.Id > 0)
            {
                request.HRRUpdateName = CurrtUser.Name;
                var response = HotelRoomRuleBll.Update(request);
                return Json(response);
            }
            else
            {
                request.HRRIsValid = 1;
                request.HRRAddName = CurrtUser.Name;
                var response = HotelRoomRuleBll.Insert(request);
                return Json(response);
            }
        }
        /// <summary>
        /// 房型政策
        /// </summary>
        /// <returns></returns>
        public JsonResult SetRoomRuleValid(int id, int valid)
        {
            var data = HotelRoomRuleBll.SetValid(id, valid, CurrtUser.Name);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 库存日历
        /// <summary>
        /// 库存日历
        /// </summary>
        /// <returns></returns>
        public ActionResult StoreList(int ruleId, int hotelId, int roomId)
        {
            ViewBag.RuleId = ruleId;
            ViewBag.RoomId = roomId;
            ViewBag.HotelId = hotelId;
            return View();
        }

        #endregion

        #region 价格日历
        /// <summary>
        /// 价格日历
        /// </summary>
        /// <returns></returns>
        public ActionResult PriceList(int ruleId, int hotelId, int roomId)
        {
            ViewBag.RuleId = ruleId;
            ViewBag.RoomId = roomId;
            ViewBag.HotelId = hotelId;
            return View();
        }
        /// <summary>
        /// 价格日历
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPriceList(HotelPriceSearchRequest request)
        {
            var list = HotelPriceBll.GetList(request);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 价格日历
        /// </summary>
        /// <returns></returns>
        public JsonResult SavePriceDetail(SaveHotelPriceModel request)
        {
            request.OperateName = CurrtUser.Name;
            var rtn = HotelPriceBll.SavePriceDetail(request);
            return Json(rtn, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 价格日历
        /// </summary>
        /// <returns></returns>
        public JsonResult SavePriceBatch(SaveHotelPriceModel request)
        {
            request.OperateName = CurrtUser.Name;
            var rtn = HotelPriceBll.SavePriceBatch(request);
            return Json(rtn, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 获取订单所需酒店详情
        /// <summary>
        /// 获取订单所需酒店详情
        /// </summary>
        /// <param name="hotelid"></param>
        /// <param name="roomid"></param>
        /// <param name="rooleid"></param>
        /// <param name="supplierid"></param>
        /// <returns></returns>
        public JsonResult GetOrderNeedInfo(int hotelid, int roomid, int rooleid, int supplierid)
        {
            var model = OrderBll.GetHotelRuleDetial(hotelid, roomid, rooleid, supplierid);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
