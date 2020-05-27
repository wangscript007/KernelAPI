using Dapper;
using Kernel.Dapper.Repository;
using Kernel.IService.Repository.System;
using Kernel.Model.Core;
using Kernel.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Repository.System
{
    public class SysModuleRepository : BaseRepository<SysModule>, ISysModuleRepository
    {
        public override string DBName => DapperConst.DB_MYSQL;

        //查询有权限的菜单
        public async Task<IEnumerable<T>> GetSysModuleList_V1_0<T>(params string[] modType)
        {
            using (var conn = Connection)
            {
                return await conn.GetListAsync<T>("where IsEnabled = '1' and ModType in @ModType order by SortKey", new { ModType = modType });
            }
        }

        //查询全部菜单
        public async Task<IEnumerable<T>> GetMenuPermList_V1_0<T>(string roleID, params string[] modTypes)
        {
            using (var conn = Connection)
            {
                return await conn.QueryAsync<T>("select a.*, if(b.ModID is null, 0, 1) havePerm from SysModule a left join SysMenuPerm b on b.RoleID = @RoleID and a.ModID = b.ModID where  a.ModType in @ModType order by a.SortKey", new { RoleID = roleID, ModType = modTypes });
            }
        }


    }
}
