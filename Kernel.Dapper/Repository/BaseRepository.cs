using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static Dapper.SimpleCRUD;

namespace Kernel.Dapper.Repository
{
    /// <summary>
    /// 仓储层基类，通过泛型实现通用的CRUD操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : IBaseRepository<T>, IRepository
    {
        private IDbConnection _connection = null;

        public ConnectionConfig CurrentConnectionConfig { get; set; }

        public IDbConnection Connection
        {
            get
            {
                switch (CurrentConnectionConfig.DbType)
                {
                    //case Dialect.MySQL:
                    //    _connection = new MySql.Data.MySqlClient.MySqlConnection(CurrentConnectionConfig.ConnectionString);
                    //    break;
                    //case Dialect.SQLite:
                    //    _connection = new SQLiteConnection(CurrentConnectionConfig.ConnectionString);
                    //    break;
                    case Dialect.SQLServer:
                        _connection = new System.Data.SqlClient.SqlConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    case Dialect.Oracle:
                        _connection = new Oracle.ManagedDataAccess.Client.OracleConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    default:
                        throw new Exception("未指定数据库类型！");
                }
                return _connection;
            }
        }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(T model)
        {
            int? result;
            using (_connection = Connection)
            {
                result = _connection.Insert(model);
            }
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public async Task<bool> AddAsync(T model)
        {
            int? result;
            using (_connection = Connection)
            {
                result = await _connection.InsertAsync(model);
            }
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        public bool Delete(object id)
        {
            int? result;
            using (_connection = Connection)
            {
                result = _connection.Delete<T>(id);
            }
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        public async Task<bool> DeleteAsync(object id)
        {
            int? result;
            using (_connection = Connection)
            {
                result = await _connection.DeleteAsync<T>(id);
            }
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            using (_connection = Connection)
            {
                result = _connection.DeleteList<T>(strWhere, parameters);
            }
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            using (_connection = Connection)
            {
                result = await _connection.DeleteListAsync<T>(strWhere, parameters);
            }
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(T model)
        {
            int? result;
            using (_connection = Connection)
            {
                result = _connection.Update(model);
            }
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public async Task<bool> UpdateAsync(T model)
        {
            int? result;
            using (_connection = Connection)
            {
                result = await _connection.UpdateAsync(model);
            }
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据ID获取实体对象
        /// </summary>
        public T GetModel(object id)
        {
            using (_connection = Connection)
            {
                return _connection.Get<T>(id);
            }
        }

        /// <summary>
        /// 根据ID获取实体对象
        /// </summary>
        public async Task<T> GetModelAsync(object id)
        {
            using (_connection = Connection)
            {
                return await _connection.GetAsync<T>(id);
            }
        }

        /// <summary>
        /// 根据条件获取实体对象集合
        /// </summary>
        public IEnumerable<T> GetModelList(string strWhere, object parameters)
        {
            using (_connection = Connection)
            {
                return _connection.GetList<T>(strWhere, parameters);
            }
        }

        /// <summary>
        /// 根据条件获取实体对象集合
        /// </summary>
        public async Task<IEnumerable<T>> GetModelListAsync(string strWhere, object parameters)
        {
            using (_connection = Connection)
            {
                return await _connection.GetListAsync<T>(strWhere, parameters);
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
            using (_connection = Connection)
            {
                return _connection.GetListPaged<T>(pageNum, rowsNum, strWhere, orderBy, parameters); ;
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
            using (_connection = Connection)
            {
                return await _connection.GetListPagedAsync<T>(pageNum, rowsNum, strWhere, orderBy, parameters); ;
            }
        }

        #endregion
    }

}
