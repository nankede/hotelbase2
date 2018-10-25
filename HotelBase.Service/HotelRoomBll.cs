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
    /// 酒店 房型业务逻辑
    /// </summary>
    public static class HotelRoomBll
    {
        /// <summary>
        /// 酒店查询
        /// </summary>
        /// <param name="request"></param>
        public static BasePageResponse<H_HotelRoomModel> GetList(HotelRoomSearchRequest request)
        {
            var db = new H_HotelRoomAccess();
            var query = db.Query().Where(x => x.HIId == request.HotelId);
            if (request.IsValiad == 1)
            {
                query.Where(x => x.HRIsValid == 1);
            }
            var list = query.ToList();


            var response = new BasePageResponse<H_HotelRoomModel>()
            {
                IsSuccess = 1,
                Total = list?.Count ?? 0,
                List = list
            };

            return response;
        }

        /// <summary>
        /// 查询酒店房型详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static H_HotelRoomModel GetDetail(int id)
        {
            var model = new H_HotelRoomAccess().Query().FirstOrDefault(x => x.Id == id);
            return model;
        }

        /// <summary>
        /// 新增酒店房型详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse Insert(H_HotelRoomModel model)
        {
            var res = new BaseResponse();
            if (string.IsNullOrEmpty(model.HRName))
            {
                res.Msg = "房型名称不能为空";
                return res;
            }
            var id = new H_HotelRoomAccess().Add(model);
            if (id <= 0)
            {
                res.Msg = "新增失败";
                return res;
            }
            else
            {
                res = new BaseResponse
                {
                    AddId = id,
                    IsSuccess = 1
                };
            }
            return res;
        }

        /// <summary>
        /// 修改酒店房型详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse Update(H_HotelRoomModel model)
        {
            var res = new BaseResponse();
            if (model.Id <= 0)
            {
                res.Msg = "无效的酒店";
                return res;
            }
            if (string.IsNullOrEmpty(model.HRName))
            {
                res.Msg = "酒店名称不能为空";
                return res;
            }
            var i = new H_HotelRoomAccess().Update(model);
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
            var i = new H_HotelRoomAccess().Update().Where(x => x.Id == id)
                .Set(x => x.HRIsValid == valid && x.HRUpdateName == name && x.HRUpdateTime == DateTime.Now)
                .Execute();
            var res = new BaseResponse
            {
                IsSuccess = i > 0 ? 1 : 0,
                Msg = i > 0 ? string.Empty : "更新失败",
            };
            return res;
        }

    }
}
