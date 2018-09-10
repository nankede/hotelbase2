using Dapper;
using HotelBase.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.DataAccess
{
    public static class MysqlHelper
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
        public static List<T> GetList<T>(string sql, object param = null)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    var resutl = conn.Query<T>(sql, param).ToList();
                    return resutl;
                }
                catch (Exception ex)
                {
                    LogHelper.Error("GetScalar查询异常", ex);
                }
                finally
                {
                    conn.Close();
                }
                return default(List<T>);
            }
        }

        /// <summary>
        /// 分页用的Limt
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetPageSql(int index, int size)
        {
            return $" limit {(index - 1) * size},{size} ; ";
        }

        /// <summary>
        /// GetModel
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static T GetScalar<T>(string sql, object param = null)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    var resutl = conn.ExecuteScalar<T>(sql, param);
                    return resutl;
                }
                catch (Exception ex)
                {
                    LogHelper.Error("GetScalar查询异常", ex);
                }
                finally
                {
                    conn.Close();
                }
                return default(T);
            }
        }

        /// <summary>
        /// GetModel
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static T GetModel<T>(string sql, object param = null)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    var resutl = conn.QueryFirst<T>(sql, param);
                    return resutl;
                }
                catch (Exception ex)
                {
                    LogHelper.Error("GetModel查询异常", ex);
                }
                finally
                {
                    conn.Close();
                }
                return default(T);
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
                try
                {
                    var resutl = conn.Execute(sql, param);
                    return resutl;
                }
                catch (Exception ex)
                {
                    LogHelper.Error("GetModel查询异常", ex);
                }
                finally
                {
                    conn.Close();
                }

                return 0;
            }
        }
    }
}
