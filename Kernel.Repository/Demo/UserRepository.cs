using Dapper;
using Kernel.Dapper.Repository;
using Kernel.IService.Repository.Demo;
using Kernel.Model.Demo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kernel.Repository.Demo
{
    public class UserRepository : BaseRepository<SysUser>, IUserRepository
    {
        public async Task<SysUser> GetUserInfo_V1_0(SysUserInParams model)
        {
            //Oracle数据库查询
            SysUser user = await GetModelAsync(model.UserID);

            using (var conn = Connection)
            {
                SysUserExt1 user2 = await conn.QueryFirstOrDefaultAsync<SysUserExt1>("select * from sys_user where user_id = :UserID", new { model.UserID });
            }

            return user;
            //using (var conn = Connection)
            //{
            //    //执行事务
            //    conn.Open();
            //    IDbTransaction trans = conn.BeginTransaction();

            //    //Get
            //    user = await conn.GetAsync<SYS_USER>(userID, trans);

            //    //GetList
            //    var userList = conn.GetList<SYS_USER>(new { USER_ID = userID }, trans);
            //    userList = await conn.GetListAsync<SYS_USER>(new { USER_ID = userID }, trans);

            //    //GetListPaged
            //    var userListPaged = conn.GetListPaged<SYS_USER>(1, 10, "WHERE DICT_IS_ENABLED = :DICT_IS_ENABLED", "USER_LOGIN DESC", new { DICT_IS_ENABLED = "1" }, trans);
            //    userListPaged = await conn.GetListPagedAsync<SYS_USER>(1, 10, "WHERE DICT_IS_ENABLED = :DICT_IS_ENABLED", "USER_LOGIN DESC", new { DICT_IS_ENABLED = "1" }, trans);

            //    //Insert
            //    var newId = conn.Insert<string, SYS_USER>(new SYS_USER { USER_ID = "b135eff584dd46a5af83364315ff9168", USER_NAME = "User", USER_LOGIN = "Person", DICT_IS_LOCKED = "1" }, trans);
            //    newId = await conn.InsertAsync<string, SYS_USER>(new SYS_USER { USER_ID = "b135eff584dd46a5af83364315ff9169", USER_NAME = "User", USER_LOGIN = "Person", DICT_IS_LOCKED = "1" }, trans);

            //    //Update
            //    int count = conn.Update(new SYS_USER { USER_ID = "b135eff584dd46a5af83364315ff9168", USER_NAME = "User2", USER_LOGIN = "Person2", DICT_IS_LOCKED = "1" }, trans);
            //    count = await conn.UpdateAsync(new SYS_USER { USER_ID = "b135eff584dd46a5af83364315ff9169", USER_NAME = "User3", USER_LOGIN = "Person3", DICT_IS_LOCKED = "1" }, trans);

            //    //Delete
            //    count = conn.Delete<SYS_USER>("b135eff584dd46a5af83364315ff9168", trans);
            //    count = conn.Delete(new SYS_USER { USER_ID = "b135eff584dd46a5af83364315ff9168" }, trans);
            //    count = await conn.DeleteAsync<SYS_USER>("b135eff584dd46a5af83364315ff9168", trans);
            //    count = await conn.DeleteAsync(new SYS_USER { USER_ID = "b135eff584dd46a5af83364315ff9169" }, trans);

            //    //DeleteList
            //    count = conn.DeleteList<SYS_USER>("WHERE USER_ID = :USER_ID", new { USER_ID = "b135eff584dd46a5af83364315ff9168" }, trans);
            //    count = conn.DeleteList<SYS_USER>(new { USER_ID = "b135eff584dd46a5af83364315ff9168" }, trans);
            //    count = await conn.DeleteListAsync<SYS_USER>("WHERE USER_ID = :USER_ID", new { USER_ID = "b135eff584dd46a5af83364315ff9168" }, trans);
            //    count = await conn.DeleteListAsync<SYS_USER>(new { USER_ID = "b135eff584dd46a5af83364315ff9168" }, trans);

            //    //RecordCount
            //    count = conn.RecordCount<SYS_USER>("WHERE PWD_FAILURES > :PWD_FAILURES", new { PWD_FAILURES = 5 }, trans);
            //    count = await conn.RecordCountAsync<SYS_USER>("WHERE PWD_FAILURES > :PWD_FAILURES", new { PWD_FAILURES = 5 }, trans);

            //    trans.Commit();

            //    return user;

            //}

        }

        public async Task<SysUserExt1> GetUserInfo_V2_0(SysUserInParams model)
        {
            using (var conn = Connection)
            {
                return await conn.GetAsync<SysUserExt1>(model.UserID);
            }
        }

        public async Task<IEnumerable<SysUserExt2>> GetUserList_V1_0(SysUserInParams model)
        {
            using (var conn = Connection)
            {
                return await conn.GetListPagedAsync<SysUserExt2>(model.pageIndex, model.pageSize, "", "USER_LOGIN DESC");
            }
        }

    }
}
