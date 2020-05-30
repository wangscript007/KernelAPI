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

        public async Task<IEnumerable<SysFuncPermItem>> GetSysFuncPermList_V1_0(string roleID)
        {
            using (var conn = Connection)
            {
                return await conn.QueryAsync<SysFuncPermItem>(@"select a.*, if(b.FuncID is null, 0, 1) HavePerm from sysfunc a left join sysfuncperm b 
on a.ModID = b.ModID and a.FuncID = b.FuncID and b.RoleID = @RoleID order by a.SortKey", new { RoleID = roleID });
            }
        }

        //保存功能权限
        public async Task<bool> SaveSysFuncPerm_V1_0(string roleID, IEnumerable<SysFuncPerm> models)
        {
            using (var conn = Connection)
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                //先删除后添加
                conn.DeleteList<SysFuncPerm>("where RoleID = @RoleID", new { RoleID = roleID }, trans);
                foreach (var model in models)
                {
                    model.RoleID = roleID;
                    model.CreateBy = KernelApp.Request.UserID;
                    model.CreateTime = DateTime.Now;
                    await conn.InsertAsync<string, SysFuncPerm>(model, trans);
                }

                trans.Commit();
            }

            return true;
        }

        //查询功能权限
        public async Task<IEnumerable<SysModuleFuncPerm>> GetModuleFuncPerm_V1_0(string modID)
        {
            using (var conn = Connection)
            {
                return await conn.QueryAsync<SysModuleFuncPerm>("select distinct a.*, if(b.FuncID is null, 0, 1) HavePerm from SysFunc a left join SysFuncPerm b on a.FuncID = b.FuncID and b.RoleID in @RoleID and b.ModID = @ModID where a.ModID = @ModID", new { RoleID = KernelApp.Request.RoleIDs, ModID = modID});
            }
        }


    }
}
