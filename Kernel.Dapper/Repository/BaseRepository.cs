using Kernel.Core.Utils;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SimpleCRUD;

namespace Kernel.Dapper.Repository
{
    /// <summary>
    /// 仓储层基类，通过泛型实现通用的CRUD操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepository<T> : IBaseRepository<T>
    {

        public ConnectionConfig CurrentConnectionConfig { get; set; }
        public abstract string DBName { get; }

        public BaseRepository()
        {
            IOptionsMonitor<DapperFactoryOptions> optionsMonitor = ServiceHost.GetService<IOptionsMonitor<DapperFactoryOptions>>();
            var option = optionsMonitor.Get(DBName).DapperActions.FirstOrDefault();

            if (CurrentConnectionConfig == null) CurrentConnectionConfig = new ConnectionConfig();
            if (option != null)
                option(CurrentConnectionConfig);
            else
                throw new ArgumentNullException(nameof(option));

        }

        public IDbConnection Connection
        {
            get
            {
                var connStr = CurrentConnectionConfig.ConnectionString;
                if (CurrentConnectionConfig.UseMultitenant)
                {
                    connStr = string.Format(connStr, KernelApp.Request.CurrentTenant.Label);
                }

                IDbConnection conn = null;
                switch (CurrentConnectionConfig.DbType)
                {
                    case Dialect.MySQL:
                        conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
                        break;
                    //case Dialect.SQLite:
                    //    _connection = new SQLiteConnection(conn);
                    //    break;
                    case Dialect.SQLServer:
                        conn = new System.Data.SqlClient.SqlConnection(connStr);
                        break;
                    case Dialect.Oracle:
                        conn = new Oracle.ManagedDataAccess.Client.OracleConnection(connStr);
                        break;
                    default:
                        throw new Exception("未指定数据库类型！");
                }
                return conn;
            }
        }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(T model)
        {
            int? result;
            using (var conn = Connection)
            {
                result = conn.Insert(model);
            }
            return result > 0;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public async Task<bool> AddAsync(T model)
        {
            int? result;
            using (var conn = Connection)
            {
                result = await conn.InsertAsync(model);
            }
            return result > 0;
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        public bool Delete(object id)
        {
            int? result;
            using (var conn = Connection)
            {
                result = conn.Delete<T>(id);
            }
            return result > 0;
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        public async Task<bool> DeleteAsync(object id)
        {
            int? result;
            using (var conn = Connection)
            {
                result = await conn.DeleteAsync<T>(id);
            }
            return result > 0;
        }

        /// <summary>
        /// 按条件删除数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool DeleteList(string strWhere, object parameters)
        {
            int? result;
            using (var conn = Connection)
            {
                result = conn.DeleteList<T>(strWhere, parameters);
            }
            return result > 0;
        }

        /// <summary>
        /// 按条件删除数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<bool> DeleteListAsync(string strWhere, object parameters)
        {
            int? result;
            using (var conn = Connection)
            {
                result = await conn.DeleteListAsync<T>(strWhere, parameters);
            }
            return result > 0;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(T model)
        {
            int? result;
            using (var conn = Connection)
            {
                result = conn.Update(model);
            }
            return result > 0;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public async Task<bool> UpdateAsync(T model)
        {
            int? result;
            using (var conn = Connection)
            {
                result = await conn.UpdateAsync(model);
            }
            return result > 0;
        }

        /// <summary>
        /// 根据ID获取实体对象
        /// </summary>
        public T GetModel(object id)
        {
            using (var conn = Connection)
            {
                return conn.Get<T>(id);
            }
        }

        /// <summary>
        /// 根据ID获取实体对象
        /// </summary>
        public async Task<T> GetModelAsync(object id)
        {
            using (var conn = Connection)
            {
                return await conn.GetAsync<T>(id);
            }
        }

        /// <summary>
        /// 根据条件获取实体对象集合
        /// </summary>
        public IEnumerable<T> GetModelList(string strWhere, object parameters)
        {
            using (var conn = Connection)
            {
                return conn.GetList<T>(strWhere, parameters);
            }
        }

        /// <summary>
        /// 根据条件获取实体对象集合
        /// </summary>
        public async Task<IEnumerable<T>> GetModelListAsync(string strWhere, object parameters)
        {
            using (var conn = Connection)
            {
                return await conn.GetListAsync<T>(strWhere, parameters);
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="rowsNum">每页行数</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderBy">Orde by排序</param>
        /// <param name="parameters">parameters参数</param>
        /// <returns></returns>
        public IEnumerable<T> GetListPage(int pageNum, int rowsNum, string strWhere, string orderBy, object parameters)
        {
            using (var conn = Connection)
            {
                return conn.GetListPaged<T>(pageNum, rowsNum, strWhere, orderBy, parameters); 
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="rowsNum">每页行数</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderBy">Orde by排序</param>
        /// <param name="parameters">parameters参数</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetListPageAsync(int pageNum, int rowsNum, string strWhere, string orderBy, object parameters)
        {
            using (var conn = Connection)
            {
                return await conn.GetListPagedAsync<T>(pageNum, rowsNum, strWhere, orderBy, parameters); 
            }
        }

        #endregion
    }

}
