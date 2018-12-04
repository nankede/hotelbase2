using HotelBase.Entity;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBase.DataAccess.Resource;

namespace HotelBase.Service
{
    /// <summary>
    /// 价格政策
    /// </summary>
    public class HotelRoomRuleBll
    {
        /// <summary>
        /// 酒店查询
        /// </summary>
        /// <param name="request"></param>
        public static BasePageResponse<H_HotelRoomRuleModel> GetList(HotelRoomRuleSearchRequest request)
        {
            var db = new H_HotelRoomRuleAccess();
            var query = db.Query().Where(x => x.HRId == request.RoomId).OrderByDescending(x => x.Id);
            if (request.IsValiad >= 0)
            {
                query.Where(x => x.HRRIsValid == request.IsValiad);
            }
            var list = query.ToList();


            var response = new BasePageResponse<H_HotelRoomRuleModel>()
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
        public static H_HotelRoomRuleModel GetDetail(int id)
        {
            var model = new H_HotelRoomRuleAccess().Query().FirstOrDefault(x => x.Id == id);
            return model;
        }

        /// <summary>
        /// 新增酒店房型详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse Insert(H_HotelRoomRuleModel model)
        {
            var res = new BaseResponse();
            if (string.IsNullOrEmpty(model.HRRName))
            {
                res.Msg = "名称不能为空";
                return res;
            }
            var id = new H_HotelRoomRuleAccess().Add(model);
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
        /// 修改酒店房型详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse Update(H_HotelRoomRuleModel model)
        {
            var res = new BaseResponse();
            if (model.Id <= 0)
            {
                res.Msg = "无效的酒店";
                return res;
            }
            if (string.IsNullOrEmpty(model.HRRName))
            {
                res.Msg = "酒店名称不能为空";
                return res;
            }
            var i = new H_HotelRoomRuleAccess().Update(model);
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
            var i = new H_HotelRoomRuleAccess().Update().Where(x => x.Id == id)
                .Set(x => x.HRRIsValid == valid && x.HRRUpdateName == name && x.HRRUpdateTime == DateTime.Now)
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
