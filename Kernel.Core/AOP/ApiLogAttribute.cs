using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Kernel.Core.Utils;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kernel.Core.AOP
{
    public class ApiLogAttribute : ActionFilterAttribute
    {
        private string LogFlag { get; set; }
        private string ActionArguments { get; set; }

        /// <summary>
        /// 请求体中的所有值
        /// </summary>
        private string RequestBody { get; set; }

        private Stopwatch Stopwatch { get; set; }

        public ApiLogAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            Stopwatch.Stop();

            string apiName = context.ActionDescriptor.DisplayName;
            double elapsed = Stopwatch.Elapsed.TotalMilliseconds;

            if (KernelApp.Settings.EnabledActionLog == "1")
            {
                IActionLogHandle actionLogger = ServiceHost.GetScopeService<IActionLogHandle>();
                actionLogger.WriteActionLog(apiName, elapsed);
            }
            else
            {
                if (Debugger.IsAttached)
                    Trace.WriteLine($"{apiName}执行花费时间：{elapsed}ms");
            }
        }
    }

    public interface IActionLogHandle
    {
        Task WriteActionLog(string apiName, double elapsed);
    }

}
