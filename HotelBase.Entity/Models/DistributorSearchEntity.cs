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
}
