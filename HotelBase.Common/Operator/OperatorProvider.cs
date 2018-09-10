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
        private const string LOGIN_USER_KEY = "LoginUser";

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
        private string LoginProvider = Configs.GetValue("LoginProvider");

        /// <summary>
        /// 从配置文件读取登陆用户信息保存时间。
        /// </summary>
        private int LoginTimeout = Convert.ToInt32(Configs.GetValue("LoginTimeout"));

        /// <summary>
        /// 从Session/Cookie获取或设置用户操作模型。
        /// </summary>
        /// <returns></returns>
        public Operator Current
        {
            get
            {
                Operator operatorModel = new Operator();
                if (LoginProvider == "Cookie")
                {
                    operatorModel = WebHelper.GetCookie(LOGIN_USER_KEY).DESDecrypt().ToObject<Operator>();
                }
                else
                {
                    operatorModel = WebHelper.GetSession(LOGIN_USER_KEY).DESDecrypt().ToObject<Operator>();
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

    /// <summary>
    /// 操作模型，保存登陆用户必要信息。
    /// </summary>
    public class Operator
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public List<string> RoleId { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginCount { get; set; }
        /// <summary>
        /// 消息数
        /// </summary>
        public int MessageCount { get; set; }
    }
}