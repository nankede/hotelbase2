using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HotelBase.Common;
using HotelBase.Service;
using HotelBase.Web.Controllers;

namespace HotelBase.Web.Controller.System
{
    public class SystemController : BaseController
    {
        public ActionResult User()
        {
            return View();
        }
    }
}
