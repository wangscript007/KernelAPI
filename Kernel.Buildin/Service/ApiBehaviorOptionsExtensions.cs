using Kernel.Core.Extensions;
using Kernel.Model.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Buildin.Service
{
    public static class ApiBehaviorOptionsExtensions
    {
        public static void AddBuildinApiBehaviorOptions(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.GetValidationSummary();
                    var result = new CommandResult<List<string>>()
                    {
                        Success = false,
                        Message = "参数验证不通过",
                        Data = errors
                    };

                    return new JsonResult(result) { StatusCode = StatusCodes.Status416RangeNotSatisfiable };
                };
            });
        }


    }
}
