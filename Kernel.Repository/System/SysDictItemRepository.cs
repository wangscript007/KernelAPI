using Dapper;
using Kernel.Dapper.Repository;
using Kernel.IService.Repository.System;
using Kernel.Model.Core;
using Kernel.Model.Demo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Kernel.Repository.System
{
    public class SysDictItemRepository : BaseRepository<SysDictItem>, ISysDictItemRepository
    {
        public override string DBName => DapperConst.DB_MYSQL;

        public async Task<Dictionary<string, string>> GetSysDict_V1_0(string dictCode)
        {
            using (var conn = Connection)
            {
                var list = await conn.GetListAsync<SysDictItem>("where IsEnabled = '1' and DictCode = @DictCode order by SortKey", new { DictCode = dictCode });
                return list.ToDictionary(k => k.ItemCode, v => v.ItemName);
            }
        }

    }
}
