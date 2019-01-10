using Dapper;
using HotelBase.Entity;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.DataAccess.Distributor
{
    public class H_DistributorAccess
    {

        /// <summary>
        /// 分销商列表
        /// </summary>
        public static BasePageResponse<DistributorSearchResponse> GetDistributorList(DistributorSearchRequest request)
        {
            var response = new BasePageResponse<DistributorSearchResponse>();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT
                        Id,
	                    DName,
	                    DCooperationModeName,
	                    DAdvanceMoney,
	                    DLinkMan,
	                    DIsValid
                    FROM
                        H_DistributorInfo
                    WHERE
                        1 = 1");
            //分销商
            if (!string.IsNullOrWhiteSpace(request.DName))
            {
                sb.AppendFormat(" AND DName Like '%{0}%'", request.DName);
            }
            //分销商id
            if (request.Id > 0)
            {
                sb.AppendFormat(" AND Id = {0}", request.Id);
            }
            //省份
            if (request.DProviceId > 0)
            {
                sb.AppendFormat(" AND DProviceId = {0}", request.DProviceId);
            }
            //城市
            if (request.DCityId > 0)
            {
                sb.AppendFormat(" AND DCityId = {0}", request.DCityId);
            }
            //合作模式
            if (request.DCooperationMode > 0)
            {
                sb.AppendFormat(" AND DCooperationMode = {0}", request.DCooperationMode);
            }
            //有效性
            if (request.DIsValid >= 0)
            {
                sb.AppendFormat(" AND DIsValid = {0}", request.DIsValid);
            }
            sb.Append(" Order By Id DESC");
            var list = MysqlHelper.GetList<DistributorSearchResponse>(sb.ToString());
            var total = list?.Count ?? 0;
            if (total > 0)
            {
                response.IsSuccess = 1;
                response.Total = total;
                response.List = list.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)?.ToList();
            }
            return response;
        }


        /// <summary>
        /// 分销商详情
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static H_DistributorInfoModel GetModel(int did)
        {
            if (did <= 0)
            {
                return null;
            }
            var para = new DynamicParameters();
            var sql = "SELECT * FROM H_DistributorInfo  WHERE  id=@id  LIMIT 1;   ";
            para.Add("@id", did);
            var data = MysqlHelper.GetModel<H_DistributorInfoModel>(sql, para);
            return data;
        }

        /// <summary>
        /// 新增分销商
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int AddModel(H_DistributorInfoModel model)
        {
            var sql = new StringBuilder();
            sql.Append(" INSERT INTO `hotelbase`.`h_distributorinfo`(`DName`, `DProviceId`, `DProviceName`, `DCityId`, `DCityName`, `DPart1`, `DPart1Name`, `DPart2`, `DPart2Name`, `DCooperationMode`, `DCooperationModeName`, `DSettlement`, `DSettlementName`, `DStartDate`, `DEndDate`, `DSignType`, `DSignTypeName`, `DSettlementType`, `DSettlementTypeName`, `DAdvanceMoney`, `DCommissionPoint`, `DInvoiceTitle`, `DTaxNumbe`, `DInvoiceProject`, `DBillType`, `DLinkMan`, `DLinkMobile`, `DLinkEmail`, `DLineAddress`, `DLineChannel`, `DRemark`, `DIsValid`, `DAddName`, `DUpdateName`, `DAddTime`, `DUpdateTime`)  VALUES ");
            sql.Append("(@DName, @DProviceId, @DProviceName, @DCityId, @DCityName, @DPart1, @DPart1Name, @DPart2, @DPart2Name, @DCooperationMode, @DCooperationModeName, @DSettlement, @DSettlementName, @DStartDate, @DEndDate, @DSignType, @DSignTypeName, @DSettlementType, @DSettlementTypeName, @DAdvanceMoney, @DCommissionPoint, @DInvoiceTitle, @DTaxNumbe, @DInvoiceProject, @DBillType, @DLinkMan, @DLinkMobile, @DLinkEmail, @DLineAddress, @DLineChannel, @DRemark, @DIsValid, @DAddName, @DUpdateName, @DAddTime, @DUpdateTime);");
            var para = new DynamicParameters();
            para.Add("@DName", model.DName);
            para.Add("@DProviceId", model.DProviceId);
            para.Add("@DProviceName", model.DProviceName);
            para.Add("@DCityId", model.DCityId);
            para.Add("@DCityName", model.DCityName);
            para.Add("@DPart1", model.DPart1);
            para.Add("@DPart1Name", model.DPart1Name);
            para.Add("@DPart2", model.DPart2);
            para.Add("@DPart2Name", model.DPart2Name);
            para.Add("@DCooperationMode", model.DCooperationMode);
            para.Add("@DCooperationModeName", model.DCooperationModeName);
            para.Add("@DSettlement", model.DSettlement);
            para.Add("@DSettlementName", model.DSettlementName);
            para.Add("@DStartDate", model.DStartDate);
            para.Add("@DEndDate", model.DEndDate);
            para.Add("@DSignType", model.DSignType);
            para.Add("@DSignTypeName", model.DSignTypeName);
            para.Add("@DSettlementType", model.DSettlementType);
            para.Add("@DSettlementTypeName", model.DSettlementTypeName);
            para.Add("@DAdvanceMoney", model.DAdvanceMoney);
            para.Add("@DCommissionPoint", model.DCommissionPoint);
            para.Add("@DInvoiceTitle", model.DInvoiceTitle);
            para.Add("@DTaxNumbe", model.DTaxNumbe);
            para.Add("@DInvoiceProject", model.DInvoiceProject);
            para.Add("@DBillType", model.DBillType);
            para.Add("@DLinkMan", model.DLinkMan);
            para.Add("@DLinkMobile", model.DLinkMobile);
            para.Add("@DLinkEmail", model.DLinkEmail);
            para.Add("@DLineAddress", model.DLineAddress);
            para.Add("@DLineChannel", model.DLineChannel);
            para.Add("@DRemark", model.DRemark);
            para.Add("@DIsValid", model.DIsValid);
            para.Add("@DAddName", model.DAddName);
            para.Add("@DUpdateName", model.DUpdateName);
            para.Add("@DAddTime", model.DAddTime);
            para.Add("@DUpdateTime", model.DUpdateTime);
            var id = MysqlHelper.Insert(sql.ToString(), para);
            //_DicList = null;
            return id;
        }


        /// <summary>
        /// 修改分销商
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int UpdateModel(H_DistributorInfoModel model)
        {
            var sql = new StringBuilder();
            sql.Append(@" UPDATE `hotelbase`.`h_distributorinfo` 
                                    SET `DName` = @DName,
                                    `DProviceId` = @DProviceId,
                                    `DProviceName` = @DProviceName,
                                    `DCityId` = @DCityId,
                                    `DCityName` = @DCityName,
                                    `DPart1` = @DPart1,
                                    `DPart1Name` = @DPart1Name,
                                    `DPart2` = @DPart2,
                                    `DPart2Name` = @DPart2Name,
                                    `DCooperationMode` = @DCooperationMode,
                                    `DCooperationModeName` = @DCooperationModeName,
                                    `DSettlement` = @DSettlement,
                                    `DSettlementName` = @DSettlementName,
                                    `DStartDate` = @DStartDate,
                                    `DEndDate` = @DEndDate,
                                    `DSignType` = @DSignType,
                                    `DSignTypeName` = @DSignTypeName,
                                    `DSettlementType` = @DSettlementType,
                                    `DSettlementTypeName` = @DSettlementTypeName,
                                    `DAdvanceMoney` = @DAdvanceMoney,
                                    `DCommissionPoint` = @DCommissionPoint,
                                    `DInvoiceTitle` = @DInvoiceTitle,
                                    `DTaxNumbe` = @DTaxNumbe,
                                    `DInvoiceProject` = @DInvoiceProject,
                                    `DBillType` = @DBillType,
                                    `DLinkMan` = @DLinkMan,
                                    `DLinkMobile` = @DLinkMobile,
                                    `DLinkEmail` = @DLinkEmail,
                                    `DLineAddress` = @DLineAddress,
                                    `DLineChannel` = @DLineChannel,
                                    `DRemark` = @DRemark,
                                    `DIsValid` = @DIsValid,
                                    `DAddName` = @DAddName,
                                    `DUpdateName` = @DUpdateName,
                                    `DAddTime` = @DAddTime,
                                    `DUpdateTime` = @DUpdateTime
                                    WHERE
	                                    `Id` = @Id;");
            var para = new DynamicParameters();
            para.Add("@Id", model.Id);
            para.Add("@DName", model.DName);
            para.Add("@DProviceId", model.DProviceId);
            para.Add("@DProviceName", model.DProviceName);
            para.Add("@DCityId", model.DCityId);
            para.Add("@DCityName", model.DCityName);
            para.Add("@DPart1", model.DPart1);
            para.Add("@DPart1Name", model.DPart1Name);
            para.Add("@DPart2", model.DPart2);
            para.Add("@DPart2Name", model.DPart2Name);
            para.Add("@DCooperationMode", model.DCooperationMode);
            para.Add("@DCooperationModeName", model.DCooperationModeName);
            para.Add("@DSettlement", model.DSettlement);
            para.Add("@DSettlementName", model.DSettlementName);
            para.Add("@DStartDate", model.DStartDate);
            para.Add("@DEndDate", model.DEndDate);
            para.Add("@DSignType", model.DSignType);
            para.Add("@DSignTypeName", model.DSignTypeName);
            para.Add("@DSettlementType", model.DSettlementType);
            para.Add("@DSettlementTypeName", model.DSettlementTypeName);
            para.Add("@DAdvanceMoney", model.DAdvanceMoney);
            para.Add("@DCommissionPoint", model.DCommissionPoint);
            para.Add("@DInvoiceTitle", model.DInvoiceTitle);
            para.Add("@DTaxNumbe", model.DTaxNumbe);
            para.Add("@DInvoiceProject", model.DInvoiceProject);
            para.Add("@DBillType", model.DBillType);
            para.Add("@DLinkMan", model.DLinkMan);
            para.Add("@DLinkMobile", model.DLinkMobile);
            para.Add("@DLinkEmail", model.DLinkEmail);
            para.Add("@DLineAddress", model.DLineAddress);
            para.Add("@DLineChannel", model.DLineChannel);
            para.Add("@DRemark", model.DRemark);
            para.Add("@DIsValid", model.DIsValid);
            para.Add("@DAddName", model.DAddName);
            para.Add("@DUpdateName", model.DUpdateName);
            para.Add("@DAddTime", model.DAddTime);
            para.Add("@DUpdateTime", model.DUpdateTime);
            var c = MysqlHelper.Update(sql.ToString(), para);
            return c;
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
            sql.Append(@" UPDATE `h_distributorinfo` SET `DIsValid` = @Valid  , `DUpdateName`=@DUpdateName, `DUpdateTime`=@DUpdateTime ");
            sql.Append(" WHERE  `Id` = @Id   Limit 1; ");
            var para = new DynamicParameters();
            para.Add("@Id", id);
            para.Add("@Valid", valid == 1 ? 1 : 0);
            para.Add("@DUpdateName", name);
            para.Add("@DUpdateTime", DateTime.Now);
            var c = MysqlHelper.Update(sql.ToString(), para);
            return c;
        }
    }
}
