//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="ho_hotelorderlogModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：ho_hotelorderlog表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using HotelBase.Entity.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// ho_hotelorderlog表实体类
    /// </summary>
    [Serializable, Table("HO_HotelOrderLog")]
    public class HO_HotelOrderLogModel
    {
        /// <summary>
        /// 数据库字段：Id
        /// </summary>
        private int _id = 0;

        /// <summary>
        /// 自增主键
        /// </summary>
        [Key(KeyType.Identity)]
        [Column("Id")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 数据库字段：HOLOrderId
        /// </summary>
        private int _hOLOrderId = 0;

        /// <summary>
        /// 外部Id
        /// </summary>
        [Column("HOLOrderId")]
        public int HOLOrderId
        {
            get { return _hOLOrderId; }
            set { _hOLOrderId = value; }
        }

        /// <summary>
        /// 数据库字段：HOLLogType
        /// </summary>
        private int _hOLLogType = 0;

        /// <summary>
        /// 日志类型
        /// </summary>
        [Column("HOLLogType")]
        public int HOLLogType
        {
            get { return _hOLLogType; }
            set { _hOLLogType = value; }
        }

        /// <summary>
        /// 数据库字段：HOLRemark
        /// </summary>
        private string _hOLRemark = string.Empty;

        /// <summary>
        /// 操作内容
        /// </summary>
        [Column("HOLRemark")]
        public string HOLRemark
        {
            get { return _hOLRemark; }
            set { _hOLRemark = value; }
        }

        /// <summary>
        /// 数据库字段：HOLAddId
        /// </summary>
        private int _hOLAddId = 0;

        /// <summary>
        /// 新增人
        /// </summary>
        [Column("HOLAddId")]
        public int HOLAddId
        {
            get { return _hOLAddId; }
            set { _hOLAddId = value; }
        }

        /// <summary>
        /// 数据库字段：HOLAddName
        /// </summary>
        private string _hOLAddName = string.Empty;

        /// <summary>
        /// 新增人
        /// </summary>
        [Column("HOLAddName")]
        public string HOLAddName
        {
            get { return _hOLAddName; }
            set { _hOLAddName = value; }
        }

        /// <summary>
        /// 数据库字段：HOLAddDepartId
        /// </summary>
        private int _hOLAddDepartId = 0;

        /// <summary>
        /// 新增人部门
        /// </summary>
        [Column("HOLAddDepartId")]
        public int HOLAddDepartId
        {
            get { return _hOLAddDepartId; }
            set { _hOLAddDepartId = value; }
        }

        /// <summary>
        /// 数据库字段：HOLAddDepartName
        /// </summary>
        private string _hOLAddDepartName = string.Empty;

        /// <summary>
        /// 新增人部门
        /// </summary>
        [Column("HOLAddDepartName")]
        public string HOLAddDepartName
        {
            get { return _hOLAddDepartName; }
            set { _hOLAddDepartName = value; }
        }

        /// <summary>
        /// 数据库字段：HOLAddTime
        /// </summary>
        private DateTime _hOLAddTime = DateTime.Now;

        /// <summary>
        /// 新增时间
        /// </summary>
        [Column("HOLAddTime")]
        public DateTime HOLAddTime
        {
            get { return _hOLAddTime; }
            set { _hOLAddTime = value; }
        }

    }
}
