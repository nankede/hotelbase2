using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Models
{
    public class GetDicListRequest : BaseRequest
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// 字典
    /// </summary>
    public class BaseDic
    {
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
