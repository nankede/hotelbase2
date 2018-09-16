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
    public static class H_HotelInfoAccess
    {
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
                sqlWhere.Append(" AND SName Like @Name ");
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
        public static int SetValid(int id, int valid)
        {
            var sql = new StringBuilder();
            sql.Append(@" UPDATE `h_hotelinfo` SET `HIIsValid` = @Valid ");
            sql.Append(" WHERE  `Id` = @Id   Limit 1; ");
            var para = new DynamicParameters();
            para.Add("@Id", id);
            para.Add("@Valid", valid == 1 ? 1 : 0);
            var c = MysqlHelper.Update(sql.ToString(), para);
            return c;
        }


        /// <summary>
        /// 修改酒店
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Update(H_HotelInfoModel model)
        {
            var sql = new StringBuilder();

            sql.Append(@" UPDATE `h_hotelinfo` SET
             `HIName` =@HIName, `HIProvinceId`=@HIProvinceId, `HIProvince`=@HIProvince, `HICityId`=@HICityId, `HICity`=@HICity
            , `HICountyId`=@HICountyId, `HICounty`=@HICounty, `HIAddress`=@HIAddress, `HIShoppingAreaId`=@HIShoppingAreaId
            , `HIShoppingArea`=@HIShoppingArea, `HIFacilities`=@HIFacilities, `HICheckIn`=@HICheckIn, `HICheckOut`=@HICheckOut
            , `HIChildRemark`=@HIChildRemark, `HIPetRemark`=@HIPetRemark, `HIHotelIntroduction` =@HIHotelIntroduction
            , `HIIsValid` =@HIIsValid, `HIUpdateName`=@HIUpdateName, `HIUpdateTime`=@HIUpdateTime  ");
            sql.Append(" WHERE  `Id` =@Id   Limit 1;  ");

            var para = new DynamicParameters();
            para.Add("@Id", model.Id);
            para.Add("@HIName", model.HIName ?? string.Empty);
            para.Add("@HIProvinceId", model.HIProvinceId);
            para.Add("@HIProvince", model.HIProvince ?? string.Empty);
            para.Add("@HICityId", model.HICityId);
            para.Add("@HICity", model.HICity ?? string.Empty);
            para.Add("@HICountyId", model.HICountyId);
            para.Add("@HICounty", model.HICounty ?? string.Empty);
            para.Add("@HIAddress", model.HIAddress ?? string.Empty);
            para.Add("@HIShoppingAreaId", model.HIShoppingAreaId);
            para.Add("@HIShoppingArea", model.HIShoppingArea ?? string.Empty);
            para.Add("@HIFacilities", model.HIFacilities ?? string.Empty);
            para.Add("@HICheckIn", model.HICheckIn);
            para.Add("@HICheckOut", model.HICheckOut);
            para.Add("@HIChildRemark", model.HIChildRemark ?? string.Empty);
            para.Add("@HIPetRemark", model.HIPetRemark ?? string.Empty);
            para.Add("@HIHotelIntroduction", model.HIHotelIntroduction ?? string.Empty);
            para.Add("@HIIsValid", model.HIIsValid);
            para.Add("@HIUpdateName", model.HIUpdateName ?? string.Empty);
            para.Add("@HIUpdateTime", model.HIUpdateTime);
            var c = MysqlHelper.Update(sql.ToString(), para);
            return c;
        }

        /// <summary>
        /// 新增酒店
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Insert(H_HotelInfoModel model)
        {
            var sql = new StringBuilder();

            sql.Append(@" INSERT INTO `h_hotelinfo` 
            ( `HIName`, `HIProvinceId`, `HIProvince`, `HICityId`, `HICity`
            , `HICountyId`, `HICounty`, `HIAddress`, `HIShoppingAreaId`
            , `HIShoppingArea`, `HIFacilities`, `HICheckIn`, `HICheckOut`
            , `HIChildRemark`, `HIPetRemark`, `HIHotelIntroduction`
            , `HIIsValid`, `HIAddName`, `HIUpdateName`
            , `HIAddTime`, `HIUpdateTime`) 
              VALUES  
            (  @HIName,@HIProvinceId,@HIProvince,@HICityId,@HICity
            ,@HICountyId,@HICounty,@HIAddress,@HIShoppingAreaId
            ,@HIShoppingArea,@HIFacilities,@HICheckIn,@HICheckOut
            ,@HIChildRemark,@HIPetRemark,@HIHotelIntroduction
            ,@HIIsValid,@HIAddName)   ");

            var para = new DynamicParameters();
            para.Add("@HIName", model.HIName ?? string.Empty);
            para.Add("@HIProvinceId", model.HIProvinceId);
            para.Add("@HIProvince", model.HIProvince ?? string.Empty);
            para.Add("@HICityId", model.HICityId);
            para.Add("@HICity", model.HICity ?? string.Empty);
            para.Add("@HICountyId", model.HICountyId);
            para.Add("@HICounty", model.HICounty ?? string.Empty);
            para.Add("@HIAddress", model.HIAddress ?? string.Empty);
            para.Add("@HIShoppingAreaId", model.HIShoppingAreaId);
            para.Add("@HIShoppingArea", model.HIShoppingArea ?? string.Empty);
            para.Add("@HIFacilities", model.HIFacilities ?? string.Empty);
            para.Add("@HICheckIn", model.HICheckIn);
            para.Add("@HICheckOut", model.HICheckOut);
            para.Add("@HIChildRemark", model.HIChildRemark ?? string.Empty);
            para.Add("@HIPetRemark", model.HIPetRemark ?? string.Empty);
            para.Add("@HIHotelIntroduction", model.HIHotelIntroduction ?? string.Empty);
            para.Add("@HIIsValid", model.HIIsValid);
            para.Add("@HIAddName", model.HIAddName ?? string.Empty);
            var id = MysqlHelper.Insert(sql.ToString(), para);
            return id;
        }
    }
}
