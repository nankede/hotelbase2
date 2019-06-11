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

        /// <summary>
        /// 查询分销商列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static List<BaseDic> GetDistributor(int IsValid, int isDefault)
        {
            var list = new List<BaseDic>();
            if (isDefault == 1)
            {
                list.Add(new BaseDic { Code = 0, Name = "请选择" });
            }
            var data = H_DistributorAccess.GetDistributorList(new DistributorSearchRequest
            {
                PageSize = 100,
                DIsValid = IsValid
            });
            data?.List?.ForEach(x =>
            {
                list.Add(new BaseDic { Code = x.Id, Name = x.DName });

            });
            return list;
        }



        /// <summary>
        /// 资源匹配
        /// </summary>
        /// <param name="resourceids"></param>
        /// <param name="distributorid"></param>
        /// <returns></returns>
        public static BaseResponse BathGive(List<int> resourceids, string distributorid)
        {
            var addList = new List<H_RelationModel>();
            var set = false;
            var hDb = new H_RelationAccess();
            if (resourceids != null && resourceids.Any() && !string.IsNullOrWhiteSpace(distributorid))
            {
                foreach (var item in resourceids)
                {
                    //var supplierid=SupplierBll.get
                    var list = new H_RelationModel
                    {
                        DistributorId = distributorid,
                        RelationId = item.ToString()
                    };
                    addList.Add(list);
                }
            }
            if (addList != null && addList.Count > 0)
            {
                set = hDb.AddBatch(addList);
            }
            var res = new BaseResponse
            {
                IsSuccess = set ? 1 : 0,
                Msg = set ? string.Empty : "失败",
            };
            return res;
        }


        /// <summary>
        /// 资源匹配
        /// </summary>
        /// <param name="resourceids"></param>
        /// <param name="distributorid"></param>
        /// <returns></returns>
        public static BaseResponse OutGive(List<int> resourceids, string distributorid)
        {
            var addList = new List<H_RelationModel>();
            var set = 0;
            var hDb = new H_RelationAccess();
            if (resourceids != null && resourceids.Any() && !string.IsNullOrWhiteSpace(distributorid))
            {
                foreach (var item in resourceids)
                {
                    var outgive = hDb.Query().Where(s => s.DistributorId == distributorid && s.RelationId == item.ToString()).FirstOrDefault();
                    set = hDb.Delete(outgive);
                }
            }
            var res = new BaseResponse
            {
                IsSuccess = set > 0 ? 1 : 0,
                Msg = set > 0 ? string.Empty : "失败",
            };
            return res;
        }

        /// <summary>
        /// 根据分销商id获取已匹配过资源的供应商
        /// </summary>
        /// <param name="distributorid"></param>
        /// <returns></returns>
        public static List<H_RelationModel> GetGiveSupplier(string distributorid)
        {
            var list = new List<H_RelationModel>();
            if (!string.IsNullOrWhiteSpace(distributorid))
            {
                var hDb = new H_RelationAccess();
                list = hDb.Query().Where(s => s.DistributorId == distributorid).ToList();
            }
            return list;
        }


    }
}
