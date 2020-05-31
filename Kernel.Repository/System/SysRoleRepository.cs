using Dapper;
using Kernel.Core.Extensions;
using Kernel.Dapper.ORM;
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

        public ISysDictItemRepository SysDictItemRepository { get; set; }


        public async Task<IEnumerable<string>> GetUserRoles_V1_0(string userID)
        {
            using (var conn = Connection)
            {
                return await conn.QueryAsync<string>("select RoleID from sysrolerelation where UserID = @UserID", new { UserID = userID});
            }
        }

        public async Task<SysRole> GetSysRole_V1_0(string roleID)
        {
            using (var conn = Connection)
            {
                return await conn.GetAsync<SysRole>(roleID);
            }
        }

        public async Task<int> DeleteSysRole_V1_0(string[] roleIDs)
        {
            using (var conn = Connection)
            {
                return await conn.DeleteListAsync<SysRole>("WHERE RoleID IN @RoleID", new { RoleID = roleIDs });
            }
        }



        public async Task AddSysRole_V1_0(SysRole model)
        {
            using (var conn = Connection)
            {
                await conn.InsertAsync<string, SysRole>(model);
            }
        }

        public async Task UpdateSysRole_V1_0(SysRole model)
        {
            using (var conn = Connection)
            {
                await conn.UpdateAsync<SysRole>(model);
            }
        }


        public async Task<LayuiTableResult<SysRoleListRecord>> GetSysRoleList_V1_0(SysRoleListIn model)
        {
            DynamicBuilder builder = new DynamicBuilder();
            builder.Build(model, null, (columnName, columnValue) =>
            {
                if (columnName == "RoleName")
                {
                    builder.Conditions.Append(string.Format(" AND {0} LIKE @{1} ", columnName, columnName));
                    builder.Parameters.Add(columnName, "%" + columnValue + "%");
                }
            });

            using (var conn = Connection)
            {
                var result = new LayuiTableResult<SysRoleListRecord>();
                result.Data = await conn.GetListPagedAsync<SysRoleListRecord>(model.Page, model.Limit, builder.Conditions.ToString(), "UpdateTime desc", builder.Parameters);
                var dictIsActive = await SysDictItemRepository.GetSysDict_V1_0("DictIsActive");
                foreach (var item in result.Data)
                {
                    item.IsActiveName = dictIsActive.GetValue(item.DictIsActive);
                }
                result.Count = await conn.RecordCountAsync<SysRoleListRecord>(builder.Conditions.ToString(), builder.Parameters);
                return result;
            }
        }
    }
}
