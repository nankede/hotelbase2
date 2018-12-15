using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HotelBase.Common;
using HotelBase.Entity.Models;

namespace HotelBase.Web.Controllers
{
    /// <summary>
    /// 控制器基类。
    /// </summary>
    public class BaseController : System.Web.Mvc.Controller
    {
        public string loginCookie = "hotel_login";
        public UserModel CurrtUser
        {
            get
            {
                return OperatorProvider.Instance.Current;
            }
        }

        public BaseController()
        {

        }
    }

    /// <summary>
    /// 表示一个特性，该特性用于标识用户是否需要登陆。
    /// </summary>
    public class LoginCheckedAttribute : ActionFilterAttribute
    {

        public bool Ignore { get; set; }
        public LoginCheckedAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            OperatorProvider.Instance.Current = new UserModel
            {
                Id = 1,
                Name = "测试"
            };

            return;

            if (!Ignore)
            {
                return;
            }
            if (OperatorProvider.Instance.Current == null)
            {
                string fromUrl = filterContext.HttpContext.Request.Url.AbsolutePath;
                string loginUrl = $"/home/login?go={fromUrl}";

                filterContext.HttpContext.Response.Write($"<script>top.location.href = '{loginUrl}'</script>");
            }
        }
    }

    /// <summary>
    /// 表示一个特性，该特性用于捕获程序运行异常。
    /// </summary>
    public class ErrorCheckedAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //filterContext.HttpContext.Response.StatusCode = 500;
            //filterContext.ExceptionHandled = true;
            //StringBuilder script = new StringBuilder();
            //var msg = filterContext.Exception.Message + filterContext.Exception.StackTrace;
            //LogHelper.Error(msg);
            //script.Append("<script>top.alert('" + msg + "'); top.window.location.href='/Account/Login'</script>");
            //filterContext.Result = new ContentResult() { Content = script.ToString() };
            //if (OperatorProvider.Instance.Current == null)
            //{
            //    script.Append("<script>top.alert('登陆超时，请重新认证。'); top.window.location.href='/Account/Login'</script>");
            //    filterContext.Result = new ContentResult() { Content = script.ToString() };
            //}
            //else
            //{
            //    Operator onlineUser = OperatorProvider.Instance.Current;
            //    LogHelper.Write(Level.Error, filterContext.Exception.Message, filterContext.Exception.StackTrace, onlineUser.UserId, onlineUser.RealName);
            //    script.Append("<script>top.window.alert('系统出现异常，请联系开发人员确认。');</script>");
            //    filterContext.Result = new ContentResult() { Content = script.ToString() };
            //}
        }
    }
}

