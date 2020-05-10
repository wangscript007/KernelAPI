using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Kernel.Core.AOP
{
    public class TestAOP : IInterceptor
    {
        public void Intercept(IInvocation invocation)

        {
            Debug.WriteLine("你正在调用方法 \"{0}\"  参数是 {1}... ",
               invocation.Method.Name,
               string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

            //在被拦截的方法执行完毕后 继续执行           
            invocation.Proceed();

            Debug.WriteLine("方法执行完毕，返回结果：{0}", invocation.ReturnValue);

        }
    }
}
