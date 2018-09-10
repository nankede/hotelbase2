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
            return View();
        }

        public ActionResult Default()
        {
            return View();
        }
    }
}