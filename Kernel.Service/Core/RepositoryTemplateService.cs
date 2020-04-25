using Kernel.IService.Service.Core;
using Kernel.Model.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Service.Core
{
    public class RepositoryTemplateService : ITemplateService
    {

        public string TableHandle(TableSchema tableSchema)
        {
            string genCode = $@"
using Dapper;
using Kernel.Dapper.Repository;
using Kernel.IService.Repository.Core;
using Kernel.Model.Core;
using Kernel.Model.Core.{tableSchema.TableAliasName};
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kernel.Repository.Core
{{
    /// <summary>
    /// {tableSchema.Comments}
    /// </summary>
    public class {tableSchema.TableAliasName}Repository : BaseRepository<{tableSchema.TableAliasName}>, I{tableSchema.TableAliasName}Repository
    {{
        public override string DBName => DapperConst.DB_MYSQL;

        public async Task<{tableSchema.TableAliasName}> Get{tableSchema.TableAliasName}_V1_0(string attachID)
        {{
            using (var conn = Connection)
            {{
                return await conn.GetAsync<{tableSchema.TableAliasName}>(attachID);
            }}
        }}

        public async void Add{tableSchema.TableAliasName}_V1_0({tableSchema.TableAliasName} {tableSchema.TableAliasName})
        {{
            using (var conn = Connection)
            {{
                await conn.InsertAsync<string, {tableSchema.TableAliasName}>({tableSchema.TableAliasName});
            }}
        }}

        public async Task<IEnumerable<{tableSchema.TableAliasName}>> Get{tableSchema.TableAliasName}List_V1_0(string bizID)
        {{
            using (var conn = Connection)
            {{
                return await conn.GetListAsync<{tableSchema.TableAliasName}>(""WHERE ATTACH_BIZ_ID = @ATTACH_BIZ_ID"", new {{ ATTACH_BIZ_ID = bizID });
            }}
        }}

        public async Task<IEnumerable<{tableSchema.TableAliasName}>> Get{tableSchema.TableAliasName}List_V1_0(string[] attachIDs)
        {{
            using (var conn = Connection)
            {{
                return await conn.GetListAsync<{tableSchema.TableAliasName}>(""WHERE ATTACH_ID IN @ATTACH_ID"", new {{ ATTACH_ID = attachIDs }});
            }}
        }}


        public async Task<int> Delete{tableSchema.TableAliasName}_V1_0(string[] attachIDs)
        {{
            using (var conn = Connection)
            {{
                return await conn.DeleteListAsync<{tableSchema.TableAliasName}>(""WHERE ATTACH_ID IN @ATTACH_ID"", new {{ ATTACH_ID = attachIDs }});
            }}
        }}

        public async Task<int> Delete{tableSchema.TableAliasName}_V1_0(string bizID)
        {{
            using (var conn = Connection)
            {{
                return await conn.DeleteListAsync<{tableSchema.TableAliasName}>(""WHERE ATTACH_BIZ_ID = @ATTACH_BIZ_ID"", new {{ ATTACH_BIZ_ID = bizID }});
            }}
        }}

    }}
}}
";

            return genCode;
        }

        public void ForeachFields(List<FieldSchema> fieldSchemas)
        {

        }
    }
}
