using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Common
{
    public class CommonHelper
    {
        public static T CheckPropertiesNull<T>(T model)
        {
            Type type = typeof(T);
            object obj = Activator.CreateInstance(type);

            foreach (var pi in type.GetProperties())
            {
                object v = pi.GetValue(model, new object[] { });

                //输出值为null的属性名称
                if (v == null)
                {
                    pi.SetValue(model, pi.GetValue(obj, new object[] { }));
                }
            }

            return model;
        }

    }
}
