using Dapper;
using Kernel.Core.Basic;
using Kernel.Dapper.ORM;
using Kernel.Model.Demo;
using System;
using System.Linq;

namespace Kernel.Buildin.Dapper
{

    public class ColumnMapper
    {
        public static void SetMapper()
        {
            var types = typeof(SysUser).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IDBModel)));
            foreach (var type in types)
            {
                //数据库字段名和c#属性名不一致，手动添加映射关系
                SqlMapper.SetTypeMap(type, new ColumnAttributeTypeMapper(type));

                //每个需要用到[colmun(Name="")]特性的model，都要在这里添加映射
            }
        }
    }
}
