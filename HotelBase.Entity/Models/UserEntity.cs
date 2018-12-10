using HotelBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Models
{
    public class UserListRequest : BaseRequest
    {

    }

    public class DepartistRequest : BaseRequest
    {

    }

    public class UserModelResponse : BaseResponse
    {
        public UserModel Model { get; set; }
    }
}
