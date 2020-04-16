using Kernel.Core.Utils;
using Kernel.Dapper.Generation;
using Kernel.IService.Repository.Core;
using Kernel.IService.Service.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Service.Core
{
    public class CodeGeneratorService : ICodeGeneratorService
    {
        public IGenerationOracleRepository GenerationRepository { get; set; }

        public string Generation(string tableName)
        {
            ModelTemplate template = new ModelTemplate();

            var tableSchema = GenerationRepository.GetTableSchema(tableName);
            var fieldSchemas = GenerationRepository.GetFieldSchema(tableName);
            StringBuilder fieldBuilder = new StringBuilder();

            foreach (var fieldSchema in fieldSchemas)
            {
                var code = template.FieldHandle(fieldSchema);
                fieldBuilder.AppendLine(code);
            }
            var finalCode = template.TableHandle(tableSchema, fieldBuilder.ToString());
            return finalCode;
        }
    }
}
