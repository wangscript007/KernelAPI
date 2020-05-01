using System;
using System.Collections.Generic;
using System.Text;
using static Dapper.SimpleCRUD;

namespace Kernel.Dapper
{
    public class ConnectionConfig
    {
        public string ConnectionString { get; set; }
        public Dialect DbType { get; set; }
        public bool UseMultitenant { get; set; } = false;
    }

    public class DapperFactoryOptions
    {
        public IList<Action<ConnectionConfig>> DapperActions { get; } = new List<Action<ConnectionConfig>>();
    }

}
