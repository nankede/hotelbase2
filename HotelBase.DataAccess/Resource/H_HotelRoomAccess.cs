using Dapper;
using HotelBase.Entity;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component.Access;

namespace HotelBase.DataAccess.Resource
{
    /// <summary>
    /// 酒店房型查询
    /// </summary>
    public class H_HotelRoomAccess : BaseAccess<H_HotelRoomModel>
    {
        public H_HotelRoomAccess() : base(MysqlHelper.Db_HotelBase)
        {
        }
    }

    /// <summary>
    /// 酒店房型 价格政策查询
    /// </summary>
    public class H_HotelRoomRuleAccess : BaseAccess<H_HotelRoomRuleModel>
    {
        public H_HotelRoomRuleAccess() : base(MysqlHelper.Db_HotelBase)
        {
        }
    }

    /// <summary>
    /// 酒店房型 价格政策查询
    /// </summary>
    public class H_HoteRulePriceAccess : BaseAccess<H_HoteRulePriceModel>
    {
        public H_HoteRulePriceAccess() : base(MysqlHelper.Db_HotelBase)
        {
        }
    }
}
