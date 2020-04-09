using Kernel.Core.Models;
using Kernel.IService.Repository.Demo;
using Kernel.Model.Core;
using Kernel.Model.Demo;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Demo.User.V2_0
{
    public class GetUserCommandHandler : IRequestHandler<GetUserCommand, CommandResult<SysUserExt1>>
    {
        IUserRepository _userRepository;

        public GetUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<CommandResult<SysUserExt1>> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                try
                {
                    SysUserExt1 userInfo = await _userRepository.GetUserInfo_V2_0(request.Data);
                    userInfo.orgName = "集团"; // await _OrgService.GetOrgNameByID(user.USER_ID);

                    return new CommandResult<SysUserExt1>() { data = userInfo };
                }
                catch (Exception ex)
                {
                    return new CommandResult<SysUserExt1>()
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
