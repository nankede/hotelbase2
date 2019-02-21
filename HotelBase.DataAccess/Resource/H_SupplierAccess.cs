using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component.Access;
using Dapper;
using HotelBase.Entity;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;

namespace HotelBase.DataAccess.Resource
{
    /// <summary>
    /// 供应商-Access
    /// </summary>
    public  class H_SupplierAccess : BaseAccess<H_SupplierModel>
    {

        public H_SupplierAccess() : base(MysqlHelper.Db_HotelBase)
        {
        }

        /// <summary>
        /// 新增供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Insert(H_SupplierModel model)
        {
            var sql = new StringBuilder();

            sql.Append(@" INSERT INTO `h_supplier` 
            ( `SCode`, `SSourceId`, `SName`, `SFullName`, `SAddress`, `SLinker`
            , `SLinkPhone`, `SLinkQQ`, `SLinkMail`, `SLinkFax`, `SFinanceLinker`
            , `SFinancePhone`, `SFinanceBankName`, `SFinanceAccount`, `SFinanceName`
            , `SInvoiceTitle`, `SInvoiceTax`, `SInvoiceItem`, `SInvoiceType`
            , `SCooperationBegin`, `SCooperationEnd`, `SIsValid`, `SAddName`
            , `SPMId`, `SPMName` )  ");
            sql.Append(@" VALUES  
            ( @SCode, @SSourceId, @SName, @SFullName, @SAddress, @SLinker
            , @SLinkPhone, @SLinkQQ, @SLinkMail, @SLinkFax, @SFinanceLinker
            , @SFinancePhone, @SFinanceBankName, @SFinanceAccount, @SFinanceName
            , @SInvoiceTitle, @SInvoiceTax, @SInvoiceItem, @SInvoiceType
            , @SCooperationBegin, @SCooperationEnd, @SIsValid, @SAddName 
            , @SPMId, @SPMName)  ");

            var para = new DynamicParameters();
            para.Add("@SCode", model.SCode ?? string.Empty);
            para.Add("@SSourceId", model.SSourceId);
            para.Add("@SName", model.SName ?? string.Empty);
            para.Add("@SFullName", model.SFullName ?? string.Empty);
            para.Add("@SAddress", model.SAddress ?? string.Empty);
            para.Add("@SLinker", model.SLinker ?? string.Empty);
            para.Add("@SLinkPhone", model.SLinkPhone ?? string.Empty);
            para.Add("@SLinkQQ", model.SLinkQQ ?? string.Empty);
            para.Add("@SLinkMail", model.SLinkMail ?? string.Empty);
            para.Add("@SLinkFax", model.SLinkFax ?? string.Empty);
            para.Add("@SFinanceLinker", model.SFinanceLinker ?? string.Empty);
            para.Add("@SFinancePhone", model.SFinancePhone ?? string.Empty);
            para.Add("@SFinanceBankName", model.SFinanceBankName ?? string.Empty);
            para.Add("@SFinanceAccount", model.SFinanceAccount ?? string.Empty);
            para.Add("@SFinanceName", model.SFinanceName ?? string.Empty);
            para.Add("@SInvoiceTitle", model.SInvoiceTitle ?? string.Empty);
            para.Add("@SInvoiceTax", model.SInvoiceTax ?? string.Empty);
            para.Add("@SInvoiceItem", model.SInvoiceItem ?? string.Empty);
            para.Add("@SInvoiceType", model.SInvoiceType ?? string.Empty);
            para.Add("@SCooperationBegin", model.SCooperationBegin);
            para.Add("@SCooperationEnd", model.SCooperationEnd);
            para.Add("@SIsValid", model.SIsValid);
            para.Add("@SAddName", model.SAddName ?? string.Empty);
            para.Add("@SPMId", model.SPMId);
            para.Add("@SPMName", model.SPMName ?? string.Empty);
            var id = MysqlHelper.Insert(sql.ToString(), para);
            return id;
        }

        /// <summary>
        /// 修改供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Update(H_SupplierModel model)
        {
            var sql = new StringBuilder();

            sql.Append(@" UPDATE `h_supplier` SET
             `SCode` = @SCode, `SSourceId` = @SSourceId, `SName` = @SName
			, `SFullName` = @SFullName, `SAddress` = @SAddress, `SLinker` =@SLinker 
            , `SLinkPhone` = @SLinkPhone, `SLinkQQ` = @SLinkQQ, `SLinkMail` = @SLinkMail
            , `SLinkFax` = @SLinkFax, `SFinanceLinker` = @SFinanceLinker
            , `SFinancePhone` = @SFinancePhone, `SFinanceBankName` = @SFinanceBankName
            , `SFinanceAccount` = @SFinanceAccount, `SFinanceName` =@SFinanceName
            , `SInvoiceTitle` = @SInvoiceTitle, `SInvoiceTax` = @SInvoiceTax
            , `SInvoiceItem` = @SInvoiceItem, `SInvoiceType` =@SInvoiceType
            , `SCooperationBegin` = @SCooperationBegin, `SCooperationEnd` = @SCooperationEnd
            , `SIsValid` = @SIsValid, `SUpdateTime`  =@SUpdateTime, `SUpdateName` =@SUpdateName ,`SPMName` = @SPMName ");
            sql.Append(" WHERE  `Id` =@Id   Limit 1;  ");

            var para = new DynamicParameters();
            para.Add("@Id", model.Id);
            para.Add("@SCode", model.SCode ?? string.Empty);
            para.Add("@SSourceId", model.SSourceId);
            para.Add("@SName", model.SName ?? string.Empty);
            para.Add("@SFullName", model.SFullName ?? string.Empty);
            para.Add("@SAddress", model.SAddress ?? string.Empty);
            para.Add("@SLinker", model.SLinker ?? string.Empty);
            para.Add("@SLinkPhone", model.SLinkPhone ?? string.Empty);
            para.Add("@SLinkQQ", model.SLinkQQ ?? string.Empty);
            para.Add("@SLinkMail", model.SLinkMail ?? string.Empty);
            para.Add("@SLinkFax", model.SLinkFax ?? string.Empty);
            para.Add("@SFinanceLinker", model.SFinanceLinker ?? string.Empty);
            para.Add("@SFinancePhone", model.SFinancePhone ?? string.Empty);
            para.Add("@SFinanceBankName", model.SFinanceBankName ?? string.Empty);
            para.Add("@SFinanceAccount", model.SFinanceAccount ?? string.Empty);
            para.Add("@SFinanceName", model.SFinanceName ?? string.Empty);
            para.Add("@SInvoiceTitle", model.SInvoiceTitle ?? string.Empty);
            para.Add("@SInvoiceTax", model.SInvoiceTax ?? string.Empty);
            para.Add("@SInvoiceItem", model.SInvoiceItem ?? string.Empty);
            para.Add("@SInvoiceType", model.SInvoiceType ?? string.Empty);
            para.Add("@SCooperationBegin", model.SCooperationBegin);
            para.Add("@SCooperationEnd", model.SCooperationEnd);
            para.Add("@SIsValid", model.SIsValid);
            para.Add("@SUpdateName", model.SUpdateName ?? string.Empty);
            para.Add("@SUpdateTime", model.SUpdateTime);
            para.Add("@SPMName", model.SPMName);
            var c = MysqlHelper.Update(sql.ToString(), para);
            return c;
        }

        /// <summary>
        /// 查询供应商列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BasePageResponse<H_SupplierModel> GetList(SupplierSearchRequest request)
        {
            var response = new BasePageResponse<H_SupplierModel>();
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
            if (request.SourceId > 0)
            {
                sqlWhere.Append(" AND SSourceId = @SourceId ");
                para.Add("@SourceId", request.SourceId);
            }
            if (request.IsValid > 0)
            {
                sqlWhere.Append(" AND SIsValid = @IsValid ");
                para.Add("@IsValid", request.IsValid == 1 ? 1 : 0);
            }
            if (!string.IsNullOrEmpty(request.Code))
            {
                sqlWhere.Append(" AND SCode = @Code ");
                para.Add("@Code", request.Code);
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                sqlWhere.Append(" AND SName Like @Name ");
                para.Add("@Name", $"%{request.Name}%");
            }
            if (!string.IsNullOrEmpty(request.LinkerName))
            {
                sqlWhere.Append(" AND SLinker Like @LinkerName ");
                para.Add("@LinkerName", $"%{request.LinkerName}%");
            }
            #endregion

            sqlTotal.Append(" SELECT Count(1) FROM H_Supplier WHERE 1=1 ");
            sqlTotal.Append(sqlWhere);
            var total = MysqlHelper.GetScalar<int>(sqlTotal.ToString(), para);
            response.IsSuccess = 1;
            if (total > 0)
            {
                sql.Append(" SELECT * FROM H_Supplier   WHERE 1=1  ");
                sql.Append(sqlWhere);
                sql.Append(" ORDER BY ID DESC ");
                sql.Append(MysqlHelper.GetPageSql(request.PageIndex, request.PageSize));
                response.Total = total;
                response.List = MysqlHelper.GetList<H_SupplierModel>(sql.ToString(), para);
            }
            return response;
        }


        /// <summary>
        /// 供应商
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static H_SupplierModel GetModel(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            var para = new DynamicParameters();
            var sql = "SELECT * FROM H_Supplier  WHERE  id=@id  LIMIT 1;   ";
            para.Add("@id", id);
            var data = MysqlHelper.GetModel<H_SupplierModel>(sql, para);
            return data;
        }
    }
}
