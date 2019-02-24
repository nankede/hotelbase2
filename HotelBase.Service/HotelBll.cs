using HotelBase.Common;
using HotelBase.DataAccess.Resource;
using HotelBase.DataAccess.System;
using HotelBase.Entity;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Service
{
    /// <summary>
    /// 酒店业务逻辑
    /// </summary>
    public static class HotelBll
    {
        /// <summary>
        /// 酒店查询
        /// </summary>
        /// <param name="request"></param>
        public static BasePageResponse<HotelSearchResponse> GetList(HotelSearchRequest request)
        {
            var response = H_HotelInfoAccess.GetList(request);
            return response;
        }

        /// <summary>
        /// 查询酒店详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static H_HotelInfoModel GetDetail(int id)
        {
            var model = H_HotelInfoAccess.GetModel(id);
            return model;
        }

        /// <summary>
        /// 新增供酒店详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse Insert(H_HotelInfoModel model)
        {
            var res = new BaseResponse();
            if (string.IsNullOrEmpty(model.HIName))
            {
                res.Msg = "酒店名称不能为空";
                return res;
            }
            var id = new H_HotelInfoAccess().Add(model);
            if (id <= 0)
            {
                res.Msg = "新增失败";
                return res;
            }
            else
            {
                res = new BaseResponse
                {
                    AddId = (int)id,
                    IsSuccess = 1
                };
            }
            return res;
        }

        /// <summary>
        /// 修改酒店详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse Update(H_HotelInfoModel model)
        {
            var res = new BaseResponse();
            if (model.Id <= 0)
            {
                res.Msg = "无效的酒店";
                return res;
            }
            if (string.IsNullOrEmpty(model.HIName))
            {
                res.Msg = "酒店名称不能为空";
                return res;
            }

            model = CommonHelper.CheckPropertiesNull(model);

            var i = new H_HotelInfoAccess().Update(model);
            res = new BaseResponse
            {
                IsSuccess = i ? 1 : 0,
                Msg = i ? string.Empty : "更新失败",
            };
            return res;
        }

        /// <summary>
        /// 设置有效性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BaseResponse SetValid(int id, int valid, string name)
        {
            var i = H_HotelInfoAccess.SetValid(id, valid, name);
            var res = new BaseResponse
            {
                IsSuccess = i > 0 ? 1 : 0,
                Msg = i > 0 ? string.Empty : "更新失败",
            };
            return res;
        }

        #region 酒店图片

        /// <summary>
        /// 酒店图片
        /// </summary>
        /// <param name="request"></param>
        public static List<H_HotelPictureModel> GetPicList(HotelPicSearchRequest request)
        {
            var db = new H_HotelPictureAccess();

            return db.Query().Where(x => x.HIId == request.HotelId)?.ToList();

            //  return db.GetList(request);
        }

        /// <summary>
        /// 酒店图片
        /// </summary>
        /// <param name="request"></param>
        public static BaseResponse SavePicModel(H_HotelPictureModel model)
        {
            var id = 0;
            var rtn = new BaseResponse();
            if (model.HIId <= 0)
            {
                rtn.IsSuccess = 0;
                rtn.Msg = "酒店Id不能为空";
                return rtn;
            }
            if (model.HPType <= 0)
            {
                rtn.IsSuccess = 0;
                rtn.Msg = "酒店图片类型不能为空";
                return rtn;
            }
            if (string.IsNullOrEmpty(model.HPUrl))
            {
                rtn.IsSuccess = 0;
                rtn.Msg = "酒店图片地址不能为空";
                return rtn;
            }

            var db = new H_HotelPictureAccess();
            id = (int)db.Add(model);
            rtn = new BaseResponse { AddId = id, IsSuccess = 1 };
            return rtn;

        }

        #endregion


        /// <summary>
        /// 模糊搜索酒店
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<H_HotelInfoModel> GetHotelList(string name)
        {
            var db = new H_HotelInfoAccess();
            var query = db.Query().Where(x => x.HIName.Contains(name) && x.HIIsValid == 1);
            return query.Top(20).ToList();
        }
    }


}
