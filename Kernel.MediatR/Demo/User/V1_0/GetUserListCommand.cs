using Kernel.Model.Core;
using Kernel.Model.Demo;
using MediatR;
using System.Collections.Generic;

namespace Kernel.MediatR.Demo.User.V1_0
{
    public class GetUserListCommand : IRequest<CommandResult<IEnumerable<SysUserExt2>>>
    {
        public SysUserInParams Data { get; private set; }
        public GetUserListCommand(SysUserInParams data)
        {
            Data = data;
        }
    }
}
