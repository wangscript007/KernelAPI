using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Buildin.Service
{
    public static class StaticFilesExtensions
    {
        public static void AddBuildinStaticFiles(this IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
                FileProvider = new PhysicalFileProvider
                (
                    //本地资源路径
                    //注：在linux下，这个路径不能以斜杠结尾，不然会报错：Request path must not end in a slash
                    KernelApp.Settings.ResourcesRootPath
                ),
                //URL路径,URL路径可以自定义，可以不用跟本地资源路径一致
                RequestPath = new PathString("/" + KernelApp.Settings.ResourcesRootFolder)
            });

        }

    }
}
