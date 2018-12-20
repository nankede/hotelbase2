using Component.Access;
using Dapper;
using HotelBase.Entity;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.DataAccess.Resource
{
    /// <summary>
    /// 酒店查询
    /// </summary>
    public class H_HotelInfoAccess : BaseAccess<H_HotelInfoModel>
    {
        public H_HotelInfoAccess() : base(MysqlHelper.Db_HotelBase)
        {
        }

        /// <summary>
        /// 酒店查询
        /// </summary>
        /// <param name="request"></param>
        public static BasePageResponse<H_HotelInfoModel> GetList(HotelSearchRequest request)
        {

            var response = new BasePageResponse<H_HotelInfoModel>();
            var sql = new StringBuilder();
            var sqlTotal = new StringBuilder();
            var sqlWhere = new StringBuilder();
            var para = new DynamicParameters();
            #region Where条件

            if (request.Id > 0)
            {
                sqlWhere.Append(" AND Id = @Id ");
                para.Add("@Id", request.Id);
            }
            //if (request.SourceId > 0)
            //{
            //    sqlWhere.Append(" AND SSourceId = @SourceId ");
            //    para.Add("@SourceId", request.SourceId);
            //}
            //if (!string.IsNullOrEmpty(request.LinkerName))
            //{
            //    sqlWhere.Append(" AND SLinker Like @LinkerName ");
            //    para.Add("@LinkerName", $"%{request.LinkerName}%");
            //}
            if (request.IsValid > 0)
            {
                sqlWhere.Append(" AND SIsValid = @IsValid ");
                para.Add("@IsValid", request.IsValid == 1 ? 1 : 0);
            }
            if (request.ProvId > 0)
            {
                sqlWhere.Append(" AND HIProvinceId = @ProvId ");
                para.Add("@ProvId", request.ProvId);
            }
            if (request.CityId > 0)
            {
                sqlWhere.Append(" AND HICityId = @CityId ");
                para.Add("@CityId", request.CityId);
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                sqlWhere.Append(" AND HIName Like @Name ");
                para.Add("@Name", $"%{request.Name}%");
            }

            #endregion

            sqlTotal.Append(" SELECT Count(1) FROM h_hotelinfo WHERE 1=1 ");
            sqlTotal.Append(sqlWhere);
            var total = MysqlHelper.GetScalar<int>(sqlTotal.ToString(), para);
            response.IsSuccess = 1;
            if (total > 0)
            {
                sql.Append(" SELECT * FROM h_hotelinfo   WHERE 1=1  ");
                sql.Append(sqlWhere);
                sql.Append(" ORDER BY ID DESC ");
                sql.Append(MysqlHelper.GetPageSql(request.PageIndex, request.PageSize));
                response.Total = total;
                response.List = MysqlHelper.GetList<H_HotelInfoModel>(sql.ToString(), para);
            }
            return response;
        }

        /// <summary>
        /// 设置有效性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valid"></param>
        /// <returns></returns>
        public static int SetValid(int id, int valid, string name)
        {
            var sql = new StringBuilder();
            sql.Append(@" UPDATE `h_hotelinfo` SET `HIIsValid` = @Valid  , `HIUpdateName`=@HIUpdateName, `HIUpdateTime`=@HIUpdateTime ");
            sql.Append(" WHERE  `Id` = @Id   Limit 1; ");
            var para = new DynamicParameters();
            para.Add("@Id", id);
            para.Add("@Valid", valid == 1 ? 1 : 0);
            para.Add("@HIUpdateName", name);
            para.Add("@HIUpdateTime", DateTime.Now);
            var c = MysqlHelper.Update(sql.ToString(), para);
            return c;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static H_HotelInfoModel GetModel(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            var para = new DynamicParameters();
            var sql = "SELECT * FROM h_hotelinfo  WHERE  id=@id  LIMIT 1;   ";
            para.Add("@id", id);
            var data = MysqlHelper.GetModel<H_HotelInfoModel>(sql, para);
            return data;
        }

    }
}
