using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kernel.Buildin.Service
{
    public static class AutofacExtensions
    {
        public static void AddBuildinAutofac(this ContainerBuilder builder, Type baseType, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => baseType.IsAssignableFrom(t) && t != baseType)
                .PropertiesAutowired()
                .EnableClassInterceptors(); // 允许在Controller类上使用拦截器

        }


    }
}
