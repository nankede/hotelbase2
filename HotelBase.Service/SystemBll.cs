﻿using HotelBase.DataAccess;
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
    public class SystemBll
    {
        #region 用户相关
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<UserModel> GetUserList(UserListRequest request)
        {
            var response = Sys_UserInfoAccess.GetUserList(request);
            return response;
        }
        /// <summary>
        /// 用户实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static UserModelResponse GetUserModel(int id, string account)
        {
            var model = Sys_UserInfoAccess.GetUserModel(id, account);
            var response = new UserModelResponse
            {
                IsSuccess = model?.Id > 0 ? 1 : 0,
                Model = model
            };
            return response;
        }
        #endregion

        #region 数字字典
        /// <summary>
        /// 数据字典列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<Sys_BaseDictionaryModel> GetDicList(GetDicListRequest request)
        {
            return Sys_BaseDictionaryAccess.GetDicList(request);
        }
        
        /// <summary>
        /// 根据父Code查数据字典列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static List<BaseDic> GetDicListByPCode(int pCode)
        {
            var list = new List<BaseDic>();
            list.Add(new BaseDic { Code = 0, Name = "请选择" });
            var pId = GetDicModel(0, pCode)?.Id ?? 0;
            if (pId > 0)
            {
                var data = Sys_BaseDictionaryAccess.GetDicList(new GetDicListRequest
                {
                    ParentId = pId,
                    PageSize = 100
                });
                data?.List?.ForEach(x =>
                {
                    list.Add(new BaseDic { Code = x.DCode, Name = x.DName });

                });
            }
            return list;
        }

        /// <summary>
        /// 数据字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Sys_BaseDictionaryModel GetDicModel(int id, int code)
        {
            return Sys_BaseDictionaryAccess.GetDicModel(id, code);
        }

        /// <summary>
        /// 获取Code数据字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Sys_BaseDictionaryModel GetNewDicModel(int pId)
        {
            return Sys_BaseDictionaryAccess.GetNewDicModel(pId);
        }


        /// <summary>
        /// 新增数据字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BaseResponse AddDicModel(Sys_BaseDictionaryModel model)
        {
            var res = new BaseResponse();
            var id = 0;
            if (model.DCode == 0)
            {
                res.Msg = "无效的Code";
                return res;
            }
            if (string.IsNullOrEmpty(model.DName))
            {
                res.Msg = "字典名称不能为空";
                return res;
            }
            var dic = Sys_BaseDictionaryAccess.GetDicModel(0, model.DCode);
            if (dic != null && dic.Id > 0)
            {
                res.Msg = "已存在相同Code的字典";
                return res;
            }
            id = Sys_BaseDictionaryAccess.AddDicModel(model);
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
        /// 修改数据字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BaseResponse UpdateDicModel(Sys_BaseDictionaryModel model)
        {
            var res = new BaseResponse();
            var i = Sys_BaseDictionaryAccess.UpdateDicModel(model);
            res = new BaseResponse
            {
                IsSuccess = i > 0 ? 1 : 0,
                Msg = i > 0 ? string.Empty : "更新失败",
            };
            return res;
        }
        #endregion

        #region 国家地区

        /// <summary>
        /// 查询区域列表
        /// pid =1 省份
        /// </summary>
        /// <returns></returns>
        public static List<AreaInfoModel> GetAreaList(int pId)
        {
            return Sys_AreaInfoAccess.GetAreaList(pId);
        }

        #endregion
    }
}