using Dapper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Kernel.Dapper.ORM
{
    /// <summary>
    /// 动态构建查询参数
    /// </summary>
    public class DynamicBuilder
    {
        public DynamicParameters Parameters = new DynamicParameters();
        public StringBuilder Conditions = new StringBuilder("WHERE 1=1 ");
        public string ParamsPrefix { get; set; } = "@";


        public void Build<T>(T model, Action<T> specialAction = null, Action<string, object> specialSubAction = null)
        {
            specialAction?.Invoke(model);

            Type inParamType = model.GetType();
            PropertyInfo[] PropertyList = inParamType.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                var columnAttribute = item.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                if (columnAttribute == null) continue;

                var columnName = columnAttribute.Name;
                var columnValue = item.GetValue(model, null);

                bool isJoin = true;
                if (columnValue == null || columnValue.Equals(string.Empty))
                {
                    isJoin = false;
                }
                else if (columnValue.GetType() == typeof(DateTime) && (DateTime)columnValue
                    == DateTime.MinValue)
                {
                    //日期类型
                    isJoin = false;
                }

                if (!isJoin)
                    continue;

                specialSubAction?.Invoke(columnName, columnValue);

                if (Parameters.ParameterNames.FirstOrDefault(o => o == columnName) == null)
                {
                    Conditions.Append($" AND {columnName}={ParamsPrefix}{columnName} ");
                    Parameters.Add(columnName, columnValue);
                }

            }
        }

    }
}
