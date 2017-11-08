using System;
using System.Reflection;

namespace MvvmMobile.Core.Binding
{
    public static class BindingHelper
    {
        public static object GetDeepPropertyValue(object instance, string path)
        {
            var pp = path.Split('.');
            var typeInfo = instance.GetType().GetTypeInfo();

            foreach(var prop in pp)
            {
                var propInfo = typeInfo.GetDeclaredProperty(prop);
                if (propInfo == null)
                {
                    throw new ArgumentException($"BindingHelper.GetDeepPropertyValue: Properties path '{path}' is not correct");
                }

                instance = propInfo.GetValue(instance, null);
                typeInfo = propInfo.PropertyType.GetTypeInfo();
            }

            return instance;
        }

        public static void SetDeepPropertyValue(object instance, string path, object value)
        {
            var pp = path.Split('.');
            var typeInfo = instance.GetType().GetTypeInfo();

            int i = 0;
            foreach(var prop in pp)
            {
                var propInfo = typeInfo.GetDeclaredProperty(prop);
                if (propInfo == null)
                {
                    throw new ArgumentException($"BindingHelper.SetDeepPropertyValue: Properties path '{path}' is not correct");
                }

                var newInstance = propInfo.GetValue(instance, null);
                if (i == pp.Length - 1)
                {
                    propInfo.SetValue(instance, value);
                    return;
                }

                instance = newInstance;
                typeInfo = propInfo.PropertyType.GetTypeInfo();
                i++;
            }
        }

        public static bool IsValueType(object obj) 
        {
            return obj != null && obj.GetType().GetTypeInfo().IsValueType;
        }
    }
}