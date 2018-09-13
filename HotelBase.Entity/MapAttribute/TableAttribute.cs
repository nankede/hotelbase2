using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBase.Entity.MapAttribute
{
    /// <summary>
    /// 标记实体表
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public TableAttribute(string tableName)
        {
            TableName = tableName;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; }
    }
}
