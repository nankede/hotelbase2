using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Models
{
    public class OrderSearchRequset : BaseRequest
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string HOCustomerSerialId { get; set; }

        /// <summary>
        /// 查询人员归属  1：预订人  2：入住人
        /// </summary>
        public int CustomerType { get; set; }

        /// <summary>
        /// 人
        /// </summary>
        public string PeopleName { get; set; }

        /// <summary>
        ///手机
        /// </summary>
        public string PeopleMobile { get; set; }

        /// <summary>
        /// 查询时间类型 1:入住时间 2：创建时间
        /// </summary>
        public int TimeType { get; set; }

        /// <summary>
        /// 查询开始时间(入住开始时间，创建开始时间)
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 查询结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 来源id
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 酒店名称
        /// </summary>
        public string HotelName { get; set; }

        /// <summary>
        /// 酒店id
        /// </summary>
        public string HIId { get; set; }

        /// <summary>
        /// 第三方流水号
        /// </summary>
        public string HOOutSerialId { get; set; }

        /// <summary>
        /// 供应商流水号
        /// </summary>
        public string HOSupplierSerialId { get; set; }

        /// <summary>
        /// 分销商流水号
        /// </summary>
        public string HODistributorSerialId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string HOStatus { get; set; }
    }

    /// <summary>
    /// 查询返回
    /// </summary>

    public class OrderSearchResponse : BaseResponse
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public int Id { get; set; }

        public string HOCustomerSerialId { get; set; }

        /// <summary>
        /// 供应商来源id
        /// </summary>
        public string HOSupplierSourceId { get; set; }

        /// <summary>
        /// 供应商来源
        /// </summary>
        public string HOSupplierSourceName { get; set; }

        /// <summary>
        /// 渠道
        /// </summary>
        public string HODistributorName { get; set; }

        /// <summary>
        /// 酒店Id
        /// </summary>
        public string HIId { get; set; }

        /// <summary>
        /// 酒店名称
        /// </summary>
        public string HName { get; set; }

        /// <summary>
        /// 入住时间
        /// </summary>
        public string HOCheckInDate { get; set; }

        /// <summary>
        /// 离店时间
        /// </summary>
        public string HOCheckOutDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string HOAddTime { get; set; }

        /// <summary>
        /// 预订人
        /// </summary>
        public string HOLinkerName { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string HOStatus { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal HOSellPrice { get; set; }

        /// <summary>
        /// 结算价
        /// </summary>
        public decimal HOContractPrice { get; set; }

    }


    public class OrdrModel : HO_HotelOrderModel
    {
        /// <summary>
        /// 确认方式
        /// </summary>
        public string SSubWay { get; set; }

        /// <summary>
        /// 传真号
        /// </summary>
        public string SLinkMail { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string SLinkFax { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string ProviceName { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 间夜
        /// </summary>
        public int Night { get; set; }

        /// <summary>
        /// 营收
        /// </summary>
        public decimal Reven { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 供应商酒店id
        /// </summary>
        public string HIOutId { get; set; }

        /// <summary>
        /// 分销商渠道
        /// </summary>
        public string DPart1Name { get; set; }

        /// <summary>
        /// 供应商房型id
        /// </summary>
        public string HROutId { get; set; }
    }

    public class ExeclOrder
    {
        public string HOCustomerSerialId { get; set; }

        public string ProviceName { get; set; }

        public string CityName { get; set; }

        public string HOSupperlierName { get; set; }

        public string HName { get; set; }

        public int HIId { get; set; }

        public string HRName { get; set; }

        public int HRId { get; set; }

        public int HORoomCount { get; set; }

        public string HOSupplierCorfirmSerialId { get; set; }

        public string HOSupplierSerialId { get; set; }

        public int HONight { get; set; }

        public DateTime HOCheckInDate { get; set; }

        public DateTime HOCheckOutDate { get; set; }

        public DateTime HOAddTime { get; set; }

        public string HOLinkerName { get; set; }

        public string HOLinkerMobile { get; set; }

        public decimal HOSellPrice { get; set; }

        public decimal HOContractPrice { get; set; }

        public decimal Reven { get; set; }

        public string Status { get; set; }

        public string HODistributorName { get; set; }

        public string HODistributorSerialId { get; set; }

        public string HOPart1 { get; set; }

    }

    public class OrderLogSearchRequset : BaseRequest
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string CustomerSerialId { get; set; }
    }
}
