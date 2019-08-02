using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Models
{
    #region 酒店

    /// <summary>
    /// 酒店查询
    /// </summary>
    public class HotelSearchRequest : BaseRequest
    {
        /// <summary>酒店Id</summary>
        public int Id { get; set; }
        /// <summary>供应商来源</summary>
        public int SourceId { get; set; }
        /// <summary>有效性  1 有效  2 无效/summary>
        public int IsValid { get; set; } = -1;
        /// <summary>省份Id</summary>
        public int ProvId { get; set; }
        /// <summary>城市Id</summary>
        public int CityId { get; set; }
        /// <summary>酒店名称</summary>
        public string Name { get; set; }
        /// <summary>供应商名称</summary>
        public string SupplierName { get; set; }
    }

    /// <summary>
    /// 酒店查询
    /// </summary>
    public class HotelSearchResponse
    {
        /// <summary>酒店Id</summary>
        public int Id { get; set; }
        /// <summary>供应商来源</summary>
        public int SourceId { get; set; }
        /// <summary>有效性1 有效 0 无效</summary>
        public int Valid { get; set; } = -1;
        /// <summary>省份Id</summary>
        public int ProvId { get; set; }
        /// <summary>省份</summary>
        public string ProvName { get; set; }
        /// <summary>城市Id</summary>
        public int CityId { get; set; }
        /// <summary>城市</summary>
        public string CityName { get; set; }
        /// <summary>酒店名称</summary>
        public string Name { get; set; }
        /// <summary>供应商来源</summary>
        public string Source { get; set; }
        /// <summary>供应商名称</summary>
        public string SupplierName { get; set; }

    }



    /// <summary>
    /// 酒店导出
    /// </summary>
    public class HotelExportResponse
    {
        /// <summary>酒店Id</summary>
        public int Id { get; set; }
        /// <summary>省份Id</summary>
        public int ProvId { get; set; }
        /// <summary>省份</summary>
        public string ProvName { get; set; }
        /// <summary>城市Id</summary>
        public int CityId { get; set; }
        /// <summary>城市</summary>
        public string CityName { get; set; }
        /// <summary>酒店名称</summary>
        public string Name { get; set; }
        /// <summary>
        /// 供应商酒店id
        /// </summary>
        public int OutId { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string HotelAddress { get; set; }
        /// <summary>
        /// 经纬
        /// </summary>
        public string GdLonLat { get; set; }

        public string HotelPhone { get; set; }

    }

    #endregion

    #region 酒店房型

    /// <summary>
    /// 酒店查询
    /// </summary>
    public class HotelRoomSearchRequest : BaseRequest
    {
        public int HotelId { get; set; }
        public int IsValiad { get; set; }

    }

    #endregion

    #region 酒店房型-政策

    /// <summary>
    /// 酒店查询
    /// </summary>
    public class HotelRoomRuleSearchRequest : BaseRequest
    {
        /// <summary>
        /// 酒店Id
        /// </summary>
        public int HotelId { get; set; }
        /// <summary>
        /// 房型
        /// </summary>
        public int RoomId { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        public int IsValiad { get; set; }

    }

    #endregion

    #region 酒店房型-价格

    /// <summary>
    /// 酒店查询
    /// </summary>
    public class HotelPriceSearchRequest : BaseRequest
    {
        /// <summary>
        /// 酒店Id
        /// </summary>
        public int HotelId { get; set; }
        /// <summary>
        /// 房型
        /// </summary>
        public int RoomId { get; set; }
        /// <summary>
        /// 政策
        /// </summary>
        public int RuleId { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        public int IsValiad { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }

    }

    public class HotelPriceModel
    {
        public int Type { get; set; }
        public int Id { get; set; }
        public string PriceDate { get; set; }
        public decimal SellPrice { get; set; }
        public decimal ContractPrice { get; set; }
        public int Count { get; set; }
        public int RetainCount { get; set; }

    }

    public class SaveHotelPriceModel : HotelPriceModel
    {
        /// <summary>
        /// 酒店Id
        /// </summary>
        public int HotelId { get; set; }
        /// <summary>
        /// 房型
        /// </summary>
        public int RoomId { get; set; }
        /// <summary>
        /// 政策
        /// </summary>
        public int RuleId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperateName { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public string MonthList { get; set; }
        /// <summary>
        /// 星期
        /// </summary>
        public string WeekList { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string DateList { get; set; }
    }

    #endregion

    #region 酒店图片


    /// <summary>
    /// 酒店查询
    /// </summary>
    public class HotelPicSearchRequest : BaseRequest
    {
        /// <summary>酒店Id</summary>
        public int HotelId { get; set; }
    }

    #endregion



    /// <summary>
    /// 查询资源日志
    /// </summary>
    public class GetResourceLogRequest : BaseRequest
    {
        public int HotelId { get; set; }
        public int OutId { get; set; }
        public int TypeId { get; set; }

    }

    public class GetResourceLogResponse : H_ResourceLogModel
    {
        /// <summary> 外部类型 </summary>
        public string OutType { get; set; }
        /// <summary> 外部酒店Id </summary>
        public string OutId { get; set; }
        /// <summary> 酒店名称 </summary>
        public string HotelName { get; set; }
        /// <summary> 日期 </summary>
        public string PriceDate { get; set; }
        /// <summary> 价格 </summary>
        public string Price { get; set; }
        /// <summary> 库存 </summary>
        public string Stone { get; set; }
        public string TypeName { get; set; }
        public string AddDate { get { return RLAddTime.ToString("yyyy-MM-dd HH:mm:ss"); } }
    }
}
