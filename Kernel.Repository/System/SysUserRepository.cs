using Dapper;
using Kernel.Dapper.Repository;
using Kernel.IService.Repository.System;
using Kernel.Model.Core;
using Kernel.Model.System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kernel.Core.Extensions;
using Kernel.Dapper.ORM;

namespace Kernel.Repository.System
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class SysUserRepository : BaseRepository<SysUser>, ISysUserRepository
    {
        public override string DBName => DapperConst.DB_MYSQL;
        public ISysDictItemRepository SysDictItemRepository { get; set; }

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

        public async Task UpdateSysUser_V1_0(SysUser sysUser)
        {
            using (var conn = Connection)
            {
                await conn.UpdateAsync<SysUser>(sysUser);
            }
        }


        public async Task<LayuiTableResult<SysUserListRecord>> GetSysUserList_V1_0(SysUserListIn model)
        {
            DynamicBuilder builder = new DynamicBuilder();
            builder.Build(model, null, (columnName, columnValue) =>
            {
                if (columnName == "UserName")
                {
                    builder.Conditions.Append(string.Format(" AND {0} LIKE @{1} ", columnName, columnName));
                    builder.Parameters.Add(columnName, "%" + columnValue + "%");
                }
                else if (columnName == "LoginID")
                {
                    builder.Conditions.Append(string.Format(" AND {0} LIKE @{1} ", columnName, columnName));
                    builder.Parameters.Add(columnName, "%" + columnValue + "%");
                }
            });

            using (var conn = Connection)
            {
                var result = new LayuiTableResult<SysUserListRecord>();
                result.Data = await conn.GetListPagedAsync<SysUserListRecord>(model.Page, model.Limit, builder.Conditions.ToString(), "UpdateTime desc", builder.Parameters);
                var dictIsActive = await SysDictItemRepository.GetSysDict_V1_0("DictIsActive");
                foreach (var item in result.Data)
                {
                    item.IsActiveName = dictIsActive.GetValue(item.DictIsActive);
                }
                result.Count = await conn.RecordCountAsync<SysUserListRecord>(builder.Conditions.ToString(), builder.Parameters);
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

        public async Task<int> UpdatePwd_V1_0(SysUser model)
        {
            using (var conn = Connection)
            {
                return await conn.ExecuteAsync("update SysUser set LoginPwd = @LoginPwd where UserID = @UserID", new { LoginPwd = model.LoginPwd, UserID = model.UserID });
            }
        }

    }
}
