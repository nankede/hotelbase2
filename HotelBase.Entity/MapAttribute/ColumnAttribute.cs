using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBase.Entity.MapAttribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        public ColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ColumnName { get; }
    }
}
