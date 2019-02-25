using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Common
{
    public enum OrderStatusType
    {
        [Description("待确认")]
        LoadComform,
        [Description("确认成功")]
        ComformSuccess,
        [Description("确认失败")]
        ComformFail,
        [Description("取消")]
        Cancel
    }
}
