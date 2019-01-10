using Component.Access.MapAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Tables
{
    [Serializable, Table("H_DistributorInfo")]
    public class H_DistributorInfoModel
    {

        /// <summary>
        /// 自增主键
        /// </summary>
        [Key(KeyType.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// 分销商名称
        /// </summary>
        public string DName { get; set; }
        /// <summary>
        /// 省份Id
        /// </summary>
        public int DProviceId { get; set; } = 0;
        /// <summary>
        /// 省份
        /// </summary>
        public string DProviceName { get; set; }
        /// <summary>
        /// 城市Id
        /// </summary>
        public int DCityId { get; set; } = 0;
        /// <summary>
        /// 城市
        /// </summary>
        public string DCityName { get; set; }
        /// <summary>
        /// 一级渠道
        /// </summary>
        public int DPart1 { get; set; } = 0;
        /// <summary>
        /// 一级渠道名称
        /// </summary>
        public string DPart1Name { get; set; }
        /// <summary>
        /// 二级渠道
        /// </summary>
        public int DPart2 { get; set; } = 0;
        /// <summary>
        /// 二级渠道名称
        /// </summary>
        public string DPart2Name { get; set; }
        /// <summary>
        /// 合作模式
        /// </summary>
        public int DCooperationMode { get; set; } = 0;
        /// <summary>
        /// 合作模式名称
        /// </summary>
        public string DCooperationModeName { get; set; }
        /// <summary>
        /// 结算价
        /// </summary>
        public int DSettlement { get; set; } = 0;
        /// <summary>
        /// 结算价名称
        /// </summary>
        public string DSettlementName { get; set; }
        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime DStartDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime DEndDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 签约主体
        /// </summary>
        public int DSignType { get; set; } = 0;
        /// <summary>
        /// 签约主体名称
        /// </summary>
        public string DSignTypeName { get; set; }
        /// <summary>
        /// 结算方式
        /// </summary>
        public int DSettlementType { get; set; } = 0;
        /// <summary>
        /// 结算方式名称
        /// </summary>
        public string DSettlementTypeName { get; set; }
        /// <summary>
        /// 预付款金额
        /// </summary>
        public decimal DAdvanceMoney { get; set; } = 0.0000m;
        /// <summary>
        /// 加价/返佣比例
        /// </summary>
        public int DCommissionPoint { get; set; } = 0;
        /// <summary>
        /// 开票抬头
        /// </summary>
        public string DInvoiceTitle { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        public string DTaxNumbe { get; set; }
        /// <summary>
        /// 开票项目
        /// </summary>
        public string DInvoiceProject { get; set; }
        /// <summary>
        /// 发票类型
        /// </summary>
        public string DBillType { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string DLinkMan { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string DLinkMobile { get; set; }
        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public string DLinkEmail { get; set; }
        /// <summary>
        /// 联系人地址
        /// </summary>
        public string DLineAddress { get; set; }
        /// <summary>
        /// 上线渠道
        /// </summary>
        public string DLineChannel { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string DRemark { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public int DIsValid { get; set; } = 0;
        /// <summary>
        /// 新增人
        /// </summary>
        public string DAddName { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string DUpdateName { get; set; }
        /// <summary>
        /// 新增时间
        /// </summary>
        public DateTime DAddTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime DUpdateTime { get; set; } = DateTime.Now;
    }
}
