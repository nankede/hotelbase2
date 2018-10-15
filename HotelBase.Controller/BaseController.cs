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
        private UserModel _CurrtUser;
        public UserModel CurrtUser
        {
            get
            {
                return new UserModel
                {
                    Id = 1,
                    Name = "测试",
                    DepartId = 1,
                    DepartName = "测试"

                };
            }
            set { _CurrtUser = value; }
        }
        public BaseController()
        {

        }

    }

    /// <summary>
    /// 表示一个特性，该特性用于标识用户是否有访问权限。
    /// </summary>
    public class AuthorizeCheckedAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 是否忽略权限检查。
        /// </summary>
        public bool Ignore { get; set; }

        public AuthorizeCheckedAttribute(bool ignore = false)
        {
            this.Ignore = ignore;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore)
            {
                return;
            };
            var userId = OperatorProvider.Instance.Current.UserId;
            var action = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            var title = string.Empty;
            //bool hasPermission = PermissionService.ActionValidate(userId, action, out title);
            //if (!hasPermission)
            //{
            //    StringBuilder script = new StringBuilder();
            //    script.Append("<script>alert('对不起，您没有权限访问当前页面。');</script>");
            //    filterContext.Result = new ContentResult() { Content = script.ToString() };
            //}
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
            if (!Ignore)
            {
                return;
            }
            //if (OperatorProvider.Instance.Current == null)
            //{
            //    filterContext.HttpContext.Response.Write("<script>top.location.href = '/Account/Login'</script>");
            //}
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

