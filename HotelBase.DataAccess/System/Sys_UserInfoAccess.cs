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
        public static BasePageResponse<UserListResponse> GetUser(UserListRequest request)
        {
            var response = new BasePageResponse<UserListResponse>();
            var totalSql = "SELECT Count(1) FROM Sys_UserInfo ; ";
            var total = MysqlHelper.GetScalar<int>(totalSql);
            if (total > 0)
            {
                response.IsSuccess = 1;
                response.Total = total;
                response.List = new List<UserListResponse>();
                var sql = "SELECT * FROM Sys_UserInfo   ";
                sql += MysqlHelper.GetPageSql(request.PageIndex, request.PageSize);
                var list = MysqlHelper.GetList<Sys_UserInfoModel>(sql);
                list?.ForEach(x =>
                {
                    response.List.Add(new UserListResponse
                    {
                        Account = x.UIAccount,
                        DepartId = x.UIDepartId,
                        DepartName = x.UIDepartName,
                        Id = x.Id,
                        IsValid = x.UIIsValid,
                        Name = x.UIName,
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
            return r == 100 ? "超级管理员" : "未知职责";
        }
    }
}
