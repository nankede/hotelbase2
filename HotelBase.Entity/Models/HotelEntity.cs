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

    #region 酒店房型

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
}
