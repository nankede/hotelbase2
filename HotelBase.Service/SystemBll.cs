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
        public static BasePageResponse<UserModel> GetUserList(UserListRequest request)
        {
            var response = Sys_UserInfoAccess.GetUserList(request);
            return response;
        }
        public static UserModelResponse GetUserModel(int id, string account)
        {
            var model = Sys_UserInfoAccess.GetUserModel(id, account);
            var response = new UserModelResponse
            {
                IsSuccess = model?.Id > 0 ? 1 : 0,
                Model = model
            };
            return response;
        }
    }
}
