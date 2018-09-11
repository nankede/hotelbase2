using Dapper;
using HotelBase.Entity;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.DataAccess.System
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public static class Sys_BaseDictionaryAccess
    {
        private static List<Sys_BaseDictionaryModel> _DicList = new List<Sys_BaseDictionaryModel>();
        /// <summary>
        /// 字典数据
        /// </summary>
        public static List<Sys_BaseDictionaryModel> DicList
        {
            get
            {
                if (_DicList == null || _DicList.Count == 0)
                {
                    var sql = "SELECT * FROM Sys_BaseDictionary   ";
                    _DicList = MysqlHelper.GetList<Sys_BaseDictionaryModel>(sql);

                }
                return _DicList ?? new List<Sys_BaseDictionaryModel>();
            }
        }

        /// <summary>
        /// 查询数据字典列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<Sys_BaseDictionaryModel> GetDicList(GetDicListRequest request)
        {
            var response = new BasePageResponse<Sys_BaseDictionaryModel>();
            var list = DicList;
            if (list == null || !list.Any())
            {
                return response;
            }
            if (request.ParentId > 0)
            {
                list = list?.Where(x => x.DParentId == request.ParentId).OrderBy(x => x.DCode).ToList();
            }
            if (request.Code > 0)
            {
                list = list?.Where(x => x.DCode.ToString().StartsWith($"{request.Code}")).OrderBy(x => x.DCode).ToList();
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                list = list?.Where(x => x.DName.Contains(request.Name)).OrderBy(x => x.DCode).ToList();
            }
            if (list == null || !list.Any())
            {
                return response;
            }
            var total = list?.Count ?? 0;
            if (total > 0)
            {
                response.IsSuccess = 1;
                response.Total = total;
                response.List = list.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)?.ToList();
            }
            return response;
        }


        /// <summary>
        /// 查询数据字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Sys_BaseDictionaryModel GetDicModel(int id, int code)
        {
            var response = new BasePageResponse<Sys_BaseDictionaryModel>();
            var list = DicList;
            if (list == null || !list.Any())
            {
                return null;
            }
            if (id > 0)
            {
                return list.FirstOrDefault(x => x.Id == id);
            }
            if (code > 0)
            {
                return list.FirstOrDefault(x => x.DCode == code);
            }
            return null;
        }

        /// <summary>
        /// 数据字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Sys_BaseDictionaryModel GetNewDicModel(int pId)
        {
            var model = new Sys_BaseDictionaryModel();
            var lastModel = DicList.OrderByDescending(x => x.DCode).FirstOrDefault(x => x.DParentId == pId);
            if (lastModel == null || lastModel.Id <= 0)
            {
                var pModel = GetDicModel(pId, 0);
                if (pModel != null && pModel.Id >= 0)
                {
                    model = new Sys_BaseDictionaryModel
                    {
                        DIsValid = 1,
                        DParentId = pId,
                        DParentName = pModel.DName,
                        DCode = pModel.DCode * 100 + 1
                    };
                }
            }
            else
            {
                model = new Sys_BaseDictionaryModel
                {
                    DIsValid = 1,
                    DParentId = pId,
                    DParentName = lastModel.DParentName,
                    DCode = lastModel.DCode + 1
                };
            }
            return model;
        }


        /// <summary>
        /// 新增数据字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int AddDicModel(Sys_BaseDictionaryModel model)
        {
            var sql = new StringBuilder();
            sql.Append(" INSERT INTO `sys_basedictionary` (`DParentId`, `DParentName`, `DName`, `DCode`, `DValue`, `DRemark`, `DIsValid`, `DSort`) VALUES ");
            sql.Append("( @DParentId, @DParentName, @DName, @DCode, @DValue, @DRemark, @DIsValid, @DSort)");
            var para = new DynamicParameters();
            para.Add("@DParentId", model.DParentId);
            para.Add("@DParentName", model.DParentName ?? string.Empty);
            para.Add("@DName", model.DName ?? string.Empty);
            para.Add("@DCode", model.DCode);
            para.Add("@DValue", model.DValue ?? string.Empty);
            para.Add("@DRemark", model.DRemark ?? string.Empty);
            para.Add("@DIsValid", model.DIsValid);
            para.Add("@DSort", model.DSort);
            var id = MysqlHelper.Insert(sql.ToString(), para);
            _DicList = null;
            return id;
        }

        /// <summary>
        /// 修改数据字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int UpdateDicModel(Sys_BaseDictionaryModel model)
        {
            if (model == null || model.Id <= 0) return 0;
            var sql = new StringBuilder();
            sql.Append(" UPDATE `sys_basedictionary`  ");
            sql.Append("  SET  `DName` = @DName  , `DValue`= @DValue , `DRemark`= @DRemark , `DIsValid`= @DIsValid , `DSort`= @DSort ");
            sql.Append(" WHERE  `Id` =@Id   Limit 1;  ");
            var para = new DynamicParameters();
            para.Add("@Id", model.Id);
            para.Add("@DName", model.DName ?? string.Empty);
            para.Add("@DValue", model.DValue ?? string.Empty);
            para.Add("@DRemark", model.DRemark ?? string.Empty);
            para.Add("@DIsValid", model.DIsValid);
            para.Add("@DSort", model.DSort);
            var c = MysqlHelper.Update(sql.ToString(), para);
            _DicList = null;
            return c;
        }
    }
}
