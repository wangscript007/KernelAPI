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
    public class SysModuleRepository : BaseRepository<SysModule>, ISysModuleRepository
    {
        public override string DBName => DapperConst.DB_MYSQL;

        public async Task<IEnumerable<T>> GetSysModuleList_V1_0<T>(params string[] modType)
        {
            using (var conn = Connection)
            {
                return await conn.GetListAsync<T>("where IsEnabled = '1' and ModType in @ModType order by SortKey", new { ModType = modType });
            }
        }


    }
}
