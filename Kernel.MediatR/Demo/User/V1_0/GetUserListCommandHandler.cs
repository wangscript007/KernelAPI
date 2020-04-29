using Kernel.Core.AOP;
using Kernel.Core.Models;
using Kernel.IService.Repository.Demo;
using Kernel.Model.Core;
using Kernel.Model.Demo;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Demo.User.V1_0
{
    public class GetUserListCommandHandler : IRequestHandler<GetUserListCommand, CommandResult<IEnumerable<SysUserExt2>>>
    {
        IUserRepository _userRepository;

        public GetUserListCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<CommandResult<IEnumerable<SysUserExt2>>> Handle(GetUserListCommand request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var userList = await _userRepository.GetUserList_V1_0(request.Data);

                return new CommandResult<IEnumerable<SysUserExt2>>() { Data = userList };

            });

        }
    }
}
