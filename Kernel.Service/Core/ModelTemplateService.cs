using Kernel.IService.Service.Core;
using Kernel.Model.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Service.Core
{
    public class ModelTemplateService : ITemplateService
    {
        private string _fieldsCode;

        public string TableHandle(TableSchema tableSchema)
        {
            string genCode = $@"
using Dapper;
using Kernel.Core.Basic;
using System;

namespace Kernel.Model.Demo
{{
    /// <summary>
    /// {tableSchema.Comments}
    /// </summary>
    [Table(""{tableSchema.TableName}"")]
    public class {tableSchema.TableAliasName} : IDBModel
    {{
        {_fieldsCode}
    }}
}}
";

            return genCode;
        }

        public void ForeachFields(List<FieldSchema> fieldSchemas)
        {
            StringBuilder fieldBuilder = new StringBuilder();
            foreach (var fieldSchema in fieldSchemas)
            {
                var code = FieldHandle(fieldSchema);
                fieldBuilder.AppendLine(code);
            }
            _fieldsCode = fieldBuilder.ToString();
        }

        private string FieldHandle(FieldSchema fieldSchema)
        {
            string genCode = $@"
        /// <summary>
        /// {fieldSchema.Comments}
        /// </summary>
        [Column(""{fieldSchema.FieldName}"")]
        public virtual {fieldSchema.FieldType} {fieldSchema.FieldAliasName} {{ get; set; }}
";

            return genCode;
        }
    }
}
