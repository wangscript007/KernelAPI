using System;
using System.Collections.Generic;

namespace Kernel.EF.Demo
{
    public partial class Roles
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string TaskMask { get; set; }
        public byte RoleFlags { get; set; }
    }
}
