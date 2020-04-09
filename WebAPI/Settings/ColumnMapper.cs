using Dapper;
using Kernel.Dapper.ORM;
using Kernel.Model.Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Settings
{

    public class ColumnMapper
    {
        public static void SetMapper()
        {
            //数据库字段名和c#属性名不一致，手动添加映射关系
            SqlMapper.SetTypeMap(typeof(SysUserExt1), new ColumnAttributeTypeMapper<SysUserExt1>());

            //每个需要用到[colmun(Name="")]特性的model，都要在这里添加映射
        }
    }
}
