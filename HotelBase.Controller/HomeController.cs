using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HotelBase.Common;
using HotelBase.Service;

namespace HotelBase.Web.Controllers
{
    /// <summary>
    /// 控制器基类。
    /// </summary>
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.UserName = CurrtUser.Name;
            ViewBag.DepartName = CurrtUser.DepartName;
            ViewBag.UserId = CurrtUser.Id;
            return View();
        }

        /// <summary>
        /// 工作台
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkList()
        {
            return View();
        }


        //登陆
        public ActionResult Login(string go = "")
        {
            ViewBag.GoUrl = string.IsNullOrEmpty(go) ? "/" : go;
            return View();
        }



        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonResult GetLogin(string account, string pwd, string code)
        {
            var isLogin = false;
            var Message = string.Empty;
            var user = SystemBll.Login(account, pwd);
            if (user != null && user.Id > 0)
            {
                isLogin = true;
                CurrtUser = user;
                if (CurrtUser != null && CurrtUser.Id > 0)
                {
                    CurrtUser.DepartName = HttpUtility.UrlEncode(CurrtUser.DepartName);
                    CurrtUser.Name = HttpUtility.UrlEncode(CurrtUser.Name);
                    CurrtUser.Responsibility = HttpUtility.UrlEncode(CurrtUser.Responsibility);
                    CookieHelpers.Add(loginCookie, CurrtUser.ToJson(), DateTime.Now.AddDays(1));
                }
            }
            else
            {
                Message = "用户名或密码错误！";
            }

            return Json(new { isLogin, Message });
        }


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Logout()
        {
            CookieHelpers.Delete(loginCookie);
            return Redirect("/home/login/");

        }

    }
}