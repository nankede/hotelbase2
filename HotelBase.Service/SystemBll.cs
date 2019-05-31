using HotelBase.DataAccess;
using HotelBase.DataAccess.System;
using HotelBase.Common;
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
            var model = Sys_UserInfoAccess.GetUserDetial(id, account);
            var response = new UserModelResponse
            {
                IsSuccess = model?.Id > 0 ? 1 : 0,
                Model = model
            };
            return response;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static UserModel Login(string account, string pwd)
        {
            var user = Sys_UserInfoAccess.GetUserModel(0, account);
            if (user == null || user.Pwd != pwd)
            {
                return null;
            }
            user.Pwd = string.Empty;
            return user;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse UpdateUser(Sys_UserInfoModel model)
        {
            var db = new Sys_UserInfoAccess();
            var d = db.Update().Where(x => x.Id == model.Id).
                  Set(x => x.UIName == model.UIName
                  && x.UIDepartName == model.UIDepartName
                  && x.UIDepartId == model.UIDepartId
                  && x.UIUpdateName == model.UIUpdateName
                  && x.UIUpdateTime == model.UIUpdateTime).Top(1).Execute();
            var res = new BaseResponse
            {
                IsSuccess = d > 0 ? 1 : 0,
                Msg = d > 0 ? string.Empty : "更新失败",
            };
            return res;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public static BaseResponse InsertUser(Sys_UserInfoModel model)
        {
            var db = new Sys_UserInfoAccess();
            var id = db.Add(model);
            var res = new BaseResponse
            {
                AddId = (int)id,
                IsSuccess = id > 0 ? 1 : 0,
                Msg = id > 0 ? string.Empty : "新增失败",
            };
            return res;
        }


        #endregion

        #region 部门列表

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public static BasePageResponse<Sys_DepartInfoModel> GetDepartList(DepartistRequest request)
        {
            return Sys_DepartInfoAccess.GetDepartList(request);
        }

        /// <summary>
        /// 新增或修改部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse SaveDepart(DepartModel model, string opName)
        {
            var rtn = new BaseResponse();
            var db = new Sys_DepartInfoAccess();
            if (model.Id > 0)
            {
                var d = db.Update().Where(x => x.Id == model.Id)
                       .Set(x => x.DIName == (model.Name ?? String.Empty)
                        && x.DILeaderId == model.LearderId
                   && x.DILeaderName == (model.LearderName ?? String.Empty)
                   && x.DIParentId == model.DepartId
                   && x.DIParentName == (model.DepartName ?? String.Empty)
                   && x.DIUpdateTime == DateTime.Now
                   && x.DIUpdateName == (opName ?? String.Empty)
                   ).Execute();
                rtn = new BaseResponse
                {
                    IsSuccess = d > 0 ? 1 : 0,
                    Msg = d > 0 ? string.Empty : "更新失败",
                };
            }
            else
            {
                var dbModel = new Sys_DepartInfoModel
                {
                    Id = model.Id,
                    DIName = model.Name ?? String.Empty,
                    DILeaderId = model.LearderId,
                    DILeaderName = model.LearderName ?? String.Empty,
                    DIParentId = model.DepartId,
                    DIParentName = model.DepartName ?? String.Empty,
                    DIAddTime = DateTime.Now,
                    DIAddName = opName ?? String.Empty,
                    DIIsValid = 1
                };
                var id = db.Add(dbModel);
                rtn = new BaseResponse
                {
                    AddId = (int)id,
                    IsSuccess = id > 0 ? 1 : 0,
                    Msg = id > 0 ? string.Empty : "新增失败",
                };
            }
            return rtn;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public static DepartModelResponse GetDepart(int id)
        {
            var rtn = new DepartModelResponse();
            var model = new Sys_DepartInfoModel();
            if (id > 0)
            {
                model = new Sys_DepartInfoAccess().Query().Where(x => x.Id == id).FirstOrDefault();
            }
            if (model != null && model.Id > 0)
            {
                rtn = new DepartModelResponse
                {
                    IsSuccess = 1,
                    Model = new DepartModel
                    {
                        Name = model.DIName,
                        Id = model.Id,
                        DepartId = model.DIParentId,
                        DepartName = model.DIParentName ?? String.Empty,
                        LearderId = model.DILeaderId,
                        LearderName = model.DILeaderName ?? String.Empty
                    }
                };

            }
            else
            {
                rtn = new DepartModelResponse
                {
                    IsSuccess = 0,
                    Msg = "未查询到数据"
                };
            }
            return rtn;
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
        public static List<BaseDic> GetDicListByPCode(int pCode, int isDefault)
        {
            var list = new List<BaseDic>();
            if (isDefault == 1)
            {
                list.Add(new BaseDic { Code = 0, Name = "请选择" });
            }
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
                    AddId = (int)id,
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
