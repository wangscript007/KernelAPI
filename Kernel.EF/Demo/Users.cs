using System;
using System.Collections.Generic;

namespace Kernel.EF.Demo
{
    public partial class Users
    {
        public Guid UserId { get; set; }
        public byte[] Sid { get; set; }
        public int UserType { get; set; }
        public int AuthType { get; set; }
        public string UserName { get; set; }
    }
}
