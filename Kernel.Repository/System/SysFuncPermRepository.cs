using Dapper;
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
    public class SysFuncPermRepository : BaseRepository<SysFuncPerm>, ISysFuncPermRepository
    {
        public override string DBName => DapperConst.DB_MYSQL;

        public async Task<bool> HasApiPerm_V1_0(string apiName)
        {
            using (var conn = Connection)
            {
                return await conn.QueryFirstOrDefaultAsync<int>("select 1 from SysFuncPerm where RoleID in @RoleID and ApiName = @ApiName", new { RoleID = KernelApp.Request.RoleIDs, apiName }) > 0;
            }
        }

    }
}
