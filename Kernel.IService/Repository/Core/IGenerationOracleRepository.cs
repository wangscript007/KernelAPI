using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.IService.Repository.Core
{
    public interface IGenerationOracleRepository
    {
        List<Model.Core.FieldSchema> GetFieldSchema(string tableName);
        Model.Core.TableSchema GetTableSchema(string tableName);
    }
}
