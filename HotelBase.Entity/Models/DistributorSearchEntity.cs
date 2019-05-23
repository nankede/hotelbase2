using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Models
{
    public class DistributorSearchEntity
    {

    }

    public class DistributorSearchRequest : BaseRequest
    {
        /// <summary>
        /// 分销商
        /// </summary>
        public string DName { get; set; }

        /// <summary>
        /// 分销商id
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// 省份id
        /// </summary>
        public int DProviceId { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public int DCityId { get; set; }

        /// <summary>
        /// 合作模式
        /// </summary>
        public int DCooperationMode { get; set; }

        /// <summary>
        /// 有效性
        /// </summary>
        public int DIsValid { get; set; }
    }


    public class DistributorSearchResponse : BaseResponse
    {
        /// <summary>
        /// 分销商信息
        /// </summary>
        public string DName { get; set; }

        /// <summary>
        /// 分销商id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 合作信息
        /// </summary>
        public string DCooperationModeName { get; set; }

        /// <summary>
        /// 预付款金额
        /// </summary>
        public string DAdvanceMoney { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string DLinkMan { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public int DIsValid { get; set; }
    }


    public class GiveResourceSearchRequest : BaseRequest
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public string HotelId { get; set; }

        /// <summary>
        /// 供应商id
        /// </summary>
        public string SupplierId { get; set; }


        /// <summary>
        /// 省份id
        /// </summary>
        public int ProviceId { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 类型（0：未分配资源  1：已分配资源）
        /// </summary>
        public int Type { get; set; }
    }

    public class GiveResourceSearchResponse
    {
        /// <summary>
        /// 酒店Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 酒店
        /// </summary>
        public int HIName { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string HIProvince { get; set; }


        /// <summary>
        /// 城市
        /// </summary>
        public string HICity { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string HIAddress { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string HILinkPhone { get; set; }

        /// <summary>
        /// 经纬度
        /// </summary>
        public string HIGdLonLat { get; set; }
    }
}
