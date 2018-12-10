using Dapper;
using HotelBase.Entity.Tables;
using MySql.Data.MySqlClient;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBase.Entity.Models;
using HotelBase.Entity;

namespace HotelBase.DataAccess
{
    public class Sys_UserInfoAccess
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<UserModel> GetUserList(UserListRequest request)
        {
            var response = new BasePageResponse<UserModel>();
            var totalSql = "SELECT Count(1) FROM Sys_UserInfo ; ";
            var total = MysqlHelper.GetScalar<int>(totalSql);
            if (total > 0)
            {
                response.IsSuccess = 1;
                response.Total = total;
                response.List = new List<UserModel>();
                var sql = "SELECT * FROM Sys_UserInfo   ";
                sql += MysqlHelper.GetPageSql(request.PageIndex, request.PageSize);
                var list = MysqlHelper.GetList<Sys_UserInfoModel>(sql);
                list?.ForEach(x =>
                {
                    response.List.Add(new UserModel
                    {
                        Account = x.UIAccount,
                        DepartId = x.UIDepartId,
                        DepartName = x.UIDepartName,
                        Id = x.Id,
                        IsValid = x.UIIsValid,
                        Name = x.UIName,
                        R = x.UIResponsibility,
                        Responsibility = GetResponsibility(x.UIResponsibility)
                    });
                });
            }
            return response;
        }
        /// <summary>
        /// 获取职责
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string GetResponsibility(int r)
        {
            switch (r)
            {
                case 1:
                    return "资源维护";
                case 2:
                    return "订单维护";
                case 4:
                    return "订单统计";
                case 100:
                    return "超级管理员";
                default:
                    return "未设定";
            }
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static UserModel GetUserModel(int id, string account)
        {
            if (id <= 0 && string.IsNullOrEmpty(account))
            {
                return null;
            }
            var model = new UserModel();

            var para = new DynamicParameters();

            var sql = "SELECT * FROM Sys_UserInfo    ";
            if (id > 0)
            {
                sql += " WHERE  id=@id  LIMIT 1;  ";
                para.Add("@id", id);
            }
            else if (!string.IsNullOrEmpty(account))
            {
                sql += " WHERE  UIAccount=@account  LIMIT 1;  ";
                para.Add("@account", account);
            }
            var data = MysqlHelper.GetModel<Sys_UserInfoModel>(sql, para);
            if (data != null && data.Id > 0)
            {
                model = new UserModel
                {
                    Account = data.UIAccount,
                    DepartId = data.UIDepartId,
                    DepartName = data.UIDepartName,
                    Id = data.Id,
                    IsValid = data.UIIsValid,
                    Name = data.UIName,
                    R = data.UIResponsibility,
                    Responsibility = GetResponsibility(data.UIResponsibility),
                    Pwd = data.UIPassWord
                };
                return model;
            }
            return null;
        }
    }
}
