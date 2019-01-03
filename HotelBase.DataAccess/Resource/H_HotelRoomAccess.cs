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

    /// <summary>
    /// 酒店图片
    /// </summary>
    public class H_HotelPictureAccess : BaseAccess<H_HotelPictureModel>
    {
        public H_HotelPictureAccess() : base(MysqlHelper.Db_HotelBase)
        {
        }

        /// <summary>
        /// 酒店查询
        /// </summary>
        /// <param name="request"></param>
        public BasePageResponse<H_HotelPictureModel> GetList(HotelPicSearchRequest request)
        {

            var response = new BasePageResponse<H_HotelPictureModel>();
            response.Total = Query().Count(x => x.HIId == request.HotelId);
            if (response.Total > 0)
            {
                response.IsSuccess = 1;
                var sql = "SELECT * FROM H_HotelPicture   ";
                sql += MysqlHelper.GetPageSql(request.PageIndex, request.PageSize);
                response.List = MysqlHelper.GetList<H_HotelPictureModel>(sql) ?? new List<H_HotelPictureModel>();
            }
            return response;
        }
    }
}
