using Kernel.Dapper.Repository;
using Kernel.IService.Repository.Core;
using Kernel.Model.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data;
using System.Linq;

namespace Kernel.Repository.Core
{
    public class CodeGeneratorOracleRepository : BaseRepository<TableSchema>, ICodeGeneratorRepository
    {
        public override string DBName => DapperConst.DB_ORACLE;

        public TableSchema GetTableSchema(string tableName)
        {
            using (var conn = Connection)
            {
                string sql = $"SELECT TABLE_NAME TableName, COMMENTS Comments FROM USER_TAB_COMMENTS WHERE TABLE_TYPE IN('TABLE') AND TABLE_NAME = :TABLE_NAME";
                var schema = conn.QueryFirstOrDefault<TableSchema>(sql, new { TABLE_NAME = tableName });
                schema.TableAliasName = GetAliasName(schema.TableName);
                return schema;
            }
        }

        public List<FieldSchema> GetFieldSchema(string tableName)
        {

            using (var conn = Connection)
            {
                string sql = $"SELECT COLUMN_NAME FieldName, COMMENTS Comments FROM USER_COL_COMMENTS WHERE TABLE_NAME= :TABLE_NAME";
                var dictComments = conn.Query<FieldSchema>(sql, new { TABLE_NAME = tableName }).ToDictionary(k => k.FieldName, v => v.Comments);

                sql = $"SELECT * FROM {tableName} WHERE 1=2";
                DataTable dt = new DataTable();
                var reader = conn.ExecuteReader(sql);
                dt.Load(reader);
                List<FieldSchema> fieldSchemas = new List<FieldSchema>();
                foreach (DataColumn column in dt.Columns)
                {
                    FieldSchema fieldSchema = new FieldSchema();
                    fieldSchema.FieldName = column.ColumnName;
                    fieldSchema.FieldAliasName = GetAliasName(column.ColumnName);
                    fieldSchema.Comments = dictComments[column.ColumnName].Replace("\n", "");
                    fieldSchema.FieldType = GetType(column.DataType.Name);
                    fieldSchemas.Add(fieldSchema);
                }
                return fieldSchemas;
            }
        }

        public string GetAliasName(string name)
        {
            string[] arr = name.ToLower().Split("_");
            StringBuilder sb = new StringBuilder();
            foreach (var str in arr)
            {
                sb.Append(str.Substring(0, 1).ToUpper() + str.Substring(1));
            }
            return sb.ToString();
        }

        Dictionary<string, string> _typeDict = new Dictionary<string, string>
            {
                {"String", "string" },
                {"Int64", "int" }
            };

        public string GetType(string type)
        {
            if (_typeDict.ContainsKey(type))
                return _typeDict[type];

            return type;
        }

    }
}
