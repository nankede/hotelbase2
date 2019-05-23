using Component.Access;
using Component.Access.DapperExtensions.Lambda;
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
        public static BasePageResponse<HotelSearchResponse> GetList(HotelSearchRequest request)
        {
            var response = new BasePageResponse<HotelSearchResponse>();
            var sql = new StringBuilder();
            var sqlTotal = new StringBuilder();
            var sqlWhere = new StringBuilder();
            var para = new DynamicParameters();
            var idList = new List<int>();//酒店Id 
            var hrsList = new List<H_HotelRoomRuleModel>();//价格政策查的供应商
            var hotelList = new List<H_HotelInfoModel>();//酒店列表
            if (request.SourceId > 0 || !string.IsNullOrEmpty(request.SupplierName))
            {//需要查政策
                hrsList = GetSupplier(request.SourceId, request.SupplierName, null);
                idList = hrsList?.Select(x => x.HIId)?.ToList();
            }

            #region Where条件
            if (idList != null && idList.Count > 0)
            {
                sqlWhere.Append($" AND Id IN ({string.Join(",", idList)} ) ");
            }

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
                hotelList = MysqlHelper.GetList<H_HotelInfoModel>(sql.ToString(), para);
                //重新查资源
                hrsList = GetSupplier(0, string.Empty, hotelList.Select(x => x.Id).ToList());
                response.List = new List<HotelSearchResponse>();
                hotelList?.ForEach(x =>
                 {
                     var price = hrsList.Where(s => s.HIId == x.Id)?.ToList();
                     var source = string.Empty;
                     var supplierName = string.Empty;
                     if (price != null && price.Count > 0)
                     {
                         source = string.Join(",", price.Select(s => s.HRRSourceName).Distinct());
                         supplierName = string.Join(",", price.Select(s => s.HRRSupplierName).Distinct());
                     }
                     response.List.Add(new HotelSearchResponse
                     {
                         Id = x.Id,
                         Name = x.HIName,
                         //SourceId = x.SSourceId,
                         CityId = x.HICityId,
                         CityName = x.HICity,
                         ProvName = x.HIProvince,
                         ProvId = x.HIProvinceId,
                         Valid = x.HIIsValid,
                         Source = source ?? string.Empty,
                         SupplierName = supplierName ?? string.Empty,
                     });
                 });
            }
            return response;
        }
        /// <summary>
        /// 查询供应商
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="supplierName"></param>
        /// <param name="hotelidList"></param>
        /// <returns></returns>
        private static List<H_HotelRoomRuleModel> GetSupplier(int sourceId, string supplierName, List<int> hotelidList)
        {
            List<H_HotelRoomRuleModel> hrsList;
            var query = new H_HotelRoomRuleAccess().Query()
                .AddSelect(x => x.HIId)
                .AddSelect(x => x.HRRSourceName)
                .AddSelect(x => x.HRRSupplierName)
                ;
            if (sourceId > 0)
            {
                query.Where(x => x.HRRSourceId == sourceId);
            }
            if (!string.IsNullOrEmpty(supplierName))
            {
                query.Where(x => x.HRRSupplierName.Contains(supplierName));
            }
            if (hotelidList != null && hotelidList.Count > 0)
            {
                query.Where(x => x.HIId.In(hotelidList));
            }
            hrsList = query.Distinct().ToList();
            return hrsList;
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

        /// <summary>
        /// 获取酒店列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static BasePageResponse<H_HotelInfoModel> GetAll(GiveResourceSearchRequest request)
        {
            var response = new BasePageResponse<H_HotelInfoModel>();
            StringBuilder sb = new StringBuilder();
            sb.Append(@" SELECT DISTINCT b.* FROM 
	                            h_hotelinfo b
                            INNER JOIN h_hotelroom r ON r.HIId = b.Id
                            INNER JOIN h_hotelroomrule rr ON r.Id = rr.HRId
                            LEFT JOIN h_supplier s ON s.Id = rr.HRRSupplierId AND s.SIsValid = 1 
                            WHERE
	                            b.HIIsValid = 1
                            AND r.HRIsValid = 1
                            AND rr.HRRIsValid = 1
                            ");
            if (!string.IsNullOrWhiteSpace(request.SupplierId))
            {
                sb.AppendFormat(" AND  rr.HRRSupplierId = {0}", request.SupplierId);
            }
            if (request.ProviceId > 0)
            {
                sb.AppendFormat(" AND  b.HIProvinceId = {0}", request.ProviceId);
            }
            if (request.CityId > 0)
            {
                sb.AppendFormat(" AND  b.HICityId = {0}", request.CityId);
            }
            if (!string.IsNullOrWhiteSpace(request.HotelId))
            {
                sb.AppendFormat(" AND  b.Id = {0}", request.HotelId);
            }
            var list = MysqlHelper.GetList<H_HotelInfoModel>(sb.ToString());
            var total = list?.Count ?? 0;
            if (total > 0)
            {
                response.IsSuccess = 1;
                response.Total = total;
                response.List = list.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)?.ToList();
                response.AllList = list;
            }
            return response;
        }
    }
}
