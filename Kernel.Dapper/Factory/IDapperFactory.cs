using Kernel.Dapper.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Dapper.Factory
{
    public interface IDapperFactory
    {
        T CreateRepository<T>(string name) where T : IRepository, new();
    }
}
