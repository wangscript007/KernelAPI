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
    public class SysMenuPermRepository : BaseRepository<SysMenuPerm>, ISysMenuPermRepository
    {
        public override string DBName => DapperConst.DB_MYSQL;

        //保存菜单权限
        public async Task<bool> SaveSysMenuPerm_V1_0(string roleID, IEnumerable<SysMenuPerm> models)
        {
            using (var conn = Connection)
            {
                var trans = conn.BeginTransaction();

                //先删除后添加
                conn.DeleteList<SysMenuPerm>("where RoleID in @RoleID", new { RoleID = roleID }, trans);
                foreach (var model in models)
                {
                    await conn.InsertAsync<string, SysMenuPerm>(model, trans);
                }

                trans.Commit();
            }

            return true;
        }

    }
}
