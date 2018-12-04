using Component.Access;
using Dapper;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.DataAccess.Order
{
    public class Ho_HotelOrderLogAccess : BaseAccess<HO_HotelOrderLogModel>
    {

        public Ho_HotelOrderLogAccess() : base(MysqlHelper.Db_HotelBase)
        {

        }

        /// <summary>
        /// 新增订单日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int AddOrderLogModel(HO_HotelOrderLogModel model)
        {
            var sql = new StringBuilder();
            sql.Append(" INSERT INTO `ho_hotelorderlog` (`HOLOrderId`, `HOLLogType`, `HOLRemark`, `HOLAddId`, `HOLAddName`, `HOLAddDepartId`, `HOLAddDepartName`, `HOLAddTime`) VALUES ");
            sql.Append("( @HOLOrderId, @HOLLogType, @HOLRemark, @HOLAddId, @HOLAddName, @HOLAddDepartId, @HOLAddDepartName, @HOLAddTime)");
            var para = new DynamicParameters();
            para.Add("@HOLOrderId", model.HOLOrderId);
            para.Add("@HOLLogType", model.HOLLogType);
            para.Add("@HOLRemark", model.HOLRemark ?? string.Empty);
            para.Add("@HOLAddId", model.HOLAddId);
            para.Add("@HOLAddName", model.HOLAddName ?? string.Empty);
            para.Add("@HOLAddDepartId", model.HOLAddDepartId);
            para.Add("@HOLAddDepartName", model.HOLAddDepartName ?? string.Empty);
            para.Add("@HOLAddTime", model.HOLAddTime);
            var id = MysqlHelper.Insert(sql.ToString(), para);
            return id;
        }
    }
}
