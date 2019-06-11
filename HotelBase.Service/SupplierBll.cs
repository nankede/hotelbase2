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
    /// 供应商业务逻辑
    /// </summary>
    public class SupplierBll
    {
        /// <summary>
        /// 查询供应商列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<SupplierModel> GetList(SupplierSearchRequest request)
        {
            var data = H_SupplierAccess.GetList(request);
            var response = new BasePageResponse<SupplierModel>()
            {
                IsSuccess = data.IsSuccess,
                Total = data.Total,
                List = new List<SupplierModel>()
            };
            data?.List?.ForEach(x =>
            {
                response.List.Add(new SupplierModel
                {
                    Code = x.SCode,
                    Id = x.Id,
                    Linker = x.SLinker,
                    Name = x.SName,
                    SourceId = x.SSourceId,
                    Source = Sys_BaseDictionaryAccess.GetDicModel(0, x.SSourceId)?.DName ?? string.Empty,
                    PmName = x.SPMName,
                });
            });
            return response;
        }

        /// <summary>
        /// 新增供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse Insert(H_SupplierModel model)
        {
            var res = new BaseResponse();
            if (string.IsNullOrEmpty(model.SName))
            {
                res.Msg = "供应商名称不能为空";
                return res;
            }
            var id = H_SupplierAccess.Insert(model);
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
        /// 模糊搜索供应商
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<H_SupplierModel> GetSupplierList(int sourceId, string name)
        {
            var db = new H_SupplierAccess();
            var query = db.Query().Where(x => x.SName.Contains(name) && x.SIsValid == 1);
            if (sourceId > 0)
            {
                query.Where(x => x.SSourceId == sourceId);
            }
            return query.Top(20).ToList();
        }


        /// <summary>
        /// 模糊搜索供应商
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<H_SupplierModel> GetDistributorSupplierList(List<int> suppliers)
        {
            var db = new H_SupplierAccess();
            var query = db.Query().Where(x => suppliers.Contains(x.Id) && x.SIsValid == 1);
            return query.ToList();
        }

        /// <summary>
        /// 修改供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse Update(H_SupplierModel model)
        {
            var res = new BaseResponse();
            if (model.Id <= 0)
            {
                res.Msg = "无效的供应商";
                return res;
            }
            if (string.IsNullOrEmpty(model.SName))
            {
                res.Msg = "供应商名称不能为空";
                return res;
            }
            var i = H_SupplierAccess.Update(model);
            res = new BaseResponse
            {
                IsSuccess = i > 0 ? 1 : 0,
                Msg = i > 0 ? string.Empty : "更新失败",
            };
            return res;
        }

        /// <summary>
        /// 查询供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static H_SupplierModel GetDetail(int id)
        {
            var model = H_SupplierAccess.GetModel(id);
            return model;
        }

        /// <summary>
        /// 查询供应商列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static List<BaseDic> GetSupplier(int IsValid, int isDefault)
        {
            var list = new List<BaseDic>();
            if (isDefault == 1)
            {
                list.Add(new BaseDic { Code = 0, Name = "请选择" });
            }
            var data = H_SupplierAccess.GetList(new SupplierSearchRequest
            {
                PageSize = 100,
                IsValid = IsValid
            });
            data?.List?.ForEach(x =>
            {
                list.Add(new BaseDic { Code = x.Id, Name = x.SName });

            });
            return list;
        }
    }
}
