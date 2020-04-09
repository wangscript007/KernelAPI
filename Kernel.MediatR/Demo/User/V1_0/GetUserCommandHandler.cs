using Kernel.Core.Models;
using Kernel.IService.Repository.Demo;
using Kernel.Model.Core;
using Kernel.Model.Demo;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Demo.User.V1_0
{
    public class GetUserCommandHandler : IRequestHandler<GetUserCommand, CommandResult<SysUser>>
    {
        IUserRepository _userRepository;

        public GetUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<CommandResult<SysUser>> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                try
                {
                    SysUser userInfo = await _userRepository.GetUserInfo_V1_0(request.Data);

                    return new CommandResult<SysUser>() { data = userInfo };
                }
                catch (Exception ex)
                {
                    return new CommandResult<SysUser>()
                    {
                        success = false,
                        message = ex.Message,
                        errCode = OverallErrCode.ERR_EXCEPTION,
                        resCode = ResCode.USER_INFO_EXCEPTION,
                    };
                }


            });

        }
    }
}
