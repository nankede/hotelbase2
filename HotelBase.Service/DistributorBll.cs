using HotelBase.Common;
using HotelBase.DataAccess.Distributor;
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
    public class DistributorBll
    {
        /// <summary>
        /// 分销商列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<DistributorSearchResponse> GetDistributorList(DistributorSearchRequest request)
        {
            return H_DistributorAccess.GetDistributorList(request);
        }

        /// <summary>
        /// 分销商详情
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        public static H_DistributorInfoModel GetModel(int did)
        {
            return H_DistributorAccess.GetModel(did);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BaseResponse AddModel(H_DistributorInfoModel model)
        {
            var res = new BaseResponse();
            var id = 0;
            id = H_DistributorAccess.AddModel(model);
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
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse UpdateModel(H_DistributorInfoModel model)
        {
            var res = new BaseResponse();
            if (model.Id <= 0)
            {
                res.Msg = "无效的分销商";
                return res;
            }
            if (string.IsNullOrEmpty(model.DName))
            {
                res.Msg = "分销商名称不能为空";
                return res;
            }

            model = CommonHelper.CheckPropertiesNull(model);

            var i = H_DistributorAccess.UpdateModel(model);
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
            var i = H_DistributorAccess.SetValid(id, valid, name);
            var res = new BaseResponse
            {
                IsSuccess = i > 0 ? 1 : 0,
                Msg = i > 0 ? string.Empty : "更新失败",
            };
            return res;
        }

    }
}
