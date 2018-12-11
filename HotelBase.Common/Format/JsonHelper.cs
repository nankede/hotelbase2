using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace HotelBase.Common
{

    /// <summary>
    /// JSON序列化、反序列化扩展类。
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 将JOSN字符串转成T对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json)
        {
            try
            {
                return string.IsNullOrEmpty(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                var msg = $"ToObject发生异常，Json：{json}，异常内容：{ex.ToString()}";
                LogHelper.Error(msg);
                return default(T);
            }
        }

        /// <summary>
        /// 将对象转成JSON字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                var msg = $"ToJson发生异常，异常内容：{ex.ToString()}";
                LogHelper.Error(msg);
                return null;
            }
        }
    }
}
