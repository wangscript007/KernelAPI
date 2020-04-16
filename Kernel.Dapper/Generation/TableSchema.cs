using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.Core
{
    public class TableSchema
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表别名
        /// </summary>
        public string TableAliasName { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comments { get; set; }

    }

    public class FieldSchema
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段别名
        /// </summary>
        public string FieldAliasName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comments { get; set; }
    }

}
