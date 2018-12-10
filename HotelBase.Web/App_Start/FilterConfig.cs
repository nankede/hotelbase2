
using HotelBase.Web.Controllers;
using System.Web;
using System.Web.Mvc;

namespace HotelBase.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorCheckedAttribute());
            filters.Add(new LoginCheckedAttribute());//登录验证
        }
    }
}