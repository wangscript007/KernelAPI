using Dapper;
using Kernel.Dapper.Repository;
using Kernel.IService.Repository.System;
using Kernel.Model.Core;
using Kernel.Model.System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Repository.System
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class SysUserRepository : BaseRepository<SysUser>, ISysUserRepository
    {
        public override string DBName => DapperConst.DB_MYSQL;

        public async Task<SysUser> GetSysUser_V1_0(string userID)
        {
            using (var conn = Connection)
            {
                return await conn.GetAsync<SysUser>(userID);
            }
        }

        public async Task<SysUserLogin> GetSysUserByLoginID_V1_0(string loginID)
        {
            using (var conn = Connection)
            {
                var result = await conn.GetListAsync<SysUserLogin>("where LoginID = @LoginID", new { LoginID = loginID });
                return result.FirstOrDefault();
            }
        }

        public async Task AddSysUser_V1_0(SysUser sysUser)
        {
            using (var conn = Connection)
            {
                await conn.InsertAsync<string, SysUser>(sysUser);
            }
        }


        public async Task<LayuiTable<SysUserListRecord>> GetSysUserList_V1_0(SysUserListIn model)
        {
            using (var conn = Connection)
            {
                var result = new LayuiTable<SysUserListRecord>();
                result.Data = await conn.GetListPagedAsync<SysUserListRecord>(model.Page, model.Limit, "", "UpdateTime desc");
                result.Count = await conn.RecordCountAsync<SysUserListRecord>("");
                return result;
            }
        }


        public async Task<int> DeleteSysUser_V1_0(string[] userIDs)
        {
            using (var conn = Connection)
            {
                return await conn.DeleteListAsync<SysUser>("WHERE UserID IN @UserID", new { UserID = userIDs });
            }
        }

    }
}
