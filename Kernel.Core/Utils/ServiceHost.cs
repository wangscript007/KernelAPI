using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Kernel.Core.Utils
{
    public class ServiceHost
    {
        public static ILifetimeScope Container { get; set; }
        private static IHttpContextAccessor _accessor;

        public static void Init(IServiceProvider serviceProvider)
        {
            Container = serviceProvider.GetAutofacRoot();
            _accessor = GetService<IHttpContextAccessor>();
        }

        /// <summary>
        /// 获取服务(Single)
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : class
        {
            return Container.Resolve<T>();
        }

        public static IEnumerable<T> GetServices<T>() where T : class
        {
            return Container.Resolve<IEnumerable<T>>();
        }

        /// <summary>
        /// 获取服务(请求生命周期内)
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetScopeService<T>() where T : class
        {
            return _accessor.HttpContext.RequestServices.GetService<T>();
        }

        public static IEnumerable<T> GetScopeServices<T>() where T : class
        {
            return _accessor.HttpContext.RequestServices.GetServices<T>();
        }

    }
}
