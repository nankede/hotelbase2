using HotelBase.DataAccess;
using HotelBase.Entity;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Service
{
    public class SystemBll
    {
        public static BasePageResponse<UserListResponse> GetUserList(UserListRequest request)
        {
            var response = Sys_UserInfoAccess.GetUser(request);
            return response;
        }
    }
}
