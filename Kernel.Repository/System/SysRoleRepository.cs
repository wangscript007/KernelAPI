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
    public class SysRoleRepository : BaseRepository<SysRole>, ISysRoleRepository
    {
        public override string DBName => DapperConst.DB_MYSQL;

        public async Task<IEnumerable<string>> GetUserRoles_V1_0(string userID)
        {
            using (var conn = Connection)
            {
                return await conn.QueryAsync<string>("select RoleID from sysrolerelation where UserID = @UserID", new { UserID = userID});
            }
        }

    }
}
