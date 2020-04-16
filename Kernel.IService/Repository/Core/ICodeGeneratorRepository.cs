using Kernel.Model.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.IService.Repository.Core
{
    public interface ICodeGeneratorRepository
    {
        List<FieldSchema> GetFieldSchema(string tableName);
        TableSchema GetTableSchema(string tableName);
    }
}
