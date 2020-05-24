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
    public class ApiResourceRepository : BaseRepository<ApiResource>, IApiResourceRepository
    {
        public override string DBName => DapperConst.DB_MYSQL;

        public async Task<bool> HasApiPerm_V1_0(string userID, string navUrl, string resPath)
        {
            using (var conn = Connection)
            {
                var result = await conn.QueryFirstOrDefaultAsync<int>("select 1 from v_perm where userID = @userID and navUrl = @navUrl and resPath = @resPath", new { userID, navUrl, resPath });
                return result > 0;
            }
        }

    }
}
