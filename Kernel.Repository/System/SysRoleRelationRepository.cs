using Dapper;
using Kernel.Dapper.Repository;
using Kernel.IService.Repository.System;
using Kernel.Model.Core;
using Kernel.Model.System;
using System.Threading.Tasks;

namespace Kernel.Repository.System
{
    public class SysRoleRelationRepository : BaseRepository<SysRoleRelation>, ISysRoleRelationRepository
    {
        public override string DBName => DapperConst.DB_MYSQL;

        public async Task<SysRoleRelationSet> GetSysRoleRelation_V1_0(string userID)
        {
            using (var conn = Connection)
            {
                var result = new SysRoleRelationSet();
                result.AllRoles = await conn.QueryAsync<SysRoleRelationItem>("select a.RoleID, a.RoleName, if(a.DictIsActive = '1', 0, 1) Disabled from sysrole a", new { });
                result.UserRoles = await conn.QueryAsync<string>("select RoleID from sysrolerelation where UserID = @UserID", new { UserID = userID });

                return result;
            }
        }

        public async Task<bool> SaveSysRoleRelation_V1_0(SysRoleRelationSaveIn model)
        {
            using (var conn = Connection)
            {
                if (model.SaveType == 0)
                {
                    //添加
                    foreach (var roleID in model.RoleIDs)
                    {
                        var relation = new SysRoleRelation();
                        relation.UserID = model.UserID;
                        relation.RoleID = roleID;
                        await conn.InsertAsync<string, SysRoleRelation>(relation);
                    }
                }
                else if (model.SaveType == 1)
                {
                    //删除
                    await conn.ExecuteAsync("delete from SysRoleRelation where UserID = @UserID and RoleID in @RoleID", new { UserID = model.UserID, RoleID = model.RoleIDs });
                }

                return true;
            }
        }

    }
}
