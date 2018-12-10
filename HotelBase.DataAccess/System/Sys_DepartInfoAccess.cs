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
using Component.Access;

namespace HotelBase.DataAccess
{
    /// <summary>
    /// 酒店房型 价格政策查询
    /// </summary>
    public class Sys_DepartInfoAccess : BaseAccess<Sys_DepartInfoModel>
    {
        public Sys_DepartInfoAccess() : base(MysqlHelper.Db_HotelBase)
        {
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<Sys_DepartInfoModel> GetDepartList(DepartistRequest request)
        {
            var response = new BasePageResponse<Sys_DepartInfoModel>();
            var totalSql = "SELECT Count(1) FROM Sys_DepartInfo ; ";
            var total = MysqlHelper.GetScalar<int>(totalSql);
            if (total > 0)
            {
                response.IsSuccess = 1;
                response.Total = total;
                var sql = "SELECT * FROM Sys_DepartInfo   ";
                sql += MysqlHelper.GetPageSql(request.PageIndex, request.PageSize);
                response.List = MysqlHelper.GetList<Sys_DepartInfoModel>(sql) ?? new List<Sys_DepartInfoModel>();
            }
            return response;
        }
    }
}
