using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.System
{
    public class SysPermSave
    {
        public string RoleID { get; set; }

        public IEnumerable<SysMenuPerm> MenuPerms { get; set; }

        public IEnumerable<SysFuncPerm> FuncPerms { get; set; }

    }
}
