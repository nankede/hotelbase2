using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// 订单表
    /// </summary>
    public class HO_HotelOrder
    {
        /// <summary> 自增主键 </summary>
        public int Id { get; set; }
        /// <summary>酒店Id </summary>
        public int HIId { get; set; }
        /// <summary>房型Id </summary>
        public int HRId { get; set; }
        /// <summary> 政策Id</summary>
        public int HRRId { get; set; }
        /// <summary> 供应商Id</summary>
        public int HOSupplierId { get; set; }
        /// <summary>供应商来源 </summary>
        public int HOSupplierSourceId { get; set; }
        /// <summary>订单状态 </summary>
        public int HOStatus { get; set; }
        /// <summary>支付状态 </summary>
        public int HOPayStatus { get; set; }
        /// <summary> 房间数 </summary>
        public int HORoomCount { get; set; }
        /// <summary> 儿童数 </summary>
        public int HOChild { get; set; }
        /// <summary> 成人数 </summary>
        public int HOAdult { get; set; }
        /// <summary>来源渠道 </summary>
        public int HoPlat1 { get; set; }
        /// <summary>来源渠道 </summary>
        public int HoPlat2 { get; set; }
        /// <summary>新增人 </summary>
        public int HOAddId { get; set; }
        /// <summary> 新增部门</summary>
        public int HOAddDepartId { get; set; }
        /// <summary> 更新人</summary>
        public int HOUpdateId { get; set; }
        /// <summary>协议价 </summary>
        public decimal HOContractPrice { get; set; }
        /// <summary> 销售价</summary>
        public decimal HOSellPrice { get; set; }
        /// <summary>客户单号 </summary>
        public string HOCustomerSerialId { get; set; }
        /// <summary> 第三方订单号</summary>
        public string HOOutSerialId { get; set; }
        /// <summary> 供应商订单号</summary>
        public string HOSupplierSerialId { get; set; }
        /// <summary>入住人姓名 </summary>
        public string HOCustomerName { get; set; }
        /// <summary>入住人手机号 </summary>
        public string HOCustomerMobile { get; set; }
        /// <summary> 联系人姓名</summary>
        public string HOLinkerName { get; set; }
        /// <summary>联系人手机号 </summary>
        public string HOLinkerMobile { get; set; }
        /// <summary> 特殊要求</summary>
        public string HORemark { get; set; }
        /// <summary>新增人 </summary>
        public string HOAddName { get; set; }
        /// <summary>新增部门 </summary>
        public string HOAddDepartName { get; set; }
        /// <summary>更新人 </summary>
        public string HOUpdateName { get; set; }

        /// <summary>入住时间 </summary>
        public DateTime HOCheckInDate { get; set; }

        /// <summary>离店时间 </summary>
        public DateTime HOCheckOutDate { get; set; }
        /// <summary>最晚到店时间 </summary>
        public DateTime HOLastCheckInTime { get; set; }
        /// <summary> 新增时间</summary>
        public DateTime HOAddTime { get; set; }
        /// <summary>修改时间 </summary>
        public DateTime HOUpdateTime { get; set; }

    }
}
