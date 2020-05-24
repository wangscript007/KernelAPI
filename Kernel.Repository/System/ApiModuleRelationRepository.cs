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
    public class ApiModuleRelationRepository : BaseRepository<ApiModuleRelation>, IApiModuleRelationRepository
    {
        public override string DBName => DapperConst.DB_MYSQL;

        public async Task<ApiModuleRelation> GetApiModuleRelation_V1_0(string modID, string resID)
        {
            using (var conn = Connection)
            {
                return await conn.QueryFirstOrDefaultAsync<ApiModuleRelation>("select * from apimodulerelation where ModID = @ModID and ResID = @ResID", new { ModID = modID, ResID = resID });
            }
        }

        public async Task AddApiModuleRelation_V1_0(ApiModuleRelation apiResource)
        {
            using (var conn = Connection)
            {
                await conn.InsertAsync<string, ApiModuleRelation>(apiResource);
            }
        }


    }
}
