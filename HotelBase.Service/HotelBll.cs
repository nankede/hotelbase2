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
            var data = H_HotelInfoAccess.GetList(request);
            var response = new BasePageResponse<HotelSearchResponse>()
            {
                IsSuccess = data.IsSuccess,
                Total = data.Total,
                List = new List<HotelSearchResponse>()
            };
            data?.List?.ForEach(x =>
            {
                response.List.Add(new HotelSearchResponse
                {
                    Id = x.Id,
                    Name = x.HIName,
                    //SourceId = x.SSourceId,
                    //Source = Sys_BaseDictionaryAccess.GetDicModel(0, x.SSourceId)?.DName ?? string.Empty,
                    CityId = x.HICityId,
                    CityName = x.HICity,
                    ProvName = x.HIProvince,
                    ProvId = x.HIProvinceId,
                    Valid = x.HIIsValid,
                    SupplierName = string.Empty,
                    Source = string.Empty
                });
            });
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
            var id = H_HotelInfoAccess.Insert(model);
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
            var i = H_HotelInfoAccess.Update(model);
            res = new BaseResponse
            {
                IsSuccess = i > 0 ? 1 : 0,
                Msg = i > 0 ? string.Empty : "更新失败",
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

        #endregion

    }


}
