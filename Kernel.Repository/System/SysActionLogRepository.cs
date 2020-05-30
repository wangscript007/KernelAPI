using Dapper;
using Kernel.Core.AOP;
using Kernel.Dapper.Repository;
using Kernel.IService.Repository.System;
using Kernel.Model.Core;
using Kernel.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Repository.System
{
    public class SysActionLogRepository : BaseRepository<SysActionLog>, ISysActionLogRepository, IActionLogHandle
    {
        public override string DBName => DapperConst.DB_MYSQL;

        public async Task AddSysActionLog_V1_0(SysActionLog model)
        {
            using (var conn = Connection)
            {
                await conn.InsertAsync<string, SysActionLog>(model);
            }
        }

        public async Task WriteActionLog(string apiName, double elapsed)
        {
            SysActionLog model = new SysActionLog
            {
                RemoteIp = KernelApp.Request.RemoteIpAddress,
                CreateBy = KernelApp.Request.UserID,
                ApiName = apiName,
                Elapsed = elapsed,
                CreateTime = DateTime.Now
            };
            await AddSysActionLog_V1_0(model);
        }
    }
}
