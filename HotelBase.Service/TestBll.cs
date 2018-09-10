using HotelBase.DataAccess;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Service
{
    public class TestBll
    {
        public static List<Sys_UserInfoModel> GetUser()
        {

            var list = Class1.GetUser();
            return list;
        }
    }
}
