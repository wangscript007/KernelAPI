using Kernel.IService.Repository.Demo;
using Kernel.Model.Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Repository.Demo
{
    public class TopicRepository : ITopicRepository
    {
        public IFreeSql<MySqlFlag> fsql { get; set; }

        public async Task<int> AddTopic_V1_0(List<Topic> items)
        {
            return await Task.Run(() =>
            {
                var t1 = fsql.Insert(items.First()).ExecuteAffrows();
                return t1;
            });

        }

    }
}
