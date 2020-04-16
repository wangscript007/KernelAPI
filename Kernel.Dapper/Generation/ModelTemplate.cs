using Kernel.Model.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Dapper.Generation
{
    public class ModelTemplate
    {
        public string TableHandle(TableSchema tableSchema, string fieldsCode)
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
        {fieldsCode}
    }}
}}
";

            return genCode;
        }

        public string FieldHandle(FieldSchema fieldSchema)
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
