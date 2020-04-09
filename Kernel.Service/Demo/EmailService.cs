using Kernel.IService.Service.Demo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Service.Demo
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string email, string title)
        {
            Console.WriteLine("send email!");
        }
    }
}
