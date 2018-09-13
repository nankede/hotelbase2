using System;

namespace HotelBase.Entity.MapAttribute
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class KeyAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public KeyAttribute(KeyType type)
        {
            KeyType = type;
        }

        /// <summary>
        /// 
        /// </summary>
        public KeyType KeyType { get; }
    }
}
