using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.DataAccess
{
    public static class MysqlHelper<T>
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        private static string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        /// <summary>
        /// GetList
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> GetList(string sql, object param = null)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                var resutl = conn.Query<T>(sql, param).ToList();
                conn.Close();
                return resutl;
            }
        }

        /// <summary>
        /// GetModel
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static T GetModel(string sql, object param = null)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                var resutl = conn.ExecuteScalar<T>(sql, param);
                conn.Close();
                return resutl;
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int Update(string sql, object param = null)
        {
            return Execute(sql, param);
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int Insert(string sql, object param = null)
        {
            return Execute(sql, param);
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int Execute(string sql, object param = null)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                var resutl = conn.Execute(sql, param);
                conn.Close();
                return resutl;
            }
        }
    }
}
