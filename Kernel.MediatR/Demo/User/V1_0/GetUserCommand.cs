using Kernel.Model.Core;
using Kernel.Model.Demo;
using MediatR;

namespace Kernel.MediatR.Demo.User.V1_0
{
    public class GetUserCommand : IRequest<CommandResult<SysUser>>
    {
        public SysUserInParams Data { get; private set; }
        public GetUserCommand(SysUserInParams data)
        {
            Data = data;
        }
    }
}
