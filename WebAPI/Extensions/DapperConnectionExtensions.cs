using Kernel.Dapper.Factory;
using Kernel.Model.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SimpleCRUD;

namespace WebAPI.Extensions
{
    public static class DapperConnectionExtensions
    {
        public static void RegisterDapperConnection(this IServiceCollection services, IConfiguration configuration)
        {
            //连接sqlserver
            services.AddDapper(DapperConst.DB_SQLSERVER, m =>
            {
                m.ConnectionString = configuration.GetSection("DBConnction:SqlServerConnection").Value;
                m.DbType = Dialect.SQLServer;
            });
            //连接Oracle
            services.AddDapper(DapperConst.DB_ORACLE, m =>
            {
                m.ConnectionString = configuration.GetSection("DBConnction:OracleConnection").Value;
                m.DbType = Dialect.Oracle;
            });
            //连接MySQL
            services.AddDapper(DapperConst.DB_MYSQL, m =>
            {
                m.ConnectionString = configuration.GetSection("DBConnction:MySQLConnection").Value;
                m.DbType = Dialect.MySQL;
            });
        }

    }
}
