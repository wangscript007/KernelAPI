using Kernel.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kernel.Core.AOP
{
    public class GlobalExceptions : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _env;

        public GlobalExceptions(IHostingEnvironment env)
        {
            _env = env;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            await HandleException(context);
            context.ExceptionHandled = true;
        }

        private async Task HandleException(ExceptionContext context)
        {
            var result = new OverallResult<List<ExceptionModel>>()
            {
                Success = false,
            };

            result.Data = await GetStackTrace(context.Exception);//堆栈信息

            //这里面是自定义的操作记录日志
            if (context.Exception.GetType() == typeof(KernelException))
            {
                var exception = context.Exception as KernelException;
                result.Message = exception.Message;
                result.Code = exception.Code;
                context.Result = new JsonResult(result) { StatusCode = exception.HttpStatusCode };
            }
            else
            {
                if (_env.IsDevelopment())
                    result.Message = context.Exception.Message;
                else
                    result.Message = "发生了未知内部错误";

                context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status500InternalServerError };
            }

            //采用log4net 进行错误日志记录
            KernelApp.Log.Error(context.Exception);

        }

        private async Task<List<ExceptionModel>> GetStackTrace(Exception ex)
        {
            List<ExceptionModel> list = new List<ExceptionModel>();

            var model = new ExceptionModel();
            model.Message = ex.Message;
            model.Exception = ex.GetType().FullName;
            model.StackTrace = ex.StackTrace;
            list.Add(model);

            if (ex.InnerException != null)
            {
                
                list.AddRange(await GetStackTrace(ex.InnerException));
            }

            return list;
        }
    }

    public class ExceptionModel
    {
        public string Message { get; set; }

        public string Exception { get; set; }

        public string StackTrace { get; set; }
    }

    /// <summary>
    /// 操作日志
    /// </summary>
    public class KernelException : Exception
    {
        public string Code { get; set; }

        public int HttpStatusCode { get; set; } = StatusCodes.Status400BadRequest;

        public KernelException() { }
        public KernelException(string message) : base(message) { }
        public KernelException(string message, Exception innerException) : base(message, innerException) { }
    }

}
