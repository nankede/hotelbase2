using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Entity.Tables
{
    /// <summary>
    /// 国家地区数据
    /// </summary>
    public class Sys_AreaInfoModel
    {
        /// <summary>id</summary>
        public int id { get; set; } = 0;
        /// <summary>父级Id</summary>
        public int pid { get; set; } = 0;
        /// <summary>地区完整Id</summary>
        public string pid_path { get; set; } = string.Empty;
        /// <summary>地区完整名称</summary>
        public string p_cnname_path { get; set; } = string.Empty;
        /// <summary>地区名称</summary>
        public string name { get; set; } = string.Empty;
        /// <summary>英文名称</summary>
        public string en_name { get; set; } = string.Empty;
        /// <summary>英文简称</summary>
        public string short_en_name { get; set; } = string.Empty;
        /// <summary>地区级别：0-洲，1-国，2-省，3-市，4-县</summary>
        public int type { get; set; } = 0;
        /// <summary>中文简称</summary>
        public string shortCnName { get; set; } = string.Empty;
        /// <summary>首字母</summary>
        public string en_nprefixLetterame { get; set; } = string.Empty;
        /// <summary>全拼</summary>
        public string en_FullPinyinNamename { get; set; } = string.Empty;
    }
}
