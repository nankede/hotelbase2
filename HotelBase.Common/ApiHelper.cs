using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Common
{
    /// <summary>
    /// 调用API接口
    /// </summary>
    public static class ApiHelper
    {
        #region Get调用webapi
        /// <summary>
        /// Get调用webapi
        /// </summary>
        /// <param name="url">地址,例如http://172.28.20.19:8066/api/PublicAPI/CheckSetPassWord?applicationID=1&telephone=1</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        #endregion

        #region Post调用webapi
        /// <summary>
        /// Post调用webapi
        /// </summary>
        /// <param name="url">地址,地址栏参数同param,例如http://172.28.20.19:8066/api/PublicAPI/CheckSetPassWord?applicationID=1&telephone=1</param>
        /// <param name="param">参数,例如"{applicationID:\"1\",telephone:\"1\"}"</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param, string cntenttype = "")
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            //request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = !string.IsNullOrWhiteSpace(cntenttype) ? cntenttype : "application/json";
            //if (!string.IsNullOrWhiteSpace(cntenttype))
            //{
            //    request.ContentType = cntenttype;
            //}

            byte[] buffer = encoding.GetBytes(param);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Post调用webapi
        /// </summary>
        /// <param name="url">地址,地址栏参数同param,例如http://172.28.20.19:8066/api/PublicAPI/CheckSetPassWord?applicationID=1&telephone=1</param>
        /// <param name="param">参数,例如"{applicationID:\"1\",telephone:\"1\"}"</param>
        /// <returns></returns>
        public static T HttpPost<T>(string url, string param, string cntenttype = "")
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = !string.IsNullOrWhiteSpace(cntenttype) ? cntenttype : "application/json";
            request.ContentLength = 0;
            if (!string.IsNullOrEmpty(param))
            {
                byte[] buffer = encoding.GetBytes(param);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var rtnStr = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                rtnStr = reader.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(rtnStr))
            {
                return rtnStr.ToObject<T>();
            }
            return default(T);
        }

        #endregion

        #region 调用API接口
        /// <summary>
        /// 调用API接口
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="requestMethod">get/post</param>
        /// <param name="url">接口url</param>
        /// <param name="dic">参数</param>
        /// <returns></returns>
        public static T GetJsonResultByApi<T>(string requestMethod, string url, Dictionary<string, object> dic)
        {
            string jsonString = string.Empty;
            var jsonParams = "{";
            if (dic != null && dic.Count > 0)
            {
                url += "?";
            }
            foreach (KeyValuePair<string, object> kvp in dic)
            {
                object value;
                if (kvp.Value is String)
                    value = "\"" + kvp.Value + "\"";
                else
                    value = kvp.Value;
                url += (kvp.Key + "=" + kvp.Value + "&");
                jsonParams += (kvp.Key + ":" + value + ",");
            }
            url = url.TrimEnd('&');
            jsonParams = jsonParams.TrimEnd(',');
            jsonParams += "}";
            if (requestMethod.ToUpper() == "GET")
            {
                jsonString = HttpGet(url);
            }
            else if (requestMethod.ToUpper() == "POST")
            {
                jsonString = HttpPost(url, jsonParams);
            }
            var data = jsonString.ToObject<T>();
            return data;
        }
        #endregion
    }
}
