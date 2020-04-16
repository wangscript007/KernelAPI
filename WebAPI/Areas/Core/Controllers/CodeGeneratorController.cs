using Kernel.IService.Service.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Areas.Demo.Controllers
{
    [ApiVersion("1.0")]
    public class CodeGeneratorController : CoreBaseController
    {

        public ICodeGeneratorService CodeGeneratorService { get; set; }

        /// <summary>
        /// 生成模型层代码
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{tableName}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GenerationModelCode_V1_0(string tableName)
        {
            var code = CodeGeneratorService.Generation(tableName);
            return Ok(code);
        }

    }
}
