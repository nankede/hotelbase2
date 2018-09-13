//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="h_resourcelogModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：h_resourcelog表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using HotelBase.Entity.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// h_resourcelog表实体类
    /// </summary>
    [Serializable, Table("H_ResourceLog")]
    public class H_ResourceLogModel
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
        /// 数据库字段：RLOutId
        /// </summary>
        private int _rLOutId = 0;

        /// <summary>
        /// 外部Id
        /// </summary>
        [Column("RLOutId")]
        public int RLOutId
        {
            get { return _rLOutId; }
            set { _rLOutId = value; }
        }

        /// <summary>
        /// 数据库字段：RLLogType
        /// </summary>
        private int _rLLogType = 0;

        /// <summary>
        /// 日志类型
        /// </summary>
        [Column("RLLogType")]
        public int RLLogType
        {
            get { return _rLLogType; }
            set { _rLLogType = value; }
        }

        /// <summary>
        /// 数据库字段：RLRemark
        /// </summary>
        private string _rLRemark = string.Empty;

        /// <summary>
        /// 操作内容
        /// </summary>
        [Column("RLRemark")]
        public string RLRemark
        {
            get { return _rLRemark; }
            set { _rLRemark = value; }
        }

        /// <summary>
        /// 数据库字段：RLAddId
        /// </summary>
        private int _rLAddId = 0;

        /// <summary>
        /// 新增人
        /// </summary>
        [Column("RLAddId")]
        public int RLAddId
        {
            get { return _rLAddId; }
            set { _rLAddId = value; }
        }

        /// <summary>
        /// 数据库字段：RLAddName
        /// </summary>
        private string _rLAddName = string.Empty;

        /// <summary>
        /// 新增人
        /// </summary>
        [Column("RLAddName")]
        public string RLAddName
        {
            get { return _rLAddName; }
            set { _rLAddName = value; }
        }

        /// <summary>
        /// 数据库字段：RLAddDepartId
        /// </summary>
        private int _rLAddDepartId = 0;

        /// <summary>
        /// 新增人部门
        /// </summary>
        [Column("RLAddDepartId")]
        public int RLAddDepartId
        {
            get { return _rLAddDepartId; }
            set { _rLAddDepartId = value; }
        }

        /// <summary>
        /// 数据库字段：RLAddDepartName
        /// </summary>
        private string _rLAddDepartName = string.Empty;

        /// <summary>
        /// 新增人部门
        /// </summary>
        [Column("RLAddDepartName")]
        public string RLAddDepartName
        {
            get { return _rLAddDepartName; }
            set { _rLAddDepartName = value; }
        }

        /// <summary>
        /// 数据库字段：RLAddTime
        /// </summary>
        private DateTime _rLAddTime = DateTime.Now;

        /// <summary>
        /// 新增时间
        /// </summary>
        [Column("RLAddTime")]
        public DateTime RLAddTime
        {
            get { return _rLAddTime; }
            set { _rLAddTime = value; }
        }

    }
}
