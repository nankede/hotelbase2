using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HotelBase.Common;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using HotelBase.Service;
using HotelBase.Web.Controllers;

namespace HotelBase.Web.Controller.System
{
    public class SupplierController : BaseController
    {
        #region 供应商

        /// <summary>
        /// 供应商列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }

        #endregion
    }
}
