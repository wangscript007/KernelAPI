using Kernel.Model.Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.Demo
{
    public class MySqlFlag { }

    public interface ITopicRepository
    {
        Task<int> AddTopic_V1_0(List<Topic> items);
    }
}
