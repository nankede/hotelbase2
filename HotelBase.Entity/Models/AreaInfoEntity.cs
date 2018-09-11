using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Models
{

    public class AreaInfoModel
    {
        /// <summary>上级Id </summary>
        public int PId { get; set; }
        /// <summary> Id </summary>
        public int Id { get; set; }
        /// <summary> 名称 </summary>
        public string Name { get; set; }
        /// <summary>地区级别：0-洲，1-国，2-省，3-市，4-县 </summary>
        public int Type { get; set; }
    }
}
