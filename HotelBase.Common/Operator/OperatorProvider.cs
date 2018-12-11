using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBase.Common
{
    /// <summary>
    /// 用户登陆信息提供者。
    /// </summary>
    public class OperatorProvider
    {
        /// <summary>
        /// Session/Cookie键。
        /// </summary>
        private const string LOGIN_USER_KEY = "hotel_login";

        private OperatorProvider() { }

        static OperatorProvider() { }

        //使用内部类+静态构造函数实现延迟初始化。
        class Nested
        {
            static Nested() { }
            internal static readonly OperatorProvider instance = new OperatorProvider();
        }
        /// <summary>
        /// 在大多数情况下，静态初始化是在.NET中实现Singleton的首选方法。
        /// </summary>
        public static OperatorProvider Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// 从配置文件读取登陆提供者模式(Session/Cookie)。
        /// </summary>
        private string LoginProvider ="Cookie";

        /// <summary>
        /// 从配置文件读取登陆用户信息保存时间。
        /// </summary>
        private int LoginTimeout = Convert.ToInt32(Configs.GetValue("LoginTimeout"));

        /// <summary>
        /// 从Session/Cookie获取或设置用户操作模型。
        /// </summary>
        /// <returns></returns>
        public UserModel Current
        {
            get
            {
                UserModel operatorModel = new UserModel();
                if (LoginProvider == "Cookie")
                {
                    operatorModel = WebHelper.GetCookie(LOGIN_USER_KEY).DESDecrypt().ToObject<UserModel>();
                }
                else
                {
                    operatorModel = WebHelper.GetSession(LOGIN_USER_KEY).DESDecrypt().ToObject<UserModel>();
                }
                return operatorModel;
            }
            set
            {
                if (LoginProvider == "Cookie")
                {
                    WebHelper.SetCookie(LOGIN_USER_KEY, value.ToJson().DESEncrypt(), LoginTimeout);
                }
                else
                {
                    WebHelper.SetSession(LOGIN_USER_KEY, value.ToJson().DESEncrypt(), LoginTimeout);
                }
            }
        }

        /// <summary>
        /// 从Session/Cookie删除用户操作模型。
        /// </summary>
        public void Remove()
        {
            if (LoginProvider == "Cookie")
            {
                WebHelper.RemoveCookie(LOGIN_USER_KEY);
            }
            else
            {
                WebHelper.RemoveSession(LOGIN_USER_KEY);
            }
        }

    }

    public class UserModel
    {
        /// <summary> Id </summary>
        public int Id { get; set; }
        /// <summary> 登陆账户 </summary>
        public string Account { get; set; }
        /// <summary> 姓名 </summary>
        public string Name { get; set; }
        /// <summary> 部门Id </summary>
        public int DepartId { get; set; }
        /// <summary> 部门名称 </summary>
        public string DepartName { get; set; }
        /// <summary> 职责 </summary>
        public int R { get; set; }
        /// <summary> 职责 </summary>
        public string Responsibility { get; set; }
        /// <summary> 密码 </summary>
        public string Pwd { get; set; }
        /// <summary> 是否有效 </summary>
        public int IsValid { get; set; }
    }
}