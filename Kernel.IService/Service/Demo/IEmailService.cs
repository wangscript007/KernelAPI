using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.IService.Service.Demo
{
    public interface IEmailService
    {
        void SendEmail(string email, string title);
    }
}
