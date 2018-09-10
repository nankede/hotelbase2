using Dapper;
using HotelBase.Entity.Tables;
using MySql.Data.MySqlClient;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.DataAccess
{
    public class Class1
    {
        public static List<Sys_UserInfoModel> GetUser()
        {
            var sql = "SELECT * FROM Sys_UserInfo Limit 1;  ";
            return MysqlHelper<Sys_UserInfoModel>.GetList(sql);
        }
    }
}
