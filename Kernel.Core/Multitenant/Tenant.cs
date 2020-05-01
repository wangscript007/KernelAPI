using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Core.Multitenant
{
    public class TenantSettings
    {
        public List<Tenant> MultiTenant { get; set; } = new List<Tenant>();
    }

    /// <summary>
    /// 多租户模型类
    /// </summary>
    public class Tenant
    {
        
        /// <summary>
        /// 租户ID，GUID唯一标识
        /// 暴露到客户端多租户标识
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 多租户标签，用来做数据库路由
        /// 可以是数据库服务器IP，库名、用户名后缀
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 租户名称
        /// 租户信息标识作用，不参与业务运算
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
