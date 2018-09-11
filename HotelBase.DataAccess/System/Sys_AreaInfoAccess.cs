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
    /// 国家地区相关
    /// </summary>
    public class Sys_AreaInfoAccess
    {
        private static List<Sys_AreaInfoModel> _AreaList = new List<Sys_AreaInfoModel>();
        /// <summary>
        /// 地区数据-中国
        /// </summary>
        public static List<Sys_AreaInfoModel> AreaList
        {
            get
            {
                if (_AreaList == null || _AreaList.Count == 0)
                {
                    var sql = "SELECT * FROM Sys_AreaInfo WHERE pid_path  LIKE '3106,%' ";
                    _AreaList = MysqlHelper.GetList<Sys_AreaInfoModel>(sql);

                }
                return _AreaList ?? new List<Sys_AreaInfoModel>();
            }
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public static List<AreaInfoModel> GetAreaList(int pId)
        {
            var list = new List<AreaInfoModel>();
            var dataList = AreaList?.Where(x => x.pid == pId).ToList();
            dataList?.ForEach(x =>
            {
                list.Add(new AreaInfoModel
                {
                    Name = x.name,
                    PId = x.pid,
                    Type = x.type,
                    Id = x.id,
                });
            });
            return list;
        }


    }
}
