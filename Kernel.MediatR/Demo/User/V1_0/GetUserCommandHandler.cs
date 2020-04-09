using Kernel.Core.Basic;
using Kernel.Core.Models;
using Kernel.IService.Repository.Demo;
using Kernel.IService.Service.Demo;
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
        IEmailService _emailService;

        public GetUserCommandHandler(IUserRepository userRepository, IEmailService emailService, IServiceProvider provider)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public Task<CommandResult<SysUser>> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                try
                {
                    SysUser userInfo = await _userRepository.GetUserInfo_V1_0(request.Data);
                    _emailService.SendEmail("123@abc.com", "邮件标题");
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
