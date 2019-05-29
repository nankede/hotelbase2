//-----------------------------------------------------------------------
// <copyright company="AAAAA" file="h_resourcelogModel">
//    Copyright (c)  V1.0.1
//    作者：代码生成工具 自动生成
//    功能：h_resourcelog表实体
//-----------------------------------------------------------------------
using System;
using System.Text;
using Component.Access.MapAttribute;


namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// h_resourcelog表实体类
    /// </summary>
    [Serializable, Table("H_Relation")]
    public class H_RelationModel
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
        /// 数据库字段：DistributorId
        /// </summary>
        private string _distributorId = string.Empty;

        /// <summary>
        /// 外部Id
        /// </summary>
        [Column("DistributorId")]
        public string DistributorId
        {
            get { return _distributorId; }
            set { _distributorId = value; }
        }

        /// <summary>
        /// 数据库字段：SupplierId
        /// </summary>
        private string _supplierId = string.Empty;

        /// <summary>
        /// 外部Id
        /// </summary>
        [Column("SupplierId")]
        public string SupplierId
        {
            get { return _supplierId; }
            set { _supplierId = value; }
        }

        /// <summary>
        /// 数据库字段：RelationId
        /// </summary>
        private string _rlationId = string.Empty;

        /// <summary>
        /// 外部Id
        /// </summary>
        [Column("RelationId")]
        public string RelationId
        {
            get { return _rlationId; }
            set { _rlationId = value; }
        }
    }
}
