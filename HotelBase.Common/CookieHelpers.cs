using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HotelBase.Common
{
    /// <summary>
    /// Cookie操作类
    /// </summary>
    public class CookieHelpers
    {
        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        public static void Delete(string cookiename)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookiename];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                cookie.Value = string.Empty;
                HttpContext.Current.Response.AppendCookie(cookie);
                //  HttpContext.Current.Request.Cookies.Remove(cookiename);
            }
        }

        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        /// <returns></returns>
        public static string Get(string cookiename)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookiename];
            return cookie?.Value;
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="cookiename">cookie名</param>
        /// <param name="cookievalue">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void Add(string cookiename, string cookievalue, DateTime expires, string domain = "")
        {
            if (HttpContext.Current.Response != null && HttpContext.Current.Response.Cookies != null)
            {
                var cookie = new HttpCookie(cookiename)
                {
                    Expires = expires,
                    Domain = domain,
                    Value = cookievalue
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}
