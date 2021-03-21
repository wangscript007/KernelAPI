using Kernel.IService.Repository.Demo;
using Kernel.Model.Core;
using Kernel.Model.Demo;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Areas.Demo.Controllers
{
    /// <summary>
    /// FreeSql功能演示
    /// </summary>
    [ApiVersion("1.0")]
    public class TopicController : DemoBaseController
    {
        public ITopicRepository TopicRepo { get; set; }

        public TopicController()
        {

        }

        /// <summary>
        /// 添加文章信息
        /// </summary>
        /// <returns>返回文章信息</returns>
        [HttpPost]
        [Route("topic"), MapToApiVersion("1.0")]
        public async Task<IActionResult> AddTopic_V1_0()
        {
            var items = new List<Topic>();
            for (var a = 0; a < 10; a++) items.Add(new Topic { Id = a + 1, Title = $"newtitle{a}", Clicks = a * 100 });

            var i = await TopicRepo.AddTopic_V1_0(items);
            var result = new CommandResult<int> { Data = i };

            return Ok(result);
        }

    }
}
