using Kernel.Model.Core;
using Kernel.Model.Demo;
using MediatR;

namespace Kernel.MediatR.Demo.User.V2_0
{
    public class GetUserCommand : IRequest<CommandResult<SysUserExt1>>
    {
        public SysUserInParams Data { get; private set; }
        public GetUserCommand(SysUserInParams data)
        {
            Data = data;
        }
    }
}
