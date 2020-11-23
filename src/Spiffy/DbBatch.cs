using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Spiffy
{
    public class DbBatch : IDbBatch
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public DbBatch(IDbConnection connection, IDbTransaction transaction)
        {
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Commit unit of work & cleanup.
        /// Rolls back transaction and throws FailedCommitBatchException on failure
        /// </summary>
        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                throw new FailedCommitBatchException(ex);
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// Rollback unit of work & cleanup.
        /// <summary>
        public void Rollback()
        {
            try
            {
                _transaction.Rollback();
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// Execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Exec(string sql, DbParams param = null) =>
            Do(sql, param, cmd => cmd.Exec());

        /// <summary>
        /// Execute parameterized query and return single-value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object Scalar(string sql, DbParams param = null) =>
            Do(sql, param, cmd => cmd.Scalar());

        /// <summary>
        /// Execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
            Do(sql, param, cmd => cmd.Query(map));

        /// <summary>
        /// Execute query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map) =>
            Query(sql, DbParams.Empty, map);

        /// <summary>
        /// Execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
            Do(sql, param, cmd => cmd.QuerySingle(map));

        /// <summary>
        /// Execute query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, Func<IDataReader, T> map) =>
            QuerySingle(sql, DbParams.Empty, map);

        /// <summary>
        /// Execute paramterized query and manually cursor IDataReader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>		
        /// <param name="param"></param>
        /// <returns></returns>
        public IDataReader Read(string sql, DbParams param = null) =>
            Do(sql, param, cmd => cmd.Read());

        /// <summary>
        /// Asynchronously execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<int> ExecAsync(string sql, DbParams param = null) =>
            DoAsync(sql, param, cmd => cmd.ExecAsync());

        /// <summary>
        /// Asynchronously execute parameterized query and return single-value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<object> ScalarAsync(string sql, DbParams param = null) =>
            throw new NotImplementedException();

        /// <summary>
        /// Asynchronously execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> QueryAsync<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
            throw new NotImplementedException();

        /// <summary>
        /// Asynchronously execute query, enumerate all records and apply mapping./// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> QueryAsync<T>(string sql, Func<IDataReader, T> map) =>
            throw new NotImplementedException();

        /// <summary>
        /// Asynchronously execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<T> QuerySingleAsync<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
            throw new NotImplementedException();

        /// <summary>
        /// Asynchronously execute query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public Task<T> QuerySingleAsync<T>(string sql, Func<IDataReader, T> map) =>
            throw new NotImplementedException();

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
            _transaction.Dispose();
        }

        private T Do<T>(string sql, DbParams param, Func<DbCommand, T> func)
        {
            try
            {
                var cmd = _transaction.NewCommand(sql, param) as DbCommand;
                return func(cmd);
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }

        private async Task<T> DoAsync<T>(string sql, DbParams param, Func<DbCommand, Task<T>> func)
        {
            try
            {
                var cmd = _transaction.NewCommand(sql, param) as DbCommand;
                return await func(cmd);
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }
    }
}