using Kernel.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.AOP
{
    public class GlobalExceptions : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;

        public GlobalExceptions(IHostingEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            var result = new OverallResult<List<ExceptionModel>>()
            {
                success = false,
                errCode = OverallErrCode.ERR_EXCEPTION,
            };

            //这里面是自定义的操作记录日志
            if (context.Exception.GetType() == typeof(KernelException))
            {
                var exception = context.Exception as KernelException;
                result.message = exception.Message;
                result.resCode = exception.ResCode;
                if (_env.IsDevelopment())
                {
                    result.data = GetStackTrace(context.Exception);//堆栈信息
                }
                context.Result = new BadRequestObjectResult(result);//返回异常数据
            }
            else
            {
                result.message = "发生了未知内部错误";
                result.resCode = OverallResCode.FAILURE;
                if (_env.IsDevelopment())
                {
                    result.data = GetStackTrace(context.Exception);//堆栈信息
                }
                context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status500InternalServerError };
            }

            //采用log4net 进行错误日志记录
            //LogHelper.ErrorLog(json.Message, context.Exception);

        }

        private List<ExceptionModel> GetStackTrace(Exception ex)
        {
            List<ExceptionModel> list = new List<ExceptionModel>();

            var model = new ExceptionModel();
            model.Message = ex.Message;
            model.ExType = ex.GetType().FullName;
            model.StackTrace = ex.StackTrace;
            list.Add(model);

            if (ex.InnerException != null)
            {
                list.AddRange(GetStackTrace(ex.InnerException));
            }

            return list;
        }
    }

    public class ExceptionModel
    {
        public string Message { get; set; }

        public string ExType { get; set; }

        public string StackTrace { get; set; }
    }

    /// <summary>
    /// 操作日志
    /// </summary>
    public class KernelException : Exception
    {
        public int ResCode { get; set; } = OverallResCode.FAILURE;

        public KernelException() { }
        public KernelException(string message) : base(message) { }
        public KernelException(string message, Exception innerException) : base(message, innerException) { }
    }

}
